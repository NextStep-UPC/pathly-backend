using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using pathly_backend.IAM.Application;
using pathly_backend.IAM.Application.Dto;
using pathly_backend.IAM.Domain.Enums;
using pathly_backend.IAM.Domain.Repositories;
using pathly_backend.Shared.Common;
using System.Security.Claims; 

namespace pathly_backend.IAM.Interfaces.REST;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _svc;
    public AuthController(AuthService svc) => _svc = svc;

    [HttpPost("register")]
    [SwaggerOperation(
        Summary     = "Registro de nuevo usuario",
        Description = "Crea un usuario con rol Estudiante por defecto y devuelve el JWT de acceso."
    )]
    [ProducesResponseType(typeof(AuthResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResultDto>> Register(RegisterRequestDto dto)
        => Ok(await _svc.RegisterAsync(dto));

    [HttpPost("login")]
    [SwaggerOperation(
        Summary     = "Inicio de sesión",
        Description = "Valida credenciales y devuelve el JWT correspondiente."
    )]
    [ProducesResponseType(typeof(AuthResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResultDto>> Login(LoginRequestDto dto)
        => Ok(await _svc.LoginAsync(dto));

    [Authorize(Roles = "Admin")]
    [HttpPost("roles/grant/{id:guid}")]
    [SwaggerOperation(
        Summary     = "Otorgar rol a un usuario",
        Description = "Disponible solo para Administradores. Permite cambiar el rol del usuario (p. ej. de Estudiante a Psicólogo)."
    )]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GrantRole(
        Guid id,
        [FromQuery] UserRole role,
        [FromServices] IUserRepository repo,
        [FromServices] IUnitOfWork uow)
    {
        var user = await repo.FindByIdAsync(id)
                   ?? throw new KeyNotFoundException("Usuario no encontrado");

        user.GrantRole(role);
        await uow.SaveChangesAsync();

        return Ok($"El rol {role} ha sido asignado al ID: {id}.");
    }
    
    
    [Authorize]
    [HttpGet("me")]
    [SwaggerOperation(
        Summary     = "Verificar el estado de tu cuenta",
        Description = "Disponible para testeos y ver que toda la información se crea correctamente."
    )]
    public async Task<ActionResult<UserInfoDto>> Me(
        [FromServices] IUserRepository repo)
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (sub is null)
            return Unauthorized();

        var userId = Guid.Parse(sub);
        
        var user = await repo.FindByIdAsync(userId);
        if (user is null)
            return NotFound();
        
        var dto = new UserInfoDto(
            user.Id,
            user.Email.Value,
            user.Name?.FirstName,
            user.Name?.LastName,
            user.Role.ToString());

        return Ok(dto);
    }
}