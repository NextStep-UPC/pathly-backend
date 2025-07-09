using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pathly_backend.Sessions.Application.Dtos;
using pathly_backend.Sessions.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace pathly_backend.Sessions.Interfaces.REST
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _svc;
        public NotificationController(INotificationService svc) => _svc = svc;
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [SwaggerOperation(
            Summary     = "Crear notificación",
            Description = "Registra una nueva notificación para el usuario indicado."
        )]
        [SwaggerResponse(201, "Notificación creada", typeof(NotificationDto))]
        public async Task<ActionResult<NotificationDto>> Create([FromBody] CreateNotificationDto dto)
        {
            var n = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByUser), new { userId = dto.UserId }, n);
        }
        
        [Authorize]
        [HttpGet]
        [SwaggerOperation(
            Summary     = "Mis notificaciones",
            Description = "Devuelve las notificaciones dirigidas al usuario autenticado."
        )]
        [SwaggerResponse(200, "Listado de notificaciones", typeof(IEnumerable<NotificationDto>))]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetByUser()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var list   = await _svc.ListByUserAsync(userId);
            return Ok(list);
        }
        
        [Authorize]
        [HttpPut("{id:guid}/read")]
        [SwaggerOperation(
            Summary     = "Marcar notificación leída",
            Description = "Actualiza el estado de la notificación a leída para el usuario autenticado."
        )]
        [SwaggerResponse(204, "Marcado como leído")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _svc.MarkAsReadAsync(id, userId);
            return NoContent();
        }
    }
}