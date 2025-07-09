using System;
using pathly_backend.Shared.Common;
using pathly_backend.SanctionsAndAppeals.Domain.Enums;

namespace pathly_backend.SanctionsAndAppeals.Domain.Entities
{
    public class Appeal : Entity
    {
        public Guid        SanctionId      { get; private set; }
        public Guid        UserId          { get; private set; }
        public string      Reason          { get; private set; }
        public DateTime    CreatedAtUtc    { get; private set; }
        public AppealState State           { get; private set; }
        public Guid?       ResolvedById    { get; private set; }
        public string?     DecisionComment { get; private set; }
        public DateTime?   ResolvedAtUtc   { get; private set; }

        private Appeal() { }

        public Appeal(Guid sanctionId, Guid userId, string reason)
        {
            Id            = Guid.NewGuid();
            SanctionId    = sanctionId;
            UserId        = userId;
            Reason        = reason;
            CreatedAtUtc  = DateTime.UtcNow;
            State         = AppealState.Pending;
        }

        public void Resolve(AppealState newState, Guid adminId, string comment)
        {
            if (State != AppealState.Pending)
                throw new InvalidOperationException("Apelación ya resuelta.");
            State           = newState;
            ResolvedById    = adminId;
            DecisionComment = comment;
            ResolvedAtUtc   = DateTime.UtcNow;
        }
    }
}