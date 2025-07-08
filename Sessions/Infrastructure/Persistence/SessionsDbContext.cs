using Microsoft.EntityFrameworkCore;
using pathly_backend.Sessions.Domain.Entities;

namespace pathly_backend.Sessions.Infrastructure.Persistence;

public class SessionsDbContext : DbContext
{
    public DbSet<Session> Sessions => Set<Session>();

    public SessionsDbContext(DbContextOptions<SessionsDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Session>(cfg =>
        {
            cfg.ToTable("Sessions");

            cfg.Property(s => s.StudentId)
                .IsRequired();

            cfg.Property(s => s.PsychologistId)
                .IsRequired(false);

            cfg.Property(s => s.StartsAtUtc)
                .IsRequired();

            // ahora optional
            cfg.Property(s => s.EndsAtUtc)
                .IsRequired(false);

            cfg.Property(s => s.State)
                .HasConversion<int>()
                .IsRequired();

            cfg.Property(s => s.CancelReason)
                .HasColumnType("text")
                .IsRequired(false);
        });
    }
}