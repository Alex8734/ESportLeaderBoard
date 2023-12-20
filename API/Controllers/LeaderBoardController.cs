using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESportLeaderBoardAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class LeaderBoardController : ControllerBase
{
    private static List<LeaderBoard> _boards = new()
    {
        new(Game.MarioKart),
        new(Game.SmashBros)
    };
    [HttpGet("{game}")]
    public IActionResult GetLeaderBoard(Game game)
    {
        if (_boards.All(l => l.Game != game)) return NotFound();
        var board = _boards.First(l => l.Game == game);
        var sorted =board.Board.OrderByDescending(p => p.Value);
        return Ok(new { tournament = game, players = sorted.Select(kv => new {player = kv.Key, score = kv.Value}) .ToList()});
    }
    
    [HttpPost("{game}/{userName}")]
    public IActionResult PostScore(string userName, Game game, [FromBody]JsonOutput<int> score)
    {
        var user = PlayerController.UsersOnDC.FirstOrDefault(u => u.Name == userName);
        if(user == null) return NotFound(new JsonOutput<string>("user not found!"));
        
        _boards.First(g => g.Game == game).Add(score.Value, user);
        return Ok();
    }
    [HttpGet("Types")]
    public IActionResult GetGames()
    {
        return Ok(_boards.Select(b => b.Game));
    }
}
