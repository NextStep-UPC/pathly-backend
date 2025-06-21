using MediatR;
using System.ComponentModel.DataAnnotations;

namespace pathly_backend.Psychologist.Domain.Model.Commands;

public class DeleteSectionCommand : IRequest<bool>
{
    [Required]
    public Guid Id { get; set; }
}