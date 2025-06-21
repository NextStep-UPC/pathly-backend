namespace pathly_backend.CareerTest.Domain.Model.Entities;

public class QuestionOption
{
    public int Id { get; set; }
    public string Text { get; set; } = null!;
    public int QuestionId { get; set; }
}