using Microsoft.EntityFrameworkCore;
using pathly_backend.Profile.Domain.Entities;

namespace pathly_backend.Profile.Infrastructure.Persistence;

public class ProfileDbContext : DbContext
{
    public DbSet<Domain.Entities.Profile> Profiles => Set<Domain.Entities.Profile>();

    public ProfileDbContext(DbContextOptions<ProfileDbContext> opts) : base(opts) { }

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Profile.Domain.Entities.Profile>(cfg =>
        {
            cfg.ToTable("Profiles");
            cfg.HasKey(p => p.UserId);

            cfg.Property(p => p.FirstName).HasMaxLength(100);
            cfg.Property(p => p.LastName).HasMaxLength(100);
            cfg.Property(p => p.Bio).HasMaxLength(500);
            cfg.Property(p => p.AvatarUrl).HasMaxLength(300);
            cfg.Property(p => p.PhoneNumber).HasMaxLength(15);
            cfg.Property(p => p.BirthDate);
        });
    }
}