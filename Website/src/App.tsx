import React, {useEffect} from 'react';
import './dist/App.css';
import Leaderboard from './Leaderboard/Leaderboard';
import {IMember} from "./util/prefab";

function App() {
    const [leaderboardParams, setLeaderboardParams] = React.useState({
        Tournament: 'Tournament',
        memberArr: []
    });
    useEffect(() => {
        const timer = setInterval(() => {
            fetchLeaderboard().then(data => {
                setLeaderboardParams({
                    Tournament: data.tournament, memberArr: data.players
                });
            });
        }, 30000);
        return () => clearInterval(timer);
    }, []);
    return (
        <div className="bg-background h-screen w-screen flex items-center flex-col pt-10">
            <div className="w-1/2">
                <Leaderboard {...leaderboardParams}/>
            </div>
        </div>
    );

    async function fetchLeaderboard() {
        return await fetch('https://localhost:5001/Leaderboard/MarioKart')
            .then(response => response.json())
            .then(data => {
                return data;
            });
    }
}

export default App;
