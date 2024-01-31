import React, {useEffect} from 'react';
import './dist/App.css';
import * as signalR from '@microsoft/signalr';
import Leaderboard from './Leaderboard/Leaderboard';
import {JsonEnumStringConverterProtocol} from "./util/JsonEnumStringConverterProtocol";
import {LeaderboardProps} from "./util/prefab";

function App() {
    const [games] = React.useState<string[]>([]);
    const [leaderboardParams, setLeaderboardParams] = React.useState<LeaderboardProps>({
        Tournament: '',
        memberArr: []
    });
    
    //TODO some error
    const connection = new signalR.HubConnectionBuilder()
        .withHubProtocol(new JsonEnumStringConverterProtocol())
        .withUrl("/leaderboard-hub")
        .build();
    
    connection.on("ReceiveLeaderboard", (leaderboard : LeaderboardProps) => {
       if (leaderboard.Tournament === lastFetchedGame) {
           setLeaderboardParams({
                Tournament: leaderboard.Tournament,
                memberArr: leaderboard.memberArr
           });
       } 
       if (!games.includes(leaderboard.Tournament)){
           games.push(leaderboard.Tournament);
       }
    });
    
    connection.start().catch((err) => document.write(err));
    
    const [lastFetchedGame, setLastFetchedGame] = React.useState('');
    
    useEffect(() => {
        if (games.length === 0) return;
        let index = games.indexOf(lastFetchedGame);
        if (index === -1) index = 0;
        
        const gameToFetch = games[index];
        fetchLeaderboard(gameToFetch);
    }, [lastFetchedGame,games]);

    useEffect(() => {
        const timer = setInterval(() => {
            if (games.length === 0) return () => clearInterval(timer);
            
            const newIndex = (games.indexOf(lastFetchedGame) + 1) % games.length;
            setLastFetchedGame(games[newIndex]);
        }, 30000);
        return () => clearInterval(timer);
        
    }, [lastFetchedGame,games]);
    
    return (
        <div className="bg-background h-screen w-screen flex items-center flex-col pt-10">
            <div className="w-1/2">
                <Leaderboard {...leaderboardParams}/>
            </div>
        </div>
    );

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
