using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using pathly_backend.SanctionsAndAppeals.Application.Dtos;

namespace pathly_backend.SanctionsAndAppeals.Application.Interfaces
{
    public interface ISanctionService
    {
        Task<SanctionDto> CreateAsync(CreateSanctionDto dto, Guid adminId);
        Task<SanctionDto?> GetActiveByUserAsync(Guid userId);
        Task<IEnumerable<SanctionDto>> ListAllAsync();
        Task RevokeAsync(Guid sanctionId, Guid adminId);
    }
}