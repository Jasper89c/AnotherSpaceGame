using AnotherSpaceGame.Models;
using AnotherSpaceGame.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; // For [BindProperty]

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class BuildShipsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly TurnService _turnService;

        public BuildShipsModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context, TurnService turnService)
        {
            _userManager = userManager;
            _context = context;
            _turnService = turnService;
        }

        public ApplicationUser CurrentUser { get; set; }
        public List<Ships> BuildableShips { get; set; } = new();
        public List<Fleet> UserFleet { get; set; } = new();

        [BindProperty]
        public Dictionary<int, int> BuildAmounts { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            // Load user with all research navigation properties
            CurrentUser = await _context.Users
                .Include(u => u.CollectiveResearch)
                .Include(u => u.CyrilClassResearch)
                .Include(u => u.EClassResearch)
                .Include(u => u.FClassResearch)
                .Include(u => u.GuardianResearch)
                .Include(u => u.MarauderResearch)
                .Include(u => u.StrafezResearch)
                .Include(u => u.TerranResearch)
                .Include(u => u.ViralResearch)
                .Include(u => u.AMinerResearch)
                .Include(u => u.ViralReversedShips)
                .Include(u => u.Turns)
                .Include(u => u.Commodities)
                .Include(u => u.Fleets)
                .Include(u => u.Federation)
                .Include(u => u.UWShips)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (user.UWShips == null)
            {
                user.UWShips = new UWShips
                {
                    ApplicationUserId = user.Id,
                    ApplicationUser = user
                };
               _context.UWShips.Add(user.UWShips);
               await _context.SaveChangesAsync();
            }

            var ships = await _context.Ships.ToListAsync();

            // Helper for adding ships by ID
            void AddShipById(int id)
            {
                var ship = ships.FirstOrDefault(s => s.Id == id);
                if (ship != null && !BuildableShips.Contains(ship))
                    BuildableShips.Add(ship);
            }

            // Always available
            AddShipById(323); // LightDrone
            if (CurrentUser.Faction == Faction.Viral)
            {
                if (CurrentUser.ViralReversedShips.TerranShip1Id != 0)
                {
                    AddShipById(CurrentUser.ViralReversedShips.TerranShip1Id);
                } // Terran Ship 1
                if (CurrentUser.ViralReversedShips.TerranShip2Id != 0)
                {
                    AddShipById(CurrentUser.ViralReversedShips.TerranShip2Id);
                } // Terran Ship 2
                if (CurrentUser.ViralReversedShips.TerranShip3Id != 0)
                {
                    AddShipById(CurrentUser.ViralReversedShips.TerranShip3Id);
                } // Terran Ship 3
                if (CurrentUser.ViralReversedShips.AminerShip1Id != 0)
                {
                    AddShipById(CurrentUser.ViralReversedShips.AminerShip1Id);
                } // AMiner Ship 1
                if (CurrentUser.ViralReversedShips.AminerShip2Id != 0)
                {
                    AddShipById(CurrentUser.ViralReversedShips.AminerShip2Id);
                } // AMiner Ship 2
                if (CurrentUser.ViralReversedShips.AminerShip3Id != 0)
                {
                    AddShipById(CurrentUser.ViralReversedShips.AminerShip3Id);
                } // AMiner Ship 3
                if (CurrentUser.ViralReversedShips.MarauderShip1Id != 0)
                {
                    AddShipById(CurrentUser.ViralReversedShips.MarauderShip1Id);
                } // Marauder Ship 1
                if (CurrentUser.ViralReversedShips.MarauderShip2Id != 0)
                {
                    AddShipById(CurrentUser.ViralReversedShips.MarauderShip2Id);
                } // Marauder Ship 2
                if (CurrentUser.ViralReversedShips.MarauderShip3Id != 0)
                {
                    AddShipById(CurrentUser.ViralReversedShips.MarauderShip3Id);
                } // Marauder Ship 3
            }
            // uw ships
            if (user.UWShips.UWship1 == true)
            {
                AddShipById(324); // UW ship 1
            }
            if (user.UWShips.UWship2 == true)
            {
                AddShipById(325); // UW ship 2
            }
            if (user.UWShips.UWship3 == true)
            {
                AddShipById(326); // UW ship 2
            }
            if (user.UWShips.UWship4 == true)
            {
                AddShipById(327); // UW ship 2
            }
            // CyrilClassResearch
            if (CurrentUser.CyrilClassResearch is { CyrilCorvette: true, CyrilCorvetteTurnsRequired: 0 }) AddShipById(312);
            if (CurrentUser.CyrilClassResearch is { CyrilFrigate: true, CyrilFrigateTurnsRequired: 0 }) AddShipById(313);
            if (CurrentUser.CyrilClassResearch is { CyrilDestroyer: true, CyrilDestroyerTurnsRequired: 0 }) AddShipById(314);
            if (CurrentUser.CyrilClassResearch is { CyrilCruiser: true, CyrilCruiserTurnsRequired: 0 }) { AddShipById(315); AddShipById(316); }

            // EClassResearch
            if (CurrentUser.EClassResearch is { EClassFighter: true, EClassFighterTurnsRequired: 0 }) AddShipById(309);
            if (CurrentUser.EClassResearch is { EClassFrigate: true, EClassFrigateTurnsRequired: 0 }) AddShipById(310);
            if (CurrentUser.EClassResearch is { EClassDestroyer: true, EClassDestroyerTurnsRequired: 0 }) AddShipById(311);

            // FClassResearch
            if (CurrentUser.FClassResearch is { FClassFrigate: true, FClassFrigateTurnsRequired: 0 }) AddShipById(305);
            if (CurrentUser.FClassResearch is { FClassDestroyer: true, FClassDestroyerTurnsRequired: 0 }) { AddShipById(306); AddShipById(307); }
            if (CurrentUser.FClassResearch is { FClassCruiser: true, FClassCruiserTurnsRequired: 0 }) AddShipById(308);

            // StrafezResearch
            if (CurrentUser.StrafezResearch is { SmallStrafezRunnerFodder: true, SmallStrafezRunnerFodderTurnsRequired: 0 }) { AddShipById(317); AddShipById(319); }
            if (CurrentUser.StrafezResearch is { LargeStrafezRunnerFodder: true, LargeStrafezRunnerFodderTurnsRequired: 0 }) { AddShipById(318); AddShipById(320); }
            if (CurrentUser.StrafezResearch is { StrafezQueenKing: true, StrafezQueenKingTurnsRequired: 0 }) { AddShipById(321); AddShipById(322); }

            // TerranResearch
            if (CurrentUser.Faction == Faction.Terran)
            {
                AddShipById(219); // Ryujin
                AddShipById(235); // Scout
                if (CurrentUser.TerranResearch is { TerranFrigate: true, TerranFrigateTurnsRequired: 0 }) AddShipById(221);
                if (CurrentUser.TerranResearch is { TerranDestroyer: true, TerranDestroyerTurnsRequired: 0 }) AddShipById(222);
                if (CurrentUser.TerranResearch is { TerranCruiser: true, TerranCruiserTurnsRequired: 0 }) { AddShipById(223); AddShipById(224); }
                if (CurrentUser.TerranResearch is { TerranBattleship: true, TerranBattleshipTurnsRequired: 0 }) AddShipById(225);
                if (CurrentUser.TerranResearch is { TerranAdvancedScout: true, TerranAdvancedScoutTurnsRequired: 0 }) AddShipById(236);
                if (CurrentUser.TerranResearch is { PhotonCorvette: true, PhotonCorvetteTurnsRequired: 0 }) AddShipById(230);
                if (CurrentUser.TerranResearch is { PhotonFrigate: true, PhotonFrigateTurnsRequired: 0 }) AddShipById(231);
                if (CurrentUser.TerranResearch is { PhotonDestroyer: true, PhotonDestroyerTurnsRequired: 0 }) AddShipById(232);
                if (CurrentUser.TerranResearch is { PhotonCruiser: true, PhotonCruiserTurnsRequired: 0 }) AddShipById(233);
                if (CurrentUser.TerranResearch is { TerranStarbase: true, TerranStarbaseTurnsRequired: 0 }) { AddShipById(228); AddShipById(229); }
                if (CurrentUser.TerranResearch is { TerranDreadnaught: true, TerranDreadnaughtTurnsRequired: 0 }) { AddShipById(226); AddShipById(227); }
                if (CurrentUser.TerranResearch is { TerranJuggernaught: true, TerranJuggernaughtTurnsRequired: 0 }) AddShipById(234);
            }

            // AMinerResearch
            if (CurrentUser.Faction == Faction.AMiner)
            {
                AddShipById(237); // M.hal
                if (CurrentUser.AMinerResearch is { AsphaCorvette: true, AsphaCorvetteTurnsRequired: 0 }) AddShipById(238);
                if (CurrentUser.AMinerResearch is { AsphaFrigate: true, AsphaFrigateTurnsRequired: 0 }) AddShipById(239);
                if (CurrentUser.AMinerResearch is { AsphaDestroyer: true, AsphaDestroyerTurnsRequired: 0 }) { AddShipById(240); AddShipById(241); }
                if (CurrentUser.AMinerResearch is { AsphaCruiser: true, AsphaCruiserTurnsRequired: 0 }) { AddShipById(242); AddShipById(243); }
                if (CurrentUser.AMinerResearch is { AsphaBattleship: true, AsphaBattleshipTurnsRequired: 0 }) AddShipById(244);
                if (CurrentUser.AMinerResearch is { AsphaDreadnought: true, AsphaDreadnoughtTurnsRequired: 0 }) { AddShipById(245); AddShipById(247); }
                if (CurrentUser.AMinerResearch is { AsphaLightStarbase: true, AsphaLightStarbaseTurnsRequired: 0 }) { AddShipById(246); AddShipById(248); }
                if (CurrentUser.AMinerResearch is { AsphaHeavyStarbase: true, AsphaHeavyStarbaseTurnsRequired: 0 }) AddShipById(249);
                if (CurrentUser.AMinerResearch is { GClassJuggernaught: true, GClassJuggernaughtTurnsRequired: 0 }) AddShipById(253);
                if (CurrentUser.AMinerResearch is { AsphaSeeker: true, AsphaSeekerTurnsRequired: 0 }) AddShipById(254);
                if (CurrentUser.AMinerResearch is { AsphaRanger: true, AsphaRangerTurnsRequired: 0 }) AddShipById(255);
                if (CurrentUser.AMinerResearch is { GClassShip: true, GClassShipTurnsRequired: 0 }) { AddShipById(250); AddShipById(251); AddShipById(252); }
            }

            // GuardianResearch
            if (CurrentUser.Faction == Faction.Guardian)
            {
                AddShipById(303); // Guardian Scout
                if (CurrentUser.GuardianResearch is { FighterClass: true, FighterClassTurnsRequired: 0 }) { AddShipById(289); AddShipById(298); }
                if (CurrentUser.GuardianResearch is { CorvetteClass: true, CorvetteClassTurnsRequired: 0 }) { AddShipById(290); AddShipById(299); }
                if (CurrentUser.GuardianResearch is { FrigateClass: true, FrigateClassTurnsRequired: 0 }) AddShipById(291);
                if (CurrentUser.GuardianResearch is { DestroyerClass: true, DestroyerClassTurnsRequired: 0 }) AddShipById(292);
                if (CurrentUser.GuardianResearch is { CruiserClass: true, CruiserClassTurnsRequired: 0 }) AddShipById(293);
                if (CurrentUser.GuardianResearch is { WeaponsPlatform: true, WeaponsPlatformTurnsRequired: 0 }) AddShipById(294);
                if (CurrentUser.GuardianResearch is { LClassFrigate: true, LClassFrigateTurnsRequired: 0 }) AddShipById(295);
                if (CurrentUser.GuardianResearch is { LClassDestroyer: true, LClassDestroyerTurnsRequired: 0 }) AddShipById(296);
                if (CurrentUser.GuardianResearch is { LClassCruiser: true, LClassCruiserTurnsRequired: 0 }) AddShipById(297);
                if (CurrentUser.GuardianResearch is { KClassDestroyer: true, KClassDestroyerTurnsRequired: 0 }) AddShipById(300);
                if (CurrentUser.GuardianResearch is { KClassCruiser: true, KClassCruiserTurnsRequired: 0 }) { AddShipById(301); AddShipById(302); }
                if (CurrentUser.GuardianResearch is { AdvancedScouts: true, AdvancedScoutsTurnsRequired: 0 }) AddShipById(304);
            }

            // CollectiveResearch
            if (CurrentUser.Faction == Faction.Collective)
            {
                AddShipById(257);
                if (CurrentUser.CollectiveResearch is { FighterClass: true, FighterClassTurnsRequired: 0 }) AddShipById(263);
                if (CurrentUser.CollectiveResearch is { FrigateClass: true, FrigateClassTurnsRequired: 0 }) AddShipById(264);
                if (CurrentUser.CollectiveResearch is { DestroyerClass: true, DestroyerClassTurnsRequired: 0 }) AddShipById(265);
                if (CurrentUser.CollectiveResearch is { CruiserClass: true, CruiserClassTurnsRequired: 0 }) AddShipById(266);
                if (CurrentUser.CollectiveResearch is { HClassDreadnought: true, HClassDreadnoughtTurnsRequired: 0 }) AddShipById(267);
                if (CurrentUser.CollectiveResearch is { HClassLeviathan: true, HClassLeviathanTurnsRequired: 0 }) AddShipById(268);
                if (CurrentUser.CollectiveResearch is { HClassStarbase: true, HClassStarbaseTurnsRequired: 0 }) AddShipById(269);
                if (CurrentUser.CollectiveResearch is { RClassFrigate: true, RClassFrigateTurnsRequired: 0 }) AddShipById(258);
                if (CurrentUser.CollectiveResearch is { RClassDestroyer: true, RClassDestroyerTurnsRequired: 0 }) AddShipById(259);
                if (CurrentUser.CollectiveResearch is { RClassBattleship: true, RClassBattleshipTurnsRequired: 0 }) AddShipById(260);
                if (CurrentUser.CollectiveResearch is { RClassDreadnought: true, RClassDreadnoughtTurnsRequired: 0 }) AddShipById(261);
                if (CurrentUser.CollectiveResearch is { RClassJuggernaught: true, RClassJuggernaughtTurnsRequired: 0 }) AddShipById(262);
            }

            // ViralResearch
            if (CurrentUser.Faction == Faction.Viral)
            {
                if (CurrentUser.ViralResearch is { VClassDestroyer: true, VClassDestroyerTurnsRequired: 0 }) AddShipById(270);
                if (CurrentUser.ViralResearch is { BClassStarbase: true, BClassStarbaseTurnsRequired: 0 }) AddShipById(271);
                if (CurrentUser.ViralResearch is { BClassCruiser: true, BClassCruiserTurnsRequired: 0 }) AddShipById(272);
                if (CurrentUser.ViralResearch is { VClassCruiser: true, VClassCruiserTurnsRequired: 0 }) AddShipById(273);
                if (CurrentUser.ViralResearch is { KohoutekScout: true, KohoutekScoutTurnsRequired: 0 }) AddShipById(274);
            }

            // MarauderResearch
            if (CurrentUser.Faction == Faction.Marauder)
            {
                AddShipById(275);
                if (CurrentUser.MarauderResearch is { MarauderCorvette: true, MarauderCorvetteTurnsRequired: 0 }) AddShipById(276);
                if (CurrentUser.MarauderResearch is { MarauderFrigate: true, MarauderFrigateTurnsRequired: 0 }) AddShipById(277);
                if (CurrentUser.MarauderResearch is { MarauderDestroyer: true, MarauderDestroyerTurnsRequired: 0 }) { AddShipById(278); AddShipById(279); }
                if (CurrentUser.MarauderResearch is { MarauderCruiser: true, MarauderCruiserTurnsRequired: 0 }) { AddShipById(280); AddShipById(282); }
                if (CurrentUser.MarauderResearch is { MarauderBattleship: true, MarauderBattleshipTurnsRequired: 0 }) { AddShipById(281); AddShipById(283); }
                if (CurrentUser.MarauderResearch is { TypeDFrigate: true, TypeDFrigateTurnsRequired: 0 }) { AddShipById(286); AddShipById(285); }
                if (CurrentUser.MarauderResearch is { TypeDDestroyer: true, TypeDDestroyerTurnsRequired: 0 }) AddShipById(287);
                if (CurrentUser.MarauderResearch is { TypeDCruiser: true, TypeDCruiserTurnsRequired: 0 }) AddShipById(288);
                if (CurrentUser.MarauderResearch is { TypeDBattleship: true, TypeDBattleshipTurnsRequired: 0 }) AddShipById(284);
            }

            UserFleet = await _context.Fleets.Where(f => f.ApplicationUserId == user.Id).ToListAsync();

            return Page();
        }

        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var CurrentUser = await _context.Users
                .Include(u => u.Commodities)
                .Include(u => u.Turns)
                .FirstOrDefaultAsync(u => u.Id == user.Id);
            if (user == null)
                return Unauthorized();

            // Load ships and fleet
            var ships = await _context.Ships.ToListAsync();
            var fleet = await _context.Fleets.Where(f => f.ApplicationUserId == user.Id).ToListAsync();

            // Calculate total turns required
            int totalTurnsRequired = 0;
            var buildInstructions = new List<(Ships ship, int amount, int turnsNeeded)>();
            var TerranMetal = 0;
            var Rutile = 0;
            var Composite = 0;
            var RedCyrstal = 0;
            var WhiteCyrstal = 0;
            var StrafezOrganism = 0;
            var Credits = 0;

            foreach (var entry in BuildAmounts)
            {
                var ship = ships.FirstOrDefault(s => s.Id == entry.Key);
                int amount = entry.Value;
                if (ship == null || amount <= 0) continue;

                int buildRate = ship.BuildRate > 0 ? ship.BuildRate : 1;
                int turnsNeeded = (int)Math.Ceiling((double)amount / buildRate);
                totalTurnsRequired += turnsNeeded;
                buildInstructions.Add((ship, amount, turnsNeeded));
                TerranMetal += (ship.TerranMetal * amount);
                Rutile += (ship.Rutile * amount);
                Composite += (ship.Composite * amount);
                RedCyrstal += (ship.RedCrystal * amount);
                WhiteCyrstal += (ship.WhiteCrystal * amount);
                StrafezOrganism += (ship.StrafezOrganism * amount);
                Credits += (ship.Cost * amount);
            }
            if (totalTurnsRequired == 0)
            {
                ModelState.AddModelError(string.Empty, "You must specify a valid amount of ships to build.");
                await OnGetAsync(); // Reload ships and fleet
                return Page();
            }
            if (totalTurnsRequired > CurrentUser.Turns.CurrentTurns)
            {
                ModelState.AddModelError(string.Empty, "Not enough turns to build ships.");
                await OnGetAsync(); // Reload ships and fleet
                return Page();
            }
            if (TerranMetal > CurrentUser.Commodities.TerranMetal)
            {
                ModelState.AddModelError(string.Empty, "Not enough Teran Metal to build ships.");
                await OnGetAsync(); // Reload ships and fleet
                return Page();
            }
            if (Rutile > CurrentUser.Commodities.Rutile)
            {
                ModelState.AddModelError(string.Empty, "Not enough Rutile to build ships.");
                await OnGetAsync(); // Reload ships and fleet
                return Page();
            }
            if (Composite > CurrentUser.Commodities.Composite)
            {
                ModelState.AddModelError(string.Empty, "Not enough Composite to build ships.");
                await OnGetAsync(); // Reload ships and fleet
                return Page();
            }
            if (RedCyrstal > CurrentUser.Commodities.RedCrystal)
            {
                ModelState.AddModelError(string.Empty, "Not enough Red Crystal to build ships.");
                await OnGetAsync(); // Reload ships and fleet
                return Page();
            }
            if (WhiteCyrstal > CurrentUser.Commodities.WhiteCrystal)
            {
                ModelState.AddModelError(string.Empty, "Not enough White Crystal to build ships.");
                await OnGetAsync(); // Reload ships and fleet
                return Page();
            }
            if (StrafezOrganism > CurrentUser.Commodities.StrafezOrganism)
            {
                ModelState.AddModelError(string.Empty, "Not enough Strafez Organism to build ships.");
                await OnGetAsync(); // Reload ships and fleet
                return Page();
            }
            if (Credits > CurrentUser.Commodities.Credits)
            {
                ModelState.AddModelError(string.Empty, "Not enough Credits to build ships.");
                await OnGetAsync(); // Reload ships and fleet
                return Page();
            }

            // Add ships to fleet
            foreach (var (ship, amount, _) in buildInstructions)
            {
                var userFleet = fleet.FirstOrDefault(f => f.ShipId == ship.Id);
                if (userFleet == null)
                {
                    userFleet = new Fleet
                    {
                        ApplicationUserId = user.Id,
                        ShipId = ship.Id,
                        TotalShips = amount,
                        TotalPowerRating = amount * ship.PowerRating,
                        TotalUpkeep = amount * ship.Upkeep
                    };
                    _context.Fleets.Add(userFleet);
                }
                else
                {
                    userFleet.TotalShips += amount;
                    userFleet.TotalPowerRating += amount * ship.PowerRating;
                    userFleet.TotalUpkeep += amount * ship.Upkeep;
                }
                CurrentUser.Commodities.TerranMetal -= ship.TerranMetal * amount;
                CurrentUser.Commodities.Rutile -= ship.Rutile * amount;
                CurrentUser.Commodities.Composite -= ship.Composite * amount;
                CurrentUser.Commodities.RedCrystal -= ship.RedCrystal * amount;
                CurrentUser.Commodities.WhiteCrystal -= ship.WhiteCrystal * amount;
                CurrentUser.Commodities.StrafezOrganism -= ship.StrafezOrganism * amount;
                CurrentUser.Commodities.Credits -= ship.Cost * amount;
            }
            _context.SaveChanges();
            // Deduct turns
            var turnResult = await _turnService.TryUseTurnsAsync(user.Id, totalTurnsRequired);
            if (!turnResult.Success)
            {
                ModelState.AddModelError(string.Empty, turnResult.Message);
                await OnGetAsync(); // Reload ships and fleet
                return Page();
            }
            user.DamageProtection = DateTime.Now;
            await _context.SaveChangesAsync();
            TempData["BuildMessage"] = $"Successfully built ships! Used {totalTurnsRequired} turns.<hr>{turnResult.Message}";
            return RedirectToPage();
        }
    }
}
