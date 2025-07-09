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
        private readonly ISessionRepository   _repo;
        private readonly ISessionsUnitOfWork  _uow;
        private readonly IUserRepository      _users;
        private readonly INotificationService _notifSvc;

        public SessionService(
            ISessionRepository repo,
            ISessionsUnitOfWork uow,
            IUserRepository users,
            INotificationService notifSvc)
        {
            _repo     = repo;
            _uow      = uow;
            _users    = users;
            _notifSvc = notifSvc;
        }

        public async Task<SessionResponseDto> BookAsync(Guid studentId, BookSessionDto dto)
        {
            var s = new Session(studentId, dto.StartsAtUtc);
            await _repo.AddAsync(s);
            await _uow.SaveChangesAsync();

            if (s.PsychologistId.HasValue)
            {
                await _notifSvc.CreateAsync(new CreateNotificationDto(
                    s.PsychologistId.Value,
                    "Nueva reserva de sesión",
                    $"Tienes una sesión reservada para {s.StartsAtUtc:yyyy-MM-dd HH:mm} UTC."
                ));
            }

            return ToDto(s);
        }

        public async Task<SessionResponseDto> AssignAsync(Guid sessionId, Guid psychologistId)
        {
            var s = await _repo.FindByIdAsync(sessionId)
                ?? throw new KeyNotFoundException("Sesión no encontrada.");
            s.AssignPsychologist(psychologistId);
            await _uow.SaveChangesAsync();

            await _notifSvc.CreateAsync(new CreateNotificationDto(
                s.StudentId,
                "Sesión confirmada",
                $"Tu sesión del {s.StartsAtUtc:yyyy-MM-dd HH:mm} UTC ha sido confirmada."
            ));

            return ToDto(s);
        }

        public async Task<SessionResponseDto> CancelAsync(Guid sessionId, Guid userId, CancelSessionDto dto)
        {
            var s = await _repo.FindByIdAsync(sessionId)
                ?? throw new KeyNotFoundException("Sesión no encontrada.");
            if (s.StudentId != userId)
                throw new UnauthorizedAccessException("Solo el estudiante puede cancelar la sesión.");
            s.Cancel(dto.Reason);
            await _uow.SaveChangesAsync();

            await _notifSvc.CreateAsync(new CreateNotificationDto(
                s.StudentId,
                "Sesión cancelada",
                $"La sesión reservada para {s.StartsAtUtc:yyyy-MM-dd HH:mm} UTC ha sido cancelada."
            ));

            return ToDto(s);
        }

        public async Task<SessionResponseDto> FinishAsync(Guid sessionId, Guid psychologistId)
        {
            var s = await _repo.FindByIdAsync(sessionId)
                ?? throw new KeyNotFoundException("Sesión no encontrada.");
            if (s.PsychologistId != psychologistId)
                throw new UnauthorizedAccessException("Solo el psicólogo asignado puede finalizar esta sesión.");
            s.Finish();
            await _uow.SaveChangesAsync();

            await _notifSvc.CreateAsync(new CreateNotificationDto(
                s.StudentId,
                "Sesión finalizada",
                $"La sesión que tuviste el {s.StartsAtUtc:yyyy-MM-dd HH:mm} UTC ha sido finalizada."
            ));

            return ToDto(s);
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
            => new SessionResponseDto(
                s.Id,
                s.StudentId,
                s.PsychologistId,
                s.StartsAtUtc,
                s.EndsAtUtc,
                s.State.ToString(),
                s.CancelReason);
    }
}