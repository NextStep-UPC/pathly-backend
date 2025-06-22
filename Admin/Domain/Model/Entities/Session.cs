using System.ComponentModel.DataAnnotations;

namespace Admin.Domain.Model.Entities
{
    public class Session
    {
        [Key]
        public int Id { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Psychologist { get; set; }
        public string User { get; set; }
        public string Status { get; set; }
    }
}
