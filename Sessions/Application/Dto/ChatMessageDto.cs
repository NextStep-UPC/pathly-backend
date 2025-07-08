using System;

namespace pathly_backend.Sessions.Application.Dtos
{
    public record ChatMessageDto(
        Guid     Id,
        Guid     SessionId,
        Guid     SenderId,
        string   Content,
        DateTime SentAtUtc
    );
}