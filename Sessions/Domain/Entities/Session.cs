using pathly_backend.Sessions.Domain.Enums;

namespace pathly_backend.Sessions.Domain.Entities;

public class Session
{
    public Guid Id              { get; private set; }
    public Guid StudentId       { get; private set; }
    public Guid PsychologistId  { get; private set; }
    public DateTime StartsAtUtc { get; private set; }
    public DateTime EndsAtUtc   { get; private set; }
    public SessionState State   { get; private set; }

    private Session() { }

    public Session(Guid student, Guid psychologist,
        DateTime startsAt, DateTime endsAt)
    {
        Id             = Guid.NewGuid();
        StudentId      = student;
        PsychologistId = psychologist;
        StartsAtUtc    = startsAt.ToUniversalTime();
        EndsAtUtc      = endsAt.ToUniversalTime();
        State          = SessionState.Pending;
    }

    public void Confirm() => State = SessionState.Confirmed;
    public void Finish()  => State = SessionState.Finished;
}