using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class ExcavationDigModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly TurnService _turnService;

        public ExcavationDigModel(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            TurnService turnService)
        {
            _userManager = userManager;
            _context = context;
            _turnService = turnService;
        }

        public Planets Planet { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        [Range(1, 9, ErrorMessage = "Turns must be between 1 and 9.")]
        public int TurnsToUse { get; set; }

        public string DigMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            Planet = await _context.Planets.FirstOrDefaultAsync(p => p.Id == id && p.ApplicationUserId == user.Id);

            if (Planet == null || Planet.ApplicationUserId != user.Id)
            {
                return RedirectToPage("/ManageColonies", new { area = "Game" });
            }

            Id = id;
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            Planet = await _context.Planets.FirstOrDefaultAsync(p => p.Id == Id && p.ApplicationUserId == user.Id);
            if (Planet == null || Planet.ApplicationUserId != user.Id)
            {
                return RedirectToPage("/ManageColonies", new { area = "Game" });
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Deduct turns
            var turnResult = await _turnService.TryUseTurnsAsync(user.Id, TurnsToUse);
            if (!turnResult.Success)
            {
                ModelState.AddModelError(string.Empty, turnResult.Message);
                return Page();
            }

            // Determine artifact type based on current time
            var now = DateTime.Now;
            var artifactType = GetArtifactTypeForTime(now);

            // Get all possible artifact IDs for this type
            var possibleArtifactIds = GetArtifactIdsForType(artifactType).ToList();
            var foundArtifacts = new List<string>();
            int artifactsFound = 0;

            // Shuffle the list to ensure random selection each turn
            possibleArtifactIds = possibleArtifactIds.OrderBy(x => Random.Shared.Next()).ToList();

            for (int i = 0; i < TurnsToUse && possibleArtifactIds.Count > 0; i++)
            {
                // Each turn: 100% * artifactModifier chance
                if (Planet.ArtifactModifier >= 1.0m || Random.Shared.NextDouble() < (double)Planet.ArtifactModifier)
                {
                    // Pick and remove a random artifact from the remaining pool
                    int artifactId = possibleArtifactIds[0];
                    possibleArtifactIds.RemoveAt(0);

                    // Check if user already has this artifact
                    var artifact = await _context.Artifacts
                        .FirstOrDefaultAsync(a => a.ApplicationUserId == user.Id && a.ArtifactId == artifactId);

                    if (artifact == null)
                    {
                        // Create new artifact
                        artifact = new Artifacts(artifactId, 1, user.Id);
                        _context.Artifacts.Add(artifact);
                        foundArtifacts.Add($"{artifact.ArtifactName} (new)");
                    }
                    else
                    {
                        // Add to total
                        artifact.Total += 1;
                        if (artifact.Total > artifact.MaxTotal)
                            artifact.Total = artifact.MaxTotal;
                        foundArtifacts.Add($"{artifact.ArtifactName} (+1)");
                    }
                    artifactsFound++;
                }
            }

            await _context.SaveChangesAsync();

            string artifactList = artifactsFound > 0
                ? string.Join("<br>", foundArtifacts)
                : "No artifacts found.";

            DigMessage = $"You discovered {artifactsFound} {(artifactsFound == 1 ? "artifact" : "artifacts")} ({artifactType}):<br>{artifactList}<hr>{turnResult.Message}";
            TempData["DigMessage"] = DigMessage;
            return RedirectToPage(new { id = Planet.Id });
        }

        private ArtifactType GetArtifactTypeForTime(DateTime now)
        {
            int hour = now.Hour;
            int minute = now.Minute;

            // Common: 12:00-13:00, 3:00-4:00, 6:00-7:00, 9:00-10:00 (am/pm)
            if ((hour == 12 && minute >= 0 && minute < 60) ||
                (hour == 3 && minute >= 0 && minute < 60) ||
                (hour == 6 && minute >= 0 && minute < 60) ||
                (hour == 9 && minute >= 0 && minute < 60))
                return ArtifactType.Common;

            // Uncommon: 13:00-13:30, 4:00-4:30, 7:00-7:30, 10:00-10:30 (am/pm)
            if ((hour == 13 && minute >= 0 && minute < 30) ||
                (hour == 4 && minute >= 0 && minute < 30) ||
                (hour == 7 && minute >= 0 && minute < 30) ||
                (hour == 10 && minute >= 0 && minute < 30))
                return ArtifactType.Uncommon;

            // Rare: 13:30-13:45, 4:30-4:45, 7:30-7:45, 10:30-10:45 (am/pm)
            if ((hour == 13 && minute >= 30 && minute < 45) ||
                (hour == 4 && minute >= 30 && minute < 45) ||
                (hour == 7 && minute >= 30 && minute < 45) ||
                (hour == 10 && minute >= 30 && minute < 45))
                return ArtifactType.Rare;

            // Mirror for PM (add 12 to hour if hour < 12)
            int pmHour = (hour + 12) % 24;
            if ((pmHour == 12 && minute >= 0 && minute < 60) ||
                (pmHour == 15 && minute >= 0 && minute < 60) ||
                (pmHour == 18 && minute >= 0 && minute < 60) ||
                (pmHour == 21 && minute >= 0 && minute < 60))
                return ArtifactType.Common;
            if ((pmHour == 13 && minute >= 0 && minute < 30) ||
                (pmHour == 16 && minute >= 0 && minute < 30) ||
                (pmHour == 19 && minute >= 0 && minute < 30) ||
                (pmHour == 22 && minute >= 0 && minute < 30))
                return ArtifactType.Uncommon;
            if ((pmHour == 13 && minute >= 30 && minute < 45) ||
                (pmHour == 16 && minute >= 30 && minute < 45) ||
                (pmHour == 19 && minute >= 30 && minute < 45) ||
                (pmHour == 22 && minute >= 30 && minute < 45))
                return ArtifactType.Rare;

            // Default to Common if not in any range
            return ArtifactType.Common;
        }

        private IEnumerable<int> GetArtifactIdsForType(ArtifactType type)
        {
            // These should match your ArtifactName/ArtifactId enum values
            if (type == ArtifactType.Common)
                return Enumerable.Range(1, 16);
            if (type == ArtifactType.Uncommon)
                return Enumerable.Range(17, 12);
            if (type == ArtifactType.Rare)
                return Enumerable.Range(29, 10);
            return Enumerable.Empty<int>();
        }
    }
}
