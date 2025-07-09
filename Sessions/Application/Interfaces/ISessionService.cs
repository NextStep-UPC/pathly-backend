using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using pathly_backend.IAM.Application.Dto;
using pathly_backend.Sessions.Application.Dto;
using pathly_backend.Sessions.Application.Dtos;
using pathly_backend.Sessions.Domain.Enums;

namespace pathly_backend.Sessions.Application.Interfaces
{
    public interface ISessionService
    {
        Task<SessionResponseDto> BookAsync(Guid studentId, BookSessionDto dto);
        Task<SessionResponseDto> AssignAsync(Guid sessionId, Guid psychologistId);
        Task<SessionResponseDto> CancelAsync(Guid sessionId, Guid userId, CancelSessionDto dto);
        Task<SessionResponseDto> FinishAsync(Guid sessionId, Guid psychologistId);
        Task<IEnumerable<SessionResponseDto>> ListMineAsync(Guid userId);
        Task<IEnumerable<UserInfoDto>> ListPsychologistsAsync();
        Task<IEnumerable<SessionResponseDto>> ListAllAsync();
        Task<IEnumerable<SessionResponseDto>> ListByStateAsync(SessionState state);
    }
}