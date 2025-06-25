using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;
using X.PagedList.Extensions;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class RankingRankNearMeModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private const int DefaultPageSize = 20;

        public RankingRankNearMeModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IPagedList<UserViewModel> Users { get; set; }
        public int CurrentUserRank { get; set; }
        public int TotalUsers { get; set; }
        public string CurrentUserId { get; set; }
        public double CurrentUserPowerRating { get; set; }
        public int PageNumber { get; set; }

        public async Task<IActionResult> OnGetAsync(int? pg)
        {
            PageNumber = (pg.HasValue && pg.Value > 0) ? pg.Value : 1;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            var allUsers = _context.Users
                .AsNoTracking()
                .OrderByDescending(u => u.PowerRating)
                .ThenByDescending(u => u.TotalPlanets)
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Faction = u.Faction.ToString(),
                    TotalPlanets = u.TotalPlanets,
                    PowerRating = u.PowerRating,
                    IsNPC = false
                })
                .ToList();

            TotalUsers = allUsers.Count;

            var allNPCs = _context.NPCs
                .AsNoTracking()
                .Select(n => new UserViewModel
                {
                    Id = "NPC_" + n.Id,
                    UserName = n.UserName,
                    Faction = n.Faction.ToString(),
                    TotalPlanets = 1,
                    PowerRating = n.PowerRating,
                    IsNPC = true
                })
                .ToList();

            var allEntities = allUsers.Concat(allNPCs)
                .OrderByDescending(e => e.PowerRating)
                .ThenByDescending(e => e.TotalPlanets)
                .ToList();
            CurrentUserPowerRating = user.PowerRating;
            foreach (var player in allEntities)
            {
                if (CurrentUserPowerRating > 0)
                {
                    player.PrRatio = ((player.PowerRating - CurrentUserPowerRating) / CurrentUserPowerRating) * 100.0;
                }
                else
                {
                    player.PrRatio = 0;
                }
            }

            Users = allEntities.ToPagedList(PageNumber, DefaultPageSize);
            return Page();
        }

        public class UserViewModel
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Faction { get; set; }
            public int TotalPlanets { get; set; }
            public double PowerRating { get; set; }
            public double PrRatio { get; set; }
            public bool IsNPC { get; set; }
        }
    }
}
