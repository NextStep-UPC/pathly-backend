using pathly_backend.IAM.Domain.Entities;

namespace pathly_backend.IAM.Domain.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user);                    // ←  async
    Task<User?> FindByEmailAsync(string email);
    Task<bool>  ExistsAsync(string email);
    Task<User?> FindByIdAsync(Guid id);
}