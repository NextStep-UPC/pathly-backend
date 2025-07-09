using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using pathly_backend.VocationalTests.Domain.Entities;
using pathly_backend.VocationalTests.Domain.Repositories;
using pathly_backend.VocationalTests.Infrastructure.Persistence;

namespace pathly_backend.VocationalTests.Infrastructure.Persistence.Repositories
{
    public class EfStudentTestRepository : IStudentTestRepository
    {
        private readonly VocationalTestsDbContext _ctx;
        public EfStudentTestRepository(VocationalTestsDbContext ctx) => _ctx = ctx;

        public Task AddAsync(StudentTest st)
            => _ctx.StudentTests.AddAsync(st).AsTask();

        public Task<StudentTest?> FindByIdAsync(Guid id)
            => _ctx.StudentTests
                .Include(st => st.Answers)
                .FirstOrDefaultAsync(st => st.Id == id);

        public IQueryable<StudentTest> QueryBySessionGuid(Guid sessionId)
            => _ctx.StudentTests.Where(st => st.SessionId == sessionId);
    }
}