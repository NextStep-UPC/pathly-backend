using pathly_backend.Psychologist.Application.Internal.DTOs;
using MediatR;
namespace pathly_backend.Psychologist.Domain.Model.Commands;
using System.ComponentModel.DataAnnotations;
public class CreateSectionCommand : IRequest<Guid>
{
                public string StudentName { get; set; }
                
                [Required]
                public Guid StudentId { get; set; }
                
                [Required]
                [StringLength(200)]
                public string Title { get; set; }
        
                [StringLength(1000)]
                public string Description { get; set; }
        
                [Required]
                public DateTime Date { get; set; }
        
                [Required]
                [StringLength(50)]
                public string Status { get; set; }
        
                [Required]
                [StringLength(50)]
                public string Mode { get; set; }
        
}