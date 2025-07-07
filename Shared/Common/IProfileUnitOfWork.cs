namespace pathly_backend.Shared.Common;

public interface IProfileUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}