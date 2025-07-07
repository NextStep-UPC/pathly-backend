using System.Collections.Generic;
using pathly_backend.IAM.Domain.Events;
using pathly_backend.Shared.Common;

namespace pathly_backend.IAM.Domain;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}