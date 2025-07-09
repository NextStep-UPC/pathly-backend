using System;
using System.Linq;
using pathly_backend.Sessions.Domain.Entities;
using pathly_backend.Sessions.Domain.Enums;

namespace pathly_backend.Sessions.Domain.Repositories
{
    public interface IReportRepository
    {
        Task AddAsync(Report report);
        Task<Report?> FindByIdAsync(Guid id);
        IQueryable<Report> QueryByPsychologist(Guid psychologistId);
        IQueryable<Report> QueryAll();
    }
}