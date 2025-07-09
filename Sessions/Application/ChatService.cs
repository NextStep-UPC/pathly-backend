using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using pathly_backend.Sessions.Application.Dtos;
using pathly_backend.Sessions.Application.Interfaces;
using pathly_backend.Sessions.Domain.Entities;
using pathly_backend.Sessions.Domain.Repositories;
using pathly_backend.Sessions.Infrastructure.Persistence;

namespace pathly_backend.Sessions.Application;

public class ChatService : IChatService
{
    private readonly IChatMessageRepository _repo;
    private readonly ISessionsUnitOfWork    _uow;

    public ChatService(IChatMessageRepository repo, ISessionsUnitOfWork uow)
    {
        _repo = repo;
        _uow  = uow;
    }

    public async Task<ChatMessageDto> SaveMessageAsync(Guid sessionId, Guid senderId, string content)
    {
        var msg = new ChatMessage(sessionId, senderId, content);
        await _repo.AddAsync(msg);
        await _uow.SaveChangesAsync();
        return new ChatMessageDto(
            msg.Id, msg.SessionId, msg.SenderId, msg.Content, msg.SentAtUtc
        );
    }

    public async Task<IEnumerable<ChatMessageDto>> ListMessagesAsync(Guid sessionId)
    {
        var list = await _repo.QueryBySession(sessionId).ToListAsync();
        return list.Select(m =>
            new ChatMessageDto(
                m.Id, m.SessionId, m.SenderId, m.Content, m.SentAtUtc
            )
        );
    }
}