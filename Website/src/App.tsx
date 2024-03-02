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
        game: 'No Games...',
        players: [],
        prevPlayers: []
    });
    
    useEffect(() => {
        
        initGames().then((data) => {
            setGames(data);
        });
    }, []);
    const updateLeaderboard = (updatedPlayers: LeaderboardProps) => {
        const isSame = JSON.stringify(leaderboardParams.players) === JSON.stringify(updatedPlayers.players);

        // If the data is the same, return the old state to prevent a re-render
        if (isSame) {
            return ;
        }
        setLeaderboardParams((prev) : LeaderboardProps => {
            return {
                ...updatedPlayers,
                prevPlayers: prev.players.map((player) => player.player.hashCode),
                game: updatedPlayers.game
            }
        });
    }
    useEffect(() => {
        const connection = new SignalR.HubConnectionBuilder()
            .withUrl("http://localhost:5001/leaderboard-hub", {skipNegotiation: true, transport: HttpTransportType.WebSockets})
            .withAutomaticReconnect()
            .build();
        
        connection.on("ReceiveLeaderBoard", (leaderboard : LeaderboardProps) => {
            console.log(`Received Leaderboard ${leaderboard.game}`);
            console.log(`games : ${games}`)
            if (leaderboard.game === lastFetchedGame) {
                console.log("updating leaderboard")
                updateLeaderboard(leaderboard)
            }
            if (games.indexOf(leaderboard.game) === -1) {
                console.log(`adding game ${leaderboard.game}`)
                setGames((prev) => {
                    return [...prev, leaderboard.game];
                });
            }
        });
        
        connection.start().catch((err) => document.write(err));
    }, []);
    
    
    /*signalR.useSignalREffect("ReceiveLeaderBoard", (leaderboard : LeaderboardProps) => {
            console.log("Received Leaderboard");
            console.log(leaderboard.game);
            if (leaderboard.game === lastFetchedGame) {
                updateLeaderboard(leaderboard)
            }
            if (!games.includes(leaderboard.game)) {
                games.push(leaderboard.game);
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
                console.log(`fetched leaderboard: ${gameToFetch} ---> `);
                console.log(data);
                updateLeaderboard(data);
                
                return data;
            });
    }
}

export default App;
