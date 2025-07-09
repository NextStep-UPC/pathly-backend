using System;
using System.Collections.Generic;

namespace pathly_backend.VocationalTests.Application.Dtos
{
    public record StudentTestResultDto(
        Guid StudentTestId,
        DateTime StartedAtUtc,
        DateTime? CompletedAtUtc,
        IEnumerable<AnswerDetailDto> Answers
    );
    public record AnswerDetailDto(Guid QuestionId, string QuestionText, IEnumerable<string> SelectedOptions, string? ResponseText, IEnumerable<string> CorrectOptions);
}