namespace pathly_backend.Profile.Domain.Entities;

public class Profile
{
    public Guid      UserId      { get; private set; }
    public string?   FirstName   { get; private set; }
    public string?   LastName    { get; private set; }
    public string?   Bio         { get; private set; }
    public string?   AvatarUrl   { get; private set; }
    public DateOnly? BirthDate   { get; private set; }
    public string?   PhoneNumber { get; private set; }
    public DateTime  CreatedUtc  { get; private set; }

    private Profile() { }

    public Profile(Guid userId)
    {
        UserId     = userId;
        CreatedUtc = DateTime.UtcNow;
    }

    public void Update(string? first, string? last, string? bio,
        string? avatar, DateOnly? birth, string? phone)
    {
        FirstName   = first?.Trim();
        LastName    = last?.Trim();
        Bio         = bio?.Trim();
        AvatarUrl   = avatar?.Trim();

        if (birth is not null &&
            (birth < new DateOnly(1900, 1, 1) ||
             birth > DateOnly.FromDateTime(DateTime.Today)))
            throw new ArgumentException("Birthdate out of range.");

        BirthDate = birth;

        if (phone is not null &&
            (!phone.All(char.IsDigit) || phone.Length is < 7 or > 15))
            throw new ArgumentException("Invalid phone number.");

        PhoneNumber = phone?.Trim();
    }
}