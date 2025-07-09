using System;
using System.Collections.Generic;
using pathly_backend.Shared.Common;
using pathly_backend.VocationalTests.Domain.Enums;

namespace pathly_backend.VocationalTests.Domain.Entities
{
    public class Question : Entity
    {
        public Guid TestId { get; private set; }
        public string Text { get; private set; }
        public QuestionType Type { get; private set; }
        public IList<Option> Options { get; private set; } = new List<Option>();

        private Question() { }

        public Question(Guid testId, string text, QuestionType type)
        {
            Id = Guid.NewGuid();
            TestId = testId;
            Text = text;
            Type = type;
        }

        public void AddOption(Option option)
        {
            if (Type == QuestionType.OpenEnded)
                throw new InvalidOperationException("Respuestas con texto no pueden tener opciones.");
            Options.Add(option);
        }
    }
}