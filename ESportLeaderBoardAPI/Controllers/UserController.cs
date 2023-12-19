using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESportLeaderBoardAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayerController : ControllerBase
{
    
    public static List<Player> UsersOnDC = new();
    [Authorize]
    [HttpPut("Users")]
    public IActionResult PutDcUsers([FromBody] Player[] users)
    {
        UsersOnDC = users.ToList();
        return Ok();
    }
    [HttpGet]
    public IActionResult GetDcUsers()
    {
        return Ok(UsersOnDC);
    }
    
    [HttpPost]
    public IActionResult PostUser([FromBody] Player user)
    {
        UsersOnDC.Add(user);
        return Ok();
    }
}