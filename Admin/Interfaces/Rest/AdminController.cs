using Admin.Application.Internal.DTOs;
using Admin.Domain.Model.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Admin.Interfaces.Rest
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly GetUserStatsQuery _getUserStatsQuery;
        private readonly GetSessionsQuery _getSessionsQuery;
        private readonly GetFeedbackQuery _getFeedbackQuery;

        public AdminController(
            GetUserStatsQuery getUserStatsQuery,
            GetSessionsQuery getSessionsQuery,
            GetFeedbackQuery getFeedbackQuery)
        {
            _getUserStatsQuery = getUserStatsQuery;
            _getSessionsQuery = getSessionsQuery;
            _getFeedbackQuery = getFeedbackQuery;
        }

        [HttpGet("stats/users")]
        public ActionResult<UserStatsDTO> GetUserStats()
        {
            var result = _getUserStatsQuery.Execute();

            var dto = new UserStatsDTO
            {
                Count = result.Count,
                Percentage = result.Percentage
            };

            return Ok(dto);
        }

        [HttpGet("sessions")]
        public ActionResult<IEnumerable<SessionDTO>> GetSessions()
        {
            var sessions = _getSessionsQuery.Execute();

            var dtoList = sessions.Select(s => new SessionDTO
            {
                Date = s.Date,
                Time = s.Time,
                Psychologist = s.Psychologist,
                User = s.User,
                Status = s.Status
            });

            return Ok(dtoList);
        }

        [HttpGet("feedback")]
        public ActionResult<IEnumerable<FeedbackDTO>> GetFeedback()
        {
            var feedbackList = _getFeedbackQuery.Execute();

            var dtoList = feedbackList.Select(f => new FeedbackDTO
            {
                User = f.User,
                Comment = f.Comment,
                Date = f.Date
            });

            return Ok(dtoList);
        }
    }
}
