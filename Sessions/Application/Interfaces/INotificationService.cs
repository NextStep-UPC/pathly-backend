using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using pathly_backend.Sessions.Application.Dtos;

namespace pathly_backend.Sessions.Application.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationDto> CreateAsync(CreateNotificationDto dto);
        Task<IEnumerable<NotificationDto>> ListByUserAsync(Guid userId);
        Task MarkAsReadAsync(Guid notificationId, Guid userId);
    }
}