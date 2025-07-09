using System;
using pathly_backend.Shared.Common;

namespace pathly_backend.VocationalTests.Domain.Entities
{
    public class Answer : Entity
    {
        public Guid StudentTestId { get; private set; }
        public Guid QuestionId { get; private set; }
        public Guid? OptionId { get; private set; }
        public string? ResponseText { get; private set; }

        private Answer() { }

        public Answer(Guid studentTestId, Guid questionId, Guid? optionId, string? responseText)
        {
            Id = Guid.NewGuid();
            StudentTestId = studentTestId;
            QuestionId = questionId;
            OptionId = optionId;
            ResponseText = responseText;
        }
    }
}