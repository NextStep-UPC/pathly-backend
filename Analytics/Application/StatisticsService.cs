using System;
using System.Linq;
using System.Threading.Tasks;
using pathly_backend.Analytics.Application.Dtos;
using pathly_backend.Analytics.Application.Interfaces;
using pathly_backend.IAM.Domain.Repositories;
using pathly_backend.Sessions.Domain.Repositories;
using pathly_backend.Sessions.Domain.Enums;
using pathly_backend.Sessions.Domain.Entities;
using pathly_backend.Sessions.Application.Dtos;

namespace pathly_backend.Analytics.Application
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IUserRepository _userRepo;
        private readonly ISessionRepository _sessionRepo;
        private readonly IReportRepository _reportRepo;
        private readonly IFeedbackRepository _feedbackRepo;

        public StatisticsService(
            IUserRepository userRepo,
            ISessionRepository sessionRepo,
            IReportRepository reportRepo,
            IFeedbackRepository feedbackRepo)
        {
            _userRepo = userRepo;
            _sessionRepo = sessionRepo;
            _reportRepo = reportRepo;
            _feedbackRepo = feedbackRepo;
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            var totalStudents = (await _userRepo.ListByRoleAsync("Student")).Count();
            var totalPsychologists = (await _userRepo.ListByRoleAsync("Psychologist")).Count();

            var sessions = _sessionRepo.QueryAll();
            var byState = Enum.GetValues(typeof(SessionState))
                .Cast<SessionState>()
                .ToDictionary(
                    st => st.ToString(),
                    st => sessions.Count(s => s.State == st)
                );

            var confirmed = sessions
                .Where(s => s.AssignedAtUtc.HasValue)
                .Select(s => (s.AssignedAtUtc.Value - s.StartsAtUtc).TotalMinutes);
            var avgTime = confirmed.Any() ? confirmed.Average() : 0;

            var totalReports = _reportRepo.QueryAll().Count();
            var totalFeedbacks = _feedbackRepo.QueryAll().Count();

            return new DashboardStatsDto(
                totalStudents,
                totalPsychologists,
                byState,
                Math.Round(avgTime, 2),
                totalReports,
                totalFeedbacks
            );
        }
    }
}