using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using pathly_backend.VocationalTests.Application.Dtos;
using pathly_backend.VocationalTests.Application.Interfaces;
using pathly_backend.VocationalTests.Domain.Repositories;

namespace pathly_backend.VocationalTests.Application
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _repo;
        public TestService(ITestRepository repo) => _repo = repo;

        public async Task<IEnumerable<TestDto>> ListAllAsync()
        {
            var tests = await _repo.QueryAll()
                .Include(t => t.Questions).ThenInclude(q => q.Options)
                .ToListAsync();
            return tests.Select(t => 
                new TestDto(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.Questions.Select(q => 
                        new QuestionDto(q.Id, q.Text, q.Type, 
                            q.Options.Select(o => new OptionDto(o.Id, o.Text))))));
        }

        public async Task<TestDto?> GetByIdAsync(Guid testId)
        {
            var t = await _repo.QueryAll()
                .Include(tst => tst.Questions).ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(t => t.Id == testId);
            if (t == null) return null;
            return new TestDto(
                t.Id, t.Title, t.Description,
                t.Questions.Select(q => new QuestionDto(q.Id, q.Text, q.Type, q.Options.Select(o => new OptionDto(o.Id, o.Text)))));
        }
    }
}