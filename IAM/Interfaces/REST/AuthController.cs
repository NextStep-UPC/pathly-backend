using System.Security.Claims;
using IAM.Application.Dto;
using IAM.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace IAM.Interfaces.REST;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _svc;
    public AuthController(AuthService svc) => _svc = svc;

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(AuthLoginRequest body)
        => Ok(await _svc.LoginAsync(body));

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(AuthRegisterRequest body)
        => Ok(await _svc.RegisterAsync(body));

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        if (!User.Identity?.IsAuthenticated ?? true)
            return Unauthorized(new { message = "No estás autenticado." });

        var id = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        var email = User.Identity?.Name;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            id,
            email,
            role
        });
    }
}