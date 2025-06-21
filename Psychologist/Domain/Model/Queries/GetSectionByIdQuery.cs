using pathly_backend.Psychologist.Domain.Model.Entities;

namespace pathly_backend.Psychologist.Domain.Model.Queries;
using MediatR;

public class GetSectionByIdQuery: IRequest<Section>
{   
    public Guid Id { get; set; }
        
    public GetSectionByIdQuery(Guid id)
    {
        Id = id;
    }
}