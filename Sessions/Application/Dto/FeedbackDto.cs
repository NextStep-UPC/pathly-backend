using System;

namespace pathly_backend.Sessions.Application.Dtos
{
    public record FeedbackDto(
        Guid     Id,
        Guid     SessionId,
        Guid     StudentId,
        int      Rating,
        string?  Comment,
        DateTime CreatedAtUtc
    );
}