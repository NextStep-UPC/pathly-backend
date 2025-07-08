using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using pathly_backend.Sessions.Application.Dtos;

namespace pathly_backend.Sessions.Application.Interfaces
{
    public interface IChatService
    {
        Task<ChatMessageDto> SaveMessageAsync(Guid sessionId, Guid senderId, string content);
        Task<IEnumerable<ChatMessageDto>> ListMessagesAsync(Guid sessionId);
    }
}