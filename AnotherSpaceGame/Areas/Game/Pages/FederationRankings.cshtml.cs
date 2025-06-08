using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class FederationRankingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public FederationRankingsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Federations> FederationsList { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 20; // Federations per page

        public async Task OnGetAsync(int page = 1)
        {
            CurrentPage = page;
            int totalFeds = await _context.Federations.CountAsync();
            TotalPages = (int)System.Math.Ceiling(totalFeds / (double)PageSize);

            FederationsList = await _context.Federations
                .OrderByDescending(f => f.TotalPowerating)
                .ThenByDescending(f => f.TotalPlanets)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
