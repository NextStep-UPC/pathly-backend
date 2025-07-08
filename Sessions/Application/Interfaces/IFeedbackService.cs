using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using pathly_backend.Sessions.Application.Dtos;

namespace pathly_backend.Sessions.Application.Interfaces
{
    public interface IFeedbackService
    {
        Task<FeedbackDto> CreateAsync(Guid sessionId, Guid studentId, CreateFeedbackDto dto);
        Task<IEnumerable<FeedbackDto>> ListBySessionAsync(Guid sessionId);
    }
}