using ESportLeaderBoard.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESportLeaderBoardAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayerController : ControllerBase
{
    
    public static List<Player> UsersOnDC = new();
    
    [HttpPut]
    public IActionResult PutDcUsers([FromBody] Player[] users)
    {
        if(LeaderBoardController.Boards.Any())
        {
            return BadRequest("Can't manipulate Users while Games are still running!");
        }
        UsersOnDC = users.ToList();
        return Ok();
    }
    [HttpGet]
    public IActionResult GetDcUsers()
    {
        return Ok(UsersOnDC);
    }
    [HttpDelete("{name}")]
    public IActionResult RemoveUser(string name)
    {
        if(UsersOnDC.All(x => x.Name != name)) return NotFound();
        var user = UsersOnDC.First(x => x.Name == name);
        if(LeaderBoardController.Boards.Any(b => b.Board.ContainsKey(user)))
        {
            return BadRequest("User still in a Game!");
        }
        UsersOnDC.Remove(user);
        return Ok();
    }
    [HttpPost]
    public IActionResult PostUser([FromBody] Player user)
    {
        UsersOnDC.Add(user);
        return Ok();
    }
}