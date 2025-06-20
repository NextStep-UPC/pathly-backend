using IAM.Domain.Enums;

namespace IAM.Domain.Entities;

public class User
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public DateOnly BirthDate { get; private set; }
    public string Phone { get; private set; } = default!;
    public UserRole Role { get; private set; }

    private User() { }

    public static User Create(string email, string plainPwd, string name,
        string lastName, DateOnly birth, string phone, UserRole role) =>
        new()
        {
            Email = email.ToLowerInvariant(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPwd),
            Name = name,
            LastName = lastName,
            BirthDate = birth,
            Phone = phone,
            Role = role
        };
}
