namespace pathly_backend.CareerTest.Domain.Model.Entities;

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; } = null!;
    public List<QuestionOption> Options { get; set; } = new();
}