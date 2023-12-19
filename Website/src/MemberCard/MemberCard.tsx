import React from "react";
import {MemberCardProps} from "../util/prefab";
import '../dist/MemberCard.css';

const MemberCard:React.FC<MemberCardProps> = ({Member  ,rank})  => {
    return (
        <div className="flex justify-center items-center">
            <h2 className="text-3xl mr-3">{rank}
                {
                    rank === "3"
                    ? 'ʳᵈ'
                    : rank === "2"
                        ? 'ⁿᵈ'
                        : rank === "1"
                            ? 'ˢᵗ'
                            : 'ᵗʰ'
            }
            </h2>
            <div className={`${rank === "1" ? 'bg-gold' : rank === "2" ? 'bg-silver' : rank === "3" ? 'bg-bronce' : 'bg-primary'} font-semibold flex justify-center rounded px-4 py-1`}>
                <div>
                    <h3 className={`${rank === "1" ? 'text-black' : ''} text-2xl w-40`}>{Member ? Member.player.name : ''}</h3>
                    <p className={`${rank === "1" ? 'text-black' : ''} text-center`}>{Member ? Member.score : ''}</p>
                </div>
            </div>
        </div>
    );
}

export default MemberCard;