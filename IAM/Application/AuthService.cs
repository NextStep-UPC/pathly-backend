using IAM.Application.Dto;
using IAM.Domain.Entities;
using IAM.Domain.Enums;
using IAM.Domain.Repositories;
using IAM.Infrastructure;
using IAM.Infrastructure.Services;

namespace IAM.Application.Services;

public class AuthService
{
    private readonly IUserRepository _repo;
    private readonly JwtTokenGenerator _jwt;

    public AuthService(IUserRepository repo, JwtTokenGenerator jwt) =>
        (_repo, _jwt) = (repo, jwt);

    public async Task<AuthResponse> LoginAsync(AuthLoginRequest r)
    {
        var user = await _repo.GetByEmailAsync(r.Email)
                   ?? throw new InvalidOperationException("Credenciales inválidas");

        if (!BCrypt.Net.BCrypt.Verify(r.Password, user.PasswordHash))
            throw new InvalidOperationException("Credenciales inválidas");

        return Build(user);
    }

    public async Task<AuthResponse> RegisterAsync(AuthRegisterRequest r)
    {
        if (r.Password != r.ConfirmPassword)
            throw new InvalidOperationException("Las contraseñas no coinciden");

        if (await _repo.EmailExistsAsync(r.Email))
            throw new InvalidOperationException("El correo ya existe");

        var user = User.Create(
            r.Email, r.Password, r.Name, r.LastName, r.BirthDate, r.Phone, UserRole.Normal);

        await _repo.AddAsync(user);
        return Build(user);
    }

    private AuthResponse Build(User u) =>
        new(_jwt.Generate(u),
            new UserDto(u.Id, u.Email, u.Name, u.LastName, u.Role.ToString()));
}
