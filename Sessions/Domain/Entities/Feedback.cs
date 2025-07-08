using System;
using pathly_backend.Shared.Common;

namespace pathly_backend.Sessions.Domain.Entities
{
    public class Feedback : Entity
    {
        public Guid    SessionId   { get; private set; }
        public Guid    StudentId   { get; private set; }
        public int     Rating      { get; private set; }
        public string? Comment     { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }

        private Feedback() { }

        public Feedback(Guid sessionId, Guid studentId, int rating, string? comment)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentOutOfRangeException(nameof(rating), "La puntuación debe estar comprendida entre 1 y 5.");

            Id            = Guid.NewGuid();
            SessionId     = sessionId;
            StudentId     = studentId;
            Rating        = rating;
            Comment       = comment;
            CreatedAtUtc  = DateTime.UtcNow;
        }
    }
}