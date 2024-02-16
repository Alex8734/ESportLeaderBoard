using System.Text.Json;
using System.Text.Json.Serialization;
using ESportLeaderBoard.Model;
using ESportLeaderBoard.Model.Interfaces;
using ESportLeaderBoardAPI.Controllers;
using Microsoft.AspNetCore.SignalR;
using Game = ESportLeaderBoard.Model.Game;

namespace ESportLeaderBoardAPI.Hubs;

internal sealed class LeaderBoardHub : Hub<ILeaderBoardClient>, ILeaderBoardHub
{

    public async Task SendScore(NameScoreRequest player, Game game)
    {
        if(LeaderBoardController.Boards.Count == 0 || LeaderBoardController.Boards.All(b => b.Game != game))
        {
            LeaderBoardController.Boards.Add(new LeaderBoard(game));
        }
        var user = PlayerController.UsersOnDC.FirstOrDefault(u => u.Name == player.Name);
        var board = LeaderBoardController.Boards.FirstOrDefault(g => g.Game == game);
        if(board != null && user != null)
        {
            board.Add(player.Score, user);
            await Clients.All.ReceiveLeaderBoard(LeaderBoardResponse.FromLeaderBoard(board));
        }
        else
        {
            await Clients.Caller.ReceiveMessage("Board Not Found!");
        }
    }
}