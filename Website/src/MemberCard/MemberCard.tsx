import React, {useEffect, useState} from "react";
import {IMember, MemberCardProps, RankState} from "../util/prefab";
import '../dist/MemberCard.css';
import {motion, AnimatePresence, MotionConfig} from 'framer-motion'


const MemberCard:React.FC<MemberCardProps> = ({Member  ,rank,prevRank})  => {
    const xMultiplier = 250;
    const yMultiplier = 200;
    
    const [state, setState] = useState(getIcon(Member));

    const classes = {
        gold: {backgroundColor: "#FFEB3BFF"},
        silver: {backgroundColor: "#9E9E9EFF"},
        bronze: {backgroundColor: "#795548FF"},
        primary: {backgroundColor: "#979ED3FF"}
    };
    
    /*const [prevRanks, setPrevRanks] = useState<number[]>([rank]);

    useEffect(() => {
        setPrevRanks(prevQueue => {
            if (prevQueue.at(-1) === rank) return prevQueue;
            if (prevQueue.length === 2) {
                // Remove the oldest rank
                prevQueue.shift();
            }

            console.log('prevQueue:-->');
            console.log(prevQueue);

            // Add the new rank
            return [...prevQueue, rank];
        });
    }, [rank]);*/
    
    
    useEffect(() => {
        setState(getIcon(Member))
    }, [Member,Member?.rankState]);

    function getIcon(member: IMember | null): '↑' | "↓" | "—" | "null" {
        return member?.rankState.valueOf() === RankState.Up.valueOf()
            ? "↑"
            : member?.rankState.valueOf() === RankState.Down.valueOf()
                ? "↓"
                : "—" ?? "null";
    }
    function getIndices(num : number) : {x: number, y: number} {
        
        return {x: Math.floor((num-1)%3), y: Math.floor((num-1)/3)}
    }
    function getClasses(rank: number) {
        return rank === 1 ? 'gold' : rank === 2 ? 'silver' : rank === 3 ? 'bronze' : 'primary'
    }
    
    console.log(Member?.player.name, rank, prevRank, state)
    return (
        <AnimatePresence>
            <div 
                className="flex justify-center items-center">
                <h2 className="text-3xl mr-3">{rank}
                    {
                        rank === 3
                        ? 'ʳᵈ'
                        : rank === 2 
                            ? 'ⁿᵈ'
                            : rank === 1
                                ? 'ˢᵗ'
                                : 'ᵗʰ'
                }
                </h2>
                <motion.div
                    key={Member?.player.hashCode ?? Math.random() * Math.random() * 10}
                    style={{zIndex: 500 - rank,
                        position: 'absolute'}}
                    
                    initial={{
                        left: (getIndices(prevRank).x + 1) * xMultiplier,
                        top: (getIndices(prevRank).y + 1) * yMultiplier,
                        backgroundColor: classes[getClasses(prevRank)].backgroundColor
                    }}
                    animate={{
                        left: (getIndices(rank).x + 1) * xMultiplier,
                        top: (getIndices(rank).y + 1) * yMultiplier,
                        backgroundColor: classes[getClasses(rank)].backgroundColor
                    }}
                    transition={{
                        duration: 1,
                        ease: "easeInOut"
                    }}

                    className={`font-semibold justify-center rounded px-4 py-1`}>
                    <div>
                        <h3 className={`${rank === 1 ? 'text-black' : ''} text-2xl w-40`}>{Member ? Member.player.name : ''}</h3>
                        <p className={`${rank === 1 ? 'text-black' : ''} text-center`}>{Member ? Member.score : ''}</p>
                    </div>
                    <h4 className={`text-4xl w-50 ${Member?.rankState === RankState.Up 
                        ? 'text-green' 
                        : Member?.rankState === RankState.Down 
                            ? 'text-red' 
                            : 'text-gold'}`}>
                        {state}
                    </h4>
                </motion.div>
            </div>
        </AnimatePresence>
    );
}

export default MemberCard;