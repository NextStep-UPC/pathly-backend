using System;
using System.Linq;
using pathly_backend.Sessions.Domain.Entities;

namespace pathly_backend.Sessions.Domain.Repositories
{
    public interface IChatMessageRepository
    {
        Task AddAsync(ChatMessage message);
        IQueryable<ChatMessage> QueryBySession(Guid sessionId);
    }
}