namespace pathly_backend.Sessions.Application.Dto;

public record BookSessionDto(
    DateTime StartsAtUtc,
    DateTime EndsAtUtc);

public record SessionResponseDto(
    Guid Id,
    Guid StudentId,
    Guid? PsychologistId,
    DateTime StartsAtUtc,
    DateTime EndsAtUtc,
    string State,
    string? CancelReason);