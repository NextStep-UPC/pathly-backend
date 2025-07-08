using pathly_backend.IAM.Application.Dto;
using pathly_backend.IAM.Domain.Entities;
using pathly_backend.IAM.Domain.Repositories;
using pathly_backend.IAM.Infrastructure.Services;

using pathly_backend.Profile.Domain.Repositories;
using ProfileEntity = pathly_backend.Profile.Domain.Entities.Profile;

using pathly_backend.Shared.Common;

namespace pathly_backend.IAM.Application;

public class AuthService
{
    private readonly IUserRepository     _users;
    private readonly IProfileRepository  _profiles;
    private readonly IUnitOfWork         _iamUow;
    private readonly IProfileUnitOfWork  _profileUow;
    private readonly IJwtTokenGenerator  _jwt;

    public AuthService(
        IUserRepository     users,
        IProfileRepository  profiles,
        IUnitOfWork         iamUow,
        IProfileUnitOfWork  profileUow,
        IJwtTokenGenerator  jwt)
    {
        _users      = users;
        _profiles   = profiles;
        _iamUow     = iamUow;
        _profileUow = profileUow;
        _jwt        = jwt;
    }

    // ---------- Registro ----------
    public async Task<AuthResultDto> RegisterAsync(RegisterRequestDto dto)
    {
        if (await _users.ExistsAsync(dto.Email))
            throw new InvalidOperationException("Correo electrónico ya en uso.");

        var user = User.Register(dto.Email, dto.Password, dto.FirstName, dto.LastName);

        await _users.AddAsync(user);

        var profile = new ProfileEntity(user.Id);
        profile.Update(dto.FirstName, dto.LastName, null, null,
            dto.BirthDate, dto.PhoneNumber);
        await _profiles.AddAsync(profile);

        await _iamUow.SaveChangesAsync();
        await _profileUow.SaveChangesAsync();

        var token = _jwt.GenerateToken(user);
        return new AuthResultDto(user.Id, token);
    }

    // ---------- Login ----------
    public async Task<AuthResultDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await _users.FindByEmailAsync(dto.Email)
                   ?? throw new InvalidOperationException("Credenciales no válidas.");

        if (!user.VerifyPassword(dto.Password))
            throw new InvalidOperationException("Credenciales no válidas.");

        var token = _jwt.GenerateToken(user);
        return new AuthResultDto(user.Id, token);
    }
}