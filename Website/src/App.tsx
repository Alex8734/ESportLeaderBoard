import React, {useEffect} from 'react';
import './dist/App.css';
import Leaderboard from './Leaderboard/Leaderboard';
import {createSignalRContext} from 'react-signalr/signalr';
import {LeaderboardProps} from "./util/prefab";
import {HttpTransportType} from "@microsoft/signalr";


function App() {
    
    const signalR = createSignalRContext();
    const [games,setGames] = React.useState<string[]>([]);
    useEffect(() => {
        initGames().then((data) => {
            setGames(data);
        });
    }, [games]);
    
    const [leaderboardParams, setLeaderboardParams] = React.useState<LeaderboardProps>({
        Game: '',
        players: []
    });
    
    signalR.useSignalREffect("ReceiveLeaderBoard", (leaderboard : LeaderboardProps) => {
       console.log(leaderboard)
        if (leaderboard.Game === lastFetchedGame) {
           setLeaderboardParams({
                Game: leaderboard.Game,
                players: leaderboard.players
           });
       } 
       if (!games.includes(leaderboard.Game)){
           games.push(leaderboard.Game);
       }
    },[games, leaderboardParams]);
    
    const [lastFetchedGame, setLastFetchedGame] = React.useState('');
    
    useEffect(() => {
        console.log(games);
        console.log(`switched, last: ${lastFetchedGame}`);
        if (games.length === 0) return;
        let index = games.indexOf(lastFetchedGame);
        if (index === -1) index = 0;
        
        const gameToFetch = games[index];
        fetchLeaderboard(gameToFetch);
    }, [lastFetchedGame,games]);

    useEffect(() => {
        const timer = setInterval(() => {
            console.log(games)
            console.log("switched")
            console.log(lastFetchedGame)
            if (games.length === 0) return () => clearInterval(timer);
            const newIndex = (games.indexOf(lastFetchedGame) + 1) % games.length;
            setLastFetchedGame(games[newIndex]);
        }, 30000);
        return () => clearInterval(timer);
        
    }, [lastFetchedGame,games]);
    
    return (
        <signalR.Provider
            url={"http://localhost:5001/leaderboard-hub"}
            skipNegotiation={true}
            automaticReconnect={true}
            transport={HttpTransportType.WebSockets}>
            <div className="bg-background h-screen w-screen flex items-center flex-col pt-10">
                <div className="w-1/2">
                    <Leaderboard {...leaderboardParams}/>
                </div>
            </div>        
        </signalR.Provider>
    );
    async function initGames() {
        return await fetch("http://localhost:5001/LeaderBoard/Types")
            .then(response => response.json())
            .then((data:string[]) => {
                console.log(JSON.stringify(data));
                return data;
            });
    }
    async function fetchLeaderboard(gameToFetch: string) {
        return await fetch(`http://localhost:5001/LeaderBoard/${gameToFetch}`)
            .then(response => response.json())
            .then((data:LeaderboardProps )=> {
                console.log(JSON.stringify(data));
                setLeaderboardParams(data); 
                return data;
            });
    }
}

export default App;
