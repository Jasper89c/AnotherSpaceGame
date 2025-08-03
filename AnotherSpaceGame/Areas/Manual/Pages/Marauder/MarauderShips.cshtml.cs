using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnotherSpaceGame.Areas.Manual.Pages.Marauder
{
    public class MarauderShipsModel : PageModel
    {
        private readonly ApplicationDbContext _Context;

        public MarauderShipsModel(ApplicationDbContext context)
        {
            _Context = context;
        }

        public List<Ships> AllShips { get; set; }

        public void OnGet()
        {
            AllShips = _Context.Ships.Where(x => x.Id >= 275 && x.Id <= 288).ToList();
        }
    }
}
