namespace ESportLeaderBoardAPI;

public enum Game
{
    MarioKart,
    SmashBros,
}
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

public class Player(string name, string profilePicture, string tag)
{
    public string Name { get; } = name;
    public string ProfilePicture { get; } = profilePicture;
    public string Tag { get; } = tag;
}
public class JsonOutput<T>
{
    public JsonOutput(T value)
    {
        Value = value;
    }
    public T Value { get; set; }
}  