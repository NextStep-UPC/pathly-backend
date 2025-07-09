using System;
using System.Linq;
using pathly_backend.SanctionsAndAppeals.Domain.Entities;
using pathly_backend.SanctionsAndAppeals.Domain.Enums;

namespace pathly_backend.SanctionsAndAppeals.Domain.Repositories
{
    public interface IAppealRepository
    {
        Task AddAsync(Appeal appeal);
        Task<Appeal?> FindByIdAsync(Guid id);
        IQueryable<Appeal> QueryByState(AppealState state);
        IQueryable<Appeal> QueryByUser(Guid userId);
        IQueryable<Appeal> QueryAll();
    }
}