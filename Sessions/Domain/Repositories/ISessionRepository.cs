using System;
using System.Linq;
using pathly_backend.Sessions.Domain.Entities;
using pathly_backend.Sessions.Domain.Enums;

namespace pathly_backend.Sessions.Domain.Repositories;

public interface ISessionRepository
{
    Task AddAsync(Session s);
    Task<Session?> FindByIdAsync(Guid id);

    // existing:
    IQueryable<Session> QueryMine(Guid userId);

    // nuevos:
    IQueryable<Session> QueryAll();
    IQueryable<Session> QueryByState(SessionState state);
}