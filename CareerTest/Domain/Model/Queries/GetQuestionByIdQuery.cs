using MediatR;
using pathly_backend.CareerTest.Application.Internal.DTOs;

namespace pathly_backend.CareerTest.Domain.Model.Queries;

public class GetQuestionByIdQuery : IRequest<QuestionDto?>
{
    public int Id { get; set; }
    public GetQuestionByIdQuery(int id) => Id = id;
}