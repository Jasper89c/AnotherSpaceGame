using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AnotherSpaceGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnotherSpaceGame.Data;
using Microsoft.AspNetCore.Identity;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class MarketModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MarketModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Commodities UserCommodities { get; set; }
        public Dictionary<MarketType, List<MarketPosts>> MarketPostsByType { get; set; } = new();
        public Dictionary<MarketType, (int MinPrice, int MaxPrice, int MaxAmount)> MarketLimits { get; } = new()
        {
            { MarketType.Food, (1, 20, 500000000) }, 
            { MarketType.Ore, (1000, 25000, 1000000) },
            { MarketType.RawMaterial, (1, 4, 500000000) },
            { MarketType.ConsumerGoods, (1, 4, 500000000) },
            { MarketType.TerranMetal, (250, 1000, 50000000) },
            { MarketType.RedCrystal, (250, 1000, 50000000) },
            { MarketType.WhiteCrystal, (250, 1000, 50000000) },
            { MarketType.Rutile, (250, 1000, 50000000) },
            { MarketType.Composite, (250, 1000, 50000000) },
            { MarketType.StrafezOrganism, (250, 1000, 50000000) }
        };

        [BindProperty]
        public MarketType SellMarketType { get; set; }
        [BindProperty]
        public int SellAmount { get; set; }
        [BindProperty]
        public int SellPrice { get; set; }
        [BindProperty]
        public int PostId { get; set; }
        [BindProperty]
        public int BuyAmount { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            UserCommodities = await _context.Commodities.FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);

            foreach (MarketType type in Enum.GetValues(typeof(MarketType)))
            {
                var posts = await _context.MarketPosts
                    .Where(p => p.MarketType == type)
                    .OrderBy(p => p.Price)
                    .ThenBy(p => p.DateTime)
                    .Take(5)
                    .ToListAsync();

                MarketPostsByType[type] = posts;
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostSellAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            UserCommodities = await _context.Commodities.FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);

            if (!MarketLimits.TryGetValue(SellMarketType, out var limits))
                return Page();

            if (SellPrice < limits.MinPrice || SellPrice > limits.MaxPrice)
            {
                ModelState.AddModelError("", $"Price must be between {limits.MinPrice} and {limits.MaxPrice}.");
                return Page();
            }
            if (SellAmount < 1 || SellAmount > limits.MaxAmount)
            {
                ModelState.AddModelError("", $"Amount must be between 1 and {limits.MaxAmount}.");
                return Page();
            }

            // Check user has enough to sell
            int userAmount = GetUserCommodityAmount(UserCommodities, SellMarketType);
            if (userAmount < SellAmount)
            {
                ModelState.AddModelError("", $"You do not have enough {SellMarketType} to sell.");
                return Page();
            }

            // Deduct from user
            SetUserCommodityAmount(UserCommodities, SellMarketType, userAmount - SellAmount);

            var post = new MarketPosts
            {
                MarketType = SellMarketType,
                Amount = SellAmount,
                Price = SellPrice,
                DateTime = DateTime.Now.AddHours(48),
                ApplicationUserId = user.Id
            };
            _context.MarketPosts.Add(post);
            await _context.SaveChangesAsync();
            TempData["MarketSuccess"] = $"Successfully posted {SellAmount} {SellMarketType} for {SellPrice} credits each.";
            return RedirectToPage();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostBuyAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            UserCommodities = await _context.Commodities.FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);

            var post = await _context.MarketPosts.Include(p => p.ApplicationUser).FirstOrDefaultAsync(p => p.Id == PostId);
            if (post == null)
            {
                ModelState.AddModelError("", "Market post not found.");
                await OnGetAsync();
                return Page();
            }
            if (BuyAmount < 1 || BuyAmount > post.Amount)
            {
                ModelState.AddModelError(nameof(BuyAmount), $"Amount must be between 1 and {post.Amount}.");
                await OnGetAsync();
                return Page();
            }
            if (post.ApplicationUserId == user.Id)
            {
                ModelState.AddModelError("", "You cannot buy your own post.");
                await OnGetAsync();
                return Page();
            }
            int totalPrice = BuyAmount * post.Price;
            if (UserCommodities.Credits < totalPrice)
            {
                ModelState.AddModelError(nameof(BuyAmount), "Not enough credits.");
                await OnGetAsync();
                return Page();
            }

            // Add commodity to buyer
            SetUserCommodityAmount(UserCommodities, post.MarketType, GetUserCommodityAmount(UserCommodities, post.MarketType) + BuyAmount);
            UserCommodities.Credits -= totalPrice;

            // Give credits to seller
            var sellerCommodities = await _context.Commodities.FirstOrDefaultAsync(c => c.ApplicationUserId == post.ApplicationUserId);
            if (sellerCommodities != null)
            {
                sellerCommodities.Credits += totalPrice;
            }

            // Subtract from post or remove if all bought
            post.Amount -= BuyAmount;
            if (post.Amount <= 0)
            {
                _context.MarketPosts.Remove(post);
            }

            await _context.SaveChangesAsync();
            TempData["MarketSuccess"] = $"Successfully purchased {BuyAmount} {post.MarketType} for {post.Price * BuyAmount} credits.";
            return RedirectToPage();
        }

        // Helper to get commodity amount by type
        private int GetUserCommodityAmount(Commodities c, MarketType type) => type switch
        {
            MarketType.Food => c.Food,
            MarketType.Ore => c.Ore,
            MarketType.RawMaterial => c.RawMaterial,
            MarketType.ConsumerGoods => c.ConsumerGoods,
            MarketType.TerranMetal => c.TerranMetal,
            MarketType.RedCrystal => c.RedCrystal,
            MarketType.WhiteCrystal => c.WhiteCrystal,
            MarketType.Rutile => c.Rutile,
            MarketType.Composite => c.Composite,
            MarketType.StrafezOrganism => c.StrafezOrganism,
            _ => 0
        };

        // Helper to set commodity amount by type
        private void SetUserCommodityAmount(Commodities c, MarketType type, int value)
        {
            switch (type)
            {
                case MarketType.Food: c.Food = value; break;
                case MarketType.Ore: c.Ore = value; break;
                case MarketType.RawMaterial: c.RawMaterial = value; break;
                case MarketType.ConsumerGoods: c.ConsumerGoods = value; break;
                case MarketType.TerranMetal: c.TerranMetal = value; break;
                case MarketType.RedCrystal: c.RedCrystal = value; break;
                case MarketType.WhiteCrystal: c.WhiteCrystal = value; break;
                case MarketType.Rutile: c.Rutile = value; break;
                case MarketType.Composite: c.Composite = value; break;
                case MarketType.StrafezOrganism: c.StrafezOrganism = value; break;
            }
        }
    }
}
