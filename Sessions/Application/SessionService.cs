using Microsoft.EntityFrameworkCore;
using pathly_backend.IAM.Application.Dto;
using pathly_backend.IAM.Domain.Repositories;
using pathly_backend.Sessions.Application.Dto;
using pathly_backend.Sessions.Domain.Entities;
using pathly_backend.Sessions.Domain.Repositories;
using pathly_backend.Shared.Common;

namespace pathly_backend.Sessions.Application;

public class SessionService
{
    private readonly ISessionRepository _repo;
    private readonly IUnitOfWork        _uow;
    private readonly IUserRepository    _users;

    public SessionService(ISessionRepository repo, IUnitOfWork uow, IUserRepository users)
    {
        _repo  = repo;
        _uow   = uow;
        _users = users;
    }

    public async Task<SessionResponseDto> BookAsync(Guid studentId, BookSessionDto dto)
    {
        var s = new Session(studentId, dto.StartsAtUtc, dto.EndsAtUtc);
        await _repo.AddAsync(s);
        await _uow.SaveChangesAsync();
        return ToDto(s);
    }

    public async Task<SessionResponseDto> AssignAsync(Guid sessionId, Guid psychologistId)
    {
        var s = await _repo.FindByIdAsync(sessionId)
                ?? throw new KeyNotFoundException("Session not found");

        s.AssignPsychologist(psychologistId);
        await _uow.SaveChangesAsync();
        return ToDto(s);
    }

    public async Task<SessionResponseDto> CancelAsync(Guid sessionId, Guid userId, CancelSessionDto dto)
    {
        var s = await _repo.FindByIdAsync(sessionId)
                ?? throw new KeyNotFoundException("Session not found");

        if (s.StudentId != userId)
            throw new UnauthorizedAccessException("Solo el estudiante puede cancelar la sesión");

        s.Cancel(dto.Reason);
        await _uow.SaveChangesAsync();
        return ToDto(s);
    }

    public async Task<SessionResponseDto> FinishAsync(Guid sessionId, Guid psychologistId)
    {
        var s = await _repo.FindByIdAsync(sessionId)
                ?? throw new KeyNotFoundException("Session not found");

        if (s.PsychologistId != psychologistId)
            throw new UnauthorizedAccessException("Solo el psicólogo asignado puede finalizar esta sesión.");

        s.Finish();
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

    public async Task<IEnumerable<UserInfoDto>> ListPsychologistsAsync()
    {
        var users = await _users.ListByRoleAsync("Psychologist");
        return users.Select(u =>
            new UserInfoDto(u.Id, u.Email.Value, u.Name.FirstName, u.Name.LastName, u.Role.ToString()));
    }

    private static SessionResponseDto ToDto(Session s) =>
        new(
            s.Id,
            s.StudentId,
            s.PsychologistId,
            s.StartsAtUtc,
            s.EndsAtUtc,
            s.State.ToString(),
            s.CancelReason
        );
}