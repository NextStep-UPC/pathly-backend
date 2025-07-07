namespace pathly_backend.IAM.Application.Dto;

public record UserInfoDto(
    Guid    Id,
    string  Email,
    string? FirstName,
    string? LastName,
    string  Role);