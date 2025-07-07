using pathly_backend.Shared.Common;

namespace pathly_backend.IAM.Domain.Events;

public sealed record RoleRequested(Guid UserId, string RequestedRole) : IDomainEvent;