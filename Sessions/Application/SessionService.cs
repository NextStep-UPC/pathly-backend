using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using pathly_backend.IAM.Application.Dto;
using pathly_backend.IAM.Domain.Enums;
using pathly_backend.IAM.Domain.Repositories;
using pathly_backend.Sessions.Application.Dto;
using pathly_backend.Sessions.Application.Dtos;
using pathly_backend.Sessions.Application.Interfaces;
using pathly_backend.Sessions.Domain.Entities;
using pathly_backend.Sessions.Domain.Enums;
using pathly_backend.Sessions.Domain.Repositories;
using pathly_backend.Sessions.Infrastructure.Persistence;

namespace pathly_backend.Sessions.Application
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _repo;
        private readonly ISessionsUnitOfWork _uow;
        private readonly IUserRepository _users;
        private readonly INotificationService _notifSvc;

        public SessionService(
            ISessionRepository repo,
            ISessionsUnitOfWork uow,
            IUserRepository users,
            INotificationService notifSvc)
        {
            _repo = repo;
            _uow = uow;
            _users = users;
            _notifSvc = notifSvc;
        }

        public async Task<SessionResponseDto> BookAsync(Guid studentId, BookSessionDto dto)
        {
            bool hasActive = await _repo.QueryMine(studentId)
                .Where(s => s.StudentId == studentId && 
                            (s.State == SessionState.Pending || s.State == SessionState.Confirmed))
                .AnyAsync();
            if (hasActive)
                throw new InvalidOperationException("Ya tienes una sesión activa pendiente o confirmada.");

            var session = new Session(studentId, dto.StartsAtUtc);
            await _repo.AddAsync(session);
            await _uow.SaveChangesAsync();
            return ToDto(session);
        }

        public async Task<SessionResponseDto> AssignAsync(Guid sessionId, Guid psychologistId)
        {
            var session = await _repo.FindByIdAsync(sessionId)
                ?? throw new KeyNotFoundException("Sesión no encontrada.");

            if (session.State != SessionState.Pending)
                throw new InvalidOperationException("Solo sesiones pendientes pueden asignarse.");

            bool busy = await _repo.QueryAll()
                .Where(s => s.PsychologistId == psychologistId && s.State == SessionState.Confirmed)
                .AnyAsync();
            if (busy)
                throw new InvalidOperationException("Ya estás atendiendo otra sesión confirmada.");

            session.AssignPsychologist(psychologistId);
            await _uow.SaveChangesAsync();

            await _notifSvc.CreateAsync(new CreateNotificationDto(
                session.StudentId,
                "Sesión confirmada",
                $"Tu sesión del {session.StartsAtUtc:yyyy-MM-dd HH:mm} UTC ha sido confirmada."
            ));

            return ToDto(session);
        }

        public async Task<SessionResponseDto> CancelAsync(Guid sessionId, Guid userId, CancelSessionDto dto)
        {
            var session = await _repo.FindByIdAsync(sessionId)
                ?? throw new KeyNotFoundException("Sesión no encontrada.");
            if (session.StudentId != userId)
                throw new UnauthorizedAccessException("Solo el estudiante puede cancelar esta sesión.");

            session.Cancel(dto.Reason);
            await _uow.SaveChangesAsync();

            await _notifSvc.CreateAsync(new CreateNotificationDto(
                session.StudentId,
                "Sesión cancelada",
                $"La sesión reservada para {session.StartsAtUtc:yyyy-MM-dd HH:mm} UTC ha sido cancelada."
            ));

            return ToDto(session);
        }

        public async Task<SessionResponseDto> FinishAsync(Guid sessionId, Guid psychologistId)
        {
            var session = await _repo.FindByIdAsync(sessionId)
                ?? throw new KeyNotFoundException("Sesión no encontrada.");
            if (session.PsychologistId != psychologistId)
                throw new UnauthorizedAccessException("Solo el psicólogo asignado puede finalizar esta sesión.");

            session.Finish();
            await _uow.SaveChangesAsync();

            await _notifSvc.CreateAsync(new CreateNotificationDto(
                session.StudentId,
                "Sesión finalizada",
                $"La sesión que tuviste el {session.StartsAtUtc:yyyy-MM-dd HH:mm} UTC ha sido finalizada."
            ));

            return ToDto(session);
        }

        public async Task<IEnumerable<SessionResponseDto>> ListMineAsync(Guid userId)
        {
            var list = await _repo.QueryMine(userId).ToListAsync();
            return list.Select(ToDto);
        }

        public async Task<IEnumerable<UserInfoDto>> ListPsychologistsAsync()
        {
            var users = await _users.ListByRoleAsync(UserRole.Psychologist.ToString());
            return users.Select(u =>
                new UserInfoDto(u.Id, u.Email.Value, u.Name.FirstName, u.Name.LastName, u.Role.ToString())
            );
        }

        public async Task<IEnumerable<SessionResponseDto>> ListAllAsync()
        {
            var list = await _repo.QueryAll().ToListAsync();
            return list.Select(ToDto);
        }

        public async Task<IEnumerable<SessionResponseDto>> ListByStateAsync(SessionState state)
        {
            var list = await _repo.QueryByState(state).ToListAsync();
            return list.Select(ToDto);
        }

        private static SessionResponseDto ToDto(Session s)
            => new(
                s.Id,
                s.StudentId,
                s.PsychologistId,
                s.StartsAtUtc,
                s.EndsAtUtc,
                s.State.ToString(),
                s.CancelReason);
    }
}