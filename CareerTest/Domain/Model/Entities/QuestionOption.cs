namespace pathly_backend.CareerTest.Domain.Model.Entities;

public class QuestionOption
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string Text { get; set; } = null!;
    public Question Question { get; set; } = null!;
}