using System;
using System.Threading.Tasks;
using pathly_backend.VocationalTests.Application.Dtos;

namespace pathly_backend.VocationalTests.Application.Interfaces
{
    public interface IStudentTestService
    {
        Task<StudentTestStartDto> StartAsync(Guid sessionId, Guid testId, Guid studentId);
        Task SubmitAnswerAsync(Guid studentTestId, SubmitAnswerDto dto);
        Task CompleteAsync(Guid studentTestId);
        Task<StudentTestResultDto?> GetResultsAsync(Guid studentTestId);
    }
}