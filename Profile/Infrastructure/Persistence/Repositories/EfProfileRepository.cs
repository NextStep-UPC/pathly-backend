using Microsoft.EntityFrameworkCore;
using pathly_backend.Profile.Domain.Entities;
using pathly_backend.Profile.Domain.Repositories;
using pathly_backend.Profile.Infrastructure.Persistence;

namespace pathly_backend.Profile.Infrastructure.Repositories;

public class EfProfileRepository : IProfileRepository
{
    private readonly ProfileDbContext _ctx;
    public EfProfileRepository(ProfileDbContext ctx) => _ctx = ctx;

    public Task<Domain.Entities.Profile?> FindByIdAsync(Guid userId)
        => _ctx.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);

    public Task AddAsync(Domain.Entities.Profile profile)
        => _ctx.Profiles.AddAsync(profile).AsTask();
}