using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pathly_backend.Sessions.Application.Interfaces;
using pathly_backend.Sessions.Application.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace pathly_backend.Sessions.Interfaces.REST;

[ApiController]
[Route("api/sessions/{sessionId:guid}/messages")]
public class ChatController : ControllerBase
{
    private readonly IChatService _svc;
    public ChatController(IChatService svc) => _svc = svc;

    [Authorize]
    [HttpPost]
    [SwaggerOperation(
        Summary     = "Enviar mensaje de chat",
        Description = "Guarda un nuevo mensaje en la sesión y lo devuelve."
    )]
    [SwaggerResponse(201, "Mensaje creado", typeof(ChatMessageDto))]
    public async Task<ActionResult<ChatMessageDto>> SendMessage(
        Guid sessionId,
        [FromBody] SendChatMessageDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var msg    = await _svc.SaveMessageAsync(sessionId, userId, dto.Content);
        return CreatedAtAction(nameof(GetHistory), new { sessionId }, msg);
    }

    [Authorize] 
    [HttpGet]
    [SwaggerOperation(
        Summary     = "Histórico de chat",
        Description = "Devuelve todos los mensajes guardados para la sesión indicada, ordenados cronológicamente."
    )]
    [SwaggerResponse(200, "Lista de mensajes", typeof(IEnumerable<ChatMessageDto>))]
    public async Task<ActionResult<IEnumerable<ChatMessageDto>>> GetHistory(Guid sessionId)
    {
        var msgs = await _svc.ListMessagesAsync(sessionId);
        return Ok(msgs);
    }
}