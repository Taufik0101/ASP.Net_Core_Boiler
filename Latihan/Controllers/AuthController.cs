using Latihan.DTO;
using Latihan.Services;
using Microsoft.AspNetCore.Mvc;

namespace Latihan.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var result = await _authService.Login(loginRequest);

        return Ok(result);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(Register registerRequest)
    {
        var result = await _authService.Register(registerRequest);

        return Ok(result);
    }
}