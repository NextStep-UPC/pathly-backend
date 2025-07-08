using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pathly_backend.Sessions.Application.Dtos;
using pathly_backend.Sessions.Application.Interfaces;
using pathly_backend.Sessions.Domain.Entities;
using pathly_backend.Sessions.Domain.Enums;
using pathly_backend.Sessions.Domain.Repositories;
using pathly_backend.Sessions.Infrastructure.Persistence;
using pathly_backend.Shared.Common;

namespace pathly_backend.Sessions.Application
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _repo;
        private readonly ISessionsUnitOfWork _uow;
        private readonly ISessionRepository _sessionRepo;

        public FeedbackService(
            IFeedbackRepository repo,
            ISessionsUnitOfWork uow,
            ISessionRepository sessionRepo)
        {
            _repo        = repo;
            _uow         = uow;
            _sessionRepo = sessionRepo;
        }

        public async Task<FeedbackDto> CreateAsync(Guid sessionId, Guid studentId, CreateFeedbackDto dto)
        {
            var session = await _sessionRepo.FindByIdAsync(sessionId)
                          ?? throw new KeyNotFoundException("Session not found");
            if (session.State != SessionState.Completed)
                throw new InvalidOperationException("Only completed sessions may receive feedback.");
            if (session.StudentId != studentId)
                throw new UnauthorizedAccessException("Only session’s student can leave feedback.");

            var fb = new Feedback(sessionId, studentId, dto.Rating, dto.Comment);
            await _repo.AddAsync(fb);
            await _uow.SaveChangesAsync();

            return new FeedbackDto(fb.Id, fb.SessionId, fb.StudentId, fb.Rating, fb.Comment, fb.CreatedAtUtc);
        }

        public async Task<IEnumerable<FeedbackDto>> ListBySessionAsync(Guid sessionId)
        {
            var list = _repo.QueryBySession(sessionId).ToList();
            return list.Select(f =>
                new FeedbackDto(f.Id, f.SessionId, f.StudentId, f.Rating, f.Comment, f.CreatedAtUtc)
            );
        }
    }
}