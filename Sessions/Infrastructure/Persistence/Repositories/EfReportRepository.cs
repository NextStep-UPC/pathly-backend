using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using pathly_backend.Sessions.Domain.Entities;
using pathly_backend.Sessions.Domain.Repositories;
using pathly_backend.Sessions.Infrastructure.Persistence;

namespace pathly_backend.Sessions.Infrastructure.Persistence.Repositories
{
    public class EfReportRepository : IReportRepository
    {
        private readonly SessionsDbContext _ctx;
        public EfReportRepository(SessionsDbContext ctx) => _ctx = ctx;

        public Task AddAsync(Report report)
            => _ctx.Reports.AddAsync(report).AsTask();

        public Task<Report?> FindByIdAsync(Guid id)
            => _ctx.Reports.FirstOrDefaultAsync(r => r.Id == id);

        public IQueryable<Report> QueryByPsychologist(Guid psychologistId)
            => _ctx.Reports
                .Where(r => r.PsychologistId == psychologistId)
                .OrderByDescending(r => r.CreatedAtUtc);

        public IQueryable<Report> QueryAll()
            => _ctx.Reports.OrderByDescending(r => r.CreatedAtUtc);
    }
}