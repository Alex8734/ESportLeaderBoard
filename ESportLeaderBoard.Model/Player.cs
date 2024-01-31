using Newtonsoft.Json;
namespace ESportLeaderBoard.Model;

public class Player(string name, string profilePicture)
{
    public string Name { get; } = name;
    public string ProfilePicture { get; } = profilePicture;
}
public class NameScoreResponse
{
    [JsonProperty("player")]
    public string Name { get; set; }
    [JsonProperty("score")]
    public int Score { get; set; }
}

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
