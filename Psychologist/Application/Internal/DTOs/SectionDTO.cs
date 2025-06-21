using pathly_backend.Psychologist.Domain.Model.Entities;

namespace pathly_backend.Psychologist.Application.Internal.DTOs;

public class SectionDTo
{
    public Guid Id { get; set; }
    public string StudentName { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public string Mode { get; set; }
    
}

 public class CreateSectionDTo
{
  
    public string StudentName { get; set; }
        
   
    public string Title { get; set; }
        

    public string? Description { get; set; }
        

    public DateTime Date { get; set; }
    
    
    public string Status { get; set; } 
    
    public string Mode { get; set; } 
        
    public string? Notes { get; set; }
}
 public class UpdateSectionDTo
 {
    
     public string Title { get; set; }
     
     public string? Description { get; set; }
        

     public DateTime Date { get; set; }
        
     
     public string Status { get; set; }
     
     public string Mode { get; set; }
        
     public string? Notes { get; set; }
 }
  public class PsychologistDashboardDto
 {
     public List<StudentDTo> Students { get; set; } = new();
     public List<SectionDTo> Sections { get; set; } = new();
     public int TotalStudents { get; set; }
     public int TotalSections { get; set; }
     public int PendingSections { get; set; }
     public int CompletedSections { get; set; }
 }