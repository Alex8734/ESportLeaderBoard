

using ESportsLeaderBoard.Model;
namespace ESportLeaderBoardAPI.Model;


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

public class Player(string name, string profilePicture)
{
    public string Name { get; } = name;
    public string ProfilePicture { get; } = profilePicture;
}
