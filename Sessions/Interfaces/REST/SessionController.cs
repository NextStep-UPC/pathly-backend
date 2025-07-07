using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pathly_backend.IAM.Application.Dto;
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
        Description = "Permite a un estudiante reservar una sesión sin asignar psicólogo inicialmente."
    )]
    [ProducesResponseType(typeof(SessionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SessionResponseDto>> Book(BookSessionDto dto)
    {
        var studentId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await _svc.BookAsync(studentId, dto));
    }

    [Authorize(Roles = "Psychologist")]
    [HttpPost("{id:guid}/assign")]
    [SwaggerOperation(
        Summary = "Tomar una sesión",
        Description = "Permite a un psicólogo tomar una sesión pendiente, si aún no ha sido tomada por otro."
    )]
    [ProducesResponseType(typeof(SessionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SessionResponseDto>> Assign(Guid id)
    {
        var psychologistId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await _svc.AssignAsync(id, psychologistId));
    }

    [Authorize]
    [HttpPost("{id:guid}/cancel")]
    [SwaggerOperation(
        Summary = "Cancelar una sesión",
        Description = "Cancela una sesión. Solo el estudiante que reservó puede cancelarla si no ha sido tomada."
    )]
    [ProducesResponseType(typeof(SessionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SessionResponseDto>> Cancel(Guid id, CancelSessionDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await _svc.CancelAsync(id, userId, dto));
    }

    [Authorize(Roles = "Psychologist")]
    [HttpPost("{id:guid}/finish")]
    [SwaggerOperation(
        Summary = "Finalizar una sesión",
        Description = "Finaliza una sesión previamente tomada por el psicólogo autenticado."
    )]
    [ProducesResponseType(typeof(SessionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SessionResponseDto>> Finish(Guid id)
    {
        var psychologistId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await _svc.FinishAsync(id, psychologistId));
    }

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
    
    [Authorize]
    [HttpGet("psychologists")]
    [SwaggerOperation(
        Summary = "Listar psicólogos disponibles",
        Description = "Devuelve todos los psicólogos registrados en la plataforma (uso interno o por estudiantes al reservar)."
    )]
    [ProducesResponseType(typeof(IEnumerable<UserInfoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserInfoDto>>> GetPsychologists()
    {
        return Ok(await _svc.ListPsychologistsAsync());
    }
}