using MediatR;

namespace pathly_backend.CareerTest.Domain.Model.Commands;

public class DeleteQuestionCommand : IRequest<bool>
{
    public int Id { get; set; }
}