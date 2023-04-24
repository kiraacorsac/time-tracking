public class TitleSession : BaseSession
{
    public string Title { get; set; }

    public TitleSession(string title)
    {
        Title = title;
        Start = DateTime.Now;
        LastActivity = Start;
    }

    public void Activity(){
        LastActivity = DateTime.Now;
    }

    public void Finish(){
        End = LastActivity;
    }

    public override string ToString()
    {
        return $"[Title: {Title}; Length: {Length}]";
    }
}