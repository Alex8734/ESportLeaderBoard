using System.Collections.Immutable;
using System.Runtime.InteropServices.JavaScript;
using ESportLeaderBoard.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESportLeaderBoardAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class LeaderBoardController : ControllerBase
{
    
    private static List<LeaderBoard> _boards = new();
    public static List<LeaderBoard> Boards => _boards;
    [HttpGet("{game}")]
    public IActionResult GetLeaderBoard(Game game)
    {
        if (_boards.All(l => l.Game != game)) return NotFound();
        var board = _boards.First(l => l.Game == game);
        return Ok(LeaderBoardResponse.FromLeaderBoard(board));
    }
    
    
    
    [HttpPost("{game}/{userName}")]
    public IActionResult PostScore(string userName, Game game, [FromBody]JsonSingleOutput<int> score)
    {
        if(_boards.Count == 0 || _boards.All(b => b.Game != game))
        {
            _boards.Add(new LeaderBoard(game));
        }
        var user = PlayerController.UsersOnDC.FirstOrDefault(u => u.Name == userName);
        if(user == null) return NotFound(new JsonSingleOutput<string>("user not found!"));
        
        _boards.First(g => g.Game == game).Add(score.Value, user);
        return Ok();
    }
    
    
    
    [HttpGet("Types")]
    public IActionResult GetGames()
    {
        return Ok(_boards.Select(b => b.Game));
    }
    
    
    
    
    
    

}
