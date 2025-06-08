using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class ConfirmAttackModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ConfirmAttackModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string UserName { get; set; }

        [BindProperty(SupportsGet = true)]
        public AttackType SelectedAttackType { get; set; }

        public List<Fleet> CurrentUserFleets { get; set; } = new();
        public List<Fleet> TargetUserFleets { get; set; } = new();
        public bool IsTargetNPC { get; set; } = false;

        public ApplicationUser CurrentUser { get; set; }
        public ApplicationUser TargetUser { get; set; }
        public bool HasEnoughTurns { get; set; } = true;
        public string NotEnoughTurnsMessage { get; set; }
        public bool PowerRatingAllowed { get; set; } = true;
        public string PowerRatingWarning { get; set; }
        public bool IsFederationWar { get; set; } = false;

        public async Task<IActionResult> OnGetAsync()
        {
            // Get current user
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToPage("/Account/Login");

            // Get target user by UserName
            ApplicationUser? targetUser = _context.Users.FirstOrDefault(u => u.UserName == UserName);
            if (targetUser == null)
            {
                NPCs? npc = _context.NPCs.FirstOrDefault(u => u.UserName == UserName);
                if (npc != null)
                {
                    targetUser = new ApplicationUser
                    {
                        UserName = npc.UserName,
                        IsNPC = true,
                        PowerRating = npc.PowerRating,
                        PlayingSince = npc.PlayingSince,
                        EmpireAge = npc.EmpireAge,
                        BattlesWon = npc.BattlesWon,
                        BattlesLost = npc.BattlesLost,
                        ColoniesWon = npc.ColoniesWon,
                        ColoniesLost = npc.ColoniesLost,
                        ColoniesExplored = npc.ColoniesExplored,
                        PlanetsPlundered = npc.PlanetsPlundered,
                        TotalColonies = npc.TotalColonies,
                        TotalPlanets = npc.TotalPlanets,
                        DamageProtection = npc.DamageProtection,
                        LastAction = npc.LastAction,
                        ArtifactShield = npc.ArtifactShield,
                        FederationId = npc.FederationId,
                        Faction = npc.Faction
                    };
                }
            }
            // merg all ships to create a list of fleets for referencing
            var allships = _context.Ships.ToList();
            List<UserShipFleet> ShipFleets = new List<UserShipFleet>();
            foreach (var Ship in allships)
            {
                ShipFleets.Add(new UserShipFleet
                {
                    ShipId = Ship.Id,
                    ShipName = Ship.ShipName,
                    ShipType = Ship.ShipType,
                    BuildRate = Ship.BuildRate,
                    PowerRatingPerShip = Ship.PowerRating
                });
            }
            // Get current user's top 10 fleets, ignoring any with ShipType Starbase
            CurrentUserFleets = (from fleet in _context.Fleets
                                 join ship in _context.Ships on fleet.ShipId equals ship.Id
                                 where fleet.ApplicationUserId == currentUser.Id
                                       && ship.ShipType != ShipType.Starbase
                                       && ship.ShipType != ShipType.Scout
                                 orderby fleet.TotalPowerRating descending
                                 select fleet)
                             .Take(10)
                             .ToList();

            // Get target user's top 10 fleets (if not NPC)
            if (targetUser != null && targetUser.IsNPC != true)
            {
                TargetUserFleets = (from fleet in _context.Fleets
                                    join ship in _context.Ships on fleet.ShipId equals ship.Id
                                    where fleet.ApplicationUserId == currentUser.Id
                                          && ship.ShipType != ShipType.Starbase
                                          && ship.ShipType != ShipType.Scout
                                    orderby fleet.TotalPowerRating descending
                                    select fleet)
                             .Take(10)
                             .ToList();
            }
            if(TargetUserFleets.Count == 0 && targetUser.IsNPC == true)
                {
                    var random = Random.Shared.Next(1, 11);
                    switch (random)
                    {
                        case 1:
                            //stack 1
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219,303) });
                            // Fix for CS0103: The name 'or' does not exist in the current context
                            // Replace 'or' with proper logical OR operator '||' in C#
                            if (TargetUserFleets[0].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[0].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[0].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[0].ShipId == 304)
                            {
                                TargetUserFleets[0].ShipId = Random.Shared.Next(275, 303); // Default to a basic ship if random fails
                            }
                            var refShip0 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[0].ShipId);
                            TargetUserFleets[0].TotalShips = (int)Math.Floor((double)targetUser.PowerRating / refShip0.TotalPowerRating);
                            TargetUserFleets.Add(new Fleet { ShipId = 323 });
                            TargetUserFleets[1].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 322 });
                            TargetUserFleets[2].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 321 });
                            TargetUserFleets[3].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 320 });
                            TargetUserFleets[4].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 319 });
                            TargetUserFleets[5].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 318 });
                            TargetUserFleets[6].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 317 });
                            TargetUserFleets[7].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 316 });
                            TargetUserFleets[8].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 315 });
                            TargetUserFleets[9].TotalShips = 1;
                            break;
                        case 2:
                            //stack 1
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[0].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[0].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[0].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[0].ShipId == 304)
                            {
                                TargetUserFleets[0].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip1 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[0].ShipId);
                            TargetUserFleets[0].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 2) / refShip1.TotalPowerRating);
                            // stack 2
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[1].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[1].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[1].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[1].ShipId == 304)
                            {
                                TargetUserFleets[1].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip2 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[1].ShipId);
                            TargetUserFleets[1].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 2) / refShip2.TotalPowerRating);
                            TargetUserFleets.Add(new Fleet { ShipId = 322 });
                            TargetUserFleets[2].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 321 });
                            TargetUserFleets[3].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 320 });
                            TargetUserFleets[4].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 319 });
                            TargetUserFleets[5].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 318 });
                            TargetUserFleets[6].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 317 });
                            TargetUserFleets[7].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 316 });
                            TargetUserFleets[8].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 315 });
                            TargetUserFleets[9].TotalShips = 1;
                            break;
                        case 3:
                            //stack 1
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[0].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[0].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[0].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[0].ShipId == 304)
                            {
                                TargetUserFleets[0].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip3 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[0].ShipId);
                            TargetUserFleets[0].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 3) / refShip3.TotalPowerRating);
                            // stack 2
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[1].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[1].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[1].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[1].ShipId == 304)
                            {
                                TargetUserFleets[1].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip4 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[1].ShipId);
                            TargetUserFleets[1].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 3) / refShip4.TotalPowerRating);
                            // stack 3
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[2].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[2].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[2].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[2].ShipId == 304)
                            {
                                TargetUserFleets[2].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip5 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[2].ShipId);
                            TargetUserFleets[2].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 3) / refShip5.TotalPowerRating);
                            TargetUserFleets.Add(new Fleet { ShipId = 321 });
                            TargetUserFleets[3].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 320 });
                            TargetUserFleets[4].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 319 });
                            TargetUserFleets[5].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 318 });
                            TargetUserFleets[6].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 317 });
                            TargetUserFleets[7].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 316 });
                            TargetUserFleets[8].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 315 });
                            TargetUserFleets[9].TotalShips = 1;
                            break;
                        case 4:
                            //stack 1
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[0].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[0].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[0].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[0].ShipId == 304)
                            {
                                TargetUserFleets[0].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip6 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[0].ShipId);
                            TargetUserFleets[0].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 4) / refShip6.TotalPowerRating);
                            // stack 2
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[1].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[1].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[1].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[1].ShipId == 304)
                            {
                                TargetUserFleets[1].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip7 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[1].ShipId);
                            TargetUserFleets[1].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 4) / refShip7.TotalPowerRating);
                            // stack 3
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[2].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[2].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[2].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[2].ShipId == 304)
                            {
                                TargetUserFleets[2].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip8 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[2].ShipId);
                            TargetUserFleets[2].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 4) / refShip8.TotalPowerRating);
                            //stack 4
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[3].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[3].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[3].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[3].ShipId == 304)
                            {
                                TargetUserFleets[3].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip9 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[3].ShipId);
                            TargetUserFleets[3].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 4) / refShip8.TotalPowerRating);
                            TargetUserFleets.Add(new Fleet { ShipId = 320 });
                            TargetUserFleets[4].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 319 });
                            TargetUserFleets[5].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 318 });
                            TargetUserFleets[6].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 317 });
                            TargetUserFleets[7].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 316 });
                            TargetUserFleets[8].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 315 });
                            TargetUserFleets[9].TotalShips = 1;
                            break;
                        case 5:
                            //stack 1
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[0].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[0].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[0].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[0].ShipId == 304)
                            {
                                TargetUserFleets[0].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip10 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[0].ShipId);
                            TargetUserFleets[0].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 5) / refShip10.TotalPowerRating);
                            // stack 2
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[1].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[1].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[1].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[1].ShipId == 304)
                            {
                                TargetUserFleets[1].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip11 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[1].ShipId);
                            TargetUserFleets[1].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 5) / refShip11.TotalPowerRating);
                            // stack 3
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[2].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[2].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[2].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[2].ShipId == 304)
                            {
                                TargetUserFleets[2].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip12 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[2].ShipId);
                            TargetUserFleets[2].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 5) / refShip12.TotalPowerRating);
                            //stack 4
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[3].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[3].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[3].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[3].ShipId == 304)
                            {
                                TargetUserFleets[3].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip13 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[3].ShipId);
                            TargetUserFleets[3].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 5) / refShip13.TotalPowerRating);
                            //stack 5
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[4].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[4].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[4].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[4].ShipId == 304)
                            {
                                TargetUserFleets[4].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip14 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[4].ShipId);
                            TargetUserFleets[4].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 5) / refShip14.TotalPowerRating);
                            TargetUserFleets.Add(new Fleet { ShipId = 319 });
                            TargetUserFleets[5].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 318 });
                            TargetUserFleets[6].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 317 });
                            TargetUserFleets[7].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 316 });
                            TargetUserFleets[8].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 315 });
                            TargetUserFleets[9].TotalShips = 1;
                            break;
                        case 6:
                            //stack 1
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[0].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[0].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[0].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[0].ShipId == 304)
                            {
                                TargetUserFleets[0].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip15 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[0].ShipId);
                            TargetUserFleets[0].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 6) / refShip15.TotalPowerRating);
                            // stack 2
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[1].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[1].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[1].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[1].ShipId == 304)
                            {
                                TargetUserFleets[1].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip16 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[1].ShipId);
                            TargetUserFleets[1].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 6) / refShip16.TotalPowerRating);
                            // stack 3
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[2].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[2].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[2].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[2].ShipId == 304)
                            {
                                TargetUserFleets[2].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip17 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[2].ShipId);
                            TargetUserFleets[2].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 6) / refShip17.TotalPowerRating);
                            //stack 4
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[3].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[3].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[3].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[3].ShipId == 304)
                            {
                                TargetUserFleets[3].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip18 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[3].ShipId);
                            TargetUserFleets[3].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 6) / refShip18.TotalPowerRating);
                            //stack 5
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[4].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[4].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[4].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[4].ShipId == 304)
                            {
                                TargetUserFleets[4].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip19 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[4].ShipId);
                            TargetUserFleets[4].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 6) / refShip19.TotalPowerRating);
                            //stack 6
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[5].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[5].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[5].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[5].ShipId == 304)
                            {
                                TargetUserFleets[5].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip20 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[5].ShipId);
                            TargetUserFleets[5].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 6) / refShip20.TotalPowerRating);
                            TargetUserFleets.Add(new Fleet { ShipId = 318 });
                            TargetUserFleets[6].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 317 });
                            TargetUserFleets[7].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 316 });
                            TargetUserFleets[8].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 315 });
                            TargetUserFleets[9].TotalShips = 1;
                            break;
                        case 7:
                            //stack 1
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[0].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[0].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[0].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[0].ShipId == 304)
                            {
                                TargetUserFleets[0].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip21 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[0].ShipId);
                            TargetUserFleets[0].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 7) / refShip21.TotalPowerRating);
                            // stack 2
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[1].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[1].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[1].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[1].ShipId == 304)
                            {
                                TargetUserFleets[1].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip22 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[1].ShipId);
                            TargetUserFleets[1].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 7) / refShip22.TotalPowerRating);
                            // stack 3
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[2].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[2].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[2].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[2].ShipId == 304)
                            {
                                TargetUserFleets[2].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip23 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[2].ShipId);
                            TargetUserFleets[2].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 7) / refShip23.TotalPowerRating);
                            //stack 4
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[3].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[3].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[3].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[3].ShipId == 304)
                            {
                                TargetUserFleets[3].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip24 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[3].ShipId);
                            TargetUserFleets[3].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 7) / refShip24.TotalPowerRating);
                            //stack 5
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[4].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[4].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[4].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[4].ShipId == 304)
                            {
                                TargetUserFleets[4].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip25 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[4].ShipId);
                            TargetUserFleets[4].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 7) / refShip25.TotalPowerRating);
                            //stack 6
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[5].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[5].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[5].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[5].ShipId == 304)
                            {
                                TargetUserFleets[5].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip26 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[5].ShipId);
                            TargetUserFleets[5].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 7) / refShip26.TotalPowerRating);
                            //stack 7
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[6].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[6].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[6].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[6].ShipId == 304)
                            {
                                TargetUserFleets[6].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip27 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[6].ShipId);
                            TargetUserFleets[6].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 7) / refShip27.TotalPowerRating);
                            TargetUserFleets.Add(new Fleet { ShipId = 317 });
                            TargetUserFleets[7].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 316 });
                            TargetUserFleets[8].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 315 });
                            TargetUserFleets[9].TotalShips = 1;
                            break;
                        case 8:
                            //stack 1
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[0].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[0].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[0].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[0].ShipId == 304)
                            {
                                TargetUserFleets[0].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip28 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[0].ShipId);
                            TargetUserFleets[0].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 8) / refShip28.TotalPowerRating);
                            // stack 2
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[1].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[1].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[1].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[1].ShipId == 304)
                            {
                                TargetUserFleets[1].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip29 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[1].ShipId);
                            TargetUserFleets[1].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 8) / refShip29.TotalPowerRating);
                            // stack 3
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[2].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[2].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[2].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[2].ShipId == 304)
                            {
                                TargetUserFleets[2].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip30 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[2].ShipId);
                            TargetUserFleets[2].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 8) / refShip30.TotalPowerRating);
                            //stack 4
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[3].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[3].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[3].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[3].ShipId == 304)
                            {
                                TargetUserFleets[3].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip31 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[3].ShipId);
                            TargetUserFleets[3].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 8) / refShip31.TotalPowerRating);
                            //stack 5
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[4].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[4].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[4].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[4].ShipId == 304)
                            {
                                TargetUserFleets[4].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip32 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[4].ShipId);
                            TargetUserFleets[4].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 8) / refShip32.TotalPowerRating);
                            //stack 6
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[5].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[5].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[5].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[5].ShipId == 304)
                            {
                                TargetUserFleets[5].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip33 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[5].ShipId);
                            TargetUserFleets[5].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 8) / refShip33.TotalPowerRating);
                            //stack 7
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[6].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[6].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[6].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[6].ShipId == 304)
                            {
                                TargetUserFleets[6].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip34 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[6].ShipId);
                            TargetUserFleets[6].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 8) / refShip34.TotalPowerRating);
                            //stack 8
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[7].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[7].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[7].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[7].ShipId == 304)
                            {
                                TargetUserFleets[7].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip35 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[7].ShipId);
                            TargetUserFleets[7].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 8) / refShip35.TotalPowerRating);
                            TargetUserFleets.Add(new Fleet { ShipId = 316 });
                            TargetUserFleets[8].TotalShips = 1;
                            TargetUserFleets.Add(new Fleet { ShipId = 315 });
                            TargetUserFleets[9].TotalShips = 1;
                            break;
                        case 9:
                            //stack 1
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[0].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[0].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[0].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[0].ShipId == 304)
                            {
                                TargetUserFleets[0].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip36 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[0].ShipId);
                            TargetUserFleets[0].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 9) / refShip36.TotalPowerRating);
                            // stack 2
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[1].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[1].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[1].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[1].ShipId == 304)
                            {
                                TargetUserFleets[1].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip37 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[1].ShipId);
                            TargetUserFleets[1].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 9) / refShip37.TotalPowerRating);
                            // stack 3
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[2].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[2].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[2].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[2].ShipId == 304)
                            {
                                TargetUserFleets[2].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip38 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[2].ShipId);
                            TargetUserFleets[2].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 9) / refShip38.TotalPowerRating);
                            //stack 4
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[3].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[3].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[3].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[3].ShipId == 304)
                            {
                                TargetUserFleets[3].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip39 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[3].ShipId);
                            TargetUserFleets[3].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 9) / refShip39.TotalPowerRating);
                            //stack 5
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[4].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[4].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[4].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[4].ShipId == 304)
                            {
                                TargetUserFleets[4].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip40 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[4].ShipId);
                            TargetUserFleets[4].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 9) / refShip40.TotalPowerRating);
                            //stack 6
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[5].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[5].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[5].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[5].ShipId == 304)
                            {
                                TargetUserFleets[5].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip41 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[5].ShipId);
                            TargetUserFleets[5].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 9) / refShip41.TotalPowerRating);
                            //stack 7
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[6].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[6].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[6].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[6].ShipId == 304)
                            {
                                TargetUserFleets[6].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip42 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[6].ShipId);
                            TargetUserFleets[6].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 9) / refShip42.TotalPowerRating);
                            //stack 8
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[7].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[7].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[7].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[7].ShipId == 304)
                            {
                                TargetUserFleets[7].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip43 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[7].ShipId);
                            TargetUserFleets[7].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 9) / refShip43.TotalPowerRating);
                            // stack 9
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[8].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[8].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[8].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[8].ShipId == 304)
                            {
                                TargetUserFleets[8].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip44 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[8].ShipId);
                            TargetUserFleets[8].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 9) / refShip44.TotalPowerRating);                            
                            TargetUserFleets.Add(new Fleet { ShipId = 315 });
                            TargetUserFleets[9].TotalShips = 1;
                            break;
                        case 10:
                            //stack 1
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[0].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[0].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[0].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[0].ShipId == 304)
                            {
                                TargetUserFleets[0].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip45 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[0].ShipId);
                            TargetUserFleets[0].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 10) / refShip45.TotalPowerRating);
                            // stack 2
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[1].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[1].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[1].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[1].ShipId == 304)
                            {
                                TargetUserFleets[1].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip46 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[1].ShipId);
                            TargetUserFleets[1].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 10) / refShip46.TotalPowerRating);
                            // stack 3
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[2].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[2].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[2].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[2].ShipId == 304)
                            {
                                TargetUserFleets[2].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip47 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[2].ShipId);
                            TargetUserFleets[2].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 10) / refShip47.TotalPowerRating);
                            //stack 4
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[3].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[3].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[3].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[3].ShipId == 304)
                            {
                                TargetUserFleets[3].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip48 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[3].ShipId);
                            TargetUserFleets[3].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 10) / refShip48.TotalPowerRating);
                            //stack 5
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[4].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[4].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[4].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[4].ShipId == 304)
                            {
                                TargetUserFleets[4].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip49 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[4].ShipId);
                            TargetUserFleets[4].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 10) / refShip49.TotalPowerRating);
                            //stack 6
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[5].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[5].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[5].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[5].ShipId == 304)
                            {
                                TargetUserFleets[5].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip50 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[5].ShipId);
                            TargetUserFleets[5].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 10) / refShip50.TotalPowerRating);
                            //stack 7
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[6].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[6].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[6].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[6].ShipId == 304)
                            {
                                TargetUserFleets[6].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip51 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[6].ShipId);
                            TargetUserFleets[6].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 10) / refShip51.TotalPowerRating);
                            //stack 8
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[7].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[7].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[7].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[7].ShipId == 304)
                            {
                                TargetUserFleets[7].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip52 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[7].ShipId);
                            TargetUserFleets[7].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 10) / refShip52.TotalPowerRating);
                            // stack 9
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[8].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[8].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[8].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[8].ShipId == 304)
                            {
                                TargetUserFleets[8].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip53 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[8].ShipId);
                            TargetUserFleets[8].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 10) / refShip53.TotalPowerRating);
                            //stack 10
                            TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
                            if (TargetUserFleets[9].ShipId == 235 || TargetUserFleets[0].ShipId == 236 ||
                                TargetUserFleets[9].ShipId == 254 || TargetUserFleets[0].ShipId == 255 ||
                                TargetUserFleets[9].ShipId == 274 || TargetUserFleets[0].ShipId == 303 ||
                                TargetUserFleets[9].ShipId == 304)
                            {
                                TargetUserFleets[9].ShipId = Random.Shared.Next(275, 303);
                            }
                            var refShip54 = ShipFleets.FirstOrDefault(s => s.ShipId == TargetUserFleets[9].ShipId);
                            TargetUserFleets[9].TotalShips = (int)Math.Floor((double)(targetUser.PowerRating / 10) / refShip54.TotalPowerRating);
                            break;                    
                        default:
                            break;
                    }
                }

            CurrentUser = currentUser;
            TargetUser = targetUser;
            // Default: not at war
            IsFederationWar = false;

            // Check if the user has at least 5 turns
            HasEnoughTurns = currentUser?.Turns?.CurrentTurns >= 5;
            if (!HasEnoughTurns)
            {
                NotEnoughTurnsMessage = "You do not have enough turns to attack. (5 required)";
            }
            // Check for active damage protection
            if (targetUser != null && targetUser.DamageProtection > DateTime.UtcNow)
            {
                // Optionally, set a message to display in the UI
                NotEnoughTurnsMessage = "This user is under damage protection and cannot be attacked at this time.";
                // You can return a custom page, redirect, or just return Page() with the message
                return Page();
            }
            // Only check if both users have federations
            if (CurrentUser?.FederationId != null && TargetUser?.FederationId != null)
            {
                int currentFedId = CurrentUser.FederationId.Value;
                int targetFedId = TargetUser.FederationId.Value;

                // Check for a war in either direction
                IsFederationWar = _context.FederationWars.Any(w =>
                    (w.AttackerFederationId == currentFedId && w.DefenderFederationId == targetFedId) ||
                    (w.AttackerFederationId == targetFedId && w.DefenderFederationId == currentFedId)
                );
            }

            // Check for active counterattack
            bool HasCounterAttack = false;
            if (CurrentUser != null && TargetUser != null)
            {
                HasCounterAttack = _context.CounterAttacks.Any(ca =>
                    ca.ApplicationUserId == CurrentUser.Id &&
                    ca.TargetUserId == TargetUser.Id
                );
            }

            // Set max allowed PowerRating difference
            double maxAllowed;
            if (HasCounterAttack)
            {
                maxAllowed = TargetUser.PowerRating * 2.0;
            }
            else
            {
                maxAllowed = TargetUser.PowerRating * (IsFederationWar ? 1.5 : 1.3);
            }
            PowerRatingAllowed = CurrentUser.PowerRating <= maxAllowed;

            if (!PowerRatingAllowed)
            {
                if (HasCounterAttack)
                {
                    PowerRatingWarning = "Your Power Rating is more than double the target's. You cannot use your counterattack if you exceed this limit.";
                }
                else
                {
                    PowerRatingWarning = IsFederationWar
                        ? "Your Power Rating is more than 50% higher than the target. You cannot attack this user, even during a federation war."
                        : "Your Power Rating is more than 30% higher than the target. You cannot attack this user.";
                }
            }
            // Example: Set the max allowed colonies (adjust as needed)
            bool MaxAllowedColonies = IsAtColonyCap(currentUser); // Replace with your actual game rule

            // Check if the user exceeds the max allowed colonies
            if (MaxAllowedColonies == true)
            {
                NotEnoughTurnsMessage = $"You have exceeded the maximum allowed colonies ({MaxAllowedColonies}). You cannot attack until you are below this limit.";
                return Page();
            }

            List<int> CurrentUserFleetIds = new List<int>();    
            List<int> TargetUserFleetIds = new List<int>();    
            foreach (var fleet in CurrentUserFleets)
            {
                CurrentUserFleetIds.Add(fleet.Id);
            }
            foreach(var fleet in TargetUserFleets)
            {
                TargetUserFleetIds.Add(fleet.Id);
            }
            HttpContext.Session.SetString("SelectedFleetIds", JsonSerializer.Serialize(CurrentUserFleetIds));
            HttpContext.Session.SetString("SelectedFleetIds2", JsonSerializer.Serialize(TargetUserFleetIds));
            return Page();
        }

        private bool IsAtColonyCap(ApplicationUser user)
        {
            // Example: cap at 16, adjust as needed or use faction-based logic
            var maxColoniesByFaction = new Dictionary<Faction, int>
            {
                { Faction.Terran, 16 },
                { Faction.AMiner, 13 },
                { Faction.Collective, 14 },
                { Faction.Marauder, 13 },
                { Faction.Guardian, 11 },
                { Faction.Viral, 13 }
            };
            int cap = maxColoniesByFaction.TryGetValue(user.Faction, out int val) ? val : 10;
            return user.TotalColonies > cap - 1;
        }
    }
}
