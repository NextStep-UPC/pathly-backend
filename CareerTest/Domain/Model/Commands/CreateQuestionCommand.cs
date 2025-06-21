using MediatR;
using pathly_backend.CareerTest.Application.Internal.DTOs;
using System.Collections.Generic;

namespace pathly_backend.CareerTest.Domain.Model.Commands;

public class CreateQuestionCommand : IRequest<int>
{
    public string Text { get; set; } = null!;
    public List<QuestionOptionDto> Options { get; set; } = new();
}