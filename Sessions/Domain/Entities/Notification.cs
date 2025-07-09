using System;
using pathly_backend.Shared.Common;

namespace pathly_backend.Sessions.Domain.Entities
{
    public class Notification : Entity
    {
        public Guid    UserId      { get; private set; }
        public string  Title       { get; private set; }
        public string  Message     { get; private set; }
        public bool    IsRead      { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }

        private Notification() { } // Para EF

        public Notification(Guid userId, string title, string message)
        {
            Id            = Guid.NewGuid();
            UserId        = userId;
            Title         = title;
            Message       = message;
            IsRead        = false;
            CreatedAtUtc  = DateTime.UtcNow;
        }
        
        public void MarkAsRead()
        {
            IsRead = true;
        }
    }
}