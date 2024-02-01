export interface IMember {
  /*  snowflake: string;
    picture: IProfilePicture;*/
    player: {
        name: string;
        profilePicture: string;
    };
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
}
export interface MemberCardProps {
    Member: IMember;
    rank: string;
}
export interface IProfilePicture {
    picture: string;
    width: number;
    height: number;
}