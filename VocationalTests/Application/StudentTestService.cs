using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using pathly_backend.VocationalTests.Application.Dtos;
using pathly_backend.VocationalTests.Application.Interfaces;
using pathly_backend.VocationalTests.Domain.Entities;
using pathly_backend.VocationalTests.Domain.Repositories;

namespace pathly_backend.VocationalTests.Application
{
    public class StudentTestService : IStudentTestService
    {
        private readonly IStudentTestRepository _stRepo;
        private readonly ITestRepository _testRepo;
        private readonly IVocationalTestsUnitOfWork _uow;

        public StudentTestService(
            IStudentTestRepository stRepo,
            ITestRepository testRepo,
            IVocationalTestsUnitOfWork uow)
        {
            _stRepo = stRepo;
            _testRepo = testRepo;
            _uow = uow;
        }

        public async Task<StudentTestStartDto> StartAsync(Guid sessionId, Guid testId, Guid studentId)
        {
            var st = new StudentTest(testId, studentId, sessionId);
            await _stRepo.AddAsync(st);
            await _uow.SaveChangesAsync();
            return new StudentTestStartDto(st.Id, st.StartedAtUtc);
        }

        public async Task SubmitAnswerAsync(Guid studentTestId, SubmitAnswerDto dto)
        {
            var st = await _stRepo.FindByIdAsync(studentTestId)
                     ?? throw new KeyNotFoundException("Test de estudiante no encontrado.");
            var ans = new Answer(studentTestId, dto.QuestionId, dto.OptionId, dto.ResponseText);
            st.AddAnswer(ans);
            await _uow.SaveChangesAsync();
        }

        public async Task CompleteAsync(Guid studentTestId)
        {
            var st = await _stRepo.FindByIdAsync(studentTestId)
                     ?? throw new KeyNotFoundException("Test de estudiante no encontrado.");
            st.Complete();
            await _uow.SaveChangesAsync();
        }

        public async Task<StudentTestResultDto?> GetResultsAsync(Guid studentTestId)
        {
            var st = await _stRepo.QueryBySessionGuid(Guid.Empty)
                     .Include(x => x.Answers)
                     .FirstOrDefaultAsync(x => x.Id == studentTestId);
            if (st == null) return null;
            var test = await _testRepo.FindByIdAsync(st.TestId);
            var details = st.Answers.Select(a => new AnswerDetailDto(
                a.QuestionId,
                test!.Questions.First(q => q.Id == a.QuestionId).Text,
                a.OptionId.HasValue ? new[]{ test.Questions.First(q => q.Id == a.QuestionId)
                                      .Options.Where(o => o.Id == a.OptionId).Select(o => o.Text).First() } : Array.Empty<string>(),
                a.ResponseText,
                test.Questions.First(q => q.Id == a.QuestionId)
                    .Options.Where(o => o.IsCorrect).Select(o => o.Text)
            ));
            return new StudentTestResultDto(st.Id, st.StartedAtUtc, st.CompletedAtUtc, details);
        }
    }
}