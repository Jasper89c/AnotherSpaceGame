using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnotherSpaceGame.Areas.Manual.Pages.Viral
{
    public class ViralShipsModel : PageModel
    {
        private readonly ApplicationDbContext _Context;

        public ViralShipsModel(ApplicationDbContext context)
        {
            _Context = context;
        }

        public List<Ships> AllShips { get; set; }

        public void OnGet()
        {
            AllShips = _Context.Ships.Where(x => x.Id >= 270 && x.Id <= 274).ToList();
        }
    }
}
