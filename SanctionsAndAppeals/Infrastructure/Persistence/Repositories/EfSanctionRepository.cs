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

        public async Task<Sanction?> FindActiveByUserAsync(Guid userId)
        {
            var now = DateTime.UtcNow;
            return await _ctx.Sanctions
                .Where(s =>
                    s.UserId == userId &&
                    s.StartAtUtc <= now &&
                    (s.EndAtUtc == null || s.EndAtUtc >= now)
                )
                .FirstOrDefaultAsync();
        }

        public IQueryable<Sanction> QueryAll()
            => _ctx.Sanctions;
    }
}