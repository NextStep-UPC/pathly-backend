using Microsoft.EntityFrameworkCore;
using pathly_backend.IAM.Domain.Entities;
using pathly_backend.IAM.Domain.Enums;
using pathly_backend.IAM.Domain.Repositories;
using pathly_backend.IAM.Infrastructure.Persistence;

namespace pathly_backend.IAM.Infrastructure.Persistence.Repositories;

public class EfUserRepository : IUserRepository
{
    private readonly IamDbContext _ctx;
    public EfUserRepository(IamDbContext ctx) => _ctx = ctx;

    public async Task AddAsync(User user)           // ←  async versión
        => await _ctx.Users.AddAsync(user);

    public Task<User?> FindByEmailAsync(string email)
        => _ctx.Users.FirstOrDefaultAsync(u => u.Email.Value == email);

    public Task<bool> ExistsAsync(string email)
        => _ctx.Users.AnyAsync(u => u.Email.Value == email);

    public Task<User?> FindByIdAsync(Guid id)
        => _ctx.Users.FindAsync(id).AsTask();
    public Task<IEnumerable<User>> ListByRoleAsync(string role)
    {
        var parsedRole = Enum.Parse<UserRole>(role, ignoreCase: true);

        return Task.FromResult<IEnumerable<User>>(
            _ctx.Users.Where(u => u.Role == parsedRole).AsEnumerable()
        );
    }
}