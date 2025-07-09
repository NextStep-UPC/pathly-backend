using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pathly_backend.SanctionsAndAppeals.Application.Dtos;
using pathly_backend.SanctionsAndAppeals.Application.Interfaces;
using pathly_backend.SanctionsAndAppeals.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace pathly_backend.SanctionsAndAppeals.Interfaces.REST
{
    [ApiController]
    public class AppealController : ControllerBase
    {
        private readonly IAppealService _svc;
        public AppealController(IAppealService svc) => _svc = svc;
        
        [Authorize]
        [HttpPost("api/sanctions/{sanctionId:guid}/appeal")]
        [SwaggerOperation(Summary = "Crear apelación", Description = "Permite al usuario sancionado apelar su sanción.")]
        [SwaggerResponse(201, "Apelación creada", typeof(AppealDto))]
        public async Task<ActionResult<AppealDto>> Create(Guid sanctionId, [FromBody] CreateAppealDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var a = await _svc.CreateAsync(new CreateAppealDto(sanctionId, dto.Reason), userId);
            return CreatedAtAction(nameof(Resolve), new { appealId = a.Id }, a);
        }
        
        [Authorize]
        [HttpGet("api/sanctions/appeals/me")]
        [SwaggerOperation(Summary = "Mis apelaciones", Description = "Devuelve las apelaciones creadas por el usuario autenticado.")]
        [SwaggerResponse(200, "Listado de apelaciones", typeof(IEnumerable<AppealDto>))]
        public async Task<ActionResult<IEnumerable<AppealDto>>> MyAppeals()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(await _svc.ListByUserAsync(userId));
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("api/admin/appeals")]
        [SwaggerOperation(Summary = "Listar apelaciones", Description = "Devuelve todas las apelaciones, opcionalmente filtradas por estado.")]
        [SwaggerResponse(200, "Listado de apelaciones", typeof(IEnumerable<AppealDto>))]
        public async Task<ActionResult<IEnumerable<AppealDto>>> ListByState([FromQuery] string? state)
        {
            if (string.IsNullOrEmpty(state))
                return Ok(await _svc.ListByStateAsync(nameof(AppealState.Pending)));
            return Ok(await _svc.ListByStateAsync(state));
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut("api/appeals/{appealId:guid}/resolve")]
        [SwaggerOperation(Summary = "Resolver apelación", Description = "Permite al admin aceptar o rechazar una apelación.")]
        [SwaggerResponse(200, "Apelación resuelta", typeof(AppealDto))]
        public async Task<ActionResult<AppealDto>> Resolve(Guid appealId, [FromBody] ResolveAppealDto dto)
        {
            var adminId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var a = await _svc.ResolveAsync(appealId, dto, adminId);
            return Ok(a);
        }
    }
}