using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using pathly_backend.Sessions.Domain.Entities;
using pathly_backend.Sessions.Domain.Repositories;
using pathly_backend.Sessions.Infrastructure.Persistence;

namespace pathly_backend.Sessions.Infrastructure.Persistence.Repositories
{
    public class EfChatMessageRepository : IChatMessageRepository
    {
        private readonly SessionsDbContext _ctx;
        public EfChatMessageRepository(SessionsDbContext ctx) => _ctx = ctx;

        public Task AddAsync(ChatMessage message)
            => _ctx.ChatMessages.AddAsync(message).AsTask();

        public IQueryable<ChatMessage> QueryBySession(Guid sessionId)
            => _ctx.ChatMessages
                .Where(m => m.SessionId == sessionId)
                .OrderBy(m => m.SentAtUtc);
    }
}