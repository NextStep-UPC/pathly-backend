using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using pathly_backend.Sessions.Domain.Entities;
using pathly_backend.Sessions.Domain.Repositories;

namespace pathly_backend.Sessions.Infrastructure.Persistence.Repositories
{
    public class EfNotificationRepository : INotificationRepository
    {
        private readonly SessionsDbContext _ctx;
        public EfNotificationRepository(SessionsDbContext ctx) => _ctx = ctx;

        public Task AddAsync(Notification notification)
            => _ctx.Notifications.AddAsync(notification).AsTask();

        public IQueryable<Notification> QueryByUser(Guid userId)
            => _ctx.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAtUtc);

        public Task<Notification?> FindByIdAsync(Guid id)
            => _ctx.Notifications.FirstOrDefaultAsync(n => n.Id == id);
    }
}