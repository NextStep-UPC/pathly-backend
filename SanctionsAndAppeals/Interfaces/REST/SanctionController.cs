using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pathly_backend.SanctionsAndAppeals.Application.Dtos;
using pathly_backend.SanctionsAndAppeals.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace pathly_backend.SanctionsAndAppeals.Interfaces.REST
{
    [ApiController]
    [Route("api/sanctions")]
    public class SanctionController : ControllerBase
    {
        private readonly ISanctionService _svc;
        public SanctionController(ISanctionService svc) => _svc = svc;
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [SwaggerOperation(Summary = "Crear sanción", Description = "Imponer una sanción a un usuario.")]
        [SwaggerResponse(201, "Sanción creada", typeof(SanctionDto))]
        public async Task<ActionResult<SanctionDto>> Create([FromBody] CreateSanctionDto dto)
        {
            var adminId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var s = await _svc.CreateAsync(dto, adminId);
            return CreatedAtAction(nameof(GetActive), new { userId = s.UserId }, s);
        }
        
        [Authorize]
        [HttpGet("me")]
        [SwaggerOperation(Summary = "Mi sanción activa", Description = "Devuelve la sanción activa del usuario actual, si existe.")]
        [SwaggerResponse(200, "Sanción encontrada", typeof(SanctionDto))]
        [SwaggerResponse(204, "Sin sanción activa")]
        public async Task<ActionResult<SanctionDto>> GetActive()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var s = await _svc.GetActiveByUserAsync(userId);
            if (s == null) return NoContent();
            return Ok(s);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [SwaggerOperation(Summary = "Listar sanciones", Description = "Devuelve todas las sanciones registradas.")]
        [SwaggerResponse(200, "Listado de sanciones", typeof(IEnumerable<SanctionDto>))]
        public async Task<ActionResult<IEnumerable<SanctionDto>>> ListAll()
            => Ok(await _svc.ListAllAsync());
        
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}/revoke")]
        [SwaggerOperation(Summary = "Revocar sanción", Description = "Revoca una sanción activa.")]
        [SwaggerResponse(204, "Sanción revocada")] 
        public async Task<IActionResult> Revoke(Guid id)
        {
            var adminId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _svc.RevokeAsync(id, adminId);
            return NoContent();
        }
    }
}