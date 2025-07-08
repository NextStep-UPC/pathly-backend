using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pathly_backend.IAM.Domain.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using pathly_backend.Profile.Application;
using pathly_backend.Profile.Application.Dto;
using pathly_backend.Profile.Domain.Repositories;

namespace pathly_backend.Profile.Interfaces.REST;

[ApiController]
[Route("api/profiles")]
public class ProfileController : ControllerBase
{
    private readonly ProfileService _svc;

    public ProfileController(ProfileService svc) => _svc = svc;

    // ---------- Perfil propio ----------
    
    [Authorize]
    [HttpGet("me")]
    [SwaggerOperation(
        Summary = "Obtener mi perfil",
        Description = "Devuelve el perfil del usuario actualmente autenticado."
    )]
    [ProducesResponseType(typeof(ProfileResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProfileResponseDto>> Me()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role   = User.FindFirstValue(ClaimTypes.Role) ?? "Student";

        return Ok(await _svc.GetMineAsync(userId, role));
    }
    
    [Authorize]
    [HttpPut("me")]
    [SwaggerOperation(
        Summary = "Actualizar mi perfil",
        Description = "Permite actualizar el perfil del usuario autenticado."
    )]
    [ProducesResponseType(typeof(ProfileResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProfileResponseDto>> UpdateMe(UpdateProfileRequestDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role   = User.FindFirstValue(ClaimTypes.Role) ?? "Student";

        return Ok(await _svc.UpdateMineAsync(userId, role, dto));
    }

    // ---------- Perfil público ----------
    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Obtener perfil público de un usuario",
        Description = "Devuelve el perfil público de un usuario dado su ID. Incluye nombre, teléfono, avatar y rol."
    )]
    [ProducesResponseType(typeof(ProfileResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfileResponseDto>> Get(
        Guid id,
        [FromServices] IProfileRepository repo,
        [FromServices] IUserRepository userRepo)
    {
        var profile = await repo.FindByIdAsync(id)
                      ?? throw new KeyNotFoundException("Perfil no encontrado.");

        var user = await userRepo.FindByIdAsync(id)
                   ?? throw new KeyNotFoundException("Usuario no encontrado.");

        return Ok(new ProfileResponseDto(
            profile.UserId,
            profile.FirstName,
            profile.LastName,
            profile.BirthDate,
            profile.PhoneNumber,
            profile.Bio,
            profile.AvatarUrl,
            user.Role.ToString()));
    }
}