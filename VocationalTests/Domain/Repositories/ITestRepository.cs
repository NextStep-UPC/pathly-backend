using System;
using System.Linq;
using pathly_backend.VocationalTests.Domain.Entities;

namespace pathly_backend.VocationalTests.Domain.Repositories
{
    public interface ITestRepository
    {
        Task AddAsync(Test test);
        Task<Test?> FindByIdAsync(Guid id);
        IQueryable<Test> QueryAll();
    }
}