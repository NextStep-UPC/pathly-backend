namespace pathly_backend.Sessions.Application.Dtos
{
    public record CreateReportDto(
        Guid   ReportedUserId,
        string Reason
    );
}