using Microsoft.EntityFrameworkCore;

public static class DBTransactions
{
    public static int AddSession(Session session)
    {
        using (var db = new CorsacTimeTrackerContext())
        {
            var dbSession = new SessionModel
            {
                Name = session.ProcessName,
                PID = Convert.ToInt32(session.ProcessId),
                Start = session.Start,
                End = session.Start,
                Titles = new List<TitleModel>
            {
                new TitleModel
                {
                    Title = session.Titles[0].WindowTitle,
                    Start = session.Titles[0].Start,
                    End = session.Titles[0].Start
                }
            }
            };
            db.Add(dbSession);
            db.SaveChanges();
            return dbSession.Id;
        }
    }

    public static void UpdateSessionActivity(Session session, TitleActivityUpdate titleInfo)
    {
        using (var db = new CorsacTimeTrackerContext())
        {
            var sessionModel = db.Sessions.Include(ps => ps.Titles).First(ps => ps.Id == session.Id);
            sessionModel.End = session.LastActivity;
            sessionModel.Titles.Last().End = titleInfo.OldActivity.LastActivity;
            if (titleInfo.NewActivity != null)
            {
                sessionModel.Titles.Add(new TitleModel
                {
                    Title = titleInfo.NewActivity.WindowTitle,
                    Start = titleInfo.NewActivity.Start,
                    End = titleInfo.NewActivity.Start,
                    Session = sessionModel
            });
            }

            db.SaveChanges();
        }
    }

    public static void EndSession(Session session)
    {
        using (var db = new CorsacTimeTrackerContext())
        {
            var sessionModel = db.Sessions.Include(s => s.Titles).First(s => s.Id == session.Id);
            sessionModel.End = (DateTime)session.End;
            sessionModel.Titles.Last().End = (DateTime)session.Titles.Last().End;

            if(((DateTime)session.End - sessionModel.Start).TotalSeconds < Config.MinimumSessionSeconds) {
                db.Sessions.Remove(sessionModel);
            }
            db.SaveChanges();
        }
    }
}