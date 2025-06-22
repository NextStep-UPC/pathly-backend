using System.ComponentModel.DataAnnotations;

namespace Admin.Domain.Model.Entities
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        public string User { get; set; }
        public string Comment { get; set; }
        public string Date { get; set; }
    }
}
