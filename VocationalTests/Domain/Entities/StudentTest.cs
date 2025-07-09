using System;
using System.Collections.Generic;
using pathly_backend.Shared.Common;

namespace pathly_backend.VocationalTests.Domain.Entities
{
    public class StudentTest : Entity
    {
        public Guid TestId { get; private set; }
        public Guid StudentId { get; private set; }
        public Guid SessionId { get; private set; }
        public DateTime StartedAtUtc { get; private set; }
        public DateTime? CompletedAtUtc { get; private set; }
        public IList<Answer> Answers { get; private set; } = new List<Answer>();

        private StudentTest() { }

        public StudentTest(Guid testId, Guid studentId, Guid sessionId)
        {
            Id = Guid.NewGuid();
            TestId = testId;
            StudentId = studentId;
            SessionId = sessionId;
            StartedAtUtc = DateTime.UtcNow;
        }

        public void AddAnswer(Answer answer) => Answers.Add(answer);
        public void Complete() => CompletedAtUtc = DateTime.UtcNow;
    }
}