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
    public class ReportController : ControllerBase
    {
        private readonly IReportService _svc;
        public ReportController(IReportService svc) => _svc = svc;
        
        [Authorize(Roles = "Psychologist")]
        [HttpPost("api/sessions/{sessionId:guid}/report")]
        [SwaggerOperation(Summary = "Reportar mala conducta", Description = "Permite al psicólogo asignado generar un reporte de mala conducta sobre el alumno de la sesión.")]
        [SwaggerResponse(201, "Reporte creado", typeof(ReportDto))]
        public async Task<ActionResult<ReportDto>> Create(Guid sessionId, [FromBody] CreateReportDto dto)
        {
            var psychologistId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var rep = await _svc.CreateAsync(sessionId, psychologistId, dto);
            return CreatedAtAction(nameof(Resolve), new { reportId = rep.Id }, rep);
        }
        
        [Authorize(Roles = "Psychologist")]
        [HttpGet("api/sessions/{sessionId:guid}/report/my")]
        [SwaggerOperation(Summary = "Mis reportes", Description = "Obtiene todos los reportes de mala conducta creados por el psicólogo autenticado para la sesión.")]
        [SwaggerResponse(200, "Lista de reportes", typeof(IEnumerable<ReportDto>))]
        public async Task<ActionResult<IEnumerable<ReportDto>>> MyReports(Guid sessionId)
        {
            var psychologistId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var list = await _svc.ListByPsychologistAsync(psychologistId);
            return Ok(list);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut("api/reports/{reportId:guid}/resolve")]
        [SwaggerOperation(Summary = "Resolver reporte", Description = "Permite al administrador aceptar o rechazar un reporte pendiente.")]
        [SwaggerResponse(200, "Reporte resuelto", typeof(ReportDto))]
        public async Task<ActionResult<ReportDto>> Resolve(Guid reportId, [FromBody] ResolveReportDto dto)
        {
            var adminId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var rep = await _svc.ResolveAsync(reportId, adminId, dto);
            return Ok(rep);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("api/reports")]
        [SwaggerOperation(Summary = "Todos los reportes", Description = "Devuelve todos los reportes de mala conducta en el sistema.")]
        [SwaggerResponse(200, "Lista de reportes", typeof(IEnumerable<ReportDto>))]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetAll()
        {
            var list = await _svc.ListAllAsync();
            return Ok(list);
        }
    }
}