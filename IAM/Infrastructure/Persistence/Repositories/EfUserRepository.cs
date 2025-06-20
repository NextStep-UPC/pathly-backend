using IAM.Domain.Entities;
using IAM.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using IAM.Infrastructure.Persistence;

namespace IAM.Infrastructure.Persistence.Repositories;

public class EfUserRepository : IUserRepository
{
    private readonly IamDbContext _db;
    public EfUserRepository(IamDbContext db) => _db = db;

    public Task<User?> GetByEmailAsync(string email) =>
        _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email.ToLower());

    public async Task AddAsync(User u)
    {
        _db.Users.Add(u);
        await _db.SaveChangesAsync();
    }

    public Task<bool> EmailExistsAsync(string email) =>
        _db.Users.AnyAsync(u => u.Email == email.ToLower());
}