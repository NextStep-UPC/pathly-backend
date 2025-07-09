using System;
using pathly_backend.SanctionsAndAppeals.Domain.Enums;

namespace pathly_backend.SanctionsAndAppeals.Application.Dtos
{
    public record AppealDto(Guid Id, Guid SanctionId, Guid UserId, string Reason, DateTime CreatedAtUtc, AppealState State, Guid? ResolvedById, string? DecisionComment, DateTime? ResolvedAtUtc);
}