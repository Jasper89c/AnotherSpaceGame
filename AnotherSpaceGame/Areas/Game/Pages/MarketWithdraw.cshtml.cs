using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Data;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class MarketWithdrawModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MarketWithdrawModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<MarketPosts> UserMarketPosts { get; set; }

        [BindProperty]
        public List<int> WithdrawPostIds { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            UserMarketPosts = await _context.MarketPosts
                .Where(p => p.ApplicationUserId == user.Id)
                .OrderBy(p => p.DateTime)
                .ToListAsync();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostBatchWithdrawAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (WithdrawPostIds == null || !WithdrawPostIds.Any())
            {
                TempData["MarketWithdrawError"] = "No posts selected for withdrawal.";
                return RedirectToPage();
            }

            var posts = await _context.MarketPosts
                .Where(p => WithdrawPostIds.Contains(p.Id) && p.ApplicationUserId == user.Id)
                .ToListAsync();

            if (!posts.Any())
            {
                TempData["MarketWithdrawError"] = "No valid posts found for withdrawal.";
                return RedirectToPage();
            }

            var userCommodities = await _context.Commodities.FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);

            int totalWithdrawn = 0;
            foreach (var post in posts)
            {
                if (userCommodities != null)
                {
                    AddCommodity(userCommodities, post.MarketType, post.Amount);
                }
                totalWithdrawn++;
                _context.MarketPosts.Remove(post);
            }

            await _context.SaveChangesAsync();

            TempData["MarketWithdrawSuccess"] = $"Successfully withdrew {totalWithdrawn} market post(s).";
            return RedirectToPage();
        }

        private void AddCommodity(Commodities c, MarketType type, int value)
        {
            switch (type)
            {
                case MarketType.Food: c.Food += value; break;
                case MarketType.Ore: c.Ore += value; break;
                case MarketType.RawMaterial: c.RawMaterial += value; break;
                case MarketType.ConsumerGoods: c.ConsumerGoods += value; break;
                case MarketType.TerranMetal: c.TerranMetal += value; break;
                case MarketType.RedCrystal: c.RedCrystal += value; break;
                case MarketType.WhiteCrystal: c.WhiteCrystal += value; break;
                case MarketType.Rutile: c.Rutile += value; break;
                case MarketType.Composite: c.Composite += value; break;
                case MarketType.StrafezOrganism: c.StrafezOrganism += value; break;
            }
        }
    }
}
