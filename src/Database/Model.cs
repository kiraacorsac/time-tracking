using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

public class CorsacTimeTrackerContext : DbContext
{
    public DbSet<ProcessSessionModel> ProcessSessions { get; set; }
    public DbSet<TitleSessionModel> TitleSessions { get; set; }

    public string DbPath { get; }

    public CorsacTimeTrackerContext()
    {
        // var folder = Environment.SpecialFolder.LocalApplicationData;
        // var path = Environment.GetFolderPath(folder);
        var path = Directory.GetCurrentDirectory();
        DbPath = System.IO.Path.Join(path, "database.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

[Table("ProcessSessions")]
public class ProcessSessionModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int PID { get; set; }

    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public List<TitleSessionModel> Subsessions { get; set; } = new();
}

[Table("TitleSessions")]
public class TitleSessionModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}