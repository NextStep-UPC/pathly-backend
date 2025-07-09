using System.Threading.Tasks;
using pathly_backend.Analytics.Application.Dtos;

namespace pathly_backend.Analytics.Application.Interfaces
{
    public interface IStatisticsService
    {
        Task<DashboardStatsDto> GetDashboardStatsAsync();
    }
}