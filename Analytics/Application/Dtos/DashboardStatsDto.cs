using System.Collections.Generic;

namespace pathly_backend.Analytics.Application.Dtos
{
    public record DashboardStatsDto(
        int TotalStudents,
        int TotalPsychologists,
        Dictionary<string,int> SessionsByState,
        double AvgTimeToConfirmMinutes,
        int TotalReports,
        int TotalFeedbacks
    );
}