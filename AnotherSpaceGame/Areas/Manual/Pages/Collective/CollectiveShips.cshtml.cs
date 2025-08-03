using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnotherSpaceGame.Areas.Manual.Pages.Collective
{
    public class CollectiveShipsModel : PageModel
    {
        private readonly ApplicationDbContext _Context;

        public CollectiveShipsModel(ApplicationDbContext context)
        {
            _Context = context;
        }

        public List<Ships> AllShips { get; set; }

        public void OnGet()
        {
            AllShips = _Context.Ships.Where(x => x.Id >= 257 && x.Id <= 269).ToList();
        }
    }
}
