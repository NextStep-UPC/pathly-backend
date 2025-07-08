using System;

namespace pathly_backend.Sessions.Application.Dto
{
    public record BookSessionDto(
        DateTime StartsAtUtc
    );

    public record SessionResponseDto(
        Guid     Id,
        Guid     StudentId,
        Guid?    PsychologistId,
        DateTime StartsAtUtc,
        DateTime? EndsAtUtc,
        string   State,
        string?  CancelReason
    );
}