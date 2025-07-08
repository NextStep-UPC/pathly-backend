namespace pathly_backend.Sessions.Application.Dtos
{
    public record CreateFeedbackDto(
        int    Rating,
        string? Comment
    );
}