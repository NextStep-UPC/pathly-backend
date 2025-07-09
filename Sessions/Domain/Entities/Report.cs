using System;
using pathly_backend.Shared.Common;
using pathly_backend.Sessions.Domain.Enums;

namespace pathly_backend.Sessions.Domain.Entities
{
    public class Report : Entity
    {
        public Guid         SessionId        { get; private set; }
        public Guid         PsychologistId   { get; private set; }
        public Guid         ReportedUserId   { get; private set; }
        public string       Reason           { get; private set; }
        public DateTime     CreatedAtUtc     { get; private set; }
        public ReportState  State            { get; private set; }
        public Guid?        ResolvedByAdminId { get; private set; }
        public string?      AdminComment     { get; private set; }
        public DateTime?    ResolvedAtUtc    { get; private set; }

        private Report() { }
        
        public Report(Guid sessionId, Guid psychologistId, Guid reportedUserId, string reason)
        {
            Id                  = Guid.NewGuid();
            SessionId           = sessionId;
            PsychologistId      = psychologistId;
            ReportedUserId      = reportedUserId;
            Reason              = reason;
            CreatedAtUtc        = DateTime.UtcNow;
            State               = ReportState.Pending;
        }
        
        public void Resolve(ReportState newState, Guid adminId, string adminComment)
        {
            if (newState == ReportState.Pending)
                throw new InvalidOperationException("No puede resolver a 'Pending'.");

            State               = newState;
            ResolvedByAdminId   = adminId;
            AdminComment        = adminComment;
            ResolvedAtUtc       = DateTime.UtcNow;
        }
    }
}