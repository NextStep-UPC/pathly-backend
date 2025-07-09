using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using pathly_backend.VocationalTests.Application.Dtos;

namespace pathly_backend.VocationalTests.Application.Interfaces
{
    public interface ITestService
    {
        Task<IEnumerable<TestDto>> ListAllAsync();
        Task<TestDto?> GetByIdAsync(Guid testId);
    }
}