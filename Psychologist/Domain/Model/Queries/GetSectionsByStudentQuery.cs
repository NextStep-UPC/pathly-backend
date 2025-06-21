using pathly_backend.Psychologist.Domain.Model.Entities;

namespace pathly_backend.Psychologist.Domain.Model.Queries;
using MediatR;

public class GetSectionsByStudentQuery : IRequest<List<Section>>
{
    public Guid StudentId { get; set; }
        
    public GetSectionsByStudentQuery(Guid studentId)
    {
        StudentId = studentId;
    }
}