using pathly_backend.Psychologist.Application.Internal.DTOs;
namespace pathly_backend.Psychologist.Application.Internal.DTOs;

public class SectionResponseDto
{
    public List<StudentDTo> Students { get; set; } = new List<StudentDTo>();
    public List<SectionDTo> Sections { get; set; } = new List<SectionDTo>();
}