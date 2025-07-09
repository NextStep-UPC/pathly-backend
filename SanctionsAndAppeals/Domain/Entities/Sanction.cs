using System;
using pathly_backend.Shared.Common;

namespace pathly_backend.SanctionsAndAppeals.Domain.Entities
{
    public class Sanction : Entity
    {
        public Guid    UserId      { get; private set; }
        public Guid    AdminId     { get; private set; }
        public string  Reason      { get; private set; }
        public DateTime StartAtUtc { get; private set; }
        public DateTime? EndAtUtc  { get; private set; }
        public bool    IsActive    => EndAtUtc == null || EndAtUtc > DateTime.UtcNow;

        private Sanction() { }

        public Sanction(Guid userId, Guid adminId, string reason, DateTime? endAtUtc)
        {
            Id         = Guid.NewGuid();
            UserId     = userId;
            AdminId    = adminId;
            Reason     = reason;
            StartAtUtc = DateTime.UtcNow;
            EndAtUtc   = endAtUtc;
        }

        public void Revoke() => EndAtUtc = DateTime.UtcNow;
    }
}