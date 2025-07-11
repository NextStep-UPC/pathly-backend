﻿namespace pathly_backend.IAM.Infrastructure.Persistence;

public class OutboxMessage
{
    public Guid Id              { get; set; }
    public DateTime OccurredOnUtc { get; set; }
    public string Type          { get; set; } = default!;
    public string Content       { get; set; } = default!;
    public DateTime? ProcessedOnUtc { get; set; }
    public string? Error        { get; set; }
}