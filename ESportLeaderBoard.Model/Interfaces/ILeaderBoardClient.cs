using ESportLeaderBoard.Model;

namespace ESportLeaderBoard.Model.Interfaces;


public interface ILeaderBoardClient
{
    Task ReceiveLeaderBoard(LeaderBoardResponse leaderBoard);
    Task ReceiveMessage(string text);
}