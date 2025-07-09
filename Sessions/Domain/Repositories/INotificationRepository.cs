using System;
using System.Linq;
using System.Threading.Tasks;
using pathly_backend.Sessions.Domain.Entities;

namespace pathly_backend.Sessions.Domain.Repositories
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification);
        IQueryable<Notification> QueryByUser(Guid userId);
        Task<Notification?> FindByIdAsync(Guid id);
    }
}