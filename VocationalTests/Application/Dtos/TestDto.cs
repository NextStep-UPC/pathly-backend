using System;
using System.Collections.Generic;
using pathly_backend.VocationalTests.Domain.Enums;

namespace pathly_backend.VocationalTests.Application.Dtos
{
    public record TestDto(Guid Id, string Title, string? Description, IEnumerable<QuestionDto> Questions);
    public record QuestionDto(Guid Id, string Text, QuestionType Type, IEnumerable<OptionDto> Options);
    public record OptionDto(Guid Id, string Text);
}