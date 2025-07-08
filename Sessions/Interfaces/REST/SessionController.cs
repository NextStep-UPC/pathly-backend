using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using pathly_backend.IAM.Application.Dto;
using pathly_backend.Sessions.Application;
using pathly_backend.Sessions.Application.Dto;
using pathly_backend.Sessions.Domain.Enums;

namespace pathly_backend.Sessions.Interfaces.REST
{
    [ApiController]
    [Route("api/sessions")]
    public class SessionController : ControllerBase
    {
        private readonly SessionService _svc;

        public SessionController(SessionService svc) => _svc = svc;
        
        [Authorize(Roles = "Student")]
        [HttpPost("book")]
        [SwaggerOperation(
            Summary     = "Reservar sesión",
            Description = "Crea una nueva sesión pendiente con la fecha y hora de inicio indicadas."
        )]
        [SwaggerResponse(201, "Sesión creada correctamente", typeof(SessionResponseDto))]
        public async Task<ActionResult<SessionResponseDto>> Book([FromBody] BookSessionDto dto)
        {
            var studentId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result    = await _svc.BookAsync(studentId, dto);
            return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
        }
        
        [Authorize(Roles = "Psychologist")]
        [HttpPost("{id:guid}/assign")]
        [SwaggerOperation(
            Summary     = "Confirmar sesión",
            Description = "Asigna la sesión al psicólogo autenticado y la marca como confirmada."
        )]
        [SwaggerResponse(200, "Sesión confirmada", typeof(SessionResponseDto))]
        public async Task<ActionResult<SessionResponseDto>> Assign(Guid id)
        {
            var psychologistId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result         = await _svc.AssignAsync(id, psychologistId);
            return Ok(result);
        }
        
        [Authorize(Roles = "Student")]
        [HttpPost("{id:guid}/cancel")]
        [SwaggerOperation(
            Summary     = "Cancelar sesión",
            Description = "Permite al alumno cancelar una sesión que aún no haya sido confirmada."
        )]
        [SwaggerResponse(200, "Sesión cancelada", typeof(SessionResponseDto))]
        public async Task<ActionResult<SessionResponseDto>> Cancel(Guid id, [FromBody] CancelSessionDto dto)
        {
            var studentId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result    = await _svc.CancelAsync(id, studentId, dto);
            return Ok(result);
        }
        
        [Authorize(Roles = "Psychologist")]
        [HttpPost("{id:guid}/finish")]
        [SwaggerOperation(
            Summary     = "Finalizar sesión",
            Description = "Registra la hora de fin en la sesión confirmada y la marca como completada."
        )]
        [SwaggerResponse(200, "Sesión finalizada", typeof(SessionResponseDto))]
        public async Task<ActionResult<SessionResponseDto>> Finish(Guid id)
        {
            var psychologistId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result         = await _svc.FinishAsync(id, psychologistId);
            return Ok(result);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [SwaggerOperation(
            Summary     = "Listar sesiones",
            Description = "Devuelve todas las sesiones o sólo aquellas con el estado especificado (Pending, Confirmed, Completed)."
        )]
        [SwaggerResponse(200, "Listado de sesiones", typeof(IEnumerable<SessionResponseDto>))]
        public async Task<ActionResult<IEnumerable<SessionResponseDto>>> GetAll([FromQuery] SessionState? state)
        {
            if (state.HasValue)
                return Ok(await _svc.ListByStateAsync(state.Value));

            return Ok(await _svc.ListAllAsync());
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("pending")]
        [SwaggerOperation(
            Summary     = "Sesiones pendientes",
            Description = "Obtiene todas las sesiones que aún no han sido confirmadas."
        )]
        [SwaggerResponse(200, "Sesiones pendientes", typeof(IEnumerable<SessionResponseDto>))]
        public async Task<ActionResult<IEnumerable<SessionResponseDto>>> GetPending()
            => Ok(await _svc.ListByStateAsync(SessionState.Pending));
        
        [Authorize(Roles = "Admin")]
        [HttpGet("confirmed")]
        [SwaggerOperation(
            Summary     = "Sesiones confirmadas",
            Description = "Obtiene todas las sesiones que han sido confirmadas por un psicólogo."
        )]
        [SwaggerResponse(200, "Sesiones confirmadas", typeof(IEnumerable<SessionResponseDto>))]
        public async Task<ActionResult<IEnumerable<SessionResponseDto>>> GetConfirmed()
            => Ok(await _svc.ListByStateAsync(SessionState.Confirmed));
        
        [Authorize(Roles = "Admin")]
        [HttpGet("completed")]
        [SwaggerOperation(
            Summary     = "Sesiones completadas",
            Description = "Obtiene todas las sesiones que han sido finalizadas."
        )]
        [SwaggerResponse(200, "Sesiones completadas", typeof(IEnumerable<SessionResponseDto>))]
        public async Task<ActionResult<IEnumerable<SessionResponseDto>>> GetCompleted()
            => Ok(await _svc.ListByStateAsync(SessionState.Completed));
        
        [Authorize]
        [HttpGet("me")]
        [SwaggerOperation(
            Summary     = "Mis sesiones",
            Description = "Devuelve las sesiones donde el usuario autenticado es estudiante o psicólogo."
        )]
        [SwaggerResponse(200, "Sesiones del usuario", typeof(IEnumerable<SessionResponseDto>))]
        public async Task<ActionResult<IEnumerable<SessionResponseDto>>> MySessions()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(await _svc.ListMineAsync(userId));
        }
        
        [Authorize]
        [HttpGet("psychologists")]
        [SwaggerOperation(
            Summary     = "Listar psicólogos",
            Description = "Obtiene los datos de los psicólogos registrados en el sistema."
        )]
        [SwaggerResponse(200, "Psicólogos listados", typeof(IEnumerable<UserInfoDto>))]
        public async Task<ActionResult<IEnumerable<UserInfoDto>>> GetPsychologists()
            => Ok(await _svc.ListPsychologistsAsync());
    }
}