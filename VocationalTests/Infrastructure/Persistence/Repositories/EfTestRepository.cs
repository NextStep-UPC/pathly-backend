using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using pathly_backend.VocationalTests.Domain.Entities;
using pathly_backend.VocationalTests.Domain.Repositories;
using pathly_backend.VocationalTests.Infrastructure.Persistence;

namespace pathly_backend.VocationalTests.Infrastructure.Persistence.Repositories
{
    public class EfTestRepository : ITestRepository
    {
        private readonly VocationalTestsDbContext _ctx;
        public EfTestRepository(VocationalTestsDbContext ctx) => _ctx = ctx;

        public Task AddAsync(Test test)
            => _ctx.Tests.AddAsync(test).AsTask();

        public Task<Test?> FindByIdAsync(Guid id)
            => _ctx.Tests
                .Include(t => t.Questions).ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(t => t.Id == id);

        public IQueryable<Test> QueryAll()
            => _ctx.Tests;
    }
}