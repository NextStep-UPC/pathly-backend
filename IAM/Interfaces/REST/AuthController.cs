using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

using pathly_backend.IAM.Application.Internal.DTOs;
using pathly_backend.IAM.Application.Internal.Interfaces;

namespace pathly_backend.IAM.Interfaces.REST

{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Registrar usuario",
            Description = "Registra un nuevo usuario y devuelve un token con los datos del usuario."
        )]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var response = await _authService.RegisterAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Iniciar sesión",
            Description = "Inicia sesión con email y contraseña. Devuelve un token todo lo ingresado es válido."
        )]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpGet("me")]
        [Authorize]
        [SwaggerOperation(
            Summary = "Obtener usuario autenticado",
            Description = "Devuelve los datos del usuario actual autenticado extraídos del token."
        )]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            return Ok(new
            {
                Message = "¡Estás autentificado!",
                UserId = userId,
                Email = email
            });
        }
    }
}