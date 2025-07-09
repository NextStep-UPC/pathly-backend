using System;

namespace pathly_backend.SanctionsAndAppeals.Application.Dtos
{
    public record CreateSanctionDto(Guid UserId, string Reason, DateTime? EndAtUtc);
}