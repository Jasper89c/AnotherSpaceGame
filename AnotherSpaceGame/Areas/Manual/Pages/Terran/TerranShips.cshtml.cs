using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnotherSpaceGame.Areas.Manual.Pages.Terran
{
    public class TerranShipsModel : PageModel
    {
        private readonly ApplicationDbContext _Context;

        public TerranShipsModel(ApplicationDbContext context)
        {
            _Context = context;
        }

        public List<Ships> AllShips { get; set; }

        public void OnGet()
        {
            AllShips = _Context.Ships.Where(x => x.Id >= 219 && x.Id <= 236).ToList();
        }
    }
}
