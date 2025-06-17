using Microsoft.EntityFrameworkCore;
using pathly_backend.IAM.Domain.Model.Entities;

namespace pathly_backend.IAM.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        // Puedes agregar más DbSets en el futuro: Roles, Sessions, etc.
    }
}