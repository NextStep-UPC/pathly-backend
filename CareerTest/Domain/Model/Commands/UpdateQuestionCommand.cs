using MediatR;
using pathly_backend.CareerTest.Application.Internal.DTOs;
using System.Collections.Generic;

namespace pathly_backend.CareerTest.Domain.Model.Commands;

public class UpdateQuestionCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Text { get; set; } = null!;
    public List<QuestionOptionDto> Options { get; set; } = new();
}