using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using pathly_backend.Sessions.Application.Dtos;

namespace pathly_backend.Sessions.Application.Interfaces
{
    public interface IReportService
    {
        Task<ReportDto> CreateAsync(Guid sessionId, Guid psychologistId, CreateReportDto dto);
        Task<IEnumerable<ReportDto>> ListByPsychologistAsync(Guid psychologistId);
        Task<IEnumerable<ReportDto>> ListAllAsync();
        Task<ReportDto> ResolveAsync(Guid reportId, Guid adminId, ResolveReportDto dto);
    }
}