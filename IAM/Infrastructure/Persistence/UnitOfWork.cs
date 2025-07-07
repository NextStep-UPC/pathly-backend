using pathly_backend.Shared.Common;

namespace pathly_backend.IAM.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly IamDbContext _ctx;
    public UnitOfWork(IamDbContext ctx) => _ctx = ctx;

    public Task<int> SaveChangesAsync(CancellationToken cancellation = default)
        => _ctx.SaveChangesAsync(cancellation);
}