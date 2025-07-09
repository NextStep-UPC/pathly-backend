using System;
using System.Collections.Generic;
using pathly_backend.Shared.Common;

namespace pathly_backend.VocationalTests.Domain.Entities
{
    public class Test : Entity
    {
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public IList<Question> Questions { get; private set; } = new List<Question>();

        private Test() { }

        public Test(string title, string? description)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
        }

        public void AddQuestion(Question question) => Questions.Add(question);
    }
}