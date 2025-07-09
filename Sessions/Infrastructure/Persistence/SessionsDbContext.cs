using Microsoft.EntityFrameworkCore;
using pathly_backend.Sessions.Domain.Entities;

namespace pathly_backend.Sessions.Infrastructure.Persistence;

public class SessionsDbContext : DbContext
{
    public DbSet<Session>     Sessions     => Set<Session>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();
    public SessionsDbContext(DbContextOptions<SessionsDbContext> options) : base(options) { }
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Report> Reports => Set<Report>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Session>(cfg =>
        {
            cfg.ToTable("Sessions");
            cfg.Property(s => s.StudentId).IsRequired();
            cfg.Property(s => s.PsychologistId).IsRequired(false);
            cfg.Property(s => s.StartsAtUtc).IsRequired();
            cfg.Property(s => s.AssignedAtUtc).IsRequired(false);
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
        
        b.Entity<Notification>(cfg =>
        {
            cfg.ToTable("Notifications");
            cfg.HasKey(n => n.Id);

            cfg.Property(n => n.UserId)
                .IsRequired();

            cfg.Property(n => n.Title)
                .HasMaxLength(200)
                .IsRequired();

            cfg.Property(n => n.Message)
                .HasColumnType("text")
                .IsRequired();

            cfg.Property(n => n.IsRead)
                .IsRequired();

            cfg.Property(n => n.CreatedAtUtc)
                .IsRequired();
        });
        
        b.Entity<Report>(cfg =>
        {
            cfg.ToTable("Reports");
            cfg.HasKey(r => r.Id);
            cfg.Property(r => r.SessionId).IsRequired();
            cfg.Property(r => r.PsychologistId).IsRequired();
            cfg.Property(r => r.ReportedUserId).IsRequired();
            cfg.Property(r => r.Reason)
                .HasColumnType("text").IsRequired();
            cfg.Property(r => r.CreatedAtUtc).IsRequired();
            cfg.Property(r => r.State)
                .HasConversion<int>().IsRequired();
            cfg.Property(r => r.ResolvedByAdminId).IsRequired(false);
            cfg.Property(r => r.AdminComment)
                .HasColumnType("text").IsRequired(false);
            cfg.Property(r => r.ResolvedAtUtc).IsRequired(false);
        });
    }
}