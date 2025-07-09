namespace pathly_backend.Sessions.Application.Dtos
{
    public record CreateNotificationDto(
        Guid   UserId,
        string Title,
        string Message
    );
}