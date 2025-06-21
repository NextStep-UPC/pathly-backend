using Microsoft.EntityFrameworkCore;
using pathly_backend.CareerTest.Domain.Model.Entities;

namespace pathly_backend.CareerTest.Infrastructure.Persistence.Context
{
    public class CareerTestDbContext : DbContext
    {
        public CareerTestDbContext(DbContextOptions<CareerTestDbContext> options) : base(options) { }

        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        // Add other DbSets as needed
    }
}