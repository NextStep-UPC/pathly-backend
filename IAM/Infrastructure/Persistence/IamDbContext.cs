using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using pathly_backend.IAM.Domain;
using pathly_backend.IAM.Domain.Entities;
using pathly_backend.IAM.Domain.Events;

namespace pathly_backend.IAM.Infrastructure.Persistence;

public class IamDbContext : DbContext
{
    public DbSet<User> Users           => Set<User>();
    public DbSet<OutboxMessage> Outbox => Set<OutboxMessage>();

    public IamDbContext(DbContextOptions<IamDbContext> opts) : base(opts) { }

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<User>(cfg =>
        {
            cfg.ToTable("Users");
            cfg.HasKey(u => u.Id);
            cfg.Property(u => u.Id).ValueGeneratedNever();

            cfg.OwnsOne(u => u.Email,
                e => e.Property(p => p.Value).HasColumnName("Email").IsRequired());

            cfg.OwnsOne(u => u.PasswordHash,
                e => e.Property(p => p.Value).HasColumnName("PasswordHash").IsRequired());

            cfg.OwnsOne(u => u.Name, n =>
            {
                n.Property(p => p.FirstName).HasColumnName("FirstName");
                n.Property(p => p.LastName).HasColumnName("LastName");
            });

            cfg.Property(u => u.Role)
               .HasConversion<int>()
               .IsRequired();
        });

        b.Entity<OutboxMessage>(cfg =>
        {
            cfg.ToTable("OutboxMessages");
            cfg.HasKey(o => o.Id);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var domainEvents = ChangeTracker.Entries<IAggregateRoot>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        foreach (var @event in domainEvents)
        {
            Outbox.Add(new OutboxMessage
            {
                Id            = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                Type          = @event.GetType().FullName!,
                Content       = JsonSerializer.Serialize(@event, @event.GetType())
            });
        }

        var result = await base.SaveChangesAsync(ct);

        foreach (var aggregate in ChangeTracker.Entries<IAggregateRoot>())
            aggregate.Entity.ClearDomainEvents();

        return result;
    }
}