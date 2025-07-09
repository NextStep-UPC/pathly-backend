using System.Threading.Tasks;
using pathly_backend.VocationalTests.Application.Interfaces;
using pathly_backend.VocationalTests.Infrastructure.Persistence;

namespace pathly_backend.VocationalTests.Infrastructure.Persistence
{
    public class VocationalTestsUnitOfWork : IVocationalTestsUnitOfWork
    {
        private readonly VocationalTestsDbContext _ctx;
        public VocationalTestsUnitOfWork(VocationalTestsDbContext ctx) => _ctx = ctx;

        public Task SaveChangesAsync() => _ctx.SaveChangesAsync();
    }
}