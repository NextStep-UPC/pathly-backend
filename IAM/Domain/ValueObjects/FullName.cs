namespace pathly_backend.IAM.Domain.ValueObjects;

public sealed class FullName
{
    public string FirstName { get; private set; } = default!;
    public string LastName  { get; private set; } = default!;

    private FullName() { }

    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName  = lastName;
    }

    public static FullName Create(string first, string last)
    {
        if (string.IsNullOrWhiteSpace(first) || string.IsNullOrWhiteSpace(last))
            throw new ArgumentException("Los nombres no pueden estar vacios..");

        return new FullName(first.Trim(), last.Trim());
    }

    public override string ToString() => $"{FirstName} {LastName}";
}