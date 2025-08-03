using Microsoft.AspNetCore.Mvc.RazorPages;
using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using System.Collections.Generic;
using System.Linq;

namespace AnotherSpaceGame.Areas.Manual.Pages.Other
{
    public class UltimateWeaponWinnersModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public UltimateWeaponWinnersModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<UWWinners> Winners { get; set; }

        public void OnGet()
        {
            Winners = _context.UWWinners
                .OrderByDescending(w => w.Date)
                .ToList();
        }
    }
}