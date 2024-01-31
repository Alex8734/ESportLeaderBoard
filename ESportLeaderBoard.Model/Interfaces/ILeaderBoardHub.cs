using ESportLeaderBoard.Model;

namespace ESportLeaderBoard.Model.Interfaces;

public static class LeaderBoardConfig
{
    public const string Route = "leaderboard-hub";
}

public interface ILeaderBoardHub
{
    public Task SendScore(NameScoreResponse player, Game game);
}