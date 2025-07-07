using Microsoft.EntityFrameworkCore;
using pathly_backend.Sessions.Application.Dto;
using pathly_backend.Sessions.Domain.Entities;
using pathly_backend.Sessions.Domain.Enums;
using pathly_backend.Sessions.Domain.Repositories;
using pathly_backend.Shared.Common;

namespace pathly_backend.Sessions.Application;

public class SessionService
{
    private readonly ISessionRepository _repo;
    private readonly IUnitOfWork        _uow;

    public SessionService(ISessionRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow  = uow;
    }

    public async Task<SessionResponseDto> BookAsync(Guid studentId, BookSessionDto dto)
    {
        var s = new Session(studentId, dto.PsychologistId,
            dto.StartsAtUtc, dto.EndsAtUtc);

        await _repo.AddAsync(s);
        await _uow.SaveChangesAsync();
        return ToDto(s);
    }

    public async Task<SessionResponseDto> ConfirmAsync(Guid id)
    {
        var s = await _repo.FindByIdAsync(id)
                ?? throw new KeyNotFoundException("Session not found");

        s.Confirm();
        await _uow.SaveChangesAsync();
        return ToDto(s);
    }

    public async Task<IEnumerable<SessionResponseDto>> ListMineAsync(Guid userId)
    {
        return await _repo.QueryMine(userId)
            .OrderBy(s => s.StartsAtUtc)
            .Select(s => ToDto(s))
            .ToListAsync();
    }

    private static SessionResponseDto ToDto(Session s) =>
        new(s.Id, s.StudentId, s.PsychologistId,
            s.StartsAtUtc, s.EndsAtUtc, s.State.ToString());
}