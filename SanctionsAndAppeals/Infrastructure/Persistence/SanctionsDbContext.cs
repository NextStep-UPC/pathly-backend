using Microsoft.EntityFrameworkCore;
using pathly_backend.SanctionsAndAppeals.Domain.Entities;

namespace pathly_backend.SanctionsAndAppeals.Infrastructure.Persistence
{
    public class SanctionsDbContext : DbContext
    {
        public DbSet<Sanction> Sanctions => Set<Sanction>();
        public DbSet<Appeal>   Appeals   => Set<Appeal>();

        public SanctionsDbContext(DbContextOptions<SanctionsDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<Sanction>(cfg =>
            {
                cfg.ToTable("Sanctions");
                cfg.HasKey(s => s.Id);
                cfg.Property(s => s.UserId).IsRequired();
                cfg.Property(s => s.AdminId).IsRequired();
                cfg.Property(s => s.Reason).HasColumnType("text").IsRequired();
                cfg.Property(s => s.StartAtUtc).IsRequired();
                cfg.Property(s => s.EndAtUtc).IsRequired(false);
            });
            b.Entity<Appeal>(cfg =>
            {
                cfg.ToTable("Appeals");
                cfg.HasKey(a => a.Id);
                cfg.Property(a => a.SanctionId).IsRequired();
                cfg.Property(a => a.UserId).IsRequired();
                cfg.Property(a => a.Reason).HasColumnType("text").IsRequired();
                cfg.Property(a => a.CreatedAtUtc).IsRequired();
                cfg.Property(a => a.State).HasConversion<int>().IsRequired();
                cfg.Property(a => a.ResolvedById).IsRequired(false);
                cfg.Property(a => a.DecisionComment).HasColumnType("text").IsRequired(false);
                cfg.Property(a => a.ResolvedAtUtc).IsRequired(false);
            });
        }
    }
}