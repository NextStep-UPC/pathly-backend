using System;

namespace pathly_backend.SanctionsAndAppeals.Application.Dtos
{
    public record SanctionDto(Guid Id, Guid UserId, Guid AdminId, string Reason, DateTime StartAtUtc, DateTime? EndAtUtc, bool IsActive);
}