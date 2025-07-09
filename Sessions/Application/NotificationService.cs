using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pathly_backend.Sessions.Application.Dtos;
using pathly_backend.Sessions.Application.Interfaces;
using pathly_backend.Sessions.Domain.Entities;
using pathly_backend.Sessions.Domain.Repositories;
using pathly_backend.Sessions.Infrastructure.Persistence;

namespace pathly_backend.Sessions.Application;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repo;
    private readonly ISessionsUnitOfWork     _uow;

    public NotificationService(
        INotificationRepository repo,
        ISessionsUnitOfWork uow)
    {
        _repo = repo;
        _uow  = uow;
    }

    public async Task<NotificationDto> CreateAsync(CreateNotificationDto dto)
    {
        var n = new Notification(dto.UserId, dto.Title, dto.Message);
        await _repo.AddAsync(n);
        await _uow.SaveChangesAsync();
        return new NotificationDto(
            n.Id, n.UserId, n.Title, n.Message, n.IsRead, n.CreatedAtUtc
        );
    }

    public async Task<IEnumerable<NotificationDto>> ListByUserAsync(Guid userId)
    {
        var list = _repo.QueryByUser(userId).ToList();
        return list.Select(n =>
            new NotificationDto(
                n.Id, n.UserId, n.Title, n.Message, n.IsRead, n.CreatedAtUtc
            )
        );
    }

    public async Task MarkAsReadAsync(Guid notificationId, Guid userId)
    {
        var n = await _repo.FindByIdAsync(notificationId)
                ?? throw new KeyNotFoundException("Notificación no encontrada");
        if (n.UserId != userId)
            throw new UnauthorizedAccessException("No puedes marcar como leída esta notificación");
        n.MarkAsRead();
        await _uow.SaveChangesAsync();
    }
}