using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using pathly_backend.SanctionsAndAppeals.Domain.Entities;
using pathly_backend.SanctionsAndAppeals.Domain.Repositories;
using pathly_backend.SanctionsAndAppeals.Infrastructure.Persistence;

namespace pathly_backend.SanctionsAndAppeals.Infrastructure.Persistence.Repositories
{
    public class EfSanctionRepository : ISanctionRepository
    {
        private readonly SanctionsDbContext _ctx;
        public EfSanctionRepository(SanctionsDbContext ctx) => _ctx = ctx;

        public Task AddAsync(Sanction sanction)
            => _ctx.Sanctions.AddAsync(sanction).AsTask();

        public Task<Sanction?> FindActiveByUserAsync(Guid userId)
            => _ctx.Sanctions.FirstOrDefaultAsync(s => s.UserId == userId && s.IsActive);

        public IQueryable<Sanction> QueryAll()
            => _ctx.Sanctions;
    }
}