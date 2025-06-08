using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class RankingRankNearMeModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private static readonly int[] AllowedPageSizes = { 20, 40, 60, 80, 100 };

        public RankingRankNearMeModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<UserViewModel> Users { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 20;
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int CurrentUserRank { get; set; }
        public int TotalUsers { get; set; }
        public string CurrentUserId { get; set; }
        public double CurrentUserPowerRating { get; set; }

        public void OnGet(int? page)
        {
            // Validate PageSize
            if (!AllowedPageSizes.Contains(PageSize))
                PageSize = 20;

            // 1. Get all users and determine current user's rank and power rating
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

            // Find current user
            CurrentUserId = User.Identity?.IsAuthenticated == true
                ? _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name)?.Id
                : null;

            // Find current user's rank and power rating
            CurrentUserRank = -1;
            CurrentUserPowerRating = 0;
            if (CurrentUserId != null)
            {
                for (int i = 0; i < allUsers.Count; i++)
                {
                    if (allUsers[i].Id == CurrentUserId)
                    {
                        CurrentUserRank = i + 1;
                        CurrentUserPowerRating = allUsers[i].PowerRating;
                        break;
                    }
                }
            }

            // 2. Get all NPCs
            var allNPCs = _context.NPCs
                .AsNoTracking()
                .Select(n => new UserViewModel
                {
                    Id = "NPC_" + n.Id, // Prefix to avoid collision with user IDs
                    UserName = n.UserName,
                    Faction = n.Faction.ToString(),
                    TotalPlanets = n.TotalPlanets,
                    PowerRating = n.PowerRating,
                    IsNPC = true
                })
                .ToList();

            // 3. Merge users and NPCs, then sort
            var allEntities = allUsers.Concat(allNPCs)
                .OrderByDescending(e => e.PowerRating)
                .ThenByDescending(e => e.TotalPlanets)
                .ToList();

            // 4. Update total count and pagination
            TotalUsers = allEntities.Count;
            if (CurrentUserRank > 0)
                CurrentPage = ((CurrentUserRank - 1) / PageSize) + 1;
            else if (page.HasValue)
                CurrentPage = page.Value;
            else
                CurrentPage = 1;

            TotalPages = (int)Math.Ceiling(TotalUsers / (double)PageSize);

            // 5. Get entities for the current page
            Users = allEntities
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            // 6. Calculate PR Ratio for each entity
            foreach (var user in Users)
            {
                if (CurrentUserPowerRating > 0)
                {
                    user.PrRatio = ((user.PowerRating - CurrentUserPowerRating) / CurrentUserPowerRating) * 100.0;
                }
                else
                {
                    user.PrRatio = 0;
                }
            }
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
