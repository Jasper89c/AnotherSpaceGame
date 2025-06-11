using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class ArtifactsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ArtifactsModel(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public List<Artifacts> UserArtifacts { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            UserArtifacts = await _context.Artifacts
            .Where(a => a.ApplicationUserId == user.Id)
            .OrderBy(a => a.ArtifactType)
            .ThenBy(a => a.ArtifactName)
            .Select(a => new Artifacts
            {
                Id = a.Id,
                ArtifactId = a.ArtifactId,
                ArtifactName = a.ArtifactName,
                ArtifactType = a.ArtifactType,
                Total = a.Total,
                MaxTotal = a.MaxTotal
            })
            .AsNoTracking()
            .ToListAsync();

            return Page();
        }
    }
}
