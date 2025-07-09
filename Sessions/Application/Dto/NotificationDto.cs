using System;

namespace pathly_backend.Sessions.Application.Dtos
{
    public record NotificationDto(
        Guid    Id,
        Guid    UserId,
        string  Title,
        string  Message,
        bool    IsRead,
        DateTime CreatedAtUtc
    );
}