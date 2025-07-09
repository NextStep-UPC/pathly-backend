using System;
using System.Linq;
using pathly_backend.SanctionsAndAppeals.Domain.Entities;

namespace pathly_backend.SanctionsAndAppeals.Domain.Repositories
{
    public interface ISanctionRepository
    {
        Task AddAsync(Sanction sanction);
        Task<Sanction?> FindActiveByUserAsync(Guid userId);
        IQueryable<Sanction> QueryAll();
    }
}