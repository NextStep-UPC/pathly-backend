using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using pathly_backend.Sessions.Domain.Entities;
using pathly_backend.Sessions.Domain.Repositories;
using pathly_backend.Sessions.Infrastructure.Persistence;

namespace pathly_backend.Sessions.Infrastructure.Persistence.Repositories
{
    public class EfFeedbackRepository : IFeedbackRepository
    {
        private readonly SessionsDbContext _ctx;
        public EfFeedbackRepository(SessionsDbContext ctx) => _ctx = ctx;

        public Task AddAsync(Feedback feedback)
            => _ctx.Set<Feedback>().AddAsync(feedback).AsTask();

        public IQueryable<Feedback> QueryBySession(Guid sessionId)
            => _ctx.Set<Feedback>()
                .Where(f => f.SessionId == sessionId)
                .OrderBy(f => f.CreatedAtUtc);
    }
}