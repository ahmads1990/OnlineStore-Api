using Microsoft.AspNetCore.Mvc;

namespace OnlineStore_Api.Controllers;

[ApiController]
[Route("[Controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _userService.GetAllUsers();
        return Ok(result);

    }
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        var result = await _userService.GetUserById(userId);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}
