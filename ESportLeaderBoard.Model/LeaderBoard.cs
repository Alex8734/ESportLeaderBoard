using Newtonsoft.Json;

namespace ESportLeaderBoard.Model;



public class LeaderBoard(Game game)
{
    public Game Game { get; } = game;
    public Dictionary<Player,int> Board { get; } = new();
    public void Add(int score, Player player)
    {
        if(Board.TryGetValue(player, out var value))
        {
            Board[player] = Math.Max(score , value);
            return;
        }
        Board.Add(player, score);
    }
}
public class LeaderBoardResponse
{
    public static LeaderBoardResponse FromLeaderBoard(LeaderBoard l)
    {
        
        var sorted =l.Board.OrderByDescending(p => p.Value);
        return new LeaderBoardResponse()
        {
            Game = l.Game,
            Players = sorted.Select(kv => new PlayerScoreResponse()
            {
                Player = new PlayerResponse()
                {
                    Name = kv.Key.Name,
                    ProfilePicture = kv.Key.ProfilePicture
                }
            }).ToList()
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