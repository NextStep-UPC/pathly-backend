namespace IAM.Application.Dto;
public record AuthLoginRequest(string Email, string Password);
public record AuthRegisterRequest(
    string Name,
    string LastName,
    DateOnly BirthDate,
    string Phone,
    string Email,
    string Password,
    string ConfirmPassword);
public record AuthResponse(string Token, UserDto User);
public record UserDto(Guid Id, string Email, string Name, string LastName, string Role);
