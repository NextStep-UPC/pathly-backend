using pathly_backend.Profile.Application.Dto;
using pathly_backend.Profile.Domain.Entities;
using pathly_backend.Profile.Domain.Repositories;
using pathly_backend.Shared.Common;

namespace pathly_backend.Profile.Application;

public class ProfileService
{
    private readonly IProfileRepository _repo;
    private readonly IUnitOfWork        _uow;

    public ProfileService(IProfileRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow  = uow;
    }

    public async Task<ProfileResponseDto> GetMineAsync(Guid userId, string role)
    {
        var profile = await _repo.FindByIdAsync(userId) ?? new Domain.Entities.Profile(userId);
        return ToDto(profile, role);
    }

    public async Task<ProfileResponseDto> UpdateMineAsync(
        Guid userId, string role, UpdateProfileRequestDto dto)
    {
        var p = await _repo.FindByIdAsync(userId) ?? new Domain.Entities.Profile(userId);

        p.Update(dto.FirstName, dto.LastName, dto.Bio, dto.AvatarUrl,
            dto.BirthDate, dto.PhoneNumber);

        if (p.CreatedUtc == default)
            await _repo.AddAsync(p);

        await _uow.SaveChangesAsync();
        return ToDto(p, role);
    }

    private static ProfileResponseDto ToDto(Domain.Entities.Profile p, string role) =>
        new(p.UserId, p.FirstName, p.LastName, p.BirthDate, p.PhoneNumber, p.Bio, p.AvatarUrl, role);
}