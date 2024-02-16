using Newtonsoft.Json;
namespace ESportLeaderBoard.Model;

public class Player(string name, string profilePicture)
{
    public string Name { get; } = name;
    public int HashCode { get; } = name.GetHashCode();
    public string ProfilePicture { get; } = profilePicture;
}
public class NameScoreRequest
{
    [JsonProperty("player")]
    public string Name { get; set; }
    [JsonProperty("hashCode")]
    public int HashCode { get; set; }
    [JsonProperty("score")]
    public int Score { get; set; }
   }

public class PlayerScoreResponse
{
    [JsonIgnore]
    public const int DefaultCooldown = 15;
    [JsonProperty("player")]
    public PlayerResponse Player { get; set; }
    
    [JsonProperty("score")]
    public int Score { get; set; }
    [JsonProperty("rankState")]
    public RankState RankState { get; set; }
    [JsonIgnore]
    public int NeutralCooldown { get; set; } 
}
public class PlayerResponse
{
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("hashCode")]
    public int HashCode { get; set; }

    [JsonProperty("profilePicture")]
    public string ProfilePicture { get; set; }
    
    public static PlayerResponse FromPlayer(Player player)
    {
        return new PlayerResponse()
        {
            Name = player.Name,
            ProfilePicture = player.ProfilePicture,
            HashCode = player.HashCode
        };
    }
}

public enum RankState
{
    Up,
    Down,
    Neutral
}