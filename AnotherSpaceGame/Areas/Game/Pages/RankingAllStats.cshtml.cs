using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class RankingAllStatsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        // Static cache and lock for hourly update
        private static DateTime _lastUpdate = DateTime.MinValue;
        private static readonly object _lock = new();
        private static CachedRankings _cachedRankings;
        public DateTime LastUpdate => _lastUpdate;

        public RankingAllStatsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<EmpireStatsViewModel> BattleRatio { get; set; }
        public List<EmpireStatsViewModel> BattlesWon { get; set; }
        public List<EmpireStatsViewModel> BattlesLost { get; set; }
        public List<EmpireStatsViewModel> RichestEmpire { get; set; }
        public List<EmpireStatsViewModel> PlanetsRatio { get; set; }
        public List<EmpireStatsViewModel> PlanetsWon { get; set; }
        public List<EmpireStatsViewModel> PlanetsLost { get; set; }
        public List<EmpireStatsViewModel> PlanetsExplored { get; set; }
        public List<EmpireStatsViewModel> PlanetsPlundered { get; set; }

        public void OnGet()
        {
            lock (_lock)
            {
                if (_cachedRankings == null || (DateTime.UtcNow - _lastUpdate).TotalMinutes >= 60)
                {
                    var users = _context.Users
                        .AsNoTracking()
                        .Include(u => u.Commodities)
                        .Select(u => new EmpireStatsViewModel
                        {
                            Username = u.UserName,
                            Faction = u.Faction.ToString(),
                            TotalPlanets = u.TotalPlanets,
                            PowerRating = u.PowerRating,
                            BattlesWon = u.BattlesWon,
                            BattlesLost = u.BattlesLost,
                            Credits = u.Commodities.Credits,
                            ColoniesExplored = u.ColoniesExplored,
                            PlanetsPlundered = u.PlanetsPlundered,
                            PlanetsWon = u.ColoniesWon,
                            PlanetsLost = u.ColoniesLost,
                            PlanetsExplored = u.ColoniesExplored,
                            PlanetsPlunderedStat = u.PlanetsPlundered
                        })
                        .ToList();

                    _cachedRankings = new CachedRankings
                    {
                        BattleRatio = users
                            .OrderByDescending(u => u.BattleRatio)
                            .ThenByDescending(u => u.BattlesWon)
                            .Take(20)
                            .ToList(),

                        BattlesWon = users
                            .OrderByDescending(u => u.BattlesWon)
                            .ThenByDescending(u => u.PowerRating)
                            .Take(20)
                            .ToList(),

                        BattlesLost = users
                            .OrderByDescending(u => u.BattlesLost)
                            .ThenByDescending(u => u.PowerRating)
                            .Take(20)
                            .ToList(),

                        RichestEmpire = users
                            .OrderByDescending(u => u.Credits)
                            .ThenByDescending(u => u.PowerRating)
                            .Take(20)
                            .ToList(),

                        PlanetsRatio = users
                            .OrderByDescending(u => u.PlanetsRatio)
                            .ThenByDescending(u => u.ColoniesExplored)
                            .Take(20)
                            .ToList(),

                        PlanetsWon = users
                            .OrderByDescending(u => u.PlanetsWon)
                            .ThenByDescending(u => u.PowerRating)
                            .Take(20)
                            .ToList(),

                        PlanetsLost = users
                            .OrderByDescending(u => u.PlanetsLost)
                            .ThenByDescending(u => u.PowerRating)
                            .Take(20)
                            .ToList(),

                        PlanetsExplored = users
                            .OrderByDescending(u => u.PlanetsExplored)
                            .ThenByDescending(u => u.PowerRating)
                            .Take(20)
                            .ToList(),

                        PlanetsPlundered = users
                            .OrderByDescending(u => u.PlanetsPlunderedStat)
                            .ThenByDescending(u => u.PowerRating)
                            .Take(20)
                            .ToList()
                    };

                    _lastUpdate = DateTime.UtcNow;
                }

                BattleRatio = _cachedRankings.BattleRatio;
                BattlesWon = _cachedRankings.BattlesWon;
                BattlesLost = _cachedRankings.BattlesLost;
                RichestEmpire = _cachedRankings.RichestEmpire;
                PlanetsRatio = _cachedRankings.PlanetsRatio;
                PlanetsWon = _cachedRankings.PlanetsWon;
                PlanetsLost = _cachedRankings.PlanetsLost;
                PlanetsExplored = _cachedRankings.PlanetsExplored;
                PlanetsPlundered = _cachedRankings.PlanetsPlundered;
            }
        }

        public class EmpireStatsViewModel
        {
            public string Username { get; set; }
            public string Faction { get; set; }
            public int TotalPlanets { get; set; }
            public double PowerRating { get; set; }
            public int BattlesWon { get; set; }
            public int BattlesLost { get; set; }
            public double BattleRatio => BattlesLost > 0 ? (double)BattlesWon / BattlesLost : BattlesWon;
            public long Credits { get; set; }
            public int ColoniesExplored { get; set; }
            public int PlanetsPlundered { get; set; }
            public int PlanetsWon { get; set; }
            public int PlanetsLost { get; set; }
            public int PlanetsExplored { get; set; }
            public int PlanetsPlunderedStat { get; set; }
            public double PlanetsRatio => PlanetsPlundered > 0 ? (double)ColoniesExplored / PlanetsPlundered : ColoniesExplored;
        }

        private class CachedRankings
        {
            public List<EmpireStatsViewModel> BattleRatio { get; set; }
            public List<EmpireStatsViewModel> BattlesWon { get; set; }
            public List<EmpireStatsViewModel> BattlesLost { get; set; }
            public List<EmpireStatsViewModel> RichestEmpire { get; set; }
            public List<EmpireStatsViewModel> PlanetsRatio { get; set; }
            public List<EmpireStatsViewModel> PlanetsWon { get; set; }
            public List<EmpireStatsViewModel> PlanetsLost { get; set; }
            public List<EmpireStatsViewModel> PlanetsExplored { get; set; }
            public List<EmpireStatsViewModel> PlanetsPlundered { get; set; }
        }
    }
}
