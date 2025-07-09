using System;

namespace pathly_backend.SanctionsAndAppeals.Application.Dtos
{
    public record CreateAppealDto(Guid SanctionId, string Reason);
}