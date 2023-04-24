using ProcessId = System.UInt32;


public class CorsacTimeTracker
{
    private Action<string> log;

    private ActiveWindowService ActiveWindowService { get; set; }
    private IdleTimeService IdleTimeService { get; set; }
    private IDictionary<ProcessId, ProcessSession> ActiveSessions { get; set; }

    private Timer pollingTimer;

    public CorsacTimeTracker(ActiveWindowService activeWindowService,
                             IdleTimeService idleTimeService,
                            Action<string> logAction)
    {
        log = logAction;
        ActiveWindowService = activeWindowService;
        IdleTimeService = idleTimeService;
        ActiveSessions = new Dictionary<ProcessId, ProcessSession>();

        var info = ActiveWindowService.GetActiveWindowInfo();

        var currentSession = new ProcessSession(info.ProcessId, info.ProcessName, info.Title);
        Add(currentSession);

        pollingTimer = new Timer((state) =>
        {
            // log(ActiveSessions.ToReadable());
            if (IdleTimeService.GetIdleTime() < 1000)
            {
                Poll();
            }
            PurgeStaleSessions();
        }, null, 0, 1000);
    }

    ~CorsacTimeTracker()
    {
        pollingTimer.Dispose();
        foreach (var session in ActiveSessions.Values)
        {
            session.Finish();
        }
    }

    private void PurgeStaleSessions()
    {
        foreach (var session in ActiveSessions.Values)
        {
            session.VerifyStale();
        }
        var endedSessions = ActiveSessions.Where(s => s.Value.Ended).Select((pair) => pair.Value);
        foreach (var endedSession in endedSessions){
            DBTransactions.EndSession(endedSession);
        }

        ActiveSessions = ActiveSessions.Where(s => !s.Value.Ended).ToDictionary(s => s.Key, s => s.Value);
    }

    private void Poll()
    {
        var info = ActiveWindowService.GetActiveWindowInfo();
        if (ActiveSessions.ContainsKey(info.ProcessId))
        {
            UpdateActivity(ActiveSessions[info.ProcessId], info);
        }
        else
        {
            var currentSession = new ProcessSession(info.ProcessId, info.ProcessName, info.Title);
            Add(currentSession);
        }
    }

    private void Add(ProcessSession session)
    {
        ActiveSessions.Add(session.ProcessId, session);
        DBTransactions.AddSession(session);
    }

    private void UpdateActivity(ProcessSession session, ActiveWindowInfo info)
    {
        TitleActivityUpdate titleSessionInfo = session.Activity(info.Title);
        DBTransactions.UpdateSessionActivity(session, titleSessionInfo);
    }

}

