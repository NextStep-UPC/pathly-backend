using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pathly_backend.SanctionsAndAppeals.Application.Dtos;
using pathly_backend.SanctionsAndAppeals.Application.Interfaces;
using pathly_backend.SanctionsAndAppeals.Domain.Entities;
using pathly_backend.SanctionsAndAppeals.Domain.Enums;
using pathly_backend.SanctionsAndAppeals.Domain.Repositories;
using pathly_backend.Sessions.Infrastructure.Persistence;

namespace pathly_backend.SanctionsAndAppeals.Application.Services
{
    public class AppealService : IAppealService
    {
        private readonly IAppealRepository _repo;
        private readonly ISessionsUnitOfWork _uow;

        public AppealService(IAppealRepository repo, ISessionsUnitOfWork uow)
        {
            _repo = repo;
            _uow  = uow;
        }

        public async Task<AppealDto> CreateAsync(CreateAppealDto dto, Guid userId)
        {
            var a = new Appeal(dto.SanctionId, userId, dto.Reason);
            await _repo.AddAsync(a);
            await _uow.SaveChangesAsync();
            return new AppealDto(a.Id, a.SanctionId, a.UserId, a.Reason, a.CreatedAtUtc, a.State, a.ResolvedById, a.DecisionComment, a.ResolvedAtUtc);
        }

        public async Task<IEnumerable<AppealDto>> ListByUserAsync(Guid userId)
        {
            var list = _repo.QueryByUser(userId).ToList();
            return list.Select(a => new AppealDto(a.Id, a.SanctionId, a.UserId, a.Reason, a.CreatedAtUtc, a.State, a.ResolvedById, a.DecisionComment, a.ResolvedAtUtc));
        }

        public async Task<IEnumerable<AppealDto>> ListByStateAsync(string state)
        {
            var st = Enum.Parse<AppealState>(state, true); var list = _repo.QueryByState(st).ToList();
            return list.Select(a => new AppealDto(a.Id, a.SanctionId, a.UserId, a.Reason, a.CreatedAtUtc, a.State, a.ResolvedById, a.DecisionComment, a.ResolvedAtUtc));
        }

        public async Task<AppealDto> ResolveAsync(Guid appealId, ResolveAppealDto dto, Guid adminId)
        {
            var a = await _repo.FindByIdAsync(appealId) ?? throw new KeyNotFoundException("Appeal not found");
            var newState = dto.Action.Equals("Accepted", StringComparison.OrdinalIgnoreCase) ? AppealState.Accepted : AppealState.Rejected;
            a.Resolve(newState, adminId, dto.DecisionComment);
            await _uow.SaveChangesAsync();
            return new AppealDto(a.Id, a.SanctionId, a.UserId, a.Reason, a.CreatedAtUtc, a.State, a.ResolvedById, a.DecisionComment, a.ResolvedAtUtc);
        }
    }
}