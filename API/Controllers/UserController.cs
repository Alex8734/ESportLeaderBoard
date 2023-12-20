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
        UsersOnDC.Remove(UsersOnDC.First(x => x.Name == name));
        return Ok();
    }
    [HttpPost]
    public IActionResult PostUser([FromBody] Player user)
    {
        UsersOnDC.Add(user);
        return Ok();
    }
}