using System;
using pathly_backend.Sessions.Domain.Enums;
using pathly_backend.Shared.Common;

namespace pathly_backend.Sessions.Domain.Entities;

public class Session : Entity
{
    public Guid     StudentId       { get; private set; }
    public Guid?    PsychologistId  { get; private set; }
    public DateTime StartsAtUtc     { get; private set; }
    public DateTime? AssignedAtUtc { get; private set; }
    public DateTime? EndsAtUtc      { get; private set; }
    public SessionState State       { get; private set; }
    public string?  CancelReason    { get; private set; }

    public Session(Guid studentId, DateTime startsAtUtc)
    {
        Id            = Guid.NewGuid();
        StudentId     = studentId;
        StartsAtUtc   = startsAtUtc;
        State         = SessionState.Pending;
    }

    public void AssignPsychologist(Guid psychologistId)
    {
        if (State != SessionState.Pending)
            throw new InvalidOperationException("Solo sesiones pendientes pueden ser tomadas.");
        PsychologistId = psychologistId;
        AssignedAtUtc = DateTime.UtcNow;
        State = SessionState.Confirmed;
    }
    public void Cancel(string reason)
    {
        if (PsychologistId is not null)
            throw new InvalidOperationException("No se puede cancelar: ya hay un psicólogo asignado");
        CancelReason = reason;
        State        = SessionState.Cancelled;
    }

    public void Finish()
    {
        if (State != SessionState.Confirmed)
            throw new InvalidOperationException("Solo las sesiones confirmadas se pueden finalizar");
        EndsAtUtc = DateTime.UtcNow;
        State     = SessionState.Completed;
    }
}