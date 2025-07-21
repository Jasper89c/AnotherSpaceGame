using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class RankingTopEmpiresModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private static List<EmpireViewModel> _cachedEmpires;
        private static DateTime _lastUpdate = DateTime.MinValue;
        private static readonly object _lock = new();
        public DateTime LastUpdate => _lastUpdate;
        public RankingTopEmpiresModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<EmpireViewModel> TopEmpires { get; set; }

        public EmpireViewModel CurrentUserEmpire { get; set; }
        public int? CurrentUserRank { get; set; }
        public int TotalEmpires { get; set; }

        public void OnGet()
        {
            lock (_lock)
            {
                if (_cachedEmpires == null || (DateTime.UtcNow - _lastUpdate).TotalMinutes >= 60)
                {
                    _cachedEmpires = _context.Users
                        .AsNoTracking()
                        .OrderByDescending(u => u.PowerRating)
                        .ThenByDescending(u => u.TotalPlanets)
                        .Take(50)
                        .Select(u => new EmpireViewModel
                        {
                            Id = u.Id,
                            UserName = u.UserName,
                            Faction = u.Faction.ToString(),
                            TotalPlanets = u.TotalPlanets,
                            PowerRating = u.PowerRating,
                            LastActivity = u.LastAction
                        })
                        .ToList();

                    _lastUpdate = DateTime.UtcNow;
                }
                TopEmpires = _cachedEmpires;
            }

            // Calculate current user's rank and stats
            var userId = User.Identity?.IsAuthenticated == true ? _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name)?.Id : null;
            var allEmpires = _context.Users
                .AsNoTracking()
                .OrderByDescending(u => u.PowerRating)
                .ThenByDescending(u => u.TotalPlanets)
                .Select(u => new EmpireViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Faction = u.Faction.ToString(),
                    TotalPlanets = u.TotalPlanets,
                    PowerRating = u.PowerRating,
                    LastActivity = u.LastAction,
                    DamageProtection = u.DamageProtection
                })
                .ToList();

            TotalEmpires = allEmpires.Count;

            if (userId != null)
            {
                var userIndex = allEmpires.FindIndex(e => e.Id == userId);
                if (userIndex >= 0)
                {
                    CurrentUserRank = userIndex + 1;
                    CurrentUserEmpire = allEmpires[userIndex];
                }
            }
        }

        public class EmpireViewModel
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Faction { get; set; }
            public int TotalPlanets { get; set; }
            public double PowerRating { get; set; }
            public DateTime LastActivity { get; set; }
            public DateTime DamageProtection { get; set; }
        }
    }
}
