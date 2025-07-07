using pathly_backend.Shared.Common;

namespace pathly_backend.IAM.Domain.Events;

public sealed record UserRegistered(Guid UserId, string Email) : IDomainEvent;