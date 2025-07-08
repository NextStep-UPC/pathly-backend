using pathly_backend.Shared.Common;
using pathly_backend.IAM.Domain.Events;
using pathly_backend.IAM.Domain.ValueObjects;
using pathly_backend.IAM.Domain.Enums;

namespace pathly_backend.IAM.Domain.Entities;

public class User : IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public Guid Id { get; private set; }
    public Email Email { get; private set; } = default!;
    public PasswordHash PasswordHash { get; private set; } = default!;
    public FullName? Name { get; private set; }
    public UserRole Role { get; private set; }

    private User() { }

    private User(Guid id, Email email, PasswordHash hash, FullName name)
    {
        Id = id;
        Email = email;
        PasswordHash = hash;
        Name = name;
        Role = UserRole.Student;

        AddDomainEvent(new UserRegistered(Id, Email.Value));
    }

    public static User Register(string email, string password, string first, string last)
        => new(Guid.NewGuid(),
            Email.Create(email),
            PasswordHash.FromPlainText(password),
            FullName.Create(first, last));

    public bool VerifyPassword(string plaintext) => PasswordHash.Verify(plaintext);

    public void RequestPsychologistRole()
        => AddDomainEvent(new RoleRequested(Id, UserRole.Psychologist.ToString()));

    public void GrantRole(UserRole newRole)
    {
        Role = newRole;
        AddDomainEvent(new RoleGranted(Id, newRole.ToString()));
    }

    private void AddDomainEvent(IDomainEvent evt) => _domainEvents.Add(evt);
    public void ClearDomainEvents() => _domainEvents.Clear();
}