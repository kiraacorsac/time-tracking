public class Title : BaseSession
{
    public string WindowTitle { get; set; }

    public Title(string title)
    {
        WindowTitle = title;
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
        return $"[Title: {WindowTitle}; Length: {Length}]";
    }
}