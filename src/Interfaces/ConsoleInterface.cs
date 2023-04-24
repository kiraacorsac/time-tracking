class Program
{
    static void Main(string[] args)
    {
        var activeWindowService = new ActiveWindowService();
        var idleTimeService = new IdleTimeService();
        var logAction = (string s) =>
        {
            Console.Write("=> ");
            Console.WriteLine(s);
        };

        new CorsacTimeTracker(activeWindowService, idleTimeService, logAction);
        Console.ReadKey();
    }
}