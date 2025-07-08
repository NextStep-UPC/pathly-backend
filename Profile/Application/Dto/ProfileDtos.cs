namespace pathly_backend.Profile.Application.Dto;

public record ProfileResponseDto(
    Guid      UserId,
    string?   FirstName,
    string?   LastName,
    DateOnly? BirthDate,
    string?   PhoneNumber,
    string?   Bio,
    string?   AvatarUrl,
    string    Role);

public record UpdateProfileRequestDto(
    string?   FirstName,
    string?   LastName,
    string?   Bio,
    string?   AvatarUrl,
    DateOnly? BirthDate,
    string?   PhoneNumber);