using pathly_backend.Sessions.Domain.Enums;
using pathly_backend.Shared.Common;

namespace pathly_backend.Sessions.Domain.Entities;

public class Session : Entity
{
    public Guid StudentId { get; private set; }
    public Guid? PsychologistId { get; private set; } // nullable ahora
    public DateTime StartsAtUtc { get; private set; }
    public DateTime EndsAtUtc { get; private set; }
    public SessionState State { get; private set; }
    public string? CancelReason { get; private set; }

    public Session(Guid studentId, DateTime startsAtUtc, DateTime endsAtUtc)
    {
        Id = Guid.NewGuid();
        StudentId = studentId;
        StartsAtUtc = startsAtUtc;
        EndsAtUtc = endsAtUtc;
        State = SessionState.Pending;
    }

    public void AssignPsychologist(Guid psychologistId)
    {
        if (PsychologistId is not null)
            throw new InvalidOperationException("La sesión ya tiene un psicólogo asignado");

        PsychologistId = psychologistId;
        State = SessionState.Confirmed;
    }

    public void Cancel(string reason)
    {
        if (PsychologistId is not null)
            throw new InvalidOperationException("No se puede cancelar: ya hay un psicólogo asignado");

        CancelReason = reason;
        State = SessionState.Cancelled;
    }

    public void Finish()
    {
        if (State != SessionState.Confirmed)
            throw new InvalidOperationException("Solo las sesiones confirmadas se pueden finalizar");

        State = SessionState.Completed;
    }
}