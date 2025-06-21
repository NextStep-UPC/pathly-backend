using Microsoft.EntityFrameworkCore;
using pathly_backend.IAM.Domain.Model.Entities;
using pathly_backend.Psychologist.Domain.Model.Entities;

namespace pathly_backend.IAM.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Section> Sections { get; set; }
        
    }


}
 