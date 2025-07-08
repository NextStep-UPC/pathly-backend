namespace pathly_backend.IAM.Domain.ValueObjects;

public sealed class PasswordHash
{
    public string Value { get; private set; } = default!;

    private PasswordHash() { }

    private PasswordHash(string value) => Value = value;

    public static PasswordHash FromPlainText(string plain)
    {
        if (string.IsNullOrWhiteSpace(plain) || plain.Length < 8)
            throw new ArgumentException("La contraseña debe tener al menos 8 caracteres.", nameof(plain));

        var hash = BCrypt.Net.BCrypt.HashPassword(plain);
        return new PasswordHash(hash);
    }

    public static PasswordHash FromHash(string hash) => new(hash);

    public bool Verify(string plain) => BCrypt.Net.BCrypt.Verify(plain, Value);

    public override string ToString() => Value;
}