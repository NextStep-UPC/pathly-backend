using System;
using pathly_backend.Sessions.Domain.Enums;

namespace pathly_backend.Sessions.Application.Dtos
{
    public record ReportDto(
        Guid         Id,
        Guid         SessionId,
        Guid         PsychologistId,
        Guid         ReportedUserId,
        string       Reason,
        DateTime     CreatedAtUtc,
        ReportState  State,
        Guid?        ResolvedByAdminId,
        string?      AdminComment,
        DateTime?    ResolvedAtUtc
    );
}