using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace pathly_backend.Psychologist.Domain.Model.Entities;
  public class Student
{
    public Guid Id { get; set; } = Guid.NewGuid();
        
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
        
    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
    
}