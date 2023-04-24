using ProcessId = System.UInt32;

public class TitleActivityUpdate
{
    public TitleSession NewSession { get; set; }
    public TitleSession OldSession { get; set; }
}

public class ProcessSession : BaseSession
{
    public bool IsLongEnough { get => Ended && (this.End - this.Start)?.TotalSeconds > 30; }

    public string ProcessName { get; set; }
    public ProcessId ProcessId { get; set; }

    public IList<TitleSession> Subsessions { get; set; }


    public ProcessSession(ProcessId processId, string processName, string? maybeTitle)
    {
        Start = DateTime.Now;
        LastActivity = Start;
        ProcessId = processId;
        ProcessName = processName;

        Subsessions = new List<TitleSession>();
        TitleSession titleSession;
        var title = maybeTitle ?? "";
        titleSession = new TitleSession(title);
        Subsessions.Add(titleSession);
    }

    public TitleActivityUpdate Activity(string? maybeTitle)
    {
        LastActivity = DateTime.Now;

        var title = maybeTitle ?? "";
        var lastTitleSession = Subsessions.Last();
        if (lastTitleSession.Title == title)
        {
            lastTitleSession.Activity();
            return new TitleActivityUpdate
            {
                OldSession = lastTitleSession
            };
        }
        else
        {
            lastTitleSession.Finish();
            var newTitleSession = new TitleSession(title);
            Subsessions.Add(newTitleSession);
            return new TitleActivityUpdate
            {
                OldSession = lastTitleSession,
                NewSession = newTitleSession
            };
        }
    }

    public void Finish()
    {
        End = LastActivity;
    }

    public bool VerifyStale()
    {
        if ((DateTime.Now - LastActivity).TotalSeconds > 30)
        {
            Subsessions.Last().Finish();
            Finish();
        }
        return Ended;
    }

    override public string ToString()
    {
        return $"[ProcessName: {ProcessName}; LastActivity: {DateTime.Now - LastActivity}; Length: {Length}; Ended {Ended}; Titles: {Subsessions.ToReadable()}]";
    }
}