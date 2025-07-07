using pathly_backend.Profile.Domain.Entities;

namespace pathly_backend.Profile.Domain.Repositories;

public interface IProfileRepository
{
    Task<Entities.Profile?> FindByIdAsync(Guid userId);
    Task AddAsync(Entities.Profile profile);
}