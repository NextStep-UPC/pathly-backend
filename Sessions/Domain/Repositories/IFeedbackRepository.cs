using System;
using System.Linq;
using pathly_backend.Sessions.Domain.Entities;

namespace pathly_backend.Sessions.Domain.Repositories
{
    public interface IFeedbackRepository
    {
        Task AddAsync(Feedback feedback);
        IQueryable<Feedback> QueryBySession(Guid sessionId);
    }
}