using pathly_backend.Shared.Common;

namespace pathly_backend.Profile.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork, IProfileUnitOfWork
{
    private readonly ProfileDbContext _ctx;
    public UnitOfWork(ProfileDbContext ctx) => _ctx = ctx;

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _ctx.SaveChangesAsync(ct);
}