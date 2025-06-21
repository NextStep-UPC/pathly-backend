namespace pathly_backend.CareerTest.Application.Internal.DTOs;

public class QuestionDto
{
    public int Id { get; set; }
    public string Text { get; set; } = null!;
    public List<QuestionOptionDto> Options { get; set; } = new();
}