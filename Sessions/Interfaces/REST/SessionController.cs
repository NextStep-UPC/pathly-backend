using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pathly_backend.Sessions.Application;
using pathly_backend.Sessions.Application.Dto;
using Swashbuckle.AspNetCore.Annotations;

namespace pathly_backend.Sessions.Interfaces.REST;

[ApiController]
[Route("api/sessions")]
public class SessionController : ControllerBase
{
    private readonly SessionService _svc;

    public SessionController(SessionService svc) => _svc = svc;
    
    [Authorize(Roles = "Student")]
    [HttpPost("book")]
    [SwaggerOperation(
        Summary = "Reservar una sesión",
        Description = "Permite a un estudiante reservar una sesión con un psicólogo disponible."
    )]
    [ProducesResponseType(typeof(SessionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SessionResponseDto>> Book(BookSessionDto dto)
    {
        var studentId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await _svc.BookAsync(studentId, dto));
    }
    
    [Authorize(Roles = "Psychologist")]
    [HttpPost("{id:guid}/confirm")]
    [SwaggerOperation(
        Summary = "Confirmar sesión",
        Description = "Permite a un psicólogo confirmar una sesión previamente solicitada por un estudiante."
    )]
    [ProducesResponseType(typeof(SessionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SessionResponseDto>> Confirm(Guid id)
        => Ok(await _svc.ConfirmAsync(id));
    
    [Authorize]
    [HttpGet("me")]
    [SwaggerOperation(
        Summary = "Listar mis sesiones",
        Description = "Devuelve las sesiones asociadas al usuario autenticado, ya sea como estudiante o psicólogo."
    )]
    [ProducesResponseType(typeof(IEnumerable<SessionResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SessionResponseDto>>> MySessions()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await _svc.ListMineAsync(userId));
    }
}