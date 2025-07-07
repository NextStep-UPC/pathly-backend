using Microsoft.EntityFrameworkCore;
using pathly_backend.Sessions.Domain.Entities;
using pathly_backend.Sessions.Domain.Repositories;
using pathly_backend.Sessions.Infrastructure.Persistence;

namespace pathly_backend.Sessions.Infrastructure.Persistence.Repositories;

public class EfSessionRepository : ISessionRepository
{
    private readonly SessionsDbContext _ctx;
    public EfSessionRepository(SessionsDbContext ctx) => _ctx = ctx;

    public Task AddAsync(Session s) => _ctx.Sessions.AddAsync(s).AsTask();

    public Task<Session?> FindByIdAsync(Guid id)
        => _ctx.Sessions.FirstOrDefaultAsync(s => s.Id == id);

    public IQueryable<Session> QueryMine(Guid userId)
        => _ctx.Sessions.Where(s => s.StudentId == userId || s.PsychologistId == userId);
}