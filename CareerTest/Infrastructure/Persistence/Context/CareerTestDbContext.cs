using Microsoft.EntityFrameworkCore;
using pathly_backend.CareerTest.Domain.Model.Entities;

namespace pathly_backend.CareerTest.Infrastructure.Persistence.Context
{
    public class CareerTestDbContext : DbContext
    {
        public CareerTestDbContext(DbContextOptions<CareerTestDbContext> options) : base(options) { }

        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(q => q.QuestionId);
                entity.Property(q => q.Text).IsRequired();

                entity.HasMany(q => q.Options)
                    .WithOne(o => o.Question)
                    .HasForeignKey(o => o.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<QuestionOption>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Text).IsRequired();
            });
        }
    }
}