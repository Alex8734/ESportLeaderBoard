import React, {useEffect, useState} from 'react';
import {LeaderboardProps} from "../util/prefab";
import '../dist/Leaderboard.css';
import MemberCard from "../MemberCard/MemberCard";

const Leaderboard: React.FC<LeaderboardProps> = (leaderBoard:LeaderboardProps) => {
    function getRndInt(){
        console.log("getting random int... fuck...")
        return  Math.random() * Math.random() * 20 * Math.random()
    
    }
    function getPrevIndex(idx:number){
        return leaderBoard?.prevPlayers?.indexOf(leaderBoard.players[idx]?.player.hashCode ?? -1) ?? idx+1;
    }
    console.log(leaderBoard.game)
    const [prevIdx1, setPrevIdx1] = useState(getPrevIndex(0))
    const [prevIdx2, setPrevIdx2] = useState(getPrevIndex(1))
    const [prevIdx3, setPrevIdx3] = useState(getPrevIndex(2))

    useEffect(() => {
        setPrevIdx1(getPrevIndex(0))
        setPrevIdx2(getPrevIndex(1))
        setPrevIdx3(getPrevIndex(2))
    }, [leaderBoard]);
    console.log(prevIdx1, prevIdx2, prevIdx3)
    return (
        <>
            <h2 className="text-6xl text-center mb-10 decoration-4 text-white">{leaderBoard.game}</h2>
            <div className="grid grid-cols-3 grid-rows-1 mb-28">
                <MemberCard Member={leaderBoard.players[0]} rank={1} prevRank={prevIdx1 !== -1 ? prevIdx1 +1 : 1}/>
                <MemberCard Member={leaderBoard.players[1]} rank={2} prevRank={prevIdx2 !== -1 ? prevIdx2+1 : 2}/>
                <MemberCard Member={leaderBoard.players[2]} rank={3} prevRank={prevIdx3 !== -1 ? prevIdx3+1 : 3}/>
            </div>
            <div className="grid grid-cols-3 gap-y-7">
                {
                    leaderBoard.players.slice(3).map((member, index) => {
                        const prevIdx = leaderBoard.prevPlayers.indexOf(member?.player.hashCode ?? 0)
                        return (
                            <MemberCard key={member.player.hashCode} Member={member} rank={(index + 4)} prevRank={prevIdx !== -1 ? prevIdx+1 : index+4}/>
                        );
                    })
                }
            </div>
        </>
    );
};

export default Leaderboard;