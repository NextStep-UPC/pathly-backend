using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using pathly_backend.SanctionsAndAppeals.Application.Dtos;

namespace pathly_backend.SanctionsAndAppeals.Application.Interfaces
{
    public interface IAppealService
    {
        Task<AppealDto> CreateAsync(CreateAppealDto dto, Guid userId);
        Task<IEnumerable<AppealDto>> ListByUserAsync(Guid userId);
        Task<IEnumerable<AppealDto>> ListByStateAsync(string state);
        Task<AppealDto> ResolveAsync(Guid appealId, ResolveAppealDto dto, Guid adminId);
    }
}