using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnotherSpaceGame.Areas.Manual.Pages.AMiner
{
    public class AMinerShipsModel : PageModel
    {
        private readonly ApplicationDbContext _Context;

        public AMinerShipsModel(ApplicationDbContext context)
        {
            _Context = context;
        }

        public List<Ships> AllShips { get; set; }

        public void OnGet()
        {
            AllShips = _Context.Ships.Where(x => x.Id >= 237 && x.Id <= 256).ToList();
        }
    }
}
