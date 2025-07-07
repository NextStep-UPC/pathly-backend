using pathly_backend.Shared.Common;

namespace pathly_backend.IAM.Domain.Events;

public sealed record RoleGranted(Guid UserId, string NewRole) : IDomainEvent;