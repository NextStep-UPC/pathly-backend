using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pathly_backend.Analytics.Application.Interfaces;
using pathly_backend.Analytics.Application.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace pathly_backend.Analytics.Interfaces
{
    [ApiController]
    [Route("api/admin/stats")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _svc;
        public StatisticsController(IStatisticsService svc) => _svc = svc;

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [SwaggerOperation(
            Summary = "Obtener estadísticas del dashboard",
            Description = "Devuelve métricas de usuarios, sesiones, reportes y feedback." )]
        [SwaggerResponse(200, "Dashboard Stats", typeof(DashboardStatsDto))]
        public async Task<ActionResult<DashboardStatsDto>> GetStats()
            => Ok(await _svc.GetDashboardStatsAsync());
    }
}