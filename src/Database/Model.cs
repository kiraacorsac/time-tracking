using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

public class CorsacTimeTrackerContext : DbContext
{
    public DbSet<SessionModel> Sessions { get; set; }
    public DbSet<TitleModel> Titles { get; set; }

    public string DbPath { get; }

    public CorsacTimeTrackerContext()
    {
        // var folder = Environment.SpecialFolder.LocalApplicationData;
        // var path = Environment.GetFolderPath(folder);
        var path = Directory.GetCurrentDirectory();
        DbPath = System.IO.Path.Join(path, "timetracking.sqlite");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SessionModel>()
            .HasMany(p => p.Titles)
            .WithOne(t => t.Session)
            .OnDelete(DeleteBehavior.Cascade);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

[Table("Sessions")]
public class SessionModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }
    public string Name { get; set; }
    public int PID { get; set; }

    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public List<TitleModel> Titles { get; set; } = new();
}

[Table("Titles")]
public class TitleModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int SessionId { get; set; }
    public SessionModel Session { get; set; }

}