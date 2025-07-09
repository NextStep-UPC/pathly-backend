using System;

namespace pathly_backend.VocationalTests.Application.Dtos
{
    public record StudentTestStartDto(Guid StudentTestId, DateTime StartedAtUtc);
}