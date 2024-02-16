export interface IMember {
  /*  snowflake: string;
    picture: IProfilePicture;*/
    player: {
        name: string;
        profilePicture: string;
        hashCode: number;
    };
    rankState: RankState;
    score: number;
}
/*
  "players": [
    {
      "player": {
        "name": "string",
        "profilePicture": "string",
        "tag": "string"
      },
      "score": 5000
    }
 */

export interface LeaderboardProps {
    Game: string;
    players: IMember[];
    prevPlayers: number[];
}
export interface MemberCardProps {
    Member: IMember;
    rank: number;
    prevRank: number;
}
export interface IProfilePicture {
    picture: string;
    width: number;
    height: number;
}

export enum RankState {
    Up= "Up", //because of json parsing 
    Down="Down",
    Neutral="Neutral"
}