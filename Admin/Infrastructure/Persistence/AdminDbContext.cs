using Microsoft.EntityFrameworkCore;
using Admin.Domain.Model.Entities;

namespace Admin.Infrastructure.Persistence
{
    public class AdminDbContext : DbContext
    {
        public AdminDbContext(DbContextOptions<AdminDbContext> options)
            : base(options) { }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
    }
}
