using System;
using pathly_backend.Shared.Common;

namespace pathly_backend.Sessions.Domain.Entities
{
    public class ChatMessage : Entity
    {
        public Guid    SessionId  { get; private set; }
        public Guid    SenderId   { get; private set; }
        public string  Content    { get; private set; }
        public DateTime SentAtUtc { get; private set; }

        private ChatMessage() { } // para EF

        public ChatMessage(Guid sessionId, Guid senderId, string content)
        {
            Id         = Guid.NewGuid();
            SessionId  = sessionId;
            SenderId   = senderId;
            Content    = content;
            SentAtUtc  = DateTime.UtcNow;
        }
    }
}