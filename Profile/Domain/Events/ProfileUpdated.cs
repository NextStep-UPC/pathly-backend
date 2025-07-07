using pathly_backend.Shared.Common;

namespace pathly_backend.Profile.Domain.Events;

public record ProfileUpdated(Guid UserId) : IDomainEvent;