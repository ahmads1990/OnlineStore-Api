using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OnlineStore_Api.Dtos.Auth;

namespace OnlineStore_Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController:ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterDto registerDto)
    {
        var result = await _authService.RegisterUser(registerDto);

        if (!result.IsAuthenticated)
            return BadRequest(result.Message);

        //Todo send confirmation mail
        return Ok(result);
    }
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginDto loginDto)
    {
        var result = await _authService.LoginUser(loginDto);

        if (!result.IsAuthenticated)
            return BadRequest(result.Message);

        //Todo send confirmation mail
        return Ok(result);
    }
    [HttpPost("claim")]
    public async Task<IActionResult> AddRoleAsync(AddClaimDto claimDto)
    {
        var result = await _authService.AddClaim(claimDto);

        if (!string.IsNullOrEmpty(result))
            return BadRequest(result);

        return Ok(claimDto);
    }
}
