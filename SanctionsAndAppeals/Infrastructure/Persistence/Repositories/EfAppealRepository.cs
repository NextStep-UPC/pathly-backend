using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using pathly_backend.SanctionsAndAppeals.Domain.Entities;
using pathly_backend.SanctionsAndAppeals.Domain.Enums;
using pathly_backend.SanctionsAndAppeals.Domain.Repositories;
using pathly_backend.SanctionsAndAppeals.Infrastructure.Persistence;

namespace pathly_backend.SanctionsAndAppeals.Infrastructure.Persistence.Repositories
{
    public class EfAppealRepository : IAppealRepository
    {
        private readonly SanctionsDbContext _ctx;
        public EfAppealRepository(SanctionsDbContext ctx) => _ctx = ctx;

        public Task AddAsync(Appeal appeal)
            => _ctx.Appeals.AddAsync(appeal).AsTask();

        public Task<Appeal?> FindByIdAsync(Guid id)
            => _ctx.Appeals.FirstOrDefaultAsync(a => a.Id == id);

        public IQueryable<Appeal> QueryByState(AppealState state)
            => _ctx.Appeals.Where(a => a.State == state).OrderByDescending(a => a.CreatedAtUtc);

        public IQueryable<Appeal> QueryByUser(Guid userId)
            => _ctx.Appeals.Where(a => a.UserId == userId).OrderByDescending(a => a.CreatedAtUtc);

        public IQueryable<Appeal> QueryAll()
            => _ctx.Appeals;
    }
}