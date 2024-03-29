public abstract class BaseSession
{
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime? End { get; set; }
    public DateTime LastActivity { get; set; }
    public TimeSpan Length { get => LastActivity - Start; }
    public bool Ended { get => End != null; }
}