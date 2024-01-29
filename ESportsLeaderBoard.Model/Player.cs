using Newtonsoft.Json;
namespace ESportsLeaderBoard.Model;


public class PlayerScoreResponse
{
    [JsonProperty("player")]
    public PlayerResponse Player { get; set; }

    [JsonProperty("score")]
    public int Score { get; set; }
}
public class PlayerResponse
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("profilePicture")]
    public string ProfilePicture { get; set; }
}
public class LeaderBoardResponse
{
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