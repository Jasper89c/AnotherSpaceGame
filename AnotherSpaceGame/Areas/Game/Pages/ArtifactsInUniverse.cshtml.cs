using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class ArtifactsInUniverseModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        // Static cache for all users (per app instance)
        private static List<ArtifactUniverseView> _cachedUniverseArtifacts;
        private static DateTime _lastCacheUpdate = DateTime.MinValue;
        private static readonly object _cacheLock = new();

        public ArtifactsInUniverseModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ArtifactUniverseView> UniverseArtifacts { get; set; }

        public async Task OnGetAsync()
        {
            // Only update cache if more than 24 hours have passed
            bool updateCache = false;
            lock (_cacheLock)
            {
                if (_cachedUniverseArtifacts == null || (DateTime.UtcNow - _lastCacheUpdate).TotalHours >= 24)
                {
                    updateCache = true;
                    _lastCacheUpdate = DateTime.UtcNow;
                }
            }

            if (updateCache)
            {
                var universeArtifacts = await _context.Artifacts
                    .GroupBy(a => new { a.ArtifactId, a.ArtifactName, a.ArtifactType, a.MaxTotal })
                    .Select(g => new ArtifactUniverseView
                    {
                        ArtifactId = g.Key.ArtifactId,
                        ArtifactName = g.Key.ArtifactName,
                        ArtifactType = g.Key.ArtifactType,
                        Total = g.Sum(x => x.Total),
                        MaxTotal = g.Key.MaxTotal
                    })
                    .OrderBy(a => a.ArtifactType)
                    .ThenBy(a => a.ArtifactName)
                    .ToListAsync();

                lock (_cacheLock)
                {
                    _cachedUniverseArtifacts = universeArtifacts;
                }
            }

            lock (_cacheLock)
            {
                UniverseArtifacts = _cachedUniverseArtifacts ?? new List<ArtifactUniverseView>();
            }
        }

        public class ArtifactUniverseView
        {
            public int ArtifactId { get; set; }
            public ArtifactName ArtifactName { get; set; }
            public ArtifactType ArtifactType { get; set; }
            public int Total { get; set; }
            public int MaxTotal { get; set; }
        }
    }
}
