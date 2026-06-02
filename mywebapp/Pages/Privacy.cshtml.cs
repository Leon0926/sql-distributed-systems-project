using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace mywebapp.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly LogDbContext _log;

        public PrivacyModel(LogDbContext log)
        {
            _log = log;
        }

        public List<ActivityLog> Logs { get; set; } = new();

        public async Task OnGetAsync()
        {
            _log.Logs.Add(new ActivityLog { Action = "Visited Privacy page" });
            await _log.SaveChangesAsync();

            Logs = await _log.Logs.OrderByDescending(l => l.Timestamp).Take(5).ToListAsync();
        }
    }

}
