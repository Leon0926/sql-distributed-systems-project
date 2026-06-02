using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class IndexModel : PageModel
{
    private readonly AppDbContext _app;
    private readonly LogDbContext _log;

    public IndexModel(AppDbContext app, LogDbContext log)
    {
        _app = app;
        _log = log;
    }

    public List<User> Users { get; set; } = new();
    public List<ActivityLog> Logs { get; set; } = new();

    public async Task OnGetAsync()
    {
        // Add a test user if none exist
        if (!await _app.Users.AnyAsync())
        {
            _app.Users.Add(new User { Name = "Leon", Email = "leon@bcit.ca" });
            await _app.SaveChangesAsync();
        }

        Users = await _app.Users.ToListAsync();

        // Log this visit
        _log.Logs.Add(new ActivityLog { Action = "Visited index page" });
        await _log.SaveChangesAsync();

        Logs = await _log.Logs.OrderByDescending(l => l.Timestamp).Take(5).ToListAsync();
    }
}