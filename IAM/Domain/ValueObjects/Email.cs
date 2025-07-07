namespace pathly_backend.IAM.Domain.ValueObjects;

public sealed class Email
{
    public string Value { get; private set; } = default!;

    private Email() { }

    private Email(string value) => Value = value;

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.", nameof(email));

        var normalized = email.Trim().ToLowerInvariant();

        try
        {
            var addr = new System.Net.Mail.MailAddress(normalized);
            if (addr.Address != normalized)
                throw new ArgumentException("Invalid email format.", nameof(email));
        }
        catch
        {
            throw new ArgumentException("Invalid email format.", nameof(email));
        }

        return new Email(normalized);
    }

    public override string ToString() => Value;
}