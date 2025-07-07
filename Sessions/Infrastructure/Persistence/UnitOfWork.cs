using pathly_backend.Shared.Common;

namespace pathly_backend.Sessions.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly SessionsDbContext _ctx;
    public UnitOfWork(SessionsDbContext ctx) => _ctx = ctx;
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _ctx.SaveChangesAsync(ct);
}