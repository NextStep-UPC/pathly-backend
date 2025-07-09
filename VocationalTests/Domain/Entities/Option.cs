using System;
using pathly_backend.Shared.Common;

namespace pathly_backend.VocationalTests.Domain.Entities
{
    public class Option : Entity
    {
        public Guid QuestionId { get; private set; }
        public string Text { get; private set; }
        public bool IsCorrect { get; private set; }

        private Option() { }

        public Option(Guid questionId, string text, bool isCorrect)
        {
            Id = Guid.NewGuid();
            QuestionId = questionId;    
            Text = text;
            IsCorrect = isCorrect;
        }
    }
}