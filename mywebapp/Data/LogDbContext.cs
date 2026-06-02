using Microsoft.EntityFrameworkCore;

public class LogDbContext : DbContext
{
    public LogDbContext(DbContextOptions<LogDbContext> options) : base(options) { }
    public DbSet<ActivityLog> Logs { get; set; }
}

public class ActivityLog
{
    public int Id { get; set; }
    public string Action { get; set; } = "";
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}