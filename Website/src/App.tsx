import React, {useEffect} from 'react';
import './dist/App.css';
import Leaderboard from './Leaderboard/Leaderboard';

function App() {
    const [leaderboardParams, setLeaderboardParams] = React.useState({
        Tournament: '',
        memberArr: []
    });
    const [lastFetchedGame, setLastFetchedGame] = React.useState('SmashBros');
    useEffect(() => {
        const gameToFetch = lastFetchedGame === 'MarioKart' ? 'SmashBros' : 'MarioKart';
        fetchLeaderboard(gameToFetch);
    }, [lastFetchedGame]);

    useEffect(() => {
        const timer = setInterval(() => {
            setLastFetchedGame(lastFetchedGame === 'MarioKart' ? 'SmashBros' : 'MarioKart');
        }, 30000);
        return () => clearInterval(timer);
    }, [lastFetchedGame]);
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
            .then(data => {
                console.log(JSON.stringify(data));
                setLeaderboardParams({
                    Tournament: data.tournament, memberArr: data.players
                });
                return data;
            });
    }
}

export default App;
