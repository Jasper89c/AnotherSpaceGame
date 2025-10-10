using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    [Area("Game")]
    [Route("game/important-events")]
    [Route("game/important-events/{*slug}")]
    [Authorize]
    public class ImportantEventsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ImportantEventsModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IList<ImportantEvents> Events { get; set; } = new List<ImportantEvents>();
        [BindProperty(SupportsGet = true)]
        public string EventType { get; set; }
        // Pagination properties
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public ServerStats serverStats { get; set; }
        public DateTime UWTimer { get; set; }
        public UserProjects userProject { get; set; }
        public List<UserProjects> userProjectList { get; set; }
        public List<string> Usernames = new List<string>();

        public async Task<IActionResult> OnGetAsync()
        {
            serverStats = _context.ServerStats.FirstOrDefault();
            if (serverStats.UWEnabled == true)
            {
                userProject = _context.UserProjects.FirstOrDefault(x => x.ApplicationUserId == serverStats.UWHolderId);
                UWTimer = userProject.KalZulLoktarUnlockTimer;
            }
            userProjectList = _context.UserProjects.Where(x => x.KalZulHektar == true).ToList();
            foreach (var u in userProjectList)
            {
                var user1 = await _userManager.FindByIdAsync(u.ApplicationUserId);
                Usernames.Add(user1.UserName);
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            if (user != null)
            {
                var query = _context.ImportantEvents
                .Where(e => e.ApplicationUserId == user.Id);


                if (!string.IsNullOrEmpty(EventType))
                {
                    if (Enum.TryParse<ImportantEventTypes>(EventType, out var eventTypeEnum))
                    {
                        query = query.Where(e => e.ImportantEventTypes == eventTypeEnum);
                    }
                }
                query = query.OrderByDescending(e => e.DateAndTime);
                int totalEvents = await Task.Run(() => query.Count());
                TotalPages = (int)System.Math.Ceiling(totalEvents / (double)PageSize);

                Events = await Task.Run(() =>
                    query
                        .Skip((PageNumber - 1) * PageSize)
                        .Take(PageSize)
                        .ToList()
                );
            }
            else
            {
                Events = new List<ImportantEvents>();
                TotalPages = 1;
            }
            return Page();
        }

        public int UWProgressPercent
        {
            get
            {
                if (serverStats == null || !serverStats.UWEnabled || UWTimer == default)
                    return 0;

                // Set your event's total duration here (in minutes)
                var totalDuration = 2880; // e.g., 60 minutes

                var now = DateTime.Now;
                var end = UWTimer;
                var start = end.AddMinutes(-totalDuration);

                var elapsed = (now - start).TotalMinutes;
                var percent = (int)((elapsed / totalDuration) * 100);

                if (percent < 0) percent = 0;
                if (percent > 100) percent = 100;
                return percent;
            }
        }
    }
}
