using System;

namespace pathly_backend.VocationalTests.Application.Dtos
{
    public record SubmitAnswerDto(Guid QuestionId, Guid? OptionId, string? ResponseText);
}