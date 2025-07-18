﻿namespace pathly_backend.Shared.Common;

public abstract class Entity
{
    public Guid Id { get; protected set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
    }
}