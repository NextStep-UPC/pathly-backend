namespace pathly_backend.CareerTest.Domain.Model.Entities;

public class Question
{
    public int QuestionId { get; set; }
    public string Text { get; set; } = null!;
    public ICollection<QuestionOption> Options { get; set; } = new List<QuestionOption>();
}