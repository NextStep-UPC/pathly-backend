using pathly_backend.Sessions.Domain.Entities;

namespace pathly_backend.Sessions.Domain.Repositories;

public interface ISessionRepository
{
    Task AddAsync(Session s);
    Task<Session?> FindByIdAsync(Guid id);
    IQueryable<Session> QueryMine(Guid userId);
}