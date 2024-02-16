using Newtonsoft.Json;

namespace ESportLeaderBoard.Model;



public class LeaderBoard(Game game)
{
    public Game Game { get; } = game;
    public List<PlayerScoreResponse> Board { get; private set; } = new();
    public void Add(int score, Player player)
    {
        var before = new PlayerScoreResponse[Board.Count];
        Board.CopyTo(before);
        var boardPlayer = Board.FirstOrDefault(p => p.Player.Name == player.Name);
        if(boardPlayer is not null)
        {
            boardPlayer.Score = Math.Max(score , boardPlayer.Score);
        }else
        {
            Board.Add(new PlayerScoreResponse
            {
                Player = PlayerResponse.FromPlayer(player),
                Score = score,
                RankState = RankState.Neutral
            });
        }
        Board = Board.Order(new PlayerComparer()).ToList();
        for (int i = 0; i < Board.Count; i++)
        {
            var beforeIndex = before.ToList().FindIndex(p => p.Player.Name == Board[i].Player.Name);
            if(beforeIndex == -1) beforeIndex = 0;
            var newRankState = i < beforeIndex
                ? RankState.Up
                : i > beforeIndex
                    ? RankState.Down
                    : RankState.Neutral;
            if(Board[i].RankState != RankState.Neutral 
               && newRankState == RankState.Neutral 
               && Board[i].NeutralCooldown >= 0)
            {
                Board[i].NeutralCooldown--;
                continue;
            }
            Board[i].RankState = newRankState;
            Board[i].NeutralCooldown = PlayerScoreResponse.DefaultCooldown;
        }
    }
}

public class PlayerComparer : IComparer<PlayerScoreResponse>
{
    public int Compare(PlayerScoreResponse x, PlayerScoreResponse y)
    {
        return x == y 
            ? 0 
            : x.Score > y.Score 
                ? -1 
                : 1;
    }
}

public class LeaderBoardResponse
{
    public static LeaderBoardResponse FromLeaderBoard(LeaderBoard l)
    {
        return new LeaderBoardResponse()
        {
            Game = l.Game,
            Players = l.Board.ToList()
        };
    }
    [JsonConstructor]
    public LeaderBoardResponse()
    {
        Players = new List<PlayerScoreResponse>();
    }

    [JsonProperty("tournament")]
    public Game Game { get; set; }

    [JsonProperty("players")]
    public List<PlayerScoreResponse> Players { get; set; }
}