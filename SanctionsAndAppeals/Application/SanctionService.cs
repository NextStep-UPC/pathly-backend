using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pathly_backend.SanctionsAndAppeals.Application.Dtos;
using pathly_backend.SanctionsAndAppeals.Application.Interfaces;
using pathly_backend.SanctionsAndAppeals.Domain.Entities;
using pathly_backend.SanctionsAndAppeals.Domain.Repositories;
using pathly_backend.Sessions.Infrastructure.Persistence;

namespace pathly_backend.SanctionsAndAppeals.Application.Services
{
    public class SanctionService : ISanctionService
    {
        private readonly ISanctionRepository _repo;
        private readonly ISessionsUnitOfWork _uow;

        public SanctionService(ISanctionRepository repo, ISessionsUnitOfWork uow)
        {
            _repo = repo;
            _uow  = uow;
        }

        public async Task<SanctionDto> CreateAsync(CreateSanctionDto dto, Guid adminId)
        {
            var s = new Sanction(dto.UserId, adminId, dto.Reason, dto.EndAtUtc);
            await _repo.AddAsync(s);
            await _uow.SaveChangesAsync();
            return new SanctionDto(s.Id, s.UserId, s.AdminId, s.Reason, s.StartAtUtc, s.EndAtUtc, s.IsActive);
        }

        public async Task<SanctionDto?> GetActiveByUserAsync(Guid userId)
        {
            var s = await _repo.FindActiveByUserAsync(userId);
            if (s == null) return null;
            return new SanctionDto(s.Id, s.UserId, s.AdminId, s.Reason, s.StartAtUtc, s.EndAtUtc, s.IsActive);
        }

        public async Task<IEnumerable<SanctionDto>> ListAllAsync()
        {
            var all = _repo.QueryAll().ToList();
            return all.Select(s => new SanctionDto(s.Id, s.UserId, s.AdminId, s.Reason, s.StartAtUtc, s.EndAtUtc, s.IsActive));
        }

        public async Task RevokeAsync(Guid sanctionId, Guid adminId)
        {
            var s = await _repo.FindActiveByUserAsync(sanctionId);
            if (s == null) throw new KeyNotFoundException("Sanction not found");
            s.Revoke();
            await _uow.SaveChangesAsync();
        }
    }
}