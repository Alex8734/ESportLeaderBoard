import React, {useEffect, useState} from 'react';
import './dist/App.css';
import Leaderboard from './Leaderboard/Leaderboard';
import {createSignalRContext} from 'react-signalr/signalr';
import {LeaderboardProps} from "./util/prefab";
import {HttpTransportType} from "@microsoft/signalr";
import * as SignalR from "@microsoft/signalr";

function App() {
    const signalR = createSignalRContext()
    const [games,setGames] = React.useState<string[]>([]);
    const [leaderboardParams, setLeaderboardParams] = React.useState<LeaderboardProps>({
        Game: 'No Games...',
        players: [],
        prevPlayers: []
    });
    
    useEffect(() => {
        
        initGames().then((data) => {
            setGames(data);
        });
        console.log(games)
    }, []);
    const updateLeaderboard = (updatedPlayers: LeaderboardProps) => {
        const isSame = JSON.stringify(leaderboardParams.players) === JSON.stringify(updatedPlayers.players);

        // If the data is the same, return the old state to prevent a re-render
        if (isSame) {
            return ;
        }
        setLeaderboardParams((prev) => {
            return {
                ...updatedPlayers,
                prevPlayers: prev.players.map((player) => player.player.hashCode),
                Game: updatedPlayers.Game
            }
        });
    }
    useEffect(() => {
        const connection = new SignalR.HubConnectionBuilder()
            .withUrl("http://localhost:5001/leaderboard-hub", {skipNegotiation: true, transport: HttpTransportType.WebSockets})
            .withAutomaticReconnect()
            .build();
        
        connection.on("ReceiveLeaderBoard", (leaderboard : LeaderboardProps) => {
            console.log("Received Leaderboard");
            console.log(leaderboard.Game);
            if (leaderboard.Game === lastFetchedGame) {
                console.log("updating leaderboard")
                updateLeaderboard(leaderboard)
            }
            if (!games.includes(leaderboard.Game)) {
                setGames((prev) => {
                    return [...prev, leaderboard.Game];
                });
            }
        });
        
        connection.start().catch((err) => document.write(err));
    }, []);
    
    
    /*signalR.useSignalREffect("ReceiveLeaderBoard", (leaderboard : LeaderboardProps) => {
            console.log("Received Leaderboard");
            console.log(leaderboard.Game);
            if (leaderboard.Game === lastFetchedGame) {
                updateLeaderboard(leaderboard)
            }
            if (!games.includes(leaderboard.Game)) {
                games.push(leaderboard.Game);
            }
       
    },[]);
    */
    const [lastFetchedGame, setLastFetchedGame] = React.useState('');
    
    useEffect(() => {
        
        if (games.length === 0) return;
        let index = games.indexOf(lastFetchedGame);
        if (index === -1) {
            index = 0;
            setLastFetchedGame(games[0]);   
        }
        const gameToFetch = games[index];
        fetchLeaderboard(gameToFetch);
    }, [fetchLeaderboard, games, lastFetchedGame]);

    
    useEffect(() => {
        console.log("start timer")
        const timer = setInterval(() => {
            
            console.log(`switched, last: ${lastFetchedGame}`);
            
            if (games.length === 0) return () => clearInterval(timer);
            
            const newIndex = (games.indexOf(lastFetchedGame) + 1) % games.length;
            setLastFetchedGame(games[newIndex]);
        }, 10000);
        return () => clearInterval(timer);
        
    }, [games, lastFetchedGame]);
    
    return (
        <div className="bg-background h-screen w-screen flex items-center flex-col pt-10">
            <div className="w-1/2">
                <Leaderboard {...leaderboardParams}/>
            </div>
        </div>
    );
    async function initGames() {
        return await fetch("http://localhost:5001/LeaderBoard/Types")
            .then(response => response.json())
            .then((data:string[]) => {
                return data;
            });
    }
    
    async function fetchLeaderboard(gameToFetch: string) {
        return await fetch(`http://localhost:5001/LeaderBoard/${gameToFetch}`)
            .then(response => response.json())
            .then((data:LeaderboardProps )=> {
                console.log("fetching leaderboard")
                console.log(data);
                updateLeaderboard(data);
                
                return data;
            });
    }
}

export default App;
