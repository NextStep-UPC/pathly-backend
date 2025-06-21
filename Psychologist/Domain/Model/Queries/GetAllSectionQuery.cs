using pathly_backend.Psychologist.Application.Internal.DTOs;
namespace pathly_backend.Psychologist.Domain.Model.Queries;
using MediatR;

public class GetAllSectionQuery: IRequest<SectionResponseDto> 
{ 
}