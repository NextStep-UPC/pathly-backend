namespace pathly_backend.IAM.Application.Dto;

public record RegisterRequestDto(
    string   Email,
    string   Password,
    string   FirstName,
    string   LastName,
    DateOnly BirthDate,
    string   PhoneNumber);

public record LoginRequestDto(string Email, string Password);
public record AuthResultDto(Guid UserId, string Token);