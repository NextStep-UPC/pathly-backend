using Microsoft.EntityFrameworkCore;
using pathly_backend.VocationalTests.Domain.Entities;

namespace pathly_backend.VocationalTests.Infrastructure.Persistence
{
    public class VocationalTestsDbContext : DbContext
    {
        public DbSet<Test> Tests => Set<Test>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Option> Options => Set<Option>();
        public DbSet<StudentTest> StudentTests => Set<StudentTest>();
        public DbSet<Answer> Answers => Set<Answer>();

        public VocationalTestsDbContext(DbContextOptions<VocationalTestsDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<Test>(cfg =>
            {
                cfg.ToTable("Tests");
                cfg.HasKey(t => t.Id);
                cfg.Property(t => t.Title).IsRequired();
                cfg.Property(t => t.Description).IsRequired(false);
                cfg.HasMany(t => t.Questions).WithOne().HasForeignKey(q => q.TestId);
            });

            b.Entity<Question>(cfg =>
            {
                cfg.ToTable("Questions");
                cfg.HasKey(q => q.Id);
                cfg.Property(q => q.Text).IsRequired();
                cfg.Property(q => q.Type).HasConversion<int>();
                cfg.HasMany(q => q.Options).WithOne().HasForeignKey(o => o.QuestionId);
            });

            b.Entity<Option>(cfg =>
            {
                cfg.ToTable("Options");
                cfg.HasKey(o => o.Id);
                cfg.Property(o => o.Text).IsRequired();
                cfg.Property(o => o.IsCorrect).IsRequired();
            });

            b.Entity<StudentTest>(cfg =>
            {
                cfg.ToTable("StudentTests");
                cfg.HasKey(st => st.Id);
                cfg.Property(st => st.StartedAtUtc).IsRequired();
                cfg.Property(st => st.CompletedAtUtc).IsRequired(false);
                cfg.HasMany(st => st.Answers).WithOne().HasForeignKey(a => a.StudentTestId);
            });

            b.Entity<Answer>(cfg =>
            {
                cfg.ToTable("Answers");
                cfg.HasKey(a => a.Id);
                cfg.Property(a => a.ResponseText).IsRequired(false);
            });
        }
    }
}