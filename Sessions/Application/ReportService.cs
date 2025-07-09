using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pathly_backend.Sessions.Application.Dtos;
using pathly_backend.Sessions.Application.Interfaces;
using pathly_backend.Sessions.Domain.Entities;
using pathly_backend.Sessions.Domain.Enums;
using pathly_backend.Sessions.Domain.Repositories;
using pathly_backend.Sessions.Infrastructure.Persistence;

namespace pathly_backend.Sessions.Application
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository  _repo;
        private readonly ISessionsUnitOfWork _uow;
        private readonly ISessionRepository  _sessionRepo;
        private readonly INotificationService _notifSvc;

        public ReportService(
            IReportRepository repo,
            ISessionsUnitOfWork uow,
            ISessionRepository sessionRepo,
            INotificationService notifSvc)
        {
            _repo         = repo;
            _uow          = uow;
            _sessionRepo  = sessionRepo;
            _notifSvc     = notifSvc;
        }

        public async Task<ReportDto> CreateAsync(Guid sessionId, Guid psychologistId, CreateReportDto dto)
        {
            var session = await _sessionRepo.FindByIdAsync(sessionId)
                          ?? throw new KeyNotFoundException("Sesión no encontrada.");
            if (session.PsychologistId != psychologistId)
                throw new UnauthorizedAccessException("Solo el psicólogo asignado puede reportar esta sesión.");

            var report = new Report(sessionId, psychologistId, dto.ReportedUserId, dto.Reason);
            await _repo.AddAsync(report);
            await _uow.SaveChangesAsync();
            return ToDto(report);
        }

        public async Task<IEnumerable<ReportDto>> ListByPsychologistAsync(Guid psychologistId)
        {
            var list = _repo.QueryByPsychologist(psychologistId).ToList();
            return list.Select(ToDto);
        }

        public async Task<IEnumerable<ReportDto>> ListAllAsync()
        {
            var list = _repo.QueryAll().ToList();
            return list.Select(ToDto);
        }

        public async Task<ReportDto> ResolveAsync(Guid reportId, Guid adminId, ResolveReportDto dto)
        {
            var report = await _repo.FindByIdAsync(reportId)
                         ?? throw new KeyNotFoundException("Reporte no encontrado.");
            if (report.State != ReportState.Pending)
                throw new InvalidOperationException("Solo reportes pendientes pueden resolverse.");

            var newState = dto.Action.Equals("Accepted", StringComparison.OrdinalIgnoreCase)
                ? ReportState.Accepted
                : ReportState.Rejected;
            report.Resolve(newState, adminId, dto.AdminComment);
            await _uow.SaveChangesAsync();

            // Notificar al psicólogo de la resolución
            await _notifSvc.CreateAsync(new CreateNotificationDto(
                report.PsychologistId,
                newState == ReportState.Accepted
                    ? "Reporte aceptado"
                    : "Reporte rechazado",
                $"Tu reporte del {report.CreatedAtUtc:yyyy-MM-dd HH:mm} UTC ha sido " +
                newState.ToString().ToLower() + ". Motivo: {dto.AdminComment}."
            ));

            return ToDto(report);
        }

        private static ReportDto ToDto(Report r)
            => new ReportDto(
                r.Id, r.SessionId, r.PsychologistId, r.ReportedUserId,
                r.Reason, r.CreatedAtUtc, r.State,
                r.ResolvedByAdminId, r.AdminComment, r.ResolvedAtUtc);
    }
}