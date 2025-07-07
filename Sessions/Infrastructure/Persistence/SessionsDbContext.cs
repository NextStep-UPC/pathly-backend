using Microsoft.EntityFrameworkCore;
using pathly_backend.Sessions.Domain.Entities;

namespace pathly_backend.Sessions.Infrastructure.Persistence;

public class SessionsDbContext : DbContext
{
    public DbSet<Session> Sessions => Set<Session>();

    public SessionsDbContext(DbContextOptions<SessionsDbContext> o) : base(o) {}

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Session>(cfg =>
        {
            cfg.ToTable("Sessions");
            cfg.HasKey(s => s.Id);
            cfg.Property(s => s.State).HasConversion<int>();
        });
    }
}