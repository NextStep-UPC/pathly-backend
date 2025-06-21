namespace pathly_backend.Psychologist.Domain.Model.Entities;
 using System.ComponentModel.DataAnnotations;
 using System.ComponentModel.DataAnnotations.Schema;

public class Section
{ 
    
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string StudentName { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        
        [StringLength(1000)]
        public string Description { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Status { get; set; } // Confirmada, Pendiente, Cancelada, Completada
        
        [Required]
        [StringLength(50)]
        public string Mode { get; set; } // Videollamada, Presencial, Telefónica
        
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }

        // Establece el campo StudentId como clave foránea
        public Guid StudentId { get; set; }
        
        public enum SectionStatus
        {
            Pendiente,
            Confirmada,
            Completada,
            Cancelada
        }
        public enum SectionMode
        {
            Presencial,
            Videollamada,
            Telefónica
        }
}
    

