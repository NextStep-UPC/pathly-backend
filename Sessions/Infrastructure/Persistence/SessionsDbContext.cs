using Microsoft.EntityFrameworkCore;
using pathly_backend.Sessions.Domain.Entities;

namespace pathly_backend.Sessions.Infrastructure.Persistence;

public class SessionsDbContext : DbContext
{
    public DbSet<Session>     Sessions     => Set<Session>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    
    public DbSet<Feedback> Feedbacks => Set<Feedback>();

    public SessionsDbContext(DbContextOptions<SessionsDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Session>(cfg =>
        {
            cfg.ToTable("Sessions");
            cfg.Property(s => s.StudentId).IsRequired();
            cfg.Property(s => s.PsychologistId).IsRequired(false);
            cfg.Property(s => s.StartsAtUtc).IsRequired();
            cfg.Property(s => s.EndsAtUtc).IsRequired(false);
            cfg.Property(s => s.State).HasConversion<int>().IsRequired();
            cfg.Property(s => s.CancelReason).HasColumnType("text").IsRequired(false);
        });
        
        b.Entity<ChatMessage>(cfg =>
        {
            cfg.ToTable("ChatMessages");
            cfg.HasKey(m => m.Id);
            cfg.Property(m => m.SessionId).IsRequired();
            cfg.Property(m => m.SenderId).IsRequired();
            cfg.Property(m => m.Content).HasColumnType("text").IsRequired();
            cfg.Property(m => m.SentAtUtc).IsRequired();
        });
        
        b.Entity<Feedback>(cfg =>
        {
            cfg.ToTable("Feedbacks");
            cfg.HasKey(f => f.Id);
            cfg.Property(f => f.SessionId).IsRequired();
            cfg.Property(f => f.StudentId).IsRequired();
            cfg.Property(f => f.Rating).IsRequired();
            cfg.Property(f => f.Comment)
                .HasColumnType("text")
                .IsRequired(false);
            cfg.Property(f => f.CreatedAtUtc).IsRequired();
        });
    }
}