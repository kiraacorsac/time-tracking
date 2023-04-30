using ProcessId = System.UInt32;

public class TitleActivityUpdate
{
    public Title NewActivity { get; set; }
    public Title OldActivity { get; set; }
}

public class Session : BaseSession
{
    public string ProcessName { get; set; }
    public ProcessId ProcessId { get; set; }

    public IList<Title> Titles { get; set; }


    public Session(ProcessId processId, string processName, string? maybeTitle)
    {
        Start = DateTime.Now;
        LastActivity = Start;
        ProcessId = processId;
        ProcessName = processName;

        Titles = new List<Title>();
        var title = new Title(maybeTitle ?? "");
        Titles.Add(title);
    }

    public TitleActivityUpdate Activity(string? maybeTitle)
    {
        LastActivity = DateTime.Now;

        var title = maybeTitle ?? "";
        var lastTitle = Titles.Last();
        if (lastTitle.WindowTitle == title)
        {
            lastTitle.Activity();
            return new TitleActivityUpdate
            {
                OldActivity = lastTitle
            };
        }
        else
        {
            lastTitle.Finish();
            var newTitle = new Title(title);
            Titles.Add(newTitle);
            return new TitleActivityUpdate
            {
                OldActivity = lastTitle,
                NewActivity = newTitle
            };
        }
    }

    public void Finish()
    {
        End = LastActivity;
    }

    public bool VerifyStale()
    {
        if ((DateTime.Now - LastActivity).TotalSeconds > Config.StaleSessionTresholdSeconds)
        {
            Titles.Last().Finish();
            Finish();
        }
        return Ended;
    }

    override public string ToString()
    {
        return $"[ProcessName: {ProcessName}; LastActivity: {DateTime.Now - LastActivity}; Length: {Length}; Ended {Ended}; Titles: {Titles.ToReadable()}]";
    }
}