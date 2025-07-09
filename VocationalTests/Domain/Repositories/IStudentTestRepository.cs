using System;
using System.Linq;
using pathly_backend.VocationalTests.Domain.Entities;

namespace pathly_backend.VocationalTests.Domain.Repositories
{
    public interface IStudentTestRepository
    {
        Task AddAsync(StudentTest st);
        Task<StudentTest?> FindByIdAsync(Guid id);
        IQueryable<StudentTest> QueryBySessionGuid(Guid sessionId);
    }
}