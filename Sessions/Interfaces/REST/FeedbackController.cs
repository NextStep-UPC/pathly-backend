using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pathly_backend.Sessions.Application.Dtos;
using pathly_backend.Sessions.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace pathly_backend.Sessions.Interfaces.REST;

[ApiController]
[Route("api/sessions/{sessionId:guid}/feedback")]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _svc;
    public FeedbackController(IFeedbackService svc) => _svc = svc;

    [Authorize(Roles = "Student")]
    [HttpPost]
    [SwaggerOperation(
        Summary     = "Crear feedback",
        Description = "Permite al estudiante dejar una valoración y comentario tras una sesión completada."
    )]
    [SwaggerResponse(201, "Feedback creado", typeof(FeedbackDto))]
    public async Task<ActionResult<FeedbackDto>> Create(Guid sessionId, [FromBody] CreateFeedbackDto dto)
    {
        var studentId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var fb        = await _svc.CreateAsync(sessionId, studentId, dto);
        return CreatedAtAction(nameof(GetAll), new { sessionId }, fb);
    }

    [Authorize]
    [HttpGet]
    [SwaggerOperation(
        Summary     = "Listar feedback",
        Description = "Obtiene todas las valoraciones dejadas para la sesión indicada."
    )]
    [SwaggerResponse(200, "Feedback listados", typeof(IEnumerable<FeedbackDto>))]
    public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetAll(Guid sessionId)
    {
        var list = await _svc.ListBySessionAsync(sessionId);
        return Ok(list);
    }
}