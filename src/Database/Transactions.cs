using Microsoft.EntityFrameworkCore;

public static class DBTransactions
{
    public static void AddSession(ProcessSession session)
    {
        using (var db = new CorsacTimeTrackerContext())
        {

            var processSession = db.Add(new ProcessSessionModel
            {
                Name = session.ProcessName,
                PID = Convert.ToInt32(session.ProcessId),
                Start = session.Start,
                End = session.Start,
                Subsessions = new List<TitleSessionModel>
            {
                new TitleSessionModel
                {
                    Title = session.Subsessions[0].Title,
                    Start = session.Subsessions[0].Start,
                    End = session.Subsessions[0].Start
                }
            }
            });
            db.SaveChanges();
        }
    }

    public static void UpdateSessionActivity(ProcessSession session, TitleActivityUpdate titleSessionInfo)
    {
        using (var db = new CorsacTimeTrackerContext())
        {
            var processSession = db.ProcessSessions.Include(ps => ps.Subsessions).First(ps => ps.PID == session.ProcessId);
            processSession.End = session.LastActivity;
            processSession.Subsessions.Last().End = titleSessionInfo.OldSession.LastActivity;
            if (titleSessionInfo.NewSession != null)
            {
                processSession.Subsessions.Add(new TitleSessionModel
                {
                    Title = titleSessionInfo.NewSession.Title,
                    Start = titleSessionInfo.NewSession.Start,
                    End = titleSessionInfo.NewSession.Start
                });
            }

            db.SaveChanges();
        }
    }

    public static void EndSession(ProcessSession session)
    {
        using (var db = new CorsacTimeTrackerContext())
        {
            var processSession = db.ProcessSessions.First(ps => ps.PID == session.ProcessId);
            processSession.End = (DateTime)session.End;
            processSession.Subsessions.Last().End = (DateTime)session.Subsessions.Last().End;
            db.SaveChanges();
        }
    }
}