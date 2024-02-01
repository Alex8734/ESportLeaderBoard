import React from 'react';
import {LeaderboardProps} from "../util/prefab";
import '../dist/Leaderboard.css';
import MemberCard from "../MemberCard/MemberCard";

const Leaderboard: React.FC<LeaderboardProps> = (leaderBoard:LeaderboardProps) => {
    return (
        <>
            <h2 className="text-6xl mb-5">{leaderBoard.Game}</h2>
            <div className="grid grid-cols-3 grid-rows-1 mb-20">
                <MemberCard Member={leaderBoard.players[0]} rank="1"/>
                <MemberCard Member={leaderBoard.players[1]} rank="2"/>
                <MemberCard Member={leaderBoard.players[2]} rank="3"/>
            </div>
            <div className="grid grid-cols-3 gap-y-6">
                {
                    leaderBoard.players.slice(3).map((member, index) => {
                        return (
                            <MemberCard Member={member} rank={(index + 4).toString()}/>
                        );
                    })
                }
            </div>
        </>
    );
};

export default Leaderboard;