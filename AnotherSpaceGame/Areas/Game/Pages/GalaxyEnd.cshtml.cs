using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class GalaxyEndModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public GalaxyEndModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public GalaxyEnd? LatestGalaxyEnd { get; set; }

        public void OnGet()
        {
            LatestGalaxyEnd = _context.GalaxyEnd
                .OrderByDescending(g => g.CreatedAt)
                .FirstOrDefault();
        }
    }
}