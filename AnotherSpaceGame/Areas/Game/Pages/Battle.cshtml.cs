using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class BattleModel : PageModel
    {
        public List<int> CurrentUserFleetIds { get; set; }

        public List<int> TargetUserFleetIds { get; set; }
        [BindProperty(SupportsGet = true)]
        public string TargetUserName { get; set; }
        [BindProperty(SupportsGet = true)]
        public string TargetUserId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CurrentUserName { get; set; }
        [BindProperty(SupportsGet = true)]
        public AttackType AttackType { get; set; }
        public List<Fleet> CurrentUserFleets { get; set; }
        public List<Fleet> TargetUserFleets { get; set; }
        public List<MergedFleet> CurrentUserFleetsStart { get; set; } = new List<MergedFleet>();
        public List<MergedFleet> TargetUserFleetsStart { get; set; } = new List<MergedFleet>();

        private readonly ApplicationDbContext _context;
        private readonly TurnService _turnService;
        private readonly UserManager<ApplicationUser> _userManager;
        public string StatusMessage { get; set; } = string.Empty;

        // wave stuff
        public int DamageDefendersStackTakes { get; set; }
        public int HowManyDefendersShipsKilled { get; set; }
        public int DamageAttackersStackTakes { get; set; }
        public int HowManyAttackersShipsKilled { get; set; }
        public List<BattleResultWave1> BattleResultsWave1 { get; set; } = new List<BattleResultWave1>();
        public List<BattleResultWave2> BattleResultsWave2 { get; set; } = new List<BattleResultWave2>();
        public int AttackerTotalPowerRating { get; set; }
        public int DefenderTotalPowerRating { get; set; }
        public int AttackerTotalPowerRatingLoss { get; set; }
        public int DefenderTotalPowerRatingLoss { get; set; }
        public string EndFleets { get; set; }
        public string VictoryMessage { get; set; } = string.Empty;
        // check stuff
        public bool HasEnoughTurns { get; set; } = true;
        public string NotEnoughTurnsMessage { get; set; }
        public bool PowerRatingAllowed { get; set; } = true;
        public string PowerRatingWarning { get; set; }
        public bool IsFederationWar { get; set; } = false;
        public bool IsCounterAttack { get; set; } = false;

        public BattleModel(ApplicationDbContext context, TurnService turnService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _turnService = turnService;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            // Get the current user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            CurrentUserName = user.UserName;
            var currentUser = _context.Users
                .Include(s => s.Turns)
                .Include(s => s.Fleets)
                .Include(s => s.Federation)                
                .Include(s => s.Planets)                
                .Include(s => s.Battlelogs)                
                .Include(s => s.ImportantEvents)                
                .Include(s => s.CounterAttacks)                
                .Include(s => s.Exploration)                
                .FirstOrDefault(u => u.UserName == CurrentUserName);
            // Get target user by UserName
            ApplicationUser targetUser = _context.Users
                .Include(s => s.Turns)
                .Include(s => s.Fleets)
                .Include(s => s.Federation)
                .Include(s => s.Planets)
                .Include(s => s.Battlelogs)
                .Include(s => s.ImportantEvents)
                .Include(s => s.CounterAttacks)
                .FirstOrDefault(u => u.Id == TargetUserId);
            if (targetUser == null)
            {
                NPCs? npc = _context.NPCs.FirstOrDefault(u => u.UserName == TargetUserName);
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
                        Faction = npc.Faction,
                        Federation = new Federations
                        {
                            Id = npc.FederationId ?? 0,
                            FederationName = "NPC Fed",
                            FederationLeaderId = new Random().Next(1, 10000).ToString(),
                            FederationLeader = new ApplicationUser
                            {
                                UserName = "NPC Leader",
                                Id = new Random().Next(1, 10000).ToString()
                            },
                        },
                        Turns = new Turns
                        {
                            CurrentTurns = 100, // NPCs have a lot of turns
                            MaxTurns = 1000 // NPCs can have more max turns
                        },
                        Battlelogs = new List<BattleLogs>(),
                        ImportantEvents = new List<ImportantEvents>(),
                        CounterAttacks = new List<CounterAttacks>(),
                        Planets = new List<Planets>(),
                        Fleets = new List<Fleet>()
                    };
                }
            }
            // checks
            // Default: not at war
            IsFederationWar = false;
            //default: no counterattack
            IsCounterAttack = false;
            // remove attacker Damage protection
            currentUser.DamageProtection = DateTime.Now;
            // Check if the user has at least 5 turns
            HasEnoughTurns = currentUser.Turns.CurrentTurns >= 5;
            if (!HasEnoughTurns)
            {
                NotEnoughTurnsMessage = "You do not have enough turns to attack. (5 required)";
                return RedirectToPage("ConfirmAttack", new { NotEnoughTurnsMessage, SelectedAttackType = AttackType, TargetUser = targetUser, CurrentUser = currentUser });
            }
            // Check for active damage protection
            if (targetUser != null && targetUser.DamageProtection > DateTime.Now)
            {
                // Optionally, set a message to display in the UI
                NotEnoughTurnsMessage = "This Empire is under damage protection and cannot be attacked at this time.";
                // You can return a custom page, redirect, or just return Page() with the message
                return RedirectToPage("ConfirmAttack", new { NotEnoughTurnsMessage, SelectedAttackType = AttackType, TargetUser = targetUser, CurrentUser = currentUser });
            }
            // Set minimum PowerRating = 5000
            if (targetUser.PowerRating <= 5000)
            {
                PowerRatingWarning = "The target's Power Rating is below the minimum allowed (5001). You cannot attack this user.";
                return RedirectToPage("ConfirmAttack", new { PowerRatingWarning, SelectedAttackType = AttackType, TargetUser = targetUser, CurrentUser = currentUser });
            }
            // check 30% above or below
            var minAllowed = 0.0;
            var maxAllowed = 0.0;
            if (currentUser.PowerRating >= targetUser.PowerRating)
            {
                minAllowed = currentUser.PowerRating * 0.7;
                if (targetUser.PowerRating > minAllowed)
                {
                    PowerRatingAllowed = true;
                }
                else
                {
                    PowerRatingAllowed = false;
                }
            }
            else if (currentUser.PowerRating <= targetUser.PowerRating)
            {
                maxAllowed = currentUser.PowerRating * 1.3;
                if (targetUser.PowerRating > maxAllowed)
                {
                    PowerRatingAllowed = false;
                }
                else
                {
                    PowerRatingAllowed = true;
                }
            }

            // Check if federation war is present
            if (currentUser.FederationId != null && targetUser.FederationId != null)
            {
                int currentFedId = currentUser.FederationId.Value;
                int targetFedId = targetUser.FederationId.Value;

                // Check for a war in either direction
                IsFederationWar = _context.FederationWars.Any(w =>
                    (w.AttackerFederationId == currentFedId && w.DefenderFederationId == targetFedId) ||
                    (w.AttackerFederationId == targetFedId && w.DefenderFederationId == currentFedId)
                );
                if (currentUser.PowerRating >= targetUser.PowerRating && IsFederationWar == true)
                {
                    minAllowed = currentUser.PowerRating * 0.5;
                    if (targetUser.PowerRating > minAllowed)
                    {
                        PowerRatingAllowed = true;
                    }
                    else
                    {
                        PowerRatingAllowed = false;
                    }

                }
                else if (currentUser.PowerRating <= targetUser.PowerRating && IsFederationWar == true)
                {
                    maxAllowed = currentUser.PowerRating * 1.5;
                    if (targetUser.PowerRating > maxAllowed)
                    {
                        PowerRatingAllowed = false;
                    }
                    else
                    {
                        PowerRatingAllowed = true;
                    }
                }
            }
            // Check if Counter attack is present
            // Check for active counterattack
            if (currentUser != null && targetUser != null && targetUser.IsNPC == false)
            {
                IsCounterAttack = _context.CounterAttacks.Any(ca =>
                    ca.ApplicationUserId == currentUser.Id &&
                    ca.TargetUserId == targetUser.Id
                );
                if (currentUser.PowerRating >= targetUser.PowerRating && IsCounterAttack == true)
                {
                    minAllowed = currentUser.PowerRating * 0.01;
                    if (targetUser.PowerRating > minAllowed)
                    {
                        PowerRatingAllowed = true;
                    }
                    else
                    {
                        PowerRatingAllowed = false;
                    }

                }
                else if (currentUser.PowerRating <= targetUser.PowerRating && IsCounterAttack == true)
                {
                    maxAllowed = currentUser.PowerRating * 99;
                    if (targetUser.PowerRating > maxAllowed)
                    {
                        PowerRatingAllowed = false;
                    }
                    else
                    {
                        PowerRatingAllowed = true;
                    }
                }
            }
            // display warning if PowerRating is not allowed
            if (!PowerRatingAllowed)
            {
                PowerRatingWarning = $"The target's Power Rating ({targetUser.PowerRating}) is not within the allowed range based on your Power Rating ({currentUser.PowerRating}). You cannot attack this user.";
                return RedirectToPage("ConfirmAttack", new { PowerRatingWarning, SelectedAttackType = AttackType, TargetUser = targetUser, CurrentUser = currentUser });
            }
            // Example: Set the max allowed colonies (adjust as needed)
            bool MaxAllowedColonies = IsAtColonyCap(currentUser); // Replace with your actual game rule
            // Check if the user exceeds the max allowed colonies
            if (MaxAllowedColonies == true)
            {
                NotEnoughTurnsMessage = $"You have exceeded the maximum allowed colonies ({MaxAllowedColonies}). You cannot attack until you are below this limit.";
                return RedirectToPage("ConfirmAttack", new { NotEnoughTurnsMessage, SelectedAttackType = AttackType, TargetUser = targetUser, CurrentUser = currentUser });
            }
            // check for self attack
            if (currentUser.Id == targetUser.Id)
            {
                NotEnoughTurnsMessage = "You cannot attack yourself.";
                return RedirectToPage("ConfirmAttack", new { NotEnoughTurnsMessage, SelectedAttackType = AttackType, TargetUser = targetUser, CurrentUser = currentUser });
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
                                    where fleet.ApplicationUserId == targetUser.Id
                                          && ship.ShipType != ShipType.Starbase
                                          && ship.ShipType != ShipType.Scout
                                    orderby fleet.TotalPowerRating descending
                                    select fleet)
                             .Take(10)
                             .ToList();
            }
            if (TargetUserFleets == null && targetUser.IsNPC == true)
            {
                // merg all ships to create a list of fleets for referencing
                var allships = _context.Ships.ToList();
                List<UserShipFleet> ShipFleets = new List<UserShipFleet>();
                TargetUserFleets = new List<Fleet>();
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
                var random = Random.Shared.Next(1, 11);
                switch (random)
                {
                    case 1:
                        //stack 1
                        TargetUserFleets.Add(new Fleet { ShipId = Random.Shared.Next(219, 303) });
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
                        TargetUserFleets[0].TotalShips = (int)Math.Ceiling((double)targetUser.PowerRating / refShip0.PowerRatingPerShip);
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
                        TargetUserFleets[0].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 2) / refShip1.PowerRatingPerShip);
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
                        TargetUserFleets[1].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 2) / refShip2.PowerRatingPerShip);
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
                        TargetUserFleets[0].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 3) / refShip3.PowerRatingPerShip);
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
                        TargetUserFleets[1].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 3) / refShip4.PowerRatingPerShip);
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
                        TargetUserFleets[2].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 3) / refShip5.PowerRatingPerShip);
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
                        TargetUserFleets[0].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 4) / refShip6.PowerRatingPerShip);
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
                        TargetUserFleets[1].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 4) / refShip7.PowerRatingPerShip);
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
                        TargetUserFleets[2].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 4) / refShip8.PowerRatingPerShip);
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
                        TargetUserFleets[3].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 4) / refShip8.PowerRatingPerShip);
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
                        TargetUserFleets[0].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 5) / refShip10.PowerRatingPerShip);
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
                        TargetUserFleets[1].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 5) / refShip11.PowerRatingPerShip);
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
                        TargetUserFleets[2].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 5) / refShip12.PowerRatingPerShip);
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
                        TargetUserFleets[3].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 5) / refShip13.PowerRatingPerShip);
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
                        TargetUserFleets[4].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 5) / refShip14.PowerRatingPerShip);
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
                        TargetUserFleets[0].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 6) / refShip15.PowerRatingPerShip);
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
                        TargetUserFleets[1].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 6) / refShip16.PowerRatingPerShip);
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
                        TargetUserFleets[2].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 6) / refShip17.PowerRatingPerShip);
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
                        TargetUserFleets[3].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 6) / refShip18.PowerRatingPerShip);
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
                        TargetUserFleets[4].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 6) / refShip19.PowerRatingPerShip);
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
                        TargetUserFleets[5].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 6) / refShip20.PowerRatingPerShip);
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
                        TargetUserFleets[0].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 7) / refShip21.PowerRatingPerShip);
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
                        TargetUserFleets[1].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 7) / refShip22.PowerRatingPerShip);
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
                        TargetUserFleets[2].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 7) / refShip23.PowerRatingPerShip);
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
                        TargetUserFleets[3].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 7) / refShip24.PowerRatingPerShip);
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
                        TargetUserFleets[4].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 7) / refShip25.PowerRatingPerShip);
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
                        TargetUserFleets[5].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 7) / refShip26.PowerRatingPerShip);
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
                        TargetUserFleets[6].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 7) / refShip27.PowerRatingPerShip);
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
                        TargetUserFleets[0].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 8) / refShip28.PowerRatingPerShip);
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
                        TargetUserFleets[1].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 8) / refShip29.PowerRatingPerShip);
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
                        TargetUserFleets[2].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 8) / refShip30.PowerRatingPerShip);
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
                        TargetUserFleets[3].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 8) / refShip31.PowerRatingPerShip);
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
                        TargetUserFleets[4].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 8) / refShip32.PowerRatingPerShip);
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
                        TargetUserFleets[5].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 8) / refShip33.PowerRatingPerShip);
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
                        TargetUserFleets[6].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 8) / refShip34.PowerRatingPerShip);
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
                        TargetUserFleets[7].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 8) / refShip35.PowerRatingPerShip);
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
                        TargetUserFleets[0].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 9) / refShip36.PowerRatingPerShip);
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
                        TargetUserFleets[1].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 9) / refShip37.PowerRatingPerShip);
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
                        TargetUserFleets[2].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 9) / refShip38.PowerRatingPerShip);
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
                        TargetUserFleets[3].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 9) / refShip39.PowerRatingPerShip);
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
                        TargetUserFleets[4].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 9) / refShip40.PowerRatingPerShip);
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
                        TargetUserFleets[5].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 9) / refShip41.PowerRatingPerShip);
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
                        TargetUserFleets[6].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 9) / refShip42.PowerRatingPerShip);
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
                        TargetUserFleets[7].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 9) / refShip43.PowerRatingPerShip);
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
                        TargetUserFleets[8].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 9) / refShip44.PowerRatingPerShip);
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
                        TargetUserFleets[0].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 10) / refShip45.PowerRatingPerShip);
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
                        TargetUserFleets[1].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 10) / refShip46.PowerRatingPerShip);
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
                        TargetUserFleets[2].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 10) / refShip47.PowerRatingPerShip);
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
                        TargetUserFleets[3].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 10) / refShip48.PowerRatingPerShip);
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
                        TargetUserFleets[4].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 10) / refShip49.PowerRatingPerShip);
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
                        TargetUserFleets[5].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 10) / refShip50.PowerRatingPerShip);
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
                        TargetUserFleets[6].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 10) / refShip51.PowerRatingPerShip);
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
                        TargetUserFleets[7].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 10) / refShip52.PowerRatingPerShip);
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
                        TargetUserFleets[8].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 10) / refShip53.PowerRatingPerShip);
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
                        TargetUserFleets[9].TotalShips = (int)Math.Ceiling((double)(targetUser.PowerRating / 10) / refShip54.PowerRatingPerShip);
                        break;
                    default:
                        break;
                }
            }
            // Merge Fleets with ShipType information
            List<MergedFleet> AttackerFleet = new List<MergedFleet>();
            List<MergedFleet> DefenderFleet = new List<MergedFleet>();
            var Ships = _context.Ships.ToList();
            foreach (var item in CurrentUserFleets)
            {
                AttackerFleet.Add(new MergedFleet
                {
                    Id = item.Id,
                    ApplicationUserId = item.ApplicationUserId,
                    UserName = CurrentUserName,
                    ShipId = item.ShipId,
                    TotalShips = item.TotalShips,
                    PowerRating = Ships.FirstOrDefault(s => s.Id == item.ShipId).PowerRating,
                    TotalPowerRating = Ships.FirstOrDefault(s => s.Id == item.ShipId).PowerRating * item.TotalShips,
                    TotalUpkeep = item.TotalShips * item.TotalUpkeep,
                    ShipName = Ships.FirstOrDefault(s => s.Id == item.ShipId).ShipName,
                    ShipType = Ships.FirstOrDefault(s => s.Id == item.ShipId).ShipType,
                    Range = Ships.FirstOrDefault(s => s.Id == item.ShipId).Range,
                    ApplicationUser = item.ApplicationUser,
                    Weapon = Ships.FirstOrDefault(s => s.Id == item.ShipId).Weapon,
                    Hull = Ships.FirstOrDefault(s => s.Id == item.ShipId).Hull,
                    EnergyWeapon = Ships.FirstOrDefault(s => s.Id == item.ShipId).EnergyWeapon,
                    KineticWeapon = Ships.FirstOrDefault(s => s.Id == item.ShipId).KineticWeapon,
                    MissileWeapon = Ships.FirstOrDefault(s => s.Id == item.ShipId).MissileWeapon,
                    ChemicalWeapon = Ships.FirstOrDefault(s => s.Id == item.ShipId).ChemicalWeapon,
                    EnergyShield = Ships.FirstOrDefault(s => s.Id == item.ShipId).EnergyShield,
                    KineticShield = Ships.FirstOrDefault(s => s.Id == item.ShipId).KineticShield,
                    MissileShield = Ships.FirstOrDefault(s => s.Id == item.ShipId).MissileShield,
                    ChemicalShield = Ships.FirstOrDefault(s => s.Id == item.ShipId).ChemicalShield,
                    NoDefense = Ships.FirstOrDefault(s => s.Id == item.ShipId).NoDefense,
                    NoRetal = Ships.FirstOrDefault(s => s.Id == item.ShipId).NoRetal,
                    CanCapture = Ships.FirstOrDefault(s => s.Id == item.ShipId).CanCapture,
                    CapChance = Ships.FirstOrDefault(s => s.Id == item.ShipId).CapChance,
                    Composite = Ships.FirstOrDefault(s => s.Id == item.ShipId).Composite,
                    RedCrystal = Ships.FirstOrDefault(s => s.Id == item.ShipId).RedCrystal,
                    Rutile = Ships.FirstOrDefault(s => s.Id == item.ShipId).Rutile,
                    WhiteCrystal = Ships.FirstOrDefault(s => s.Id == item.ShipId).WhiteCrystal,
                    StrafezOrganism = Ships.FirstOrDefault(s => s.Id == item.ShipId).StrafezOrganism,
                    TerranMetal = Ships.FirstOrDefault(s => s.Id == item.ShipId).TerranMetal,
                    CostToBuild = Ships.FirstOrDefault(s => s.Id == item.ShipId).CostToBuild,
                    ScanningPower = Ships.FirstOrDefault(s => s.Id == item.ShipId).ScanningPower,
                    BuildRate = Ships.FirstOrDefault(s => s.Id == item.ShipId).BuildRate,
                    Cost = Ships.FirstOrDefault(s => s.Id == item.ShipId).Cost,
                    ImmuneToCapture = Ships.FirstOrDefault(s => s.Id == item.ShipId).ImmuneToCapture,
                    Upkeep = Ships.FirstOrDefault(s => s.Id == item.ShipId).Upkeep,
                    TotalShipsStart = item.TotalShips // Store the initial number of ships for the battle report
                });
            }

            foreach (var item in TargetUserFleets)
            {
                DefenderFleet.Add(new MergedFleet
                {
                    Id = item.Id,
                    ApplicationUserId = item.ApplicationUserId,
                    UserName = TargetUserName,
                    ShipId = item.ShipId,
                    TotalShips = item.TotalShips,
                    PowerRating = Ships.FirstOrDefault(s => s.Id == item.ShipId).PowerRating,
                    TotalPowerRating = Ships.FirstOrDefault(s => s.Id == item.ShipId).PowerRating * item.TotalShips,
                    TotalUpkeep = item.TotalShips * item.TotalUpkeep,
                    ShipName = Ships.FirstOrDefault(s => s.Id == item.ShipId).ShipName,
                    ShipType = Ships.FirstOrDefault(s => s.Id == item.ShipId).ShipType,
                    Range = Ships.FirstOrDefault(s => s.Id == item.ShipId).Range,
                    ApplicationUser = item.ApplicationUser,
                    Weapon = Ships.FirstOrDefault(s => s.Id == item.ShipId).Weapon,
                    Hull = Ships.FirstOrDefault(s => s.Id == item.ShipId).Hull,
                    EnergyWeapon = Ships.FirstOrDefault(s => s.Id == item.ShipId).EnergyWeapon,
                    KineticWeapon = Ships.FirstOrDefault(s => s.Id == item.ShipId).KineticWeapon,
                    MissileWeapon = Ships.FirstOrDefault(s => s.Id == item.ShipId).MissileWeapon,
                    ChemicalWeapon = Ships.FirstOrDefault(s => s.Id == item.ShipId).ChemicalWeapon,
                    EnergyShield = Ships.FirstOrDefault(s => s.Id == item.ShipId).EnergyShield,
                    KineticShield = Ships.FirstOrDefault(s => s.Id == item.ShipId).KineticShield,
                    MissileShield = Ships.FirstOrDefault(s => s.Id == item.ShipId).MissileShield,
                    ChemicalShield = Ships.FirstOrDefault(s => s.Id == item.ShipId).ChemicalShield,
                    NoDefense = Ships.FirstOrDefault(s => s.Id == item.ShipId).NoDefense,
                    NoRetal = Ships.FirstOrDefault(s => s.Id == item.ShipId).NoRetal,
                    CanCapture = Ships.FirstOrDefault(s => s.Id == item.ShipId).CanCapture,
                    CapChance = Ships.FirstOrDefault(s => s.Id == item.ShipId).CapChance,
                    Composite = Ships.FirstOrDefault(s => s.Id == item.ShipId).Composite,
                    RedCrystal = Ships.FirstOrDefault(s => s.Id == item.ShipId).RedCrystal,
                    Rutile = Ships.FirstOrDefault(s => s.Id == item.ShipId).Rutile,
                    WhiteCrystal = Ships.FirstOrDefault(s => s.Id == item.ShipId).WhiteCrystal,
                    StrafezOrganism = Ships.FirstOrDefault(s => s.Id == item.ShipId).StrafezOrganism,
                    TerranMetal = Ships.FirstOrDefault(s => s.Id == item.ShipId).TerranMetal,
                    CostToBuild = Ships.FirstOrDefault(s => s.Id == item.ShipId).CostToBuild,
                    ScanningPower = Ships.FirstOrDefault(s => s.Id == item.ShipId).ScanningPower,
                    BuildRate = Ships.FirstOrDefault(s => s.Id == item.ShipId).BuildRate,
                    Cost = Ships.FirstOrDefault(s => s.Id == item.ShipId).Cost,
                    ImmuneToCapture = Ships.FirstOrDefault(s => s.Id == item.ShipId).ImmuneToCapture,
                    Upkeep = Ships.FirstOrDefault(s => s.Id == item.ShipId).Upkeep,
                    TotalShipsStart = item.TotalShips // Store the initial number of ships for the battle report
                });
            }
            AttackerTotalPowerRating = AttackerFleet.Sum(f => f.TotalPowerRating);
            DefenderTotalPowerRating = DefenderFleet.Sum(f => f.TotalPowerRating);
            CurrentUserFleetsStart = AttackerFleet
            .Select(f => new MergedFleet
            {
        Id = f.Id,
        ApplicationUserId = f.ApplicationUserId,
        ApplicationUser = f.ApplicationUser, // If you want a deep copy, clone this too
        ShipId = f.ShipId,
        TotalShips = f.TotalShips,
        TotalPowerRating = f.TotalPowerRating,
        TotalUpkeep = f.TotalUpkeep,
        ShipName = f.ShipName,
        ShipType = f.ShipType,
        PowerRating = f.PowerRating,
        Range = f.Range,
        Weapon = f.Weapon,
        Hull = f.Hull,
        EnergyWeapon = f.EnergyWeapon,
        KineticWeapon = f.KineticWeapon,
        MissileWeapon = f.MissileWeapon,
        ChemicalWeapon = f.ChemicalWeapon,
        EnergyShield = f.EnergyShield,
        KineticShield = f.KineticShield,
        MissileShield = f.MissileShield,
        ChemicalShield = f.ChemicalShield,
        NoDefense = f.NoDefense,
        NoRetal = f.NoRetal,
        CapChance = f.CapChance,
        Cost = f.Cost,
        Upkeep = f.Upkeep,
        TerranMetal = f.TerranMetal,
        RedCrystal = f.RedCrystal,
        WhiteCrystal = f.WhiteCrystal,
        Rutile = f.Rutile,
        Composite = f.Composite,
        StrafezOrganism = f.StrafezOrganism,
        ScanningPower = f.ScanningPower,
        BuildRate = f.BuildRate,
        CostToBuild = f.CostToBuild,
        ImmuneToCapture = f.ImmuneToCapture,
        CanCapture = f.CanCapture,
        TotalShipsStart = f.TotalShipsStart
            })
            .ToList(); ;
            TargetUserFleetsStart = DefenderFleet
            .Select(f => new MergedFleet
    {
        Id = f.Id,
        ApplicationUserId = f.ApplicationUserId,
        ApplicationUser = f.ApplicationUser, // If you want a deep copy, clone this too
        ShipId = f.ShipId,
        TotalShips = f.TotalShips,
        TotalPowerRating = f.TotalPowerRating,
        TotalUpkeep = f.TotalUpkeep,
        ShipName = f.ShipName,
        ShipType = f.ShipType,
        PowerRating = f.PowerRating,
        Range = f.Range,
        Weapon = f.Weapon,
        Hull = f.Hull,
        EnergyWeapon = f.EnergyWeapon,
        KineticWeapon = f.KineticWeapon,
        MissileWeapon = f.MissileWeapon,
        ChemicalWeapon = f.ChemicalWeapon,
        EnergyShield = f.EnergyShield,
        KineticShield = f.KineticShield,
        MissileShield = f.MissileShield,
        ChemicalShield = f.ChemicalShield,
        NoDefense = f.NoDefense,
        NoRetal = f.NoRetal,
        CapChance = f.CapChance,
        Cost = f.Cost,
        Upkeep = f.Upkeep,
        TerranMetal = f.TerranMetal,
        RedCrystal = f.RedCrystal,
        WhiteCrystal = f.WhiteCrystal,
        Rutile = f.Rutile,
        Composite = f.Composite,
        StrafezOrganism = f.StrafezOrganism,
        ScanningPower = f.ScanningPower,
        BuildRate = f.BuildRate,
        CostToBuild = f.CostToBuild,
        ImmuneToCapture = f.ImmuneToCapture,
        CanCapture = f.CanCapture,
        TotalShipsStart = f.TotalShipsStart
    })
            .ToList();
            if (TargetUserFleets.Count() > 0)
            {
                // Wave 1
                // Set Stacks for Wave 1
                int stacks = Math.Max(AttackerFleet.Count, DefenderFleet.Count);
                for (int i = 0; i < stacks; i++)
                {
                    MergedFleet attacker;
                    MergedFleet defender;

                    // Attacker selection
                    if (i < AttackerFleet.Count)
                    {
                        attacker = AttackerFleet[i];
                        if (attacker.TotalShips <= 0)
                        {
                            // Pick a random fleet with TotalShips > 0, or null if none
                            var available = AttackerFleet.Where(f => f.TotalShips > 0).ToList();
                            attacker = available.Any() ? available[Random.Shared.Next(available.Count)] : null;
                        }
                    }
                    else
                    {
                        var available = AttackerFleet.Where(f => f.TotalShips > 0).ToList();
                        attacker = available.Any() ? available[Random.Shared.Next(available.Count)] : null;
                    }

                    // Defender selection
                    if (i < DefenderFleet.Count)
                    {
                        defender = DefenderFleet[i];
                        if (defender.TotalShips <= 0)
                        {
                            var available = DefenderFleet.Where(f => f.TotalShips > 0).ToList();
                            defender = available.Any() ? available[Random.Shared.Next(available.Count)] : null;
                        }
                    }
                    else
                    {
                        var available = DefenderFleet.Where(f => f.TotalShips > 0).ToList();
                        defender = available.Any() ? available[Random.Shared.Next(available.Count)] : null;
                    }

                    // If either side has no valid fleets left, you may want to break or continue
                    if (attacker == null || defender == null)
                        continue; // or break;
                    // Defender Attacks First
                    if (defender.Range >= attacker.Range)
                    {
                        
                        // Defender Attacks First
                        // Defender Attacks
                        DamageAttackersStackTakes = CalulateDefenderDamage(attacker, defender, false).DamageDealt;
                        HowManyAttackersShipsKilled = CalulateDefenderDamage(attacker, defender, false).ShipsKilled;
                        //// Attacker Retals (if Defender does not have NoRetal && Attacker does not have NoDefense)
                        if (defender.NoRetal == false && attacker.NoDefense == false)
                        {
                            DamageDefendersStackTakes = CalulateAttackerDamage(attacker, defender, true).DamageDealt;
                            HowManyDefendersShipsKilled = CalulateAttackerDamage(attacker, defender, true).ShipsKilled;
                        }
                        else
                        {
                            DamageDefendersStackTakes = 0;
                            HowManyDefendersShipsKilled = 0;
                        }
                        // capture chance
                        if (defender.CanCapture == true && HowManyAttackersShipsKilled > 0 && attacker.ImmuneToCapture == false && attacker.CanCapture == false && targetUser.IsNPC != true)
                        {
                            // Capture Ships
                            bool willCapture = false;
                            var captureChance = (int)Math.Floor(defender.CapChance * 100);
                            if (captureChance == 100)
                            {
                                willCapture = true;
                            }
                            else
                            {
                                var randomValue = Random.Shared.Next(0, 100);
                                if (randomValue <= captureChance)
                                {
                                    willCapture = true;
                                }
                                else
                                {
                                    willCapture = false;
                                }
                            }
                            if (willCapture == true)
                            {
                                // Capture Ships
                                var totalCapturedShips = HowManyAttackersShipsKilled;
                                var ShipName = attacker.ShipName;
                                var ShipId = attacker.ShipId;
                                // add defender new fleet
                                var newDefenderFleet = new Fleet
                                {
                                    ApplicationUserId = defender.ApplicationUserId,
                                    ApplicationUser = defender.ApplicationUser,
                                    ShipId = ShipId,
                                    TotalShips = totalCapturedShips,
                                    TotalPowerRating = totalCapturedShips * Ships.FirstOrDefault(s => s.Id == ShipId).PowerRating,
                                    TotalUpkeep = totalCapturedShips * Ships.FirstOrDefault(s => s.Id == ShipId).Upkeep
                                };
                                _context.Fleets.Add(newDefenderFleet);
                                // Add to Battle Results
                                BattleResultsWave1.Add(new BattleResultWave1
                                {
                                    DefenderCapture = $"{defender.UserName} captured {totalCapturedShips} {ShipName} ships from {attacker.UserName}."
                                });
                            }
                        }
                        // add battle result
                        BattleResultsWave1.Add(new BattleResultWave1
                        {
                            IsAttacker = false,
                            AttackerShipName = attacker.ShipName,
                            DefenderShipName = defender.ShipName,
                            AttackerDamageDelt = DamageDefendersStackTakes,
                            DefenderDamageDelt = DamageAttackersStackTakes,
                            AttackerShipTotalLoss = HowManyAttackersShipsKilled,
                            DefenderShipTotalLoss = HowManyDefendersShipsKilled
                        });
                        // Update Stacks
                        attacker.TotalShips -= HowManyAttackersShipsKilled;
                        defender.TotalShips -= HowManyDefendersShipsKilled;
                        // Attacker Attacks
                        DamageDefendersStackTakes = CalulateAttackerDamage(attacker, defender, false).DamageDealt;
                        HowManyDefendersShipsKilled = CalulateAttackerDamage(attacker, defender, false).ShipsKilled;
                        //// Defender Retals (if Attacker does not have NoRetal && Defender does not have NoDefense)
                        if (attacker.NoRetal == false && defender.NoDefense == false)
                        {
                            DamageAttackersStackTakes = CalulateDefenderDamage(attacker, defender, true).DamageDealt;
                            HowManyAttackersShipsKilled = CalulateDefenderDamage(attacker, defender, true).ShipsKilled;
                        }
                        else
                        {
                            DamageAttackersStackTakes = 0;
                            HowManyAttackersShipsKilled = 0;
                        }
                        // add battle result
                        BattleResultsWave1.Add(new BattleResultWave1
                        {
                            IsAttacker = true,
                            AttackerShipName = attacker.ShipName,
                            DefenderShipName = defender.ShipName,
                            AttackerDamageDelt = DamageDefendersStackTakes,
                            DefenderDamageDelt = DamageAttackersStackTakes,
                            AttackerShipTotalLoss = HowManyAttackersShipsKilled,
                            DefenderShipTotalLoss = HowManyDefendersShipsKilled
                        });
                        // Update Stacks
                        attacker.TotalShips -= HowManyAttackersShipsKilled;
                        defender.TotalShips -= HowManyDefendersShipsKilled;
                        // add power rating losses
                        AttackerTotalPowerRatingLoss += HowManyAttackersShipsKilled * attacker.PowerRating;
                        DefenderTotalPowerRatingLoss += HowManyDefendersShipsKilled * defender.PowerRating;
                    }
                    // Attacker Attacks First
                    else
                    {
                        
                        // Attacker Attacks First
                        // Attacker Attacks
                        DamageDefendersStackTakes = CalulateAttackerDamage(attacker, defender, false).DamageDealt;
                        HowManyDefendersShipsKilled = CalulateAttackerDamage(attacker, defender, false).ShipsKilled;
                        //// Defender Retals (if Attacker does not have NoRetal && Defender does not have NoDefense)
                        if (attacker.NoRetal == false && defender.NoDefense == false)
                        {
                            DamageAttackersStackTakes = CalulateDefenderDamage(attacker, defender, true).DamageDealt;
                            HowManyAttackersShipsKilled = CalulateDefenderDamage(attacker, defender, true).ShipsKilled;
                        }
                        else
                        {
                            DamageAttackersStackTakes = 0;
                            HowManyAttackersShipsKilled = 0;
                        }
                        // add battle result
                        BattleResultsWave1.Add(new BattleResultWave1
                        {
                            IsAttacker = true,
                            AttackerShipName = attacker.ShipName,
                            DefenderShipName = defender.ShipName,
                            AttackerDamageDelt = DamageDefendersStackTakes,
                            DefenderDamageDelt = DamageAttackersStackTakes,
                            AttackerShipTotalLoss = HowManyAttackersShipsKilled,
                            DefenderShipTotalLoss = HowManyDefendersShipsKilled
                        });
                        // Update Stacks
                        attacker.TotalShips -= HowManyAttackersShipsKilled;
                        defender.TotalShips -= HowManyDefendersShipsKilled;
                        // Defender Attacks
                        DamageAttackersStackTakes = CalulateDefenderDamage(attacker, defender, false).DamageDealt;
                        HowManyAttackersShipsKilled = CalulateDefenderDamage(attacker, defender, false).ShipsKilled;
                        //// Attacker Retals (if Defender does not have NoRetal && Attacker does not have NoDefense)
                        if (defender.NoRetal == false && attacker.NoDefense == false)
                        {
                            DamageDefendersStackTakes = CalulateAttackerDamage(attacker, defender, true).DamageDealt;
                            HowManyDefendersShipsKilled = CalulateAttackerDamage(attacker, defender, true).ShipsKilled;
                        }
                        else
                        {
                            DamageDefendersStackTakes = 0;
                            HowManyDefendersShipsKilled = 0;
                        }
                        // capture chance
                        if (attacker.CanCapture == true && HowManyDefendersShipsKilled > 0 && defender.ImmuneToCapture == false && defender.CanCapture == false)
                        {
                            // Capture Ships
                            bool willCapture = false;
                            var captureChance = (int)Math.Floor(attacker.CapChance * 100);
                            if (captureChance == 100)
                            {
                                willCapture = true;
                            }
                            else
                            {
                                var randomValue = Random.Shared.Next(0, 100);
                                if (randomValue <= captureChance)
                                {
                                    willCapture = true;
                                }
                                else
                                {
                                    willCapture = false;
                                }
                            }
                            if (willCapture == true)
                            {
                                // Capture Ships
                                var totalCapturedShips = HowManyDefendersShipsKilled;
                                var ShipName = defender.ShipName;
                                var ShipId = defender.ShipId;
                                // add defender new fleet
                                var newAttackerFleet = new Fleet
                                {
                                    ApplicationUserId = attacker.ApplicationUserId,
                                    ApplicationUser = attacker.ApplicationUser,
                                    ShipId = ShipId,
                                    TotalShips = totalCapturedShips,
                                    TotalPowerRating = totalCapturedShips * Ships.FirstOrDefault(s => s.Id == ShipId).PowerRating,
                                    TotalUpkeep = totalCapturedShips * Ships.FirstOrDefault(s => s.Id == ShipId).Upkeep
                                };
                                _context.Fleets.Add(newAttackerFleet);
                                // Add to Battle Results
                                BattleResultsWave1.Add(new BattleResultWave1
                                {
                                    AttackerCapture = $"{attacker.UserName} captured {totalCapturedShips} {ShipName} ships from {defender.UserName}."
                                });
                            }
                        }
                        // add battle result
                        BattleResultsWave1.Add(new BattleResultWave1
                        {
                            IsAttacker = false,
                            AttackerShipName = attacker.ShipName,
                            DefenderShipName = defender.ShipName,
                            AttackerDamageDelt = DamageAttackersStackTakes,
                            DefenderDamageDelt = DamageDefendersStackTakes,
                            AttackerShipTotalLoss = HowManyAttackersShipsKilled,
                            DefenderShipTotalLoss = HowManyDefendersShipsKilled
                        });
                        // Update Stacks
                        attacker.TotalShips -= HowManyAttackersShipsKilled;
                        defender.TotalShips -= HowManyDefendersShipsKilled;
                        // add power rating losses
                        AttackerTotalPowerRatingLoss += HowManyAttackersShipsKilled * attacker.PowerRating;
                        DefenderTotalPowerRatingLoss += HowManyDefendersShipsKilled * defender.PowerRating;
                    }

                    // Alter Fleets based on damage dealt and taken
                    foreach (var item in AttackerFleet)
                    {
                        item.TotalPowerRating = item.TotalShips * item.PowerRating;
                    }
                    AttackerFleet.OrderByDescending(p => p.TotalPowerRating).ToList();
                    foreach (var item in DefenderFleet)
                    {
                        item.TotalPowerRating = item.TotalShips * item.PowerRating;
                    }
                    DefenderFleet.OrderByDescending(p => p.TotalPowerRating).ToList();
                    // Wave 1 End
                    // check if defender has any fleets left
                    // defender no fleet
                    if (DefenderFleet.All(f => f.TotalShips <= 0))
                    {
                        // Defender has no fleet left
                        // Attacker wins by default
                        // Sort Planets For Capture
                        List<Planets> DefenderPlanets = new List<Planets>();
                        if (targetUser.IsNPC == true)
                        {
                            var planetTypeWeights = new List<(PlanetType Type, double Weight)>
                            {
                               (PlanetType.Barren, 5),
                               (PlanetType.Icy, 5),
                               (PlanetType.Marshy, 5),
                               (PlanetType.Forest, 5),
                               (PlanetType.Oceanic, 5),
                               (PlanetType.Rocky, 5),
                               (PlanetType.Desert, 5),
                               (PlanetType.Balanced, 5),
                               (PlanetType.Gas, 5),
                               (PlanetType.URich, 0.5),
                               (PlanetType.UEden, 0.5),
                               (PlanetType.USpazial, 0.5),
                               (PlanetType.ULarge, 0.5),
                               (PlanetType.UFertile, 0.5),
                               (PlanetType.Dead, 0.5),
                               (PlanetType.ClusterLevel1, 0.25),
                               (PlanetType.ClusterLevel2, 0.10),
                               (PlanetType.ClusterLevel3, 0.05)
                            };
                            var random = new System.Random();
                            var chosenType = GetRandomPlanetTypeWeighted(planetTypeWeights);
                            //planet 1
                            var NewPlanet = new Planets
                            {
                                ApplicationUserId = user.Id,
                                ApplicationUser = user,
                                Name = "E." + random.Next(1000, 9999).ToString(),
                                Type = chosenType,
                                FoodRequired = 1,
                                GoodsRequired = 1,
                                CurrentPopulation = 10,
                                MaxPopulation = 10,
                                Loyalty = 2500,
                                AvailableLabour = 8,
                                Housing = 1,
                                Commercial = 0,
                                Industry = 0,
                                Agriculture = 0,
                                Mining = 1,
                                MineralProduced = 0,
                                PowerRating = 0

                            };
                            // Set type-specific values
                            switch (chosenType)
                            {
                                case PlanetType.Barren:
                                    NewPlanet.PopulationModifier = 0.5m;
                                    NewPlanet.AgricultureModifier = 0m;
                                    NewPlanet.OreModifier = 0.02m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.AvailableOre = random.Next(150, 500);
                                    NewPlanet.TotalLand = random.Next(50, 1100);
                                    break;
                                case PlanetType.Icy:
                                    NewPlanet.PopulationModifier = 0.75m;
                                    NewPlanet.AgricultureModifier = 1m;
                                    NewPlanet.OreModifier = 0.005m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.AvailableOre = random.Next(1500, 4500);
                                    NewPlanet.TotalLand = random.Next(24, 83);
                                    break;
                                case PlanetType.Marshy:
                                    NewPlanet.PopulationModifier = 0.8m;
                                    NewPlanet.AgricultureModifier = 0.5m;
                                    NewPlanet.OreModifier = 0.005m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(50, 150);
                                    NewPlanet.AvailableOre = random.Next(0, 150);
                                    break;
                                case PlanetType.Forest:
                                    NewPlanet.PopulationModifier = 0.9m;
                                    NewPlanet.AgricultureModifier = 2m;
                                    NewPlanet.OreModifier = 0.005m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(50, 350);
                                    NewPlanet.AvailableOre = random.Next(0, 50);
                                    break;
                                case PlanetType.Oceanic:
                                    NewPlanet.PopulationModifier = 0.8m;
                                    NewPlanet.AgricultureModifier = 0m;
                                    NewPlanet.OreModifier = 0.005m;
                                    NewPlanet.ArtifactModifier = 0.10m;
                                    NewPlanet.TotalLand = random.Next(10, 50);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                                case PlanetType.Rocky:
                                    NewPlanet.PopulationModifier = 0.75m;
                                    NewPlanet.AgricultureModifier = 1m;
                                    NewPlanet.OreModifier = 0.001m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(34, 121);
                                    NewPlanet.AvailableOre = random.Next(500, 3300);
                                    break;
                                case PlanetType.Desert:
                                    NewPlanet.PopulationModifier = 0.75m;
                                    NewPlanet.AgricultureModifier = 0.75m;
                                    NewPlanet.OreModifier = 0.02m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(100, 250);
                                    NewPlanet.AvailableOre = random.Next(100, 350);
                                    break;
                                case PlanetType.Balanced:
                                    NewPlanet.PopulationModifier = 1.2m;
                                    NewPlanet.AgricultureModifier = 1m;
                                    NewPlanet.OreModifier = 0.01m;
                                    NewPlanet.ArtifactModifier = 0.05m;
                                    NewPlanet.TotalLand = random.Next(185, 1050);
                                    NewPlanet.AvailableOre = random.Next(750, 1100);
                                    break;
                                case PlanetType.Gas:
                                    NewPlanet.PopulationModifier = 1m;
                                    NewPlanet.AgricultureModifier = 0m;
                                    NewPlanet.OreModifier = 0m;
                                    NewPlanet.ArtifactModifier = 0.05m;
                                    NewPlanet.TotalLand = random.Next(2, 6);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                                case PlanetType.URich:
                                    NewPlanet.PopulationModifier = 0.1m;
                                    NewPlanet.AgricultureModifier = 0m;
                                    NewPlanet.OreModifier = 0.03m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(11, 28);
                                    NewPlanet.AvailableOre = random.Next(50000, 350000);
                                    break;
                                case PlanetType.UEden:
                                    NewPlanet.PopulationModifier = 10m;
                                    NewPlanet.AgricultureModifier = 0.02m;
                                    NewPlanet.OreModifier = 0m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(500, 2500);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                                case PlanetType.USpazial:
                                    NewPlanet.PopulationModifier = 0.1m;
                                    NewPlanet.AgricultureModifier = 0m;
                                    NewPlanet.OreModifier = 0m;
                                    NewPlanet.ArtifactModifier = 0.15m;
                                    NewPlanet.TotalLand = random.Next(2, 4);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                                case PlanetType.ULarge:
                                    NewPlanet.PopulationModifier = 0.2m;
                                    NewPlanet.AgricultureModifier = 0m;
                                    NewPlanet.OreModifier = 0m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(7000, 16000);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                                case PlanetType.UFertile:
                                    NewPlanet.PopulationModifier = 0.5m;
                                    NewPlanet.AgricultureModifier = 1.75m;
                                    NewPlanet.OreModifier = 0m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(950, 2150);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                                case PlanetType.Dead:
                                    NewPlanet.PopulationModifier = 0.05m;
                                    NewPlanet.AgricultureModifier = 0m;
                                    NewPlanet.OreModifier = 0m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(2, 4);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                                case PlanetType.ClusterLevel1:
                                    NewPlanet.PopulationModifier = 1.1m;
                                    NewPlanet.AgricultureModifier = 1.15m;
                                    NewPlanet.OreModifier = 0.02m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(1200, 5001);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                                case PlanetType.ClusterLevel2:
                                    NewPlanet.PopulationModifier = 1.2m;
                                    NewPlanet.AgricultureModifier = 1.3m;
                                    NewPlanet.OreModifier = 0.02m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(12000, 25001);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                                case PlanetType.ClusterLevel3:
                                    NewPlanet.PopulationModifier = 1.3m;
                                    NewPlanet.AgricultureModifier = 1.45m;
                                    NewPlanet.OreModifier = 0.02m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(55000, 125001);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                                default:
                                    NewPlanet.PopulationModifier = 1m;
                                    NewPlanet.AgricultureModifier = 1m;
                                    NewPlanet.OreModifier = 1m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(2, 4);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                            }
                            // Calculate LandAvailable
                            NewPlanet.LandAvailable = NewPlanet.TotalLand - (NewPlanet.Housing + NewPlanet.Commercial + NewPlanet.Industry + NewPlanet.Agriculture + NewPlanet.Mining);
                            // planet 2
                            var NewPlanet2 = new Planets
                            {
                                ApplicationUserId = user.Id,
                                ApplicationUser = user,
                                Name = "E." + random.Next(1000, 9999).ToString(),
                                Type = chosenType,
                                FoodRequired = 1,
                                GoodsRequired = 1,
                                CurrentPopulation = 10,
                                MaxPopulation = 10,
                                Loyalty = 2500,
                                AvailableLabour = 8,
                                Housing = 1,
                                Commercial = 0,
                                Industry = 0,
                                Agriculture = 0,
                                Mining = 1,
                                MineralProduced = 0,
                                PowerRating = 0
                            };
                            // Set type-specific values
                            switch (chosenType)
                            {
                                case PlanetType.Barren:
                                    NewPlanet2.PopulationModifier = 0.5m;
                                    NewPlanet2.AgricultureModifier = 0m;
                                    NewPlanet2.OreModifier = 0.02m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.AvailableOre = random.Next(150, 500);
                                    NewPlanet2.TotalLand = random.Next(50, 1100);
                                    break;
                                case PlanetType.Icy:
                                    NewPlanet2.PopulationModifier = 0.75m;
                                    NewPlanet2.AgricultureModifier = 1m;
                                    NewPlanet2.OreModifier = 0.005m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.AvailableOre = random.Next(1500, 4500);
                                    NewPlanet2.TotalLand = random.Next(24, 83);
                                    break;
                                case PlanetType.Marshy:
                                    NewPlanet2.PopulationModifier = 0.8m;
                                    NewPlanet2.AgricultureModifier = 0.5m;
                                    NewPlanet2.OreModifier = 0.005m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(50, 150);
                                    NewPlanet2.AvailableOre = random.Next(0, 150);
                                    break;
                                case PlanetType.Forest:
                                    NewPlanet2.PopulationModifier = 0.9m;
                                    NewPlanet2.AgricultureModifier = 2m;
                                    NewPlanet2.OreModifier = 0.005m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(50, 350);
                                    NewPlanet2.AvailableOre = random.Next(0, 50);
                                    break;
                                case PlanetType.Oceanic:
                                    NewPlanet2.PopulationModifier = 0.8m;
                                    NewPlanet2.AgricultureModifier = 0m;
                                    NewPlanet2.OreModifier = 0.005m;
                                    NewPlanet2.ArtifactModifier = 0.10m;
                                    NewPlanet2.TotalLand = random.Next(10, 50);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                                case PlanetType.Rocky:
                                    NewPlanet2.PopulationModifier = 0.75m;
                                    NewPlanet2.AgricultureModifier = 1m;
                                    NewPlanet2.OreModifier = 0.001m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(34, 121);
                                    NewPlanet2.AvailableOre = random.Next(500, 3300);
                                    break;
                                case PlanetType.Desert:
                                    NewPlanet2.PopulationModifier = 0.75m;
                                    NewPlanet2.AgricultureModifier = 0.75m;
                                    NewPlanet2.OreModifier = 0.02m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(100, 250);
                                    NewPlanet2.AvailableOre = random.Next(100, 350);
                                    break;
                                case PlanetType.Balanced:
                                    NewPlanet2.PopulationModifier = 1.2m;
                                    NewPlanet2.AgricultureModifier = 1m;
                                    NewPlanet2.OreModifier = 0.01m;
                                    NewPlanet2.ArtifactModifier = 0.05m;
                                    NewPlanet2.TotalLand = random.Next(185, 1050);
                                    NewPlanet2.AvailableOre = random.Next(750, 1100);
                                    break;
                                case PlanetType.Gas:
                                    NewPlanet2.PopulationModifier = 1m;
                                    NewPlanet2.AgricultureModifier = 0m;
                                    NewPlanet2.OreModifier = 0m;
                                    NewPlanet2.ArtifactModifier = 0.05m;
                                    NewPlanet2.TotalLand = random.Next(2, 6);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                                case PlanetType.URich:
                                    NewPlanet2.PopulationModifier = 0.1m;
                                    NewPlanet2.AgricultureModifier = 0m;
                                    NewPlanet2.OreModifier = 0.03m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(11, 28);
                                    NewPlanet2.AvailableOre = random.Next(50000, 350000);
                                    break;
                                case PlanetType.UEden:
                                    NewPlanet2.PopulationModifier = 10m;
                                    NewPlanet2.AgricultureModifier = 0.02m;
                                    NewPlanet2.OreModifier = 0m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(500, 2500);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                                case PlanetType.USpazial:
                                    NewPlanet2.PopulationModifier = 0.1m;
                                    NewPlanet2.AgricultureModifier = 0m;
                                    NewPlanet2.OreModifier = 0m;
                                    NewPlanet2.ArtifactModifier = 0.15m;
                                    NewPlanet2.TotalLand = random.Next(2, 4);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                                case PlanetType.ULarge:
                                    NewPlanet2.PopulationModifier = 0.2m;
                                    NewPlanet2.AgricultureModifier = 0m;
                                    NewPlanet2.OreModifier = 0m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(7000, 16000);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                                case PlanetType.UFertile:
                                    NewPlanet2.PopulationModifier = 0.5m;
                                    NewPlanet2.AgricultureModifier = 1.75m;
                                    NewPlanet2.OreModifier = 0m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(950, 2150);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                                case PlanetType.Dead:
                                    NewPlanet2.PopulationModifier = 0.05m;
                                    NewPlanet2.AgricultureModifier = 0m;
                                    NewPlanet2.OreModifier = 0m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(2, 4);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                                case PlanetType.ClusterLevel1:
                                    NewPlanet2.PopulationModifier = 1.1m;
                                    NewPlanet2.AgricultureModifier = 1.15m;
                                    NewPlanet2.OreModifier = 0.02m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(1200, 5001);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                                case PlanetType.ClusterLevel2:
                                    NewPlanet2.PopulationModifier = 1.2m;
                                    NewPlanet2.AgricultureModifier = 1.3m;
                                    NewPlanet2.OreModifier = 0.02m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(12000, 25001);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                                case PlanetType.ClusterLevel3:
                                    NewPlanet2.PopulationModifier = 1.3m;
                                    NewPlanet2.AgricultureModifier = 1.45m;
                                    NewPlanet2.OreModifier = 0.02m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(55000, 125001);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                                default:
                                    NewPlanet2.PopulationModifier = 1m;
                                    NewPlanet2.AgricultureModifier = 1m;
                                    NewPlanet2.OreModifier = 1m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(2, 4);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                            }
                            // Calculate LandAvailable
                            NewPlanet2.LandAvailable = NewPlanet2.TotalLand - (NewPlanet2.Housing + NewPlanet2.Commercial + NewPlanet2.Industry + NewPlanet2.Agriculture + NewPlanet2.Mining);
                            // planet 3
                            var NewPlanet3 = new Planets
                            {
                                ApplicationUserId = user.Id,
                                ApplicationUser = user,
                                Name = "E." + random.Next(1000, 9999).ToString(),
                                Type = chosenType,
                                FoodRequired = 1,
                                GoodsRequired = 1,
                                CurrentPopulation = 10,
                                MaxPopulation = 10,
                                Loyalty = 2500,
                                AvailableLabour = 8,
                                Housing = 1,
                                Commercial = 0,
                                Industry = 0,
                                Agriculture = 0,
                                Mining = 1,
                                MineralProduced = 0,
                                PowerRating = 0
                            };
                            // Set type-specific values
                            switch (chosenType)
                            {
                                case PlanetType.Barren:
                                    NewPlanet3.PopulationModifier = 0.5m;
                                    NewPlanet3.AgricultureModifier = 0m;
                                    NewPlanet3.OreModifier = 0.02m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.AvailableOre = random.Next(150, 500);
                                    NewPlanet3.TotalLand = random.Next(50, 1100);
                                    break;
                                case PlanetType.Icy:
                                    NewPlanet3.PopulationModifier = 0.75m;
                                    NewPlanet3.AgricultureModifier = 1m;
                                    NewPlanet3.OreModifier = 0.005m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.AvailableOre = random.Next(1500, 4500);
                                    NewPlanet3.TotalLand = random.Next(24, 83);
                                    break;
                                case PlanetType.Marshy:
                                    NewPlanet3.PopulationModifier = 0.8m;
                                    NewPlanet3.AgricultureModifier = 0.5m;
                                    NewPlanet3.OreModifier = 0.005m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(50, 150);
                                    NewPlanet3.AvailableOre = random.Next(0, 150);
                                    break;
                                case PlanetType.Forest:
                                    NewPlanet3.PopulationModifier = 0.9m;
                                    NewPlanet3.AgricultureModifier = 2m;
                                    NewPlanet3.OreModifier = 0.005m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(50, 350);
                                    NewPlanet3.AvailableOre = random.Next(0, 50);
                                    break;
                                case PlanetType.Oceanic:
                                    NewPlanet3.PopulationModifier = 0.8m;
                                    NewPlanet3.AgricultureModifier = 0m;
                                    NewPlanet3.OreModifier = 0.005m;
                                    NewPlanet3.ArtifactModifier = 0.10m;
                                    NewPlanet3.TotalLand = random.Next(10, 50);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                                case PlanetType.Rocky:
                                    NewPlanet3.PopulationModifier = 0.75m;
                                    NewPlanet3.AgricultureModifier = 1m;
                                    NewPlanet3.OreModifier = 0.001m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(34, 121);
                                    NewPlanet3.AvailableOre = random.Next(500, 3300);
                                    break;
                                case PlanetType.Desert:
                                    NewPlanet3.PopulationModifier = 0.75m;
                                    NewPlanet3.AgricultureModifier = 0.75m;
                                    NewPlanet3.OreModifier = 0.02m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(100, 250);
                                    NewPlanet3.AvailableOre = random.Next(100, 350);
                                    break;
                                case PlanetType.Balanced:
                                    NewPlanet3.PopulationModifier = 1.2m;
                                    NewPlanet3.AgricultureModifier = 1m;
                                    NewPlanet3.OreModifier = 0.01m;
                                    NewPlanet3.ArtifactModifier = 0.05m;
                                    NewPlanet3.TotalLand = random.Next(185, 1050);
                                    NewPlanet3.AvailableOre = random.Next(750, 1100);
                                    break;
                                case PlanetType.Gas:
                                    NewPlanet3.PopulationModifier = 1m;
                                    NewPlanet3.AgricultureModifier = 0m;
                                    NewPlanet3.OreModifier = 0m;
                                    NewPlanet3.ArtifactModifier = 0.05m;
                                    NewPlanet3.TotalLand = random.Next(2, 6);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                                case PlanetType.URich:
                                    NewPlanet3.PopulationModifier = 0.1m;
                                    NewPlanet3.AgricultureModifier = 0m;
                                    NewPlanet3.OreModifier = 0.03m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(11, 28);
                                    NewPlanet3.AvailableOre = random.Next(50000, 350000);
                                    break;
                                case PlanetType.UEden:
                                    NewPlanet3.PopulationModifier = 10m;
                                    NewPlanet3.AgricultureModifier = 0.02m;
                                    NewPlanet3.OreModifier = 0m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(500, 2500);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                                case PlanetType.USpazial:
                                    NewPlanet3.PopulationModifier = 0.1m;
                                    NewPlanet3.AgricultureModifier = 0m;
                                    NewPlanet3.OreModifier = 0m;
                                    NewPlanet3.ArtifactModifier = 0.15m;
                                    NewPlanet3.TotalLand = random.Next(2, 4);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                                case PlanetType.ULarge:
                                    NewPlanet3.PopulationModifier = 0.2m;
                                    NewPlanet3.AgricultureModifier = 0m;
                                    NewPlanet3.OreModifier = 0m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(7000, 16000);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                                case PlanetType.UFertile:
                                    NewPlanet3.PopulationModifier = 0.5m;
                                    NewPlanet3.AgricultureModifier = 1.75m;
                                    NewPlanet3.OreModifier = 0m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(950, 2150);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                                case PlanetType.Dead:
                                    NewPlanet3.PopulationModifier = 0.05m;
                                    NewPlanet3.AgricultureModifier = 0m;
                                    NewPlanet3.OreModifier = 0m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(2, 4);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                                case PlanetType.ClusterLevel1:
                                    NewPlanet3.PopulationModifier = 1.1m;
                                    NewPlanet3.AgricultureModifier = 1.15m;
                                    NewPlanet3.OreModifier = 0.02m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(1200, 5001);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                                case PlanetType.ClusterLevel2:
                                    NewPlanet3.PopulationModifier = 1.2m;
                                    NewPlanet3.AgricultureModifier = 1.3m;
                                    NewPlanet3.OreModifier = 0.02m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(12000, 25001);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                                case PlanetType.ClusterLevel3:
                                    NewPlanet3.PopulationModifier = 1.3m;
                                    NewPlanet3.AgricultureModifier = 1.45m;
                                    NewPlanet3.OreModifier = 0.02m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(55000, 125001);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                                default:
                                    NewPlanet3.PopulationModifier = 1m;
                                    NewPlanet3.AgricultureModifier = 1m;
                                    NewPlanet3.OreModifier = 1m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(2, 4);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                            }
                            // Calculate LandAvailable
                            NewPlanet3.LandAvailable = NewPlanet3.TotalLand - (NewPlanet3.Housing + NewPlanet3.Commercial + NewPlanet3.Industry + NewPlanet3.Agriculture + NewPlanet3.Mining);
                            // Add planets to list
                            DefenderPlanets.Add(NewPlanet);
                            DefenderPlanets.Add(NewPlanet2);
                            DefenderPlanets.Add(NewPlanet3);
                        }
                        else
                        {
                            DefenderPlanets = _context.Planets.Where(p => p.ApplicationUserId == TargetUserFleets.FirstOrDefault().ApplicationUserId && !p.Name.Contains("H.")).OrderBy(x => x.DateTimeAcquired).ToList();
                        }
                        if (DefenderPlanets.Count > 0)
                        {
                            if (DefenderPlanets.Count == 1)
                            {
                                DefenderPlanets[0].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                                DefenderPlanets[0].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                                DefenderPlanets[0].DateTimeAcquired = DateTime.UtcNow;
                                currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                                _context.Planets.Update(DefenderPlanets[0]);
                            }
                            else if (DefenderPlanets.Count == 2)
                            {
                                DefenderPlanets[0].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                                DefenderPlanets[0].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                                DefenderPlanets[0].DateTimeAcquired = DateTime.UtcNow;
                                currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                                DefenderPlanets[1].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                                DefenderPlanets[1].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                                DefenderPlanets[1].DateTimeAcquired = DateTime.UtcNow;
                                currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                                _context.Planets.Update(DefenderPlanets[0]);
                                _context.Planets.Update(DefenderPlanets[1]);
                            }
                            else if (DefenderPlanets.Count > 2)
                            {
                                DefenderPlanets[0].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                                DefenderPlanets[0].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                                DefenderPlanets[0].DateTimeAcquired = DateTime.UtcNow;
                                currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                                DefenderPlanets[1].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                                DefenderPlanets[1].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                                DefenderPlanets[1].DateTimeAcquired = DateTime.UtcNow;
                                currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                                DefenderPlanets[2].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                                DefenderPlanets[2].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                                DefenderPlanets[2].DateTimeAcquired = DateTime.UtcNow;
                                currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                                _context.Planets.Update(DefenderPlanets[0]);
                                _context.Planets.Update(DefenderPlanets[1]);
                                _context.Planets.Update(DefenderPlanets[2]);
                            }
                        }
                        // Fleet Report + Important Events for attacker and defender
                        EndFleets = "<h3>Attacker's Fleet</h3>";
                        EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                        foreach (var ship in AttackerFleet)
                        {
                            int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                            EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                        }
                        EndFleets += "</table>";

                        EndFleets += "<h3>Defender's Fleet</h3>";
                        EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                        foreach (var ship in DefenderFleet)
                        {
                            int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                            EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                        }
                        EndFleets += "</table>";
                        if (DefenderPlanets.Count > 0)
                        {
                            EndFleets += "<h3>Planets Won</h3>";
                            EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Type</th><th>Land</th></tr>";
                            foreach (var planet in DefenderPlanets)
                            {
                                EndFleets += $"<tr><td>{planet.Name}</td><td>{planet.Type}</td><td>{planet.TotalLand}</td></tr>";
                            }
                            EndFleets += "</table>";
                        }
                        else
                        {
                            EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Type</th><th>Land</th></tr>";
                            EndFleets += "<tr><td colspan='3'>Defender has no planets</td></tr>"; ;
                            EndFleets += "/table>";
                        }
                        ImportantEvents attackerEvent = new ImportantEvents
                        {
                            ApplicationUserId = currentUser.Id,
                            ImportantEventTypes = ImportantEventTypes.Battles,
                            Text = $"You have won the battle against {TargetUserName}.<br />{EndFleets}<br />",
                            DateAndTime = DateTime.Now
                        };
                        _context.ImportantEvents.Add(attackerEvent);
                        if (targetUser.IsNPC != true)
                        {
                            ImportantEvents DefenderEvent = new ImportantEvents
                            {
                                ApplicationUserId = targetUser.Id,
                                ImportantEventTypes = ImportantEventTypes.Battles,
                                Text = $"You have lost the battle against {currentUser.UserName}.<br />{EndFleets}<br />",
                                DateAndTime = DateTime.Now
                            };
                            _context.ImportantEvents.Add(DefenderEvent);
                        }
                        // battle logs
                        BattleLogs AttackerBattleLog = new BattleLogs
                        {
                            ApplicationUserId = currentUser.Id,
                            Attacker = currentUser.UserName,
                            Defender = targetUser.UserName,
                            DateAndTime = DateTime.Now,
                            Outcome = "Win",
                            FleetReport = EndFleets
                        };
                        _context.Battlelogs.Add(AttackerBattleLog);
                        if (targetUser.IsNPC != true)
                        {
                            BattleLogs DefenderBattleLog = new BattleLogs
                            {
                                ApplicationUserId = targetUser.Id,
                                Attacker = currentUser.UserName,
                                Defender = targetUser.UserName,
                                DateAndTime = DateTime.Now,
                                Outcome = $"Loss",
                                FleetReport = EndFleets
                            };
                            _context.Battlelogs.Add(DefenderBattleLog);
                        }
                        // update database for attacker and defenders fleets
                        foreach (var fleet in AttackerFleet)
                        {
                            var existingFleet = _context.Fleets.FirstOrDefault(f =>f.ApplicationUserId == attacker.ApplicationUserId && f.ShipId == attacker.ShipId);
                            existingFleet.TotalShips = fleet.TotalShips;
                            existingFleet.TotalPowerRating = fleet.PowerRating * fleet.TotalShips;
                            existingFleet.TotalUpkeep = fleet.Upkeep * fleet.TotalShips;
                            if (existingFleet.TotalShips <= 0)
                            {
                                _context.Fleets.Remove(existingFleet); // Remove fleet if no ships left
                            }
                            else
                            {
                                _context.Fleets.Update(existingFleet);
                            }
                        }
                        if (targetUser.IsNPC != true)
                        {
                            foreach (var fleet in DefenderFleet)
                            {
                                var existingFleet = _context.Fleets.FirstOrDefault(f => f.ApplicationUserId == defender.ApplicationUserId && f.ShipId == defender.ShipId);
                                existingFleet.TotalShips = fleet.TotalShips;
                                existingFleet.TotalPowerRating = fleet.PowerRating * fleet.TotalShips;
                                existingFleet.TotalUpkeep = fleet.Upkeep * fleet.TotalShips;
                                _context.Fleets.Update(existingFleet);
                                if (existingFleet.TotalShips <= 0)
                                {
                                    _context.Fleets.Remove(existingFleet); // Remove fleet if no ships left
                                }
                                else
                                {
                                    _context.Fleets.Update(existingFleet);
                                }
                            }
                        }
                        // Update Attackers Stats
                        currentUser.BattlesWon++;
                        // Update Defenders Stats + Damage Protection
                        targetUser.BattlesLost++;
                        if (DefenderPlanets.Count > 0)
                        {
                            if (DefenderPlanets.Count == 1)
                            {

                                targetUser.TotalPlanets -= DefenderPlanets[0].TotalPlanets;
                                targetUser.TotalColonies -= 1;
                                targetUser.ColoniesLost += 1;
                                currentUser.ColoniesWon += 1;
                                currentUser.TotalColonies += 1;
                                currentUser.TotalPlanets += DefenderPlanets[0].TotalPlanets;

                            }
                            else if (DefenderPlanets.Count == 2)
                            {

                                targetUser.TotalPlanets -= DefenderPlanets[0].TotalPlanets + DefenderPlanets[1].TotalPlanets;
                                targetUser.TotalColonies -= 2;
                                targetUser.ColoniesLost += 2;
                                currentUser.ColoniesWon += 2;
                                currentUser.TotalColonies += 2;
                                currentUser.TotalPlanets += DefenderPlanets[0].TotalPlanets + DefenderPlanets[1].TotalPlanets;
                            }
                            else if (DefenderPlanets.Count > 2)
                            {

                                targetUser.TotalPlanets -= DefenderPlanets[0].TotalPlanets + DefenderPlanets[1].TotalPlanets + DefenderPlanets[2].TotalPlanets;
                                targetUser.TotalColonies -= 3;
                                targetUser.ColoniesLost += 3;
                                currentUser.ColoniesWon += 3;
                                currentUser.TotalColonies += 3;
                                currentUser.TotalPlanets += DefenderPlanets[0].TotalPlanets + DefenderPlanets[1].TotalPlanets + DefenderPlanets[2].TotalPlanets;
                            }
                        }
                        if (targetUser.IsNPC == true)
                        {
                            targetUser.TotalPlanets = 1; // NPCs should always have at least one planet
                            targetUser.TotalColonies = 1; // NPCs should always have at least one colony
                            targetUser.DamageProtection = DateTime.Now.AddMinutes(15); // Reset damage protection for defender
                        }
                        else
                        {
                            targetUser.DamageProtection = DateTime.Now.AddDays(1); // Reset damage protection for defender
                        }
                        var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, 5);
                        StatusMessage = turnsMessage.Message;
                        VictoryMessage = $"You have won the battle against {TargetUserName}";
                        _context.SaveChanges();
                        return Page();
                    }
                    // attacker no fleet
                    else if (AttackerFleet.All(f => f.TotalShips <= 0))
                    {
                        // Attacker has no fleet left
                        // Defender wins by default
                        // Fleet Report + Important Events for attacker and defender
                        EndFleets = $"<h3>{currentUser.UserName}'s Fleet</h3>";
                        EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                        foreach (var ship in AttackerFleet)
                        {
                            int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                            EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                        }
                        EndFleets += "</table>";

                        EndFleets += $"<h3>{targetUser.UserName}'s Fleet</h3>";
                        EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                        foreach (var ship in DefenderFleet)
                        {
                            int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                            EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                        }
                        EndFleets += "</table>";
                        ImportantEvents attackerEvent = new ImportantEvents
                        {
                            ApplicationUserId = currentUser.Id,
                            ImportantEventTypes = ImportantEventTypes.Battles,
                            Text = $"You have Lost the battle against {TargetUserName}.<br />{EndFleets}<br />",
                            DateAndTime = DateTime.Now
                        };
                        _context.ImportantEvents.Add(attackerEvent);
                        if (targetUser.IsNPC != true)
                        {
                            ImportantEvents DefenderEvent = new ImportantEvents
                            {
                                ApplicationUserId = targetUser.Id,
                                ImportantEventTypes = ImportantEventTypes.Battles,
                                Text = $"You have Won the battle against {user.UserName}.<br />{EndFleets}<br />",
                                DateAndTime = DateTime.Now
                            };
                            _context.ImportantEvents.Add(DefenderEvent);
                        }
                        // battle logs
                        BattleLogs AttackerBattleLog = new BattleLogs
                        {
                            ApplicationUserId = currentUser.Id,
                            Attacker = currentUser.UserName,
                            Defender = targetUser.UserName,
                            DateAndTime = DateTime.Now,
                            Outcome = $"Loss",
                            FleetReport = EndFleets
                        };
                        _context.Battlelogs.Add(AttackerBattleLog);
                        if (targetUser.IsNPC != true)
                        {
                            BattleLogs DefenderBattleLog = new BattleLogs
                            {
                                ApplicationUserId = targetUser.Id,
                                Attacker = currentUser.UserName,
                                Defender = targetUser.UserName,
                                DateAndTime = DateTime.Now,
                                Outcome = $"Win",
                                FleetReport = EndFleets
                            };
                            _context.Battlelogs.Add(DefenderBattleLog);
                        }
                        // update database for attacker and defenders fleets
                        foreach (var fleet in AttackerFleet)
                        {
                            var existingFleet = _context.Fleets.FirstOrDefault(f => f.ApplicationUserId == attacker.ApplicationUserId && f.ShipId == attacker.ShipId);
                            existingFleet.TotalShips = fleet.TotalShips;
                            existingFleet.TotalPowerRating = fleet.PowerRating * fleet.TotalShips;
                            existingFleet.TotalUpkeep = fleet.Upkeep * fleet.TotalShips;
                            if (existingFleet.TotalShips <= 0)
                            {
                                _context.Fleets.Remove(existingFleet); // Remove fleet if no ships left
                            }
                            else
                            {
                                _context.Fleets.Update(existingFleet);
                            }
                        }
                        if (targetUser.IsNPC != true)
                        {
                            foreach (var fleet in DefenderFleet)
                            {
                            var existingFleet = _context.Fleets.FirstOrDefault(f => f.ApplicationUserId == defender.ApplicationUserId && f.ShipId == defender.ShipId);
                            existingFleet.TotalShips = fleet.TotalShips;
                            existingFleet.TotalPowerRating = fleet.PowerRating * fleet.TotalShips;
                            existingFleet.TotalUpkeep = fleet.Upkeep * fleet.TotalShips;
                            _context.Fleets.Update(existingFleet);
                            if (existingFleet.TotalShips <= 0)
                            {
                                _context.Fleets.Remove(existingFleet); // Remove fleet if no ships left
                            }
                            else
                            {
                                _context.Fleets.Update(existingFleet);
                            }
                            }
                        }
                        // Update Attackers Stats
                        user.BattlesLost += 1;
                        // Update Defenders Stats
                        targetUser.BattlesWon++;
                        if (targetUser.IsNPC == true)
                        {
                            targetUser.TotalPlanets = 1; // NPCs should always have at least one planet
                            targetUser.TotalColonies = 1; // NPCs should always have at least one colony
                            targetUser.DamageProtection = DateTime.Now.AddMinutes(15); // Reset damage protection for defender
                        }
                        else
                        {
                            targetUser.DamageProtection = DateTime.Now.AddDays(1); // Reset damage protection for defender
                        }
                        var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, 5);
                        StatusMessage = turnsMessage.Message;
                        VictoryMessage = $"You have lost the battle against {TargetUserName}";
                        _context.SaveChanges();
                        return Page();
                    }
                }
                

                // Wave 2 
                // Reorganize Fleets based on power rating
                AttackerFleet = AttackerFleet.OrderByDescending(f => f.PowerRating).ToList();
                DefenderFleet = DefenderFleet.OrderByDescending(f => f.PowerRating).ToList();
                // Set Stacks for Wave 2
                stacks = Math.Max(AttackerFleet.Count, DefenderFleet.Count);
                for (int i = 0; i < stacks; i++)
                {
                    MergedFleet attacker;
                    MergedFleet defender;

                    // Attacker selection
                    if (i < AttackerFleet.Count)
                    {
                        attacker = AttackerFleet[i];
                        if (attacker.TotalShips <= 0)
                        {
                            // Pick a random fleet with TotalShips > 0, or null if none
                            var available = AttackerFleet.Where(f => f.TotalShips > 0).ToList();
                            attacker = available.Any() ? available[Random.Shared.Next(available.Count)] : null;
                        }
                    }
                    else
                    {
                        var available = AttackerFleet.Where(f => f.TotalShips > 0).ToList();
                        attacker = available.Any() ? available[Random.Shared.Next(available.Count)] : null;
                    }

                    // Defender selection
                    if (i < DefenderFleet.Count)
                    {
                        defender = DefenderFleet[i];
                        if (defender.TotalShips <= 0)
                        {
                            var available = DefenderFleet.Where(f => f.TotalShips > 0).ToList();
                            defender = available.Any() ? available[Random.Shared.Next(available.Count)] : null;
                        }
                    }
                    else
                    {
                        var available = DefenderFleet.Where(f => f.TotalShips > 0).ToList();
                        defender = available.Any() ? available[Random.Shared.Next(available.Count)] : null;
                    }
                    // Defender Attacks First
                    if (defender.Range >= attacker.Range)
                    {
                        
                        // Defender Attacks First
                        // Defender Attacks
                        DamageAttackersStackTakes = CalulateDefenderDamage(attacker, defender, false).DamageDealt;
                        HowManyAttackersShipsKilled = CalulateDefenderDamage(attacker, defender, false).ShipsKilled;
                        //// Attacker Retals (if Defender does not have NoRetal && Attacker does not have NoDefense)
                        if (defender.NoRetal == false && attacker.NoDefense == false)
                        {
                            DamageDefendersStackTakes = CalulateAttackerDamage(attacker, defender, true).DamageDealt;
                            HowManyDefendersShipsKilled = CalulateAttackerDamage(attacker, defender, true).ShipsKilled;
                        }
                        else
                        {
                            DamageDefendersStackTakes = 0;
                            HowManyDefendersShipsKilled = 0;
                        }
                        // add battle result
                        BattleResultsWave2.Add(new BattleResultWave2
                        {
                            IsAttacker = false,
                            AttackerShipName = attacker.ShipName,
                            DefenderShipName = defender.ShipName,
                            AttackerDamageDelt = DamageDefendersStackTakes,
                            DefenderDamageDelt = DamageAttackersStackTakes,
                            AttackerShipTotalLoss = HowManyAttackersShipsKilled,
                            DefenderShipTotalLoss = HowManyDefendersShipsKilled
                        });
                        // Update Stacks
                        attacker.TotalShips -= HowManyAttackersShipsKilled;
                        defender.TotalShips -= HowManyDefendersShipsKilled;
                        // Attacker Attacks
                        DamageDefendersStackTakes = CalulateAttackerDamage(attacker, defender, false).DamageDealt;
                        HowManyDefendersShipsKilled = CalulateAttackerDamage(attacker, defender, false).ShipsKilled;
                        //// Defender Retals (if Attacker does not have NoRetal && Defender does not have NoDefense)
                        if (attacker.NoRetal == false && defender.NoDefense == false)
                        {
                            DamageAttackersStackTakes = CalulateDefenderDamage(attacker, defender, true).DamageDealt;
                            HowManyAttackersShipsKilled = CalulateDefenderDamage(attacker, defender, true).ShipsKilled;
                        }
                        else
                        {
                            DamageAttackersStackTakes = 0;
                            HowManyAttackersShipsKilled = 0;
                        }
                        // capture chance
                        if (defender.CanCapture == true && HowManyAttackersShipsKilled > 0 && attacker.ImmuneToCapture == false && attacker.CanCapture == false && targetUser.IsNPC != true)
                        {
                            // Capture Ships
                            bool willCapture = false;
                            var captureChance = (int)Math.Floor(defender.CapChance * 100);
                            if (captureChance == 100)
                            {
                                willCapture = true;
                            }
                            else
                            {
                                var randomValue = Random.Shared.Next(0, 100);
                                if (randomValue <= captureChance)
                                {
                                    willCapture = true;
                                }
                                else
                                {
                                    willCapture = false;
                                }
                            }
                            if (willCapture == true)
                            {
                                // Capture Ships
                                var totalCapturedShips = HowManyAttackersShipsKilled;
                                var ShipName = attacker.ShipName;
                                var ShipId = attacker.ShipId;
                                // add defender new fleet
                                var newDefenderFleet = new Fleet
                                {
                                    ApplicationUserId = defender.ApplicationUserId,
                                    ApplicationUser = defender.ApplicationUser,
                                    ShipId = ShipId,
                                    TotalShips = totalCapturedShips,
                                    TotalPowerRating = totalCapturedShips * Ships.FirstOrDefault(s => s.Id == ShipId).PowerRating,
                                    TotalUpkeep = totalCapturedShips * Ships.FirstOrDefault(s => s.Id == ShipId).Upkeep
                                };
                                _context.Fleets.Add(newDefenderFleet);
                                // Add to Battle Results
                                BattleResultsWave1.Add(new BattleResultWave1
                                {
                                    DefenderCapture = $"{defender.UserName} captured {totalCapturedShips} {ShipName} ships from {attacker.UserName}."
                                });
                            }
                        }
                        // add battle result
                        BattleResultsWave2.Add(new BattleResultWave2
                        {
                            IsAttacker = true,
                            AttackerShipName = attacker.ShipName,
                            DefenderShipName = defender.ShipName,
                            AttackerDamageDelt = DamageDefendersStackTakes,
                            DefenderDamageDelt = DamageAttackersStackTakes,
                            AttackerShipTotalLoss = HowManyAttackersShipsKilled,
                            DefenderShipTotalLoss = HowManyDefendersShipsKilled
                        });
                        // Update Stacks
                        attacker.TotalShips -= HowManyAttackersShipsKilled;
                        defender.TotalShips -= HowManyDefendersShipsKilled;
                        // add power rating losses
                        AttackerTotalPowerRatingLoss += HowManyAttackersShipsKilled * attacker.PowerRating;
                        DefenderTotalPowerRatingLoss += HowManyDefendersShipsKilled * defender.PowerRating;
                    }
                    // Attacker Attacks First
                    else
                    {
                        // Check if Defender has no fleet in this stack
                        if (defender == null)
                        {
                            var max = DefenderFleet.Count + 1;
                            defender = DefenderFleet[Random.Shared.Next(0, max)];
                        }
                        // Attacker Attacks First
                        // Attacker Attacks
                        DamageDefendersStackTakes = CalulateAttackerDamage(attacker, defender, false).DamageDealt;
                        HowManyDefendersShipsKilled = CalulateAttackerDamage(attacker, defender, false).ShipsKilled;
                        //// Defender Retals (if Attacker does not have NoRetal && Defender does not have NoDefense)
                        if (attacker.NoRetal == false && defender.NoDefense == false)
                        {
                            DamageAttackersStackTakes = CalulateDefenderDamage(attacker, defender, true).DamageDealt;
                            HowManyAttackersShipsKilled = CalulateDefenderDamage(attacker, defender, true).ShipsKilled;
                        }
                        else
                        {
                            DamageAttackersStackTakes = 0;
                            HowManyAttackersShipsKilled = 0;
                        }
                        // add battle result
                        BattleResultsWave2.Add(new BattleResultWave2
                        {
                            IsAttacker = true,
                            AttackerShipName = attacker.ShipName,
                            DefenderShipName = defender.ShipName,
                            AttackerDamageDelt = DamageDefendersStackTakes,
                            DefenderDamageDelt = DamageAttackersStackTakes,
                            AttackerShipTotalLoss = HowManyAttackersShipsKilled,
                            DefenderShipTotalLoss = HowManyDefendersShipsKilled
                        });
                        // Update Stacks
                        attacker.TotalShips -= HowManyAttackersShipsKilled;
                        defender.TotalShips -= HowManyDefendersShipsKilled;
                        // Defender Attacks
                        DamageAttackersStackTakes = CalulateDefenderDamage(attacker, defender, false).DamageDealt;
                        HowManyAttackersShipsKilled = CalulateDefenderDamage(attacker, defender, false).ShipsKilled;
                        //// Attacker Retals (if Defender does not have NoRetal && Attacker does not have NoDefense)
                        if (defender.NoRetal == false && attacker.NoDefense == false)
                        {
                            DamageDefendersStackTakes = CalulateAttackerDamage(attacker, defender, true).DamageDealt;
                            HowManyDefendersShipsKilled = CalulateAttackerDamage(attacker, defender, true).ShipsKilled;
                        }
                        else
                        {
                            DamageDefendersStackTakes = 0;
                            HowManyDefendersShipsKilled = 0;
                        }
                        // capture chance
                        if (attacker.CanCapture == true && HowManyDefendersShipsKilled > 0 && defender.ImmuneToCapture == false && defender.CanCapture == false)
                        {
                            // Capture Ships
                            bool willCapture = false;
                            var captureChance = (int)Math.Floor(attacker.CapChance * 100);
                            if (captureChance == 100)
                            {
                                willCapture = true;
                            }
                            else
                            {
                                var randomValue = Random.Shared.Next(0, 100);
                                if (randomValue <= captureChance)
                                {
                                    willCapture = true;
                                }
                                else
                                {
                                    willCapture = false;
                                }
                            }
                            if (willCapture == true)
                            {
                                // Capture Ships
                                var totalCapturedShips = HowManyDefendersShipsKilled;
                                var ShipName = defender.ShipName;
                                var ShipId = defender.ShipId;
                                // add defender new fleet
                                var newAttackerFleet = new Fleet
                                {
                                    ApplicationUserId = attacker.ApplicationUserId,
                                    ApplicationUser = attacker.ApplicationUser,
                                    ShipId = ShipId,
                                    TotalShips = totalCapturedShips,
                                    TotalPowerRating = totalCapturedShips * Ships.FirstOrDefault(s => s.Id == ShipId).PowerRating,
                                    TotalUpkeep = totalCapturedShips * Ships.FirstOrDefault(s => s.Id == ShipId).Upkeep
                                };
                                _context.Fleets.Add(newAttackerFleet);
                                // Add to Battle Results
                                BattleResultsWave1.Add(new BattleResultWave1
                                {
                                    AttackerCapture = $"{attacker.UserName} captured {totalCapturedShips} {ShipName} ships from {defender.UserName}."
                                });
                            }
                        }
                        // add battle result
                        BattleResultsWave2.Add(new BattleResultWave2
                        {
                            IsAttacker = false,
                            AttackerShipName = attacker.ShipName,
                            DefenderShipName = defender.ShipName,
                            AttackerDamageDelt = DamageAttackersStackTakes,
                            DefenderDamageDelt = DamageDefendersStackTakes,
                            AttackerShipTotalLoss = HowManyAttackersShipsKilled,
                            DefenderShipTotalLoss = HowManyDefendersShipsKilled
                        });
                        // Update Stacks
                        attacker.TotalShips -= HowManyAttackersShipsKilled;
                        defender.TotalShips -= HowManyDefendersShipsKilled;
                        // add power rating losses
                        AttackerTotalPowerRatingLoss += HowManyAttackersShipsKilled * attacker.PowerRating;
                        DefenderTotalPowerRatingLoss += HowManyDefendersShipsKilled * defender.PowerRating;
                    }
                    // Alter Fleets based on damage dealt and taken
                    foreach (var item in AttackerFleet)
                    {
                        item.TotalPowerRating = item.TotalShips * item.PowerRating;
                    }
                    AttackerFleet.OrderByDescending(p => p.TotalPowerRating).ToList();
                    foreach (var item in DefenderFleet)
                    {
                        item.TotalPowerRating = item.TotalShips * item.PowerRating;
                    }
                    DefenderFleet.OrderByDescending(p => p.TotalPowerRating).ToList();

                    // Wave 2 End
                    // check if defender has any fleets left
                    // defender no fleet
                    if (DefenderFleet.All(f => f.TotalShips <= 0))
                    {
                        // Defender has no fleet left
                        // Attacker wins by default
                        // Sort Planets For Capture
                        List<Planets> DefenderPlanets = new List<Planets>();
                        if (targetUser.IsNPC == true)
                        {
                            var planetTypeWeights = new List<(PlanetType Type, double Weight)>
                            {
                               (PlanetType.Barren, 5),
                               (PlanetType.Icy, 5),
                               (PlanetType.Marshy, 5),
                               (PlanetType.Forest, 5),
                               (PlanetType.Oceanic, 5),
                               (PlanetType.Rocky, 5),
                               (PlanetType.Desert, 5),
                               (PlanetType.Balanced, 5),
                               (PlanetType.Gas, 5),
                               (PlanetType.URich, 0.5),
                               (PlanetType.UEden, 0.5),
                               (PlanetType.USpazial, 0.5),
                               (PlanetType.ULarge, 0.5),
                               (PlanetType.UFertile, 0.5),
                               (PlanetType.Dead, 0.5),
                               (PlanetType.ClusterLevel1, 0.25),
                               (PlanetType.ClusterLevel2, 0.10),
                               (PlanetType.ClusterLevel3, 0.05)
                            };
                            var random = new System.Random();
                            var chosenType = GetRandomPlanetTypeWeighted(planetTypeWeights);
                            //planet 1
                            var NewPlanet = new Planets
                            {
                                ApplicationUserId = user.Id,
                                ApplicationUser = user,
                                Name = "E." + random.Next(1000, 9999).ToString(),
                                Type = chosenType,
                                FoodRequired = 1,
                                GoodsRequired = 1,
                                CurrentPopulation = 10,
                                MaxPopulation = 10,
                                Loyalty = 2500,
                                AvailableLabour = 8,
                                Housing = 1,
                                Commercial = 0,
                                Industry = 0,
                                Agriculture = 0,
                                Mining = 1,
                                MineralProduced = 0,
                                PowerRating = 0

                            };
                            // Set type-specific values
                            switch (chosenType)
                            {
                                case PlanetType.Barren:
                                    NewPlanet.PopulationModifier = 0.5m;
                                    NewPlanet.AgricultureModifier = 0m;
                                    NewPlanet.OreModifier = 0.02m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.AvailableOre = random.Next(150, 500);
                                    NewPlanet.TotalLand = random.Next(50, 1100);
                                    break;
                                case PlanetType.Icy:
                                    NewPlanet.PopulationModifier = 0.75m;
                                    NewPlanet.AgricultureModifier = 1m;
                                    NewPlanet.OreModifier = 0.005m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.AvailableOre = random.Next(1500, 4500);
                                    NewPlanet.TotalLand = random.Next(24, 83);
                                    break;
                                case PlanetType.Marshy:
                                    NewPlanet.PopulationModifier = 0.8m;
                                    NewPlanet.AgricultureModifier = 0.5m;
                                    NewPlanet.OreModifier = 0.005m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(50, 150);
                                    NewPlanet.AvailableOre = random.Next(0, 150);
                                    break;
                                case PlanetType.Forest:
                                    NewPlanet.PopulationModifier = 0.9m;
                                    NewPlanet.AgricultureModifier = 2m;
                                    NewPlanet.OreModifier = 0.005m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(50, 350);
                                    NewPlanet.AvailableOre = random.Next(0, 50);
                                    break;
                                case PlanetType.Oceanic:
                                    NewPlanet.PopulationModifier = 0.8m;
                                    NewPlanet.AgricultureModifier = 0m;
                                    NewPlanet.OreModifier = 0.005m;
                                    NewPlanet.ArtifactModifier = 0.10m;
                                    NewPlanet.TotalLand = random.Next(10, 50);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                                case PlanetType.Rocky:
                                    NewPlanet.PopulationModifier = 0.75m;
                                    NewPlanet.AgricultureModifier = 1m;
                                    NewPlanet.OreModifier = 0.001m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(34, 121);
                                    NewPlanet.AvailableOre = random.Next(500, 3300);
                                    break;
                                case PlanetType.Desert:
                                    NewPlanet.PopulationModifier = 0.75m;
                                    NewPlanet.AgricultureModifier = 0.75m;
                                    NewPlanet.OreModifier = 0.02m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(100, 250);
                                    NewPlanet.AvailableOre = random.Next(100, 350);
                                    break;
                                case PlanetType.Balanced:
                                    NewPlanet.PopulationModifier = 1.2m;
                                    NewPlanet.AgricultureModifier = 1m;
                                    NewPlanet.OreModifier = 0.01m;
                                    NewPlanet.ArtifactModifier = 0.05m;
                                    NewPlanet.TotalLand = random.Next(185, 1050);
                                    NewPlanet.AvailableOre = random.Next(750, 1100);
                                    break;
                                case PlanetType.Gas:
                                    NewPlanet.PopulationModifier = 1m;
                                    NewPlanet.AgricultureModifier = 0m;
                                    NewPlanet.OreModifier = 0m;
                                    NewPlanet.ArtifactModifier = 0.05m;
                                    NewPlanet.TotalLand = random.Next(2, 6);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                                case PlanetType.URich:
                                    NewPlanet.PopulationModifier = 0.1m;
                                    NewPlanet.AgricultureModifier = 0m;
                                    NewPlanet.OreModifier = 0.03m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(11, 28);
                                    NewPlanet.AvailableOre = random.Next(50000, 350000);
                                    break;
                                case PlanetType.UEden:
                                    NewPlanet.PopulationModifier = 10m;
                                    NewPlanet.AgricultureModifier = 0.02m;
                                    NewPlanet.OreModifier = 0m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(500, 2500);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                                case PlanetType.USpazial:
                                    NewPlanet.PopulationModifier = 0.1m;
                                    NewPlanet.AgricultureModifier = 0m;
                                    NewPlanet.OreModifier = 0m;
                                    NewPlanet.ArtifactModifier = 0.15m;
                                    NewPlanet.TotalLand = random.Next(2, 4);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                                case PlanetType.ULarge:
                                    NewPlanet.PopulationModifier = 0.2m;
                                    NewPlanet.AgricultureModifier = 0m;
                                    NewPlanet.OreModifier = 0m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(7000, 16000);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                                case PlanetType.UFertile:
                                    NewPlanet.PopulationModifier = 0.5m;
                                    NewPlanet.AgricultureModifier = 1.75m;
                                    NewPlanet.OreModifier = 0m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(950, 2150);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                                case PlanetType.Dead:
                                    NewPlanet.PopulationModifier = 0.05m;
                                    NewPlanet.AgricultureModifier = 0m;
                                    NewPlanet.OreModifier = 0m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(2, 4);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                                case PlanetType.ClusterLevel1:
                                    NewPlanet.PopulationModifier = 1.1m;
                                    NewPlanet.AgricultureModifier = 1.15m;
                                    NewPlanet.OreModifier = 0.02m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(1200, 5001);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                                case PlanetType.ClusterLevel2:
                                    NewPlanet.PopulationModifier = 1.2m;
                                    NewPlanet.AgricultureModifier = 1.3m;
                                    NewPlanet.OreModifier = 0.02m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(12000, 25001);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                                case PlanetType.ClusterLevel3:
                                    NewPlanet.PopulationModifier = 1.3m;
                                    NewPlanet.AgricultureModifier = 1.45m;
                                    NewPlanet.OreModifier = 0.02m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(55000, 125001);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                                default:
                                    NewPlanet.PopulationModifier = 1m;
                                    NewPlanet.AgricultureModifier = 1m;
                                    NewPlanet.OreModifier = 1m;
                                    NewPlanet.ArtifactModifier = 0.01m;
                                    NewPlanet.TotalLand = random.Next(2, 4);
                                    NewPlanet.AvailableOre = 0;
                                    break;
                            }
                            // Calculate LandAvailable
                            NewPlanet.LandAvailable = NewPlanet.TotalLand - (NewPlanet.Housing + NewPlanet.Commercial + NewPlanet.Industry + NewPlanet.Agriculture + NewPlanet.Mining);
                            // planet 2
                            var NewPlanet2 = new Planets
                            {
                                ApplicationUserId = user.Id,
                                ApplicationUser = user,
                                Name = "E." + random.Next(1000, 9999).ToString(),
                                Type = chosenType,
                                FoodRequired = 1,
                                GoodsRequired = 1,
                                CurrentPopulation = 10,
                                MaxPopulation = 10,
                                Loyalty = 2500,
                                AvailableLabour = 8,
                                Housing = 1,
                                Commercial = 0,
                                Industry = 0,
                                Agriculture = 0,
                                Mining = 1,
                                MineralProduced = 0,
                                PowerRating = 0
                            };
                            // Set type-specific values
                            switch (chosenType)
                            {
                                case PlanetType.Barren:
                                    NewPlanet2.PopulationModifier = 0.5m;
                                    NewPlanet2.AgricultureModifier = 0m;
                                    NewPlanet2.OreModifier = 0.02m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.AvailableOre = random.Next(150, 500);
                                    NewPlanet2.TotalLand = random.Next(50, 1100);
                                    break;
                                case PlanetType.Icy:
                                    NewPlanet2.PopulationModifier = 0.75m;
                                    NewPlanet2.AgricultureModifier = 1m;
                                    NewPlanet2.OreModifier = 0.005m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.AvailableOre = random.Next(1500, 4500);
                                    NewPlanet2.TotalLand = random.Next(24, 83);
                                    break;
                                case PlanetType.Marshy:
                                    NewPlanet2.PopulationModifier = 0.8m;
                                    NewPlanet2.AgricultureModifier = 0.5m;
                                    NewPlanet2.OreModifier = 0.005m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(50, 150);
                                    NewPlanet2.AvailableOre = random.Next(0, 150);
                                    break;
                                case PlanetType.Forest:
                                    NewPlanet2.PopulationModifier = 0.9m;
                                    NewPlanet2.AgricultureModifier = 2m;
                                    NewPlanet2.OreModifier = 0.005m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(50, 350);
                                    NewPlanet2.AvailableOre = random.Next(0, 50);
                                    break;
                                case PlanetType.Oceanic:
                                    NewPlanet2.PopulationModifier = 0.8m;
                                    NewPlanet2.AgricultureModifier = 0m;
                                    NewPlanet2.OreModifier = 0.005m;
                                    NewPlanet2.ArtifactModifier = 0.10m;
                                    NewPlanet2.TotalLand = random.Next(10, 50);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                                case PlanetType.Rocky:
                                    NewPlanet2.PopulationModifier = 0.75m;
                                    NewPlanet2.AgricultureModifier = 1m;
                                    NewPlanet2.OreModifier = 0.001m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(34, 121);
                                    NewPlanet2.AvailableOre = random.Next(500, 3300);
                                    break;
                                case PlanetType.Desert:
                                    NewPlanet2.PopulationModifier = 0.75m;
                                    NewPlanet2.AgricultureModifier = 0.75m;
                                    NewPlanet2.OreModifier = 0.02m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(100, 250);
                                    NewPlanet2.AvailableOre = random.Next(100, 350);
                                    break;
                                case PlanetType.Balanced:
                                    NewPlanet2.PopulationModifier = 1.2m;
                                    NewPlanet2.AgricultureModifier = 1m;
                                    NewPlanet2.OreModifier = 0.01m;
                                    NewPlanet2.ArtifactModifier = 0.05m;
                                    NewPlanet2.TotalLand = random.Next(185, 1050);
                                    NewPlanet2.AvailableOre = random.Next(750, 1100);
                                    break;
                                case PlanetType.Gas:
                                    NewPlanet2.PopulationModifier = 1m;
                                    NewPlanet2.AgricultureModifier = 0m;
                                    NewPlanet2.OreModifier = 0m;
                                    NewPlanet2.ArtifactModifier = 0.05m;
                                    NewPlanet2.TotalLand = random.Next(2, 6);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                                case PlanetType.URich:
                                    NewPlanet2.PopulationModifier = 0.1m;
                                    NewPlanet2.AgricultureModifier = 0m;
                                    NewPlanet2.OreModifier = 0.03m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(11, 28);
                                    NewPlanet2.AvailableOre = random.Next(50000, 350000);
                                    break;
                                case PlanetType.UEden:
                                    NewPlanet2.PopulationModifier = 10m;
                                    NewPlanet2.AgricultureModifier = 0.02m;
                                    NewPlanet2.OreModifier = 0m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(500, 2500);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                                case PlanetType.USpazial:
                                    NewPlanet2.PopulationModifier = 0.1m;
                                    NewPlanet2.AgricultureModifier = 0m;
                                    NewPlanet2.OreModifier = 0m;
                                    NewPlanet2.ArtifactModifier = 0.15m;
                                    NewPlanet2.TotalLand = random.Next(2, 4);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                                case PlanetType.ULarge:
                                    NewPlanet2.PopulationModifier = 0.2m;
                                    NewPlanet2.AgricultureModifier = 0m;
                                    NewPlanet2.OreModifier = 0m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(7000, 16000);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                                case PlanetType.UFertile:
                                    NewPlanet2.PopulationModifier = 0.5m;
                                    NewPlanet2.AgricultureModifier = 1.75m;
                                    NewPlanet2.OreModifier = 0m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(950, 2150);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                                case PlanetType.Dead:
                                    NewPlanet2.PopulationModifier = 0.05m;
                                    NewPlanet2.AgricultureModifier = 0m;
                                    NewPlanet2.OreModifier = 0m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(2, 4);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                                case PlanetType.ClusterLevel1:
                                    NewPlanet2.PopulationModifier = 1.1m;
                                    NewPlanet2.AgricultureModifier = 1.15m;
                                    NewPlanet2.OreModifier = 0.02m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(1200, 5001);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                                case PlanetType.ClusterLevel2:
                                    NewPlanet2.PopulationModifier = 1.2m;
                                    NewPlanet2.AgricultureModifier = 1.3m;
                                    NewPlanet2.OreModifier = 0.02m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(12000, 25001);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                                case PlanetType.ClusterLevel3:
                                    NewPlanet2.PopulationModifier = 1.3m;
                                    NewPlanet2.AgricultureModifier = 1.45m;
                                    NewPlanet2.OreModifier = 0.02m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(55000, 125001);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                                default:
                                    NewPlanet2.PopulationModifier = 1m;
                                    NewPlanet2.AgricultureModifier = 1m;
                                    NewPlanet2.OreModifier = 1m;
                                    NewPlanet2.ArtifactModifier = 0.01m;
                                    NewPlanet2.TotalLand = random.Next(2, 4);
                                    NewPlanet2.AvailableOre = 0;
                                    break;
                            }
                            // Calculate LandAvailable
                            NewPlanet2.LandAvailable = NewPlanet2.TotalLand - (NewPlanet2.Housing + NewPlanet2.Commercial + NewPlanet2.Industry + NewPlanet2.Agriculture + NewPlanet2.Mining);
                            // planet 3
                            var NewPlanet3 = new Planets
                            {
                                ApplicationUserId = user.Id,
                                ApplicationUser = user,
                                Name = "E." + random.Next(1000, 9999).ToString(),
                                Type = chosenType,
                                FoodRequired = 1,
                                GoodsRequired = 1,
                                CurrentPopulation = 10,
                                MaxPopulation = 10,
                                Loyalty = 2500,
                                AvailableLabour = 8,
                                Housing = 1,
                                Commercial = 0,
                                Industry = 0,
                                Agriculture = 0,
                                Mining = 1,
                                MineralProduced = 0,
                                PowerRating = 0
                            };
                            // Set type-specific values
                            switch (chosenType)
                            {
                                case PlanetType.Barren:
                                    NewPlanet3.PopulationModifier = 0.5m;
                                    NewPlanet3.AgricultureModifier = 0m;
                                    NewPlanet3.OreModifier = 0.02m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.AvailableOre = random.Next(150, 500);
                                    NewPlanet3.TotalLand = random.Next(50, 1100);
                                    break;
                                case PlanetType.Icy:
                                    NewPlanet3.PopulationModifier = 0.75m;
                                    NewPlanet3.AgricultureModifier = 1m;
                                    NewPlanet3.OreModifier = 0.005m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.AvailableOre = random.Next(1500, 4500);
                                    NewPlanet3.TotalLand = random.Next(24, 83);
                                    break;
                                case PlanetType.Marshy:
                                    NewPlanet3.PopulationModifier = 0.8m;
                                    NewPlanet3.AgricultureModifier = 0.5m;
                                    NewPlanet3.OreModifier = 0.005m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(50, 150);
                                    NewPlanet3.AvailableOre = random.Next(0, 150);
                                    break;
                                case PlanetType.Forest:
                                    NewPlanet3.PopulationModifier = 0.9m;
                                    NewPlanet3.AgricultureModifier = 2m;
                                    NewPlanet3.OreModifier = 0.005m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(50, 350);
                                    NewPlanet3.AvailableOre = random.Next(0, 50);
                                    break;
                                case PlanetType.Oceanic:
                                    NewPlanet3.PopulationModifier = 0.8m;
                                    NewPlanet3.AgricultureModifier = 0m;
                                    NewPlanet3.OreModifier = 0.005m;
                                    NewPlanet3.ArtifactModifier = 0.10m;
                                    NewPlanet3.TotalLand = random.Next(10, 50);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                                case PlanetType.Rocky:
                                    NewPlanet3.PopulationModifier = 0.75m;
                                    NewPlanet3.AgricultureModifier = 1m;
                                    NewPlanet3.OreModifier = 0.001m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(34, 121);
                                    NewPlanet3.AvailableOre = random.Next(500, 3300);
                                    break;
                                case PlanetType.Desert:
                                    NewPlanet3.PopulationModifier = 0.75m;
                                    NewPlanet3.AgricultureModifier = 0.75m;
                                    NewPlanet3.OreModifier = 0.02m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(100, 250);
                                    NewPlanet3.AvailableOre = random.Next(100, 350);
                                    break;
                                case PlanetType.Balanced:
                                    NewPlanet3.PopulationModifier = 1.2m;
                                    NewPlanet3.AgricultureModifier = 1m;
                                    NewPlanet3.OreModifier = 0.01m;
                                    NewPlanet3.ArtifactModifier = 0.05m;
                                    NewPlanet3.TotalLand = random.Next(185, 1050);
                                    NewPlanet3.AvailableOre = random.Next(750, 1100);
                                    break;
                                case PlanetType.Gas:
                                    NewPlanet3.PopulationModifier = 1m;
                                    NewPlanet3.AgricultureModifier = 0m;
                                    NewPlanet3.OreModifier = 0m;
                                    NewPlanet3.ArtifactModifier = 0.05m;
                                    NewPlanet3.TotalLand = random.Next(2, 6);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                                case PlanetType.URich:
                                    NewPlanet3.PopulationModifier = 0.1m;
                                    NewPlanet3.AgricultureModifier = 0m;
                                    NewPlanet3.OreModifier = 0.03m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(11, 28);
                                    NewPlanet3.AvailableOre = random.Next(50000, 350000);
                                    break;
                                case PlanetType.UEden:
                                    NewPlanet3.PopulationModifier = 10m;
                                    NewPlanet3.AgricultureModifier = 0.02m;
                                    NewPlanet3.OreModifier = 0m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(500, 2500);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                                case PlanetType.USpazial:
                                    NewPlanet3.PopulationModifier = 0.1m;
                                    NewPlanet3.AgricultureModifier = 0m;
                                    NewPlanet3.OreModifier = 0m;
                                    NewPlanet3.ArtifactModifier = 0.15m;
                                    NewPlanet3.TotalLand = random.Next(2, 4);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                                case PlanetType.ULarge:
                                    NewPlanet3.PopulationModifier = 0.2m;
                                    NewPlanet3.AgricultureModifier = 0m;
                                    NewPlanet3.OreModifier = 0m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(7000, 16000);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                                case PlanetType.UFertile:
                                    NewPlanet3.PopulationModifier = 0.5m;
                                    NewPlanet3.AgricultureModifier = 1.75m;
                                    NewPlanet3.OreModifier = 0m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(950, 2150);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                                case PlanetType.Dead:
                                    NewPlanet3.PopulationModifier = 0.05m;
                                    NewPlanet3.AgricultureModifier = 0m;
                                    NewPlanet3.OreModifier = 0m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(2, 4);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                                case PlanetType.ClusterLevel1:
                                    NewPlanet3.PopulationModifier = 1.1m;
                                    NewPlanet3.AgricultureModifier = 1.15m;
                                    NewPlanet3.OreModifier = 0.02m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(1200, 5001);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                                case PlanetType.ClusterLevel2:
                                    NewPlanet3.PopulationModifier = 1.2m;
                                    NewPlanet3.AgricultureModifier = 1.3m;
                                    NewPlanet3.OreModifier = 0.02m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(12000, 25001);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                                case PlanetType.ClusterLevel3:
                                    NewPlanet3.PopulationModifier = 1.3m;
                                    NewPlanet3.AgricultureModifier = 1.45m;
                                    NewPlanet3.OreModifier = 0.02m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(55000, 125001);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                                default:
                                    NewPlanet3.PopulationModifier = 1m;
                                    NewPlanet3.AgricultureModifier = 1m;
                                    NewPlanet3.OreModifier = 1m;
                                    NewPlanet3.ArtifactModifier = 0.01m;
                                    NewPlanet3.TotalLand = random.Next(2, 4);
                                    NewPlanet3.AvailableOre = 0;
                                    break;
                            }
                            // Calculate LandAvailable
                            NewPlanet3.LandAvailable = NewPlanet3.TotalLand - (NewPlanet3.Housing + NewPlanet3.Commercial + NewPlanet3.Industry + NewPlanet3.Agriculture + NewPlanet3.Mining);
                            // Add planets to list
                            DefenderPlanets.Add(NewPlanet);
                            DefenderPlanets.Add(NewPlanet2);
                            DefenderPlanets.Add(NewPlanet3);
                        }
                        else
                        {
                            DefenderPlanets = _context.Planets.Where(p => p.ApplicationUserId == TargetUserFleets.FirstOrDefault().ApplicationUserId && !p.Name.Contains("H.")).OrderBy(x => x.DateTimeAcquired).ToList();
                        }
                        if (DefenderPlanets.Count > 0)
                        {
                            if (DefenderPlanets.Count == 1)
                            {
                                DefenderPlanets[0].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                                DefenderPlanets[0].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                                DefenderPlanets[0].DateTimeAcquired = DateTime.UtcNow;
                                currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                                _context.Planets.Update(DefenderPlanets[0]);
                            }
                            else if (DefenderPlanets.Count == 2)
                            {
                                DefenderPlanets[0].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                                DefenderPlanets[0].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                                DefenderPlanets[0].DateTimeAcquired = DateTime.UtcNow;
                                currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                                DefenderPlanets[1].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                                DefenderPlanets[1].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                                DefenderPlanets[1].DateTimeAcquired = DateTime.UtcNow;
                                currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                                _context.Planets.Update(DefenderPlanets[0]);
                                _context.Planets.Update(DefenderPlanets[1]);
                            }
                            else if (DefenderPlanets.Count > 2)
                            {
                                DefenderPlanets[0].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                                DefenderPlanets[0].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                                DefenderPlanets[0].DateTimeAcquired = DateTime.UtcNow;
                                currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                                DefenderPlanets[1].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                                DefenderPlanets[1].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                                DefenderPlanets[1].DateTimeAcquired = DateTime.UtcNow;
                                currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                                DefenderPlanets[2].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                                DefenderPlanets[2].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                                DefenderPlanets[2].DateTimeAcquired = DateTime.UtcNow;
                                currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                                _context.Planets.Update(DefenderPlanets[0]);
                                _context.Planets.Update(DefenderPlanets[1]);
                                _context.Planets.Update(DefenderPlanets[2]);
                            }
                        }
                        // Fleet Report + Important Events for attacker and defender
                        EndFleets = $"<h3>{currentUser.UserName}'s Fleet</h3>";
                        EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                        foreach (var ship in AttackerFleet)
                        {
                            int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                            EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                        }
                        EndFleets += "</table>";

                        EndFleets += $"<h3>{targetUser.UserName}'s Fleet</h3>";
                        EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                        foreach (var ship in DefenderFleet)
                        {
                            int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                            EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                        }
                        EndFleets += "</table>";
                        if (DefenderPlanets.Count > 0)
                        {
                            EndFleets += "<h3>Planets Won</h3>";
                            EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Type</th><th>Land</th></tr>";
                            foreach (var planet in DefenderPlanets)
                            {
                                EndFleets += $"<tr><td>{planet.Name}</td><td>{planet.Type}</td><td>{planet.TotalLand}</td></tr>";
                            }
                            EndFleets += "</table>";
                        }
                        else
                        {
                            EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Type</th><th>Land</th></tr>";
                            EndFleets += "<tr><td colspan='3'>Defender has no planets</td></tr>";
                            EndFleets += "/table>";
                        }
                        ImportantEvents attackerEvent = new ImportantEvents
                        {
                            ApplicationUserId = currentUser.Id,
                            ImportantEventTypes = ImportantEventTypes.Battles,
                            Text = $"You have won the battle against {TargetUserName}.<br />{EndFleets}<br />",
                            DateAndTime = DateTime.Now
                        };
                        _context.ImportantEvents.Add(attackerEvent);
                        if (targetUser.IsNPC != true)
                        {
                            ImportantEvents DefenderEvent = new ImportantEvents
                            {
                                ApplicationUserId = targetUser.Id,
                                ImportantEventTypes = ImportantEventTypes.Battles,
                                Text = $"You have lost the battle against {currentUser.UserName}.<br />{EndFleets}<br />",
                                DateAndTime = DateTime.Now
                            };
                            _context.ImportantEvents.Add(DefenderEvent);
                        }
                        // battle logs
                        BattleLogs AttackerBattleLog = new BattleLogs
                        {
                            ApplicationUserId = currentUser.Id,
                            Attacker = currentUser.UserName,
                            Defender = targetUser.UserName,
                            DateAndTime = DateTime.Now,
                            Outcome = $"Win",
                            FleetReport = EndFleets
                        };
                        _context.Battlelogs.Add(AttackerBattleLog);
                        if (targetUser.IsNPC != true)
                        {
                            BattleLogs DefenderBattleLog = new BattleLogs
                            {
                                ApplicationUserId = targetUser.Id,
                                Attacker = currentUser.UserName,
                                Defender = targetUser.UserName,
                                DateAndTime = DateTime.Now,
                                Outcome = $"Loss",
                                FleetReport = EndFleets
                            };
                            _context.Battlelogs.Add(DefenderBattleLog);
                        }
                        // update database for attacker and defenders fleets
                        foreach (var fleet in AttackerFleet)
                        {
                            var existingFleet = _context.Fleets.FirstOrDefault(f => f.ApplicationUserId == attacker.ApplicationUserId && f.ShipId == attacker.ShipId);
                            existingFleet.TotalShips = fleet.TotalShips;
                            existingFleet.TotalPowerRating = fleet.PowerRating * fleet.TotalShips;
                            existingFleet.TotalUpkeep = fleet.Upkeep * fleet.TotalShips;
                            if (existingFleet.TotalShips <= 0)
                            {
                                _context.Fleets.Remove(existingFleet); // Remove fleet if no ships left
                            }
                            else
                            {
                                _context.Fleets.Update(existingFleet);
                            }
                        }
                        if (targetUser.IsNPC != true)
                        {
                            foreach (var fleet in DefenderFleet)
                            {
                                var existingFleet = _context.Fleets.FirstOrDefault(f => f.ApplicationUserId == defender.ApplicationUserId && f.ShipId == defender.ShipId);
                                existingFleet.TotalShips = fleet.TotalShips;
                                existingFleet.TotalPowerRating = fleet.PowerRating * fleet.TotalShips;
                                existingFleet.TotalUpkeep = fleet.Upkeep * fleet.TotalShips;
                                _context.Fleets.Update(existingFleet);
                                if (existingFleet.TotalShips <= 0)
                                {
                                    _context.Fleets.Remove(existingFleet); // Remove fleet if no ships left
                                }
                                else
                                {
                                    _context.Fleets.Update(existingFleet);
                                }
                            }
                        }
                        // Update Attackers Stats
                        currentUser.BattlesWon++;
                        // Update Defenders Stats + Damage Protection
                        targetUser.BattlesLost++;
                        if (DefenderPlanets.Count > 0)
                        {
                            if (DefenderPlanets.Count == 1)
                            {

                                targetUser.TotalPlanets -= DefenderPlanets[0].TotalPlanets;
                                targetUser.TotalColonies -= 1;
                                targetUser.ColoniesLost += 1;
                                currentUser.ColoniesWon += 1;
                                currentUser.TotalColonies += 1;
                                currentUser.TotalPlanets += DefenderPlanets[0].TotalPlanets;

                            }
                            else if (DefenderPlanets.Count == 2)
                            {

                                targetUser.TotalPlanets -= DefenderPlanets[0].TotalPlanets + DefenderPlanets[1].TotalPlanets;
                                targetUser.TotalColonies -= 2;
                                targetUser.ColoniesLost += 2;
                                currentUser.ColoniesWon += 2;
                                currentUser.TotalColonies += 2;
                                currentUser.TotalPlanets += DefenderPlanets[0].TotalPlanets + DefenderPlanets[1].TotalPlanets;
                            }
                            else if (DefenderPlanets.Count > 2)
                            {

                                targetUser.TotalPlanets -= DefenderPlanets[0].TotalPlanets + DefenderPlanets[1].TotalPlanets + DefenderPlanets[2].TotalPlanets;
                                targetUser.TotalColonies -= 3;
                                targetUser.ColoniesLost += 3;
                                currentUser.ColoniesWon += 3;
                                currentUser.TotalColonies += 3;
                                currentUser.TotalPlanets += DefenderPlanets[0].TotalPlanets + DefenderPlanets[1].TotalPlanets + DefenderPlanets[2].TotalPlanets;
                            }
                        }
                        if (targetUser.IsNPC == true)
                        {
                            targetUser.TotalPlanets = 1; // NPCs should always have at least one planet
                            targetUser.TotalColonies = 1; // NPCs should always have at least one colony
                            targetUser.DamageProtection = DateTime.Now.AddMinutes(15); // Reset damage protection for defender
                        }
                        else
                        {
                            targetUser.DamageProtection = DateTime.Now.AddDays(1); // Reset damage protection for defender
                        }
                        var turnsMessage = await _turnService.TryUseTurnsAsync(currentUser.Id, 5);
                        StatusMessage = turnsMessage.Message;
                        VictoryMessage = $"You have won the battle against {TargetUserName}";
                        _context.SaveChanges();
                        return Page();
                    }
                    // attacker no fleet
                    else if (AttackerFleet.All(f => f.TotalShips <= 0))
                    {
                        // Attacker has no fleet left
                        // Defender wins by default
                        // Fleet Report + Important Events for attacker and defender
                        EndFleets = $"<h3>{currentUser.UserName}'s Fleet</h3>";
                        EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                        foreach (var ship in AttackerFleet)
                        {
                            int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                            EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                        }
                        EndFleets += "</table>";

                        EndFleets += $"<h3>{targetUser.UserName}'s Fleet</h3>";
                        EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                        foreach (var ship in DefenderFleet)
                        {
                            int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                            EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                        }
                        EndFleets += "</table>";
                        ImportantEvents attackerEvent = new ImportantEvents
                        {
                            ApplicationUserId = currentUser.Id,
                            ImportantEventTypes = ImportantEventTypes.Battles,
                            Text = $"You have Lost the battle against {TargetUserName}.<br />{EndFleets}<br />",
                            DateAndTime = DateTime.Now
                        };
                        _context.ImportantEvents.Add(attackerEvent);
                        if (targetUser.IsNPC != true)
                        {
                            ImportantEvents DefenderEvent = new ImportantEvents
                            {
                                ApplicationUserId = targetUser.Id,
                                ImportantEventTypes = ImportantEventTypes.Battles,
                                Text = $"You have Won the battle against {currentUser.UserName}.<br />{EndFleets}<br />",
                                DateAndTime = DateTime.Now
                            };
                            _context.ImportantEvents.Add(DefenderEvent);
                        }
                        // battle logs
                        BattleLogs AttackerBattleLog = new BattleLogs
                        {
                            ApplicationUserId = currentUser.Id,
                            Attacker = currentUser.UserName,
                            Defender = targetUser.UserName,
                            DateAndTime = DateTime.Now,
                            Outcome = $"Loss",
                            FleetReport = EndFleets
                        };
                        _context.Battlelogs.Add(AttackerBattleLog);
                        if (targetUser.IsNPC != true)
                        {
                            BattleLogs DefenderBattleLog = new BattleLogs
                            {
                                ApplicationUserId = targetUser.Id,
                                Attacker = currentUser.UserName,
                                Defender = targetUser.UserName,
                                DateAndTime = DateTime.Now,
                                Outcome = $"Win",
                                FleetReport = EndFleets
                            };
                            _context.Battlelogs.Add(DefenderBattleLog);
                        }
                        // update database for attacker and defenders fleets
                        foreach (var fleet in AttackerFleet)
                        {
                            var existingFleet = _context.Fleets.FirstOrDefault(f => f.ApplicationUserId == attacker.ApplicationUserId && f.ShipId == attacker.ShipId);
                            existingFleet.TotalShips = fleet.TotalShips;
                            existingFleet.TotalPowerRating = fleet.PowerRating * fleet.TotalShips;
                            existingFleet.TotalUpkeep = fleet.Upkeep * fleet.TotalShips;
                            if (existingFleet.TotalShips <= 0)
                            {
                                _context.Fleets.Remove(existingFleet); // Remove fleet if no ships left
                            }
                            else
                            {
                                _context.Fleets.Update(existingFleet);
                            }
                        }
                        if (targetUser.IsNPC != true)
                        {
                            foreach (var fleet in DefenderFleet)
                            {
                                var existingFleet = _context.Fleets.FirstOrDefault(f => f.ApplicationUserId == defender.ApplicationUserId && f.ShipId == defender.ShipId);
                                existingFleet.TotalShips = fleet.TotalShips;
                                existingFleet.TotalPowerRating = fleet.PowerRating * fleet.TotalShips;
                                existingFleet.TotalUpkeep = fleet.Upkeep * fleet.TotalShips;
                                _context.Fleets.Update(existingFleet);
                                if (existingFleet.TotalShips <= 0)
                                {
                                    _context.Fleets.Remove(existingFleet); // Remove fleet if no ships left
                                }
                                else
                                {
                                    _context.Fleets.Update(existingFleet);
                                }
                            }
                        }
                        // Update Attackers Stats
                        currentUser.BattlesLost += 1;
                        // Update Defenders Stats
                        targetUser.BattlesWon++;
                        if (targetUser.IsNPC == true)
                        {
                            targetUser.TotalPlanets = 1; // NPCs should always have at least one planet
                            targetUser.TotalColonies = 1; // NPCs should always have at least one colony
                            targetUser.DamageProtection = DateTime.Now.AddMinutes(15); // Reset damage protection for defender
                        }
                        else
                        {
                            targetUser.DamageProtection = DateTime.Now.AddDays(1); // Reset damage protection for defender
                        }
                        var turnsMessage = await _turnService.TryUseTurnsAsync(currentUser.Id, 5);
                        StatusMessage = turnsMessage.Message;
                        VictoryMessage = $"You have lost the battle against {TargetUserName}";
                        _context.SaveChanges();
                        return Page();
                    }
                }
                
            }
            else
            {
                // Defender has no fleet left
                // Attacker wins by default
                // Sort Planets For Capture
                List<Planets> DefenderPlanets = new List<Planets>();
                if (targetUser.IsNPC == true)
                {
                    var planetTypeWeights = new List<(PlanetType Type, double Weight)>
                            {
                               (PlanetType.Barren, 5),
                               (PlanetType.Icy, 5),
                               (PlanetType.Marshy, 5),
                               (PlanetType.Forest, 5),
                               (PlanetType.Oceanic, 5),
                               (PlanetType.Rocky, 5),
                               (PlanetType.Desert, 5),
                               (PlanetType.Balanced, 5),
                               (PlanetType.Gas, 5),
                               (PlanetType.URich, 0.5),
                               (PlanetType.UEden, 0.5),
                               (PlanetType.USpazial, 0.5),
                               (PlanetType.ULarge, 0.5),
                               (PlanetType.UFertile, 0.5),
                               (PlanetType.Dead, 0.5),
                               (PlanetType.ClusterLevel1, 0.25),
                               (PlanetType.ClusterLevel2, 0.10),
                               (PlanetType.ClusterLevel3, 0.05)
                            };
                    var random = new System.Random();
                    var chosenType = GetRandomPlanetTypeWeighted(planetTypeWeights);
                    //planet 1
                    var NewPlanet = new Planets
                    {
                        ApplicationUserId = user.Id,
                        ApplicationUser = user,
                        Name = "E." + random.Next(1000, 9999).ToString(),
                        Type = chosenType,
                        FoodRequired = 1,
                        GoodsRequired = 1,
                        CurrentPopulation = 10,
                        MaxPopulation = 10,
                        Loyalty = 2500,
                        AvailableLabour = 8,
                        Housing = 1,
                        Commercial = 0,
                        Industry = 0,
                        Agriculture = 0,
                        Mining = 1,
                        MineralProduced = 0,
                        PowerRating = 0

                    };
                    // Set type-specific values
                    switch (chosenType)
                    {
                        case PlanetType.Barren:
                            NewPlanet.PopulationModifier = 0.5m;
                            NewPlanet.AgricultureModifier = 0m;
                            NewPlanet.OreModifier = 0.02m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.AvailableOre = random.Next(150, 500);
                            NewPlanet.TotalLand = random.Next(50, 1100);
                            break;
                        case PlanetType.Icy:
                            NewPlanet.PopulationModifier = 0.75m;
                            NewPlanet.AgricultureModifier = 1m;
                            NewPlanet.OreModifier = 0.005m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.AvailableOre = random.Next(1500, 4500);
                            NewPlanet.TotalLand = random.Next(24, 83);
                            break;
                        case PlanetType.Marshy:
                            NewPlanet.PopulationModifier = 0.8m;
                            NewPlanet.AgricultureModifier = 0.5m;
                            NewPlanet.OreModifier = 0.005m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(50, 150);
                            NewPlanet.AvailableOre = random.Next(0, 150);
                            break;
                        case PlanetType.Forest:
                            NewPlanet.PopulationModifier = 0.9m;
                            NewPlanet.AgricultureModifier = 2m;
                            NewPlanet.OreModifier = 0.005m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(50, 350);
                            NewPlanet.AvailableOre = random.Next(0, 50);
                            break;
                        case PlanetType.Oceanic:
                            NewPlanet.PopulationModifier = 0.8m;
                            NewPlanet.AgricultureModifier = 0m;
                            NewPlanet.OreModifier = 0.005m;
                            NewPlanet.ArtifactModifier = 0.10m;
                            NewPlanet.TotalLand = random.Next(10, 50);
                            NewPlanet.AvailableOre = 0;
                            break;
                        case PlanetType.Rocky:
                            NewPlanet.PopulationModifier = 0.75m;
                            NewPlanet.AgricultureModifier = 1m;
                            NewPlanet.OreModifier = 0.001m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(34, 121);
                            NewPlanet.AvailableOre = random.Next(500, 3300);
                            break;
                        case PlanetType.Desert:
                            NewPlanet.PopulationModifier = 0.75m;
                            NewPlanet.AgricultureModifier = 0.75m;
                            NewPlanet.OreModifier = 0.02m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(100, 250);
                            NewPlanet.AvailableOre = random.Next(100, 350);
                            break;
                        case PlanetType.Balanced:
                            NewPlanet.PopulationModifier = 1.2m;
                            NewPlanet.AgricultureModifier = 1m;
                            NewPlanet.OreModifier = 0.01m;
                            NewPlanet.ArtifactModifier = 0.05m;
                            NewPlanet.TotalLand = random.Next(185, 1050);
                            NewPlanet.AvailableOre = random.Next(750, 1100);
                            break;
                        case PlanetType.Gas:
                            NewPlanet.PopulationModifier = 1m;
                            NewPlanet.AgricultureModifier = 0m;
                            NewPlanet.OreModifier = 0m;
                            NewPlanet.ArtifactModifier = 0.05m;
                            NewPlanet.TotalLand = random.Next(2, 6);
                            NewPlanet.AvailableOre = 0;
                            break;
                        case PlanetType.URich:
                            NewPlanet.PopulationModifier = 0.1m;
                            NewPlanet.AgricultureModifier = 0m;
                            NewPlanet.OreModifier = 0.03m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(11, 28);
                            NewPlanet.AvailableOre = random.Next(50000, 350000);
                            break;
                        case PlanetType.UEden:
                            NewPlanet.PopulationModifier = 10m;
                            NewPlanet.AgricultureModifier = 0.02m;
                            NewPlanet.OreModifier = 0m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(500, 2500);
                            NewPlanet.AvailableOre = 0;
                            break;
                        case PlanetType.USpazial:
                            NewPlanet.PopulationModifier = 0.1m;
                            NewPlanet.AgricultureModifier = 0m;
                            NewPlanet.OreModifier = 0m;
                            NewPlanet.ArtifactModifier = 0.15m;
                            NewPlanet.TotalLand = random.Next(2, 4);
                            NewPlanet.AvailableOre = 0;
                            break;
                        case PlanetType.ULarge:
                            NewPlanet.PopulationModifier = 0.2m;
                            NewPlanet.AgricultureModifier = 0m;
                            NewPlanet.OreModifier = 0m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(7000, 16000);
                            NewPlanet.AvailableOre = 0;
                            break;
                        case PlanetType.UFertile:
                            NewPlanet.PopulationModifier = 0.5m;
                            NewPlanet.AgricultureModifier = 1.75m;
                            NewPlanet.OreModifier = 0m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(950, 2150);
                            NewPlanet.AvailableOre = 0;
                            break;
                        case PlanetType.Dead:
                            NewPlanet.PopulationModifier = 0.05m;
                            NewPlanet.AgricultureModifier = 0m;
                            NewPlanet.OreModifier = 0m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(2, 4);
                            NewPlanet.AvailableOre = 0;
                            break;
                        case PlanetType.ClusterLevel1:
                            NewPlanet.PopulationModifier = 1.1m;
                            NewPlanet.AgricultureModifier = 1.15m;
                            NewPlanet.OreModifier = 0.02m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(1200, 5001);
                            NewPlanet.AvailableOre = 0;
                            break;
                        case PlanetType.ClusterLevel2:
                            NewPlanet.PopulationModifier = 1.2m;
                            NewPlanet.AgricultureModifier = 1.3m;
                            NewPlanet.OreModifier = 0.02m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(12000, 25001);
                            NewPlanet.AvailableOre = 0;
                            break;
                        case PlanetType.ClusterLevel3:
                            NewPlanet.PopulationModifier = 1.3m;
                            NewPlanet.AgricultureModifier = 1.45m;
                            NewPlanet.OreModifier = 0.02m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(55000, 125001);
                            NewPlanet.AvailableOre = 0;
                            break;
                        default:
                            NewPlanet.PopulationModifier = 1m;
                            NewPlanet.AgricultureModifier = 1m;
                            NewPlanet.OreModifier = 1m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(2, 4);
                            NewPlanet.AvailableOre = 0;
                            break;
                    }
                    // Calculate LandAvailable
                    NewPlanet.LandAvailable = NewPlanet.TotalLand - (NewPlanet.Housing + NewPlanet.Commercial + NewPlanet.Industry + NewPlanet.Agriculture + NewPlanet.Mining);
                    // planet 2
                    var NewPlanet2 = new Planets
                    {
                        ApplicationUserId = user.Id,
                        ApplicationUser = user,
                        Name = "E." + random.Next(1000, 9999).ToString(),
                        Type = chosenType,
                        FoodRequired = 1,
                        GoodsRequired = 1,
                        CurrentPopulation = 10,
                        MaxPopulation = 10,
                        Loyalty = 2500,
                        AvailableLabour = 8,
                        Housing = 1,
                        Commercial = 0,
                        Industry = 0,
                        Agriculture = 0,
                        Mining = 1,
                        MineralProduced = 0,
                        PowerRating = 0
                    };
                    // Set type-specific values
                    switch (chosenType)
                    {
                        case PlanetType.Barren:
                            NewPlanet2.PopulationModifier = 0.5m;
                            NewPlanet2.AgricultureModifier = 0m;
                            NewPlanet2.OreModifier = 0.02m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.AvailableOre = random.Next(150, 500);
                            NewPlanet2.TotalLand = random.Next(50, 1100);
                            break;
                        case PlanetType.Icy:
                            NewPlanet2.PopulationModifier = 0.75m;
                            NewPlanet2.AgricultureModifier = 1m;
                            NewPlanet2.OreModifier = 0.005m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.AvailableOre = random.Next(1500, 4500);
                            NewPlanet2.TotalLand = random.Next(24, 83);
                            break;
                        case PlanetType.Marshy:
                            NewPlanet2.PopulationModifier = 0.8m;
                            NewPlanet2.AgricultureModifier = 0.5m;
                            NewPlanet2.OreModifier = 0.005m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(50, 150);
                            NewPlanet2.AvailableOre = random.Next(0, 150);
                            break;
                        case PlanetType.Forest:
                            NewPlanet2.PopulationModifier = 0.9m;
                            NewPlanet2.AgricultureModifier = 2m;
                            NewPlanet2.OreModifier = 0.005m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(50, 350);
                            NewPlanet2.AvailableOre = random.Next(0, 50);
                            break;
                        case PlanetType.Oceanic:
                            NewPlanet2.PopulationModifier = 0.8m;
                            NewPlanet2.AgricultureModifier = 0m;
                            NewPlanet2.OreModifier = 0.005m;
                            NewPlanet2.ArtifactModifier = 0.10m;
                            NewPlanet2.TotalLand = random.Next(10, 50);
                            NewPlanet2.AvailableOre = 0;
                            break;
                        case PlanetType.Rocky:
                            NewPlanet2.PopulationModifier = 0.75m;
                            NewPlanet2.AgricultureModifier = 1m;
                            NewPlanet2.OreModifier = 0.001m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(34, 121);
                            NewPlanet2.AvailableOre = random.Next(500, 3300);
                            break;
                        case PlanetType.Desert:
                            NewPlanet2.PopulationModifier = 0.75m;
                            NewPlanet2.AgricultureModifier = 0.75m;
                            NewPlanet2.OreModifier = 0.02m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(100, 250);
                            NewPlanet2.AvailableOre = random.Next(100, 350);
                            break;
                        case PlanetType.Balanced:
                            NewPlanet2.PopulationModifier = 1.2m;
                            NewPlanet2.AgricultureModifier = 1m;
                            NewPlanet2.OreModifier = 0.01m;
                            NewPlanet2.ArtifactModifier = 0.05m;
                            NewPlanet2.TotalLand = random.Next(185, 1050);
                            NewPlanet2.AvailableOre = random.Next(750, 1100);
                            break;
                        case PlanetType.Gas:
                            NewPlanet2.PopulationModifier = 1m;
                            NewPlanet2.AgricultureModifier = 0m;
                            NewPlanet2.OreModifier = 0m;
                            NewPlanet2.ArtifactModifier = 0.05m;
                            NewPlanet2.TotalLand = random.Next(2, 6);
                            NewPlanet2.AvailableOre = 0;
                            break;
                        case PlanetType.URich:
                            NewPlanet2.PopulationModifier = 0.1m;
                            NewPlanet2.AgricultureModifier = 0m;
                            NewPlanet2.OreModifier = 0.03m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(11, 28);
                            NewPlanet2.AvailableOre = random.Next(50000, 350000);
                            break;
                        case PlanetType.UEden:
                            NewPlanet2.PopulationModifier = 10m;
                            NewPlanet2.AgricultureModifier = 0.02m;
                            NewPlanet2.OreModifier = 0m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(500, 2500);
                            NewPlanet2.AvailableOre = 0;
                            break;
                        case PlanetType.USpazial:
                            NewPlanet2.PopulationModifier = 0.1m;
                            NewPlanet2.AgricultureModifier = 0m;
                            NewPlanet2.OreModifier = 0m;
                            NewPlanet2.ArtifactModifier = 0.15m;
                            NewPlanet2.TotalLand = random.Next(2, 4);
                            NewPlanet2.AvailableOre = 0;
                            break;
                        case PlanetType.ULarge:
                            NewPlanet2.PopulationModifier = 0.2m;
                            NewPlanet2.AgricultureModifier = 0m;
                            NewPlanet2.OreModifier = 0m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(7000, 16000);
                            NewPlanet2.AvailableOre = 0;
                            break;
                        case PlanetType.UFertile:
                            NewPlanet2.PopulationModifier = 0.5m;
                            NewPlanet2.AgricultureModifier = 1.75m;
                            NewPlanet2.OreModifier = 0m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(950, 2150);
                            NewPlanet2.AvailableOre = 0;
                            break;
                        case PlanetType.Dead:
                            NewPlanet2.PopulationModifier = 0.05m;
                            NewPlanet2.AgricultureModifier = 0m;
                            NewPlanet2.OreModifier = 0m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(2, 4);
                            NewPlanet2.AvailableOre = 0;
                            break;
                        case PlanetType.ClusterLevel1:
                            NewPlanet2.PopulationModifier = 1.1m;
                            NewPlanet2.AgricultureModifier = 1.15m;
                            NewPlanet2.OreModifier = 0.02m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(1200, 5001);
                            NewPlanet2.AvailableOre = 0;
                            break;
                        case PlanetType.ClusterLevel2:
                            NewPlanet2.PopulationModifier = 1.2m;
                            NewPlanet2.AgricultureModifier = 1.3m;
                            NewPlanet2.OreModifier = 0.02m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(12000, 25001);
                            NewPlanet2.AvailableOre = 0;
                            break;
                        case PlanetType.ClusterLevel3:
                            NewPlanet2.PopulationModifier = 1.3m;
                            NewPlanet2.AgricultureModifier = 1.45m;
                            NewPlanet2.OreModifier = 0.02m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(55000, 125001);
                            NewPlanet2.AvailableOre = 0;
                            break;
                        default:
                            NewPlanet2.PopulationModifier = 1m;
                            NewPlanet2.AgricultureModifier = 1m;
                            NewPlanet2.OreModifier = 1m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(2, 4);
                            NewPlanet2.AvailableOre = 0;
                            break;
                    }
                    // Calculate LandAvailable
                    NewPlanet2.LandAvailable = NewPlanet2.TotalLand - (NewPlanet2.Housing + NewPlanet2.Commercial + NewPlanet2.Industry + NewPlanet2.Agriculture + NewPlanet2.Mining);
                    // planet 3
                    var NewPlanet3 = new Planets
                    {
                        ApplicationUserId = user.Id,
                        ApplicationUser = user,
                        Name = "E." + random.Next(1000, 9999).ToString(),
                        Type = chosenType,
                        FoodRequired = 1,
                        GoodsRequired = 1,
                        CurrentPopulation = 10,
                        MaxPopulation = 10,
                        Loyalty = 2500,
                        AvailableLabour = 8,
                        Housing = 1,
                        Commercial = 0,
                        Industry = 0,
                        Agriculture = 0,
                        Mining = 1,
                        MineralProduced = 0,
                        PowerRating = 0
                    };
                    // Set type-specific values
                    switch (chosenType)
                    {
                        case PlanetType.Barren:
                            NewPlanet3.PopulationModifier = 0.5m;
                            NewPlanet3.AgricultureModifier = 0m;
                            NewPlanet3.OreModifier = 0.02m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.AvailableOre = random.Next(150, 500);
                            NewPlanet3.TotalLand = random.Next(50, 1100);
                            break;
                        case PlanetType.Icy:
                            NewPlanet3.PopulationModifier = 0.75m;
                            NewPlanet3.AgricultureModifier = 1m;
                            NewPlanet3.OreModifier = 0.005m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.AvailableOre = random.Next(1500, 4500);
                            NewPlanet3.TotalLand = random.Next(24, 83);
                            break;
                        case PlanetType.Marshy:
                            NewPlanet3.PopulationModifier = 0.8m;
                            NewPlanet3.AgricultureModifier = 0.5m;
                            NewPlanet3.OreModifier = 0.005m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(50, 150);
                            NewPlanet3.AvailableOre = random.Next(0, 150);
                            break;
                        case PlanetType.Forest:
                            NewPlanet3.PopulationModifier = 0.9m;
                            NewPlanet3.AgricultureModifier = 2m;
                            NewPlanet3.OreModifier = 0.005m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(50, 350);
                            NewPlanet3.AvailableOre = random.Next(0, 50);
                            break;
                        case PlanetType.Oceanic:
                            NewPlanet3.PopulationModifier = 0.8m;
                            NewPlanet3.AgricultureModifier = 0m;
                            NewPlanet3.OreModifier = 0.005m;
                            NewPlanet3.ArtifactModifier = 0.10m;
                            NewPlanet3.TotalLand = random.Next(10, 50);
                            NewPlanet3.AvailableOre = 0;
                            break;
                        case PlanetType.Rocky:
                            NewPlanet3.PopulationModifier = 0.75m;
                            NewPlanet3.AgricultureModifier = 1m;
                            NewPlanet3.OreModifier = 0.001m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(34, 121);
                            NewPlanet3.AvailableOre = random.Next(500, 3300);
                            break;
                        case PlanetType.Desert:
                            NewPlanet3.PopulationModifier = 0.75m;
                            NewPlanet3.AgricultureModifier = 0.75m;
                            NewPlanet3.OreModifier = 0.02m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(100, 250);
                            NewPlanet3.AvailableOre = random.Next(100, 350);
                            break;
                        case PlanetType.Balanced:
                            NewPlanet3.PopulationModifier = 1.2m;
                            NewPlanet3.AgricultureModifier = 1m;
                            NewPlanet3.OreModifier = 0.01m;
                            NewPlanet3.ArtifactModifier = 0.05m;
                            NewPlanet3.TotalLand = random.Next(185, 1050);
                            NewPlanet3.AvailableOre = random.Next(750, 1100);
                            break;
                        case PlanetType.Gas:
                            NewPlanet3.PopulationModifier = 1m;
                            NewPlanet3.AgricultureModifier = 0m;
                            NewPlanet3.OreModifier = 0m;
                            NewPlanet3.ArtifactModifier = 0.05m;
                            NewPlanet3.TotalLand = random.Next(2, 6);
                            NewPlanet3.AvailableOre = 0;
                            break;
                        case PlanetType.URich:
                            NewPlanet3.PopulationModifier = 0.1m;
                            NewPlanet3.AgricultureModifier = 0m;
                            NewPlanet3.OreModifier = 0.03m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(11, 28);
                            NewPlanet3.AvailableOre = random.Next(50000, 350000);
                            break;
                        case PlanetType.UEden:
                            NewPlanet3.PopulationModifier = 10m;
                            NewPlanet3.AgricultureModifier = 0.02m;
                            NewPlanet3.OreModifier = 0m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(500, 2500);
                            NewPlanet3.AvailableOre = 0;
                            break;
                        case PlanetType.USpazial:
                            NewPlanet3.PopulationModifier = 0.1m;
                            NewPlanet3.AgricultureModifier = 0m;
                            NewPlanet3.OreModifier = 0m;
                            NewPlanet3.ArtifactModifier = 0.15m;
                            NewPlanet3.TotalLand = random.Next(2, 4);
                            NewPlanet3.AvailableOre = 0;
                            break;
                        case PlanetType.ULarge:
                            NewPlanet3.PopulationModifier = 0.2m;
                            NewPlanet3.AgricultureModifier = 0m;
                            NewPlanet3.OreModifier = 0m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(7000, 16000);
                            NewPlanet3.AvailableOre = 0;
                            break;
                        case PlanetType.UFertile:
                            NewPlanet3.PopulationModifier = 0.5m;
                            NewPlanet3.AgricultureModifier = 1.75m;
                            NewPlanet3.OreModifier = 0m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(950, 2150);
                            NewPlanet3.AvailableOre = 0;
                            break;
                        case PlanetType.Dead:
                            NewPlanet3.PopulationModifier = 0.05m;
                            NewPlanet3.AgricultureModifier = 0m;
                            NewPlanet3.OreModifier = 0m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(2, 4);
                            NewPlanet3.AvailableOre = 0;
                            break;
                        case PlanetType.ClusterLevel1:
                            NewPlanet3.PopulationModifier = 1.1m;
                            NewPlanet3.AgricultureModifier = 1.15m;
                            NewPlanet3.OreModifier = 0.02m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(1200, 5001);
                            NewPlanet3.AvailableOre = 0;
                            break;
                        case PlanetType.ClusterLevel2:
                            NewPlanet3.PopulationModifier = 1.2m;
                            NewPlanet3.AgricultureModifier = 1.3m;
                            NewPlanet3.OreModifier = 0.02m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(12000, 25001);
                            NewPlanet3.AvailableOre = 0;
                            break;
                        case PlanetType.ClusterLevel3:
                            NewPlanet3.PopulationModifier = 1.3m;
                            NewPlanet3.AgricultureModifier = 1.45m;
                            NewPlanet3.OreModifier = 0.02m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(55000, 125001);
                            NewPlanet3.AvailableOre = 0;
                            break;
                        default:
                            NewPlanet3.PopulationModifier = 1m;
                            NewPlanet3.AgricultureModifier = 1m;
                            NewPlanet3.OreModifier = 1m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(2, 4);
                            NewPlanet3.AvailableOre = 0;
                            break;
                    }
                    // Calculate LandAvailable
                    NewPlanet3.LandAvailable = NewPlanet3.TotalLand - (NewPlanet3.Housing + NewPlanet3.Commercial + NewPlanet3.Industry + NewPlanet3.Agriculture + NewPlanet3.Mining);
                    // Add planets to list
                    DefenderPlanets.Add(NewPlanet);
                    DefenderPlanets.Add(NewPlanet2);
                    DefenderPlanets.Add(NewPlanet3);
                }
                else
                {
                    DefenderPlanets = _context.Planets.Where(p => p.ApplicationUserId == TargetUserFleets.FirstOrDefault().ApplicationUserId && !p.Name.Contains("H.")).OrderBy(x => x.DateTimeAcquired).ToList();
                }
                if (DefenderPlanets.Count > 0)
                {
                    if (DefenderPlanets.Count == 1)
                    {
                        DefenderPlanets[0].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                        DefenderPlanets[0].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                        DefenderPlanets[0].DateTimeAcquired = DateTime.UtcNow;
                        currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                        _context.Planets.Update(DefenderPlanets[0]);
                    }
                    else if (DefenderPlanets.Count == 2)
                    {
                        DefenderPlanets[0].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                        DefenderPlanets[0].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                        DefenderPlanets[0].DateTimeAcquired = DateTime.UtcNow;
                        currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                        DefenderPlanets[1].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                        DefenderPlanets[1].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                        DefenderPlanets[1].DateTimeAcquired = DateTime.UtcNow;
                        currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                        _context.Planets.Update(DefenderPlanets[0]);
                        _context.Planets.Update(DefenderPlanets[1]);
                    }
                    else if (DefenderPlanets.Count > 2)
                    {
                        DefenderPlanets[0].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                        DefenderPlanets[0].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                        DefenderPlanets[0].DateTimeAcquired = DateTime.UtcNow;
                        currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                        DefenderPlanets[1].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                        DefenderPlanets[1].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                        DefenderPlanets[1].DateTimeAcquired = DateTime.UtcNow;
                        currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                        DefenderPlanets[2].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                        DefenderPlanets[2].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                        DefenderPlanets[2].DateTimeAcquired = DateTime.UtcNow;
                        currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                        _context.Planets.Update(DefenderPlanets[0]);
                        _context.Planets.Update(DefenderPlanets[1]);
                        _context.Planets.Update(DefenderPlanets[2]);
                    }
                }
                // Fleet Report + Important Events for attacker and defender
                EndFleets = $"<h3>{currentUser.UserName}'s Fleet</h3>";
                EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";


                foreach (var ship in AttackerFleet)
                {
                    int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                    EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                }
                EndFleets += "</table>";

                EndFleets += $"<h3>{targetUser.UserName}'s Fleet</h3>";
                EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                foreach (var ship in DefenderFleet)
                {
                    int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                    EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                }
                EndFleets += "</table>";
                if (DefenderPlanets.Count > 0)
                {
                    EndFleets += "<h3>Planets Won</h3>";
                    EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Type</th><th>Land</th></tr>";
                    foreach (var planet in DefenderPlanets)
                    {
                        EndFleets += $"<tr><td>{planet.Name}</td><td>{planet.Type}</td><td>{planet.TotalLand}</td></tr>";
                    }
                    EndFleets += "</table>";
                }
                else
                {
                    EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Type</th><th>Land</th></tr>";
                    EndFleets += "<tr><td colspan='3'>Defender has no planets</td></tr>"; ;
                    EndFleets += "/table>";
                }
                ImportantEvents attackerEvent = new ImportantEvents
                {
                    ApplicationUserId = currentUser.Id,
                    ImportantEventTypes = ImportantEventTypes.Battles,
                    Text = $"You have won the battle against {TargetUserName}.<br />{EndFleets}<br />",
                    DateAndTime = DateTime.Now
                };
                _context.ImportantEvents.Add(attackerEvent);
                if (targetUser.IsNPC != true)
                {
                    ImportantEvents DefenderEvent = new ImportantEvents
                    {
                        ApplicationUserId = targetUser.Id,
                        ImportantEventTypes = ImportantEventTypes.Battles,
                        Text = $"You have lost the battle against {currentUser.UserName}.<br />{EndFleets}<br />",
                        DateAndTime = DateTime.Now
                    };
                    _context.ImportantEvents.Add(DefenderEvent);
                }
                // battle logs
                BattleLogs AttackerBattleLog = new BattleLogs
                {
                    ApplicationUserId = currentUser.Id,
                    Attacker = currentUser.UserName,
                    Defender = targetUser.UserName,
                    DateAndTime = DateTime.Now,
                    Outcome = $"Win",
                    FleetReport = EndFleets
                };
                _context.Battlelogs.Add(AttackerBattleLog);
                if (targetUser.IsNPC != true)
                {
                    BattleLogs DefenderBattleLog = new BattleLogs
                    {
                        ApplicationUserId = targetUser.Id,
                        Attacker = currentUser.UserName,
                        Defender = targetUser.UserName,
                        DateAndTime = DateTime.Now,
                        Outcome = $"Loss",
                        FleetReport = EndFleets
                    };
                    _context.Battlelogs.Add(DefenderBattleLog);
                }
                // update database for attacker and defenders fleets
                foreach (var fleet in AttackerFleet)
                {
                    var existingFleet = _context.Fleets.FirstOrDefault(f => f.ApplicationUserId == currentUser.Id && f.ShipId == fleet.ShipId);
                    existingFleet.TotalShips = fleet.TotalShips;
                    existingFleet.TotalPowerRating = fleet.PowerRating * fleet.TotalShips;
                    existingFleet.TotalUpkeep = fleet.Upkeep * fleet.TotalShips;
                    if (existingFleet.TotalShips <= 0)
                    {
                        _context.Fleets.Remove(existingFleet); // Remove fleet if no ships left
                    }
                    else
                    {
                        _context.Fleets.Update(existingFleet);
                    }
                }
                if (targetUser.IsNPC != true)
                {
                    foreach (var fleet in DefenderFleet)
                    {
                        var existingFleet = _context.Fleets.FirstOrDefault(f => f.ApplicationUserId == targetUser.Id && f.ShipId == fleet.ShipId);
                        existingFleet.TotalShips = fleet.TotalShips;
                        existingFleet.TotalPowerRating = fleet.PowerRating * fleet.TotalShips;
                        existingFleet.TotalUpkeep = fleet.Upkeep * fleet.TotalShips;
                        _context.Fleets.Update(existingFleet);
                        if (existingFleet.TotalShips <= 0)
                        {
                            _context.Fleets.Remove(existingFleet); // Remove fleet if no ships left
                        }
                        else
                        {
                            _context.Fleets.Update(existingFleet);
                        }
                    }
                }
                // Update Attackers Stats
                currentUser.BattlesWon++;
                // Update Defenders Stats + Damage Protection
                targetUser.BattlesLost++;
                if (DefenderPlanets.Count > 0)
                {
                    if (DefenderPlanets.Count == 1)
                    {
                        targetUser.TotalPlanets -= DefenderPlanets[0].TotalPlanets;
                        targetUser.TotalColonies -= 1;
                        targetUser.ColoniesLost += 1;
                        currentUser.ColoniesWon += 1;
                        currentUser.TotalColonies += 1;
                        currentUser.TotalPlanets += DefenderPlanets[0].TotalPlanets;

                    }
                    else if (DefenderPlanets.Count == 2)
                    {
                        targetUser.TotalPlanets -= DefenderPlanets[0].TotalPlanets + DefenderPlanets[1].TotalPlanets;
                        targetUser.TotalColonies -= 2;
                        targetUser.ColoniesLost += 2;
                        currentUser.ColoniesWon += 2;
                        currentUser.TotalColonies += 2;
                        currentUser.TotalPlanets += DefenderPlanets[0].TotalPlanets + DefenderPlanets[1].TotalPlanets;
                    }
                    else if (DefenderPlanets.Count > 2)
                    {
                        targetUser.TotalPlanets -= DefenderPlanets[0].TotalPlanets + DefenderPlanets[1].TotalPlanets + DefenderPlanets[2].TotalPlanets;
                        targetUser.TotalColonies -= 3;
                        targetUser.ColoniesLost += 3;
                        currentUser.ColoniesWon += 3;
                        currentUser.TotalColonies += 3;
                        currentUser.TotalPlanets += DefenderPlanets[0].TotalPlanets + DefenderPlanets[1].TotalPlanets + DefenderPlanets[2].TotalPlanets;
                    }
                }
                if (targetUser.IsNPC == true)
                {
                    targetUser.TotalPlanets = 1; // NPCs should always have at least one planet
                    targetUser.TotalColonies = 1; // NPCs should always have at least one colony
                    targetUser.DamageProtection = DateTime.Now.AddMinutes(15); // Reset damage protection for defender
                }
                else
                {
                    targetUser.DamageProtection = DateTime.Now.AddDays(1); // Reset damage protection for defender
                }
                var turnsMessage = await _turnService.TryUseTurnsAsync(currentUser.Id, 5);
                StatusMessage = turnsMessage.Message;
                VictoryMessage = $"You have won the battle against {TargetUserName}";
                _context.SaveChanges();
                return Page();
            }
            if (DefenderTotalPowerRatingLoss <= AttackerTotalPowerRatingLoss)
            {
                // Defender wins by default
                // Fleet Report + Important Events for attacker and defender
                EndFleets = $"<h3>{currentUser.UserName}'s Fleet</h3>";
                EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                foreach (var ship in AttackerFleet)
                {
                    int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                    EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                }
                EndFleets += "</table>";

                EndFleets += $"<h3>{targetUser.UserName}'s Fleet</h3>";
                EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                foreach (var ship in DefenderFleet)
                {
                    int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                    EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                }
                EndFleets += "</table>";
                ImportantEvents attackerEvent = new ImportantEvents
                {
                    ApplicationUserId = currentUser.Id,
                    ImportantEventTypes = ImportantEventTypes.Battles,
                    Text = $"You have Lost the battle against {TargetUserName}.<br />{EndFleets}<br />",
                    DateAndTime = DateTime.Now
                };
                _context.ImportantEvents.Add(attackerEvent);
                if (targetUser.IsNPC != true)
                {
                    ImportantEvents DefenderEvent = new ImportantEvents
                    {
                        ApplicationUserId = targetUser.Id,
                        ImportantEventTypes = ImportantEventTypes.Battles,
                        Text = $"You have Won the battle against {currentUser.UserName}.<br />{EndFleets}<br />",
                        DateAndTime = DateTime.Now
                    };
                    _context.ImportantEvents.Add(DefenderEvent);
                }
                // battle logs
                BattleLogs AttackerBattleLog = new BattleLogs
                {
                    ApplicationUserId = currentUser.Id,
                    Attacker = currentUser.UserName,
                    Defender = targetUser.UserName,
                    DateAndTime = DateTime.Now,
                    Outcome = $"Loss",
                    FleetReport = EndFleets
                };
                _context.Battlelogs.Add(AttackerBattleLog);
                if (targetUser.IsNPC != true)
                {
                    BattleLogs DefenderBattleLog = new BattleLogs
                    {
                        ApplicationUserId = targetUser.Id,
                        Attacker = currentUser.UserName,
                        Defender = targetUser.UserName,
                        DateAndTime = DateTime.Now,
                        Outcome = $"Win",
                        FleetReport = EndFleets
                    };
                    _context.Battlelogs.Add(DefenderBattleLog);
                }
                // update database for attacker and defenders fleets
                foreach (var fleet in AttackerFleet)
                {
                    var existingFleet = _context.Fleets.FirstOrDefault(f => f.ApplicationUserId == currentUser.Id && f.ShipId == fleet.ShipId);
                    existingFleet.TotalShips = fleet.TotalShips;
                    existingFleet.TotalPowerRating = fleet.PowerRating * fleet.TotalShips;
                    existingFleet.TotalUpkeep = fleet.Upkeep * fleet.TotalShips;
                    if (existingFleet.TotalShips <= 0)
                    {
                        _context.Fleets.Remove(existingFleet); // Remove fleet if no ships left
                    }
                    else
                    {
                        _context.Fleets.Update(existingFleet);
                    }
                }
                if (targetUser.IsNPC != true)
                {
                    foreach (var fleet in DefenderFleet)
                    {
                        var existingFleet = _context.Fleets.FirstOrDefault(f => f.ApplicationUserId == targetUser.Id && f.ShipId == fleet.ShipId);
                        existingFleet.TotalShips = fleet.TotalShips;
                        existingFleet.TotalPowerRating = fleet.PowerRating * fleet.TotalShips;
                        existingFleet.TotalUpkeep = fleet.Upkeep * fleet.TotalShips;
                        _context.Fleets.Update(existingFleet);
                        if (existingFleet.TotalShips <= 0)
                        {
                            _context.Fleets.Remove(existingFleet); // Remove fleet if no ships left
                        }
                        else
                        {
                            _context.Fleets.Update(existingFleet);
                        }
                    }
                }
                // Update Attackers Stats
                currentUser.BattlesLost += 1;
                // Update Defenders Stats
                targetUser.BattlesWon++;
                if (targetUser.IsNPC == true)
                {
                    targetUser.TotalPlanets = 1; // NPCs should always have at least one planet
                    targetUser.TotalColonies = 1; // NPCs should always have at least one colony
                    targetUser.DamageProtection = DateTime.Now.AddMinutes(15); // Reset damage protection for defender
                }
                else
                {
                    targetUser.DamageProtection = DateTime.Now.AddDays(1); // Reset damage protection for defender
                }
                var turnsMessage = await _turnService.TryUseTurnsAsync(currentUser.Id, 5);
                StatusMessage = turnsMessage.Message;
                VictoryMessage = $"You have lost the battle against {TargetUserName}";
                _context.SaveChanges();
                return Page();
            }
            else
            {
                // Attacker wins by default
                // Sort Planets For Capture
                List<Planets> DefenderPlanets = new List<Planets>();
                if (targetUser.IsNPC == true)
                {
                    var planetTypeWeights = new List<(PlanetType Type, double Weight)>
                            {
                               (PlanetType.Barren, 5),
                               (PlanetType.Icy, 5),
                               (PlanetType.Marshy, 5),
                               (PlanetType.Forest, 5),
                               (PlanetType.Oceanic, 5),
                               (PlanetType.Rocky, 5),
                               (PlanetType.Desert, 5),
                               (PlanetType.Balanced, 5),
                               (PlanetType.Gas, 5),
                               (PlanetType.URich, 0.5),
                               (PlanetType.UEden, 0.5),
                               (PlanetType.USpazial, 0.5),
                               (PlanetType.ULarge, 0.5),
                               (PlanetType.UFertile, 0.5),
                               (PlanetType.Dead, 0.5),
                               (PlanetType.ClusterLevel1, 0.25),
                               (PlanetType.ClusterLevel2, 0.10),
                               (PlanetType.ClusterLevel3, 0.05)
                            };
                    var random = new System.Random();
                    var chosenType = GetRandomPlanetTypeWeighted(planetTypeWeights);
                    //planet 1
                    var NewPlanet = new Planets
                    {
                        ApplicationUserId = user.Id,
                        ApplicationUser = user,
                        Name = "E." + random.Next(1000, 9999).ToString(),
                        Type = chosenType,
                        FoodRequired = 1,
                        GoodsRequired = 1,
                        CurrentPopulation = 10,
                        MaxPopulation = 10,
                        Loyalty = 2500,
                        AvailableLabour = 8,
                        Housing = 1,
                        Commercial = 0,
                        Industry = 0,
                        Agriculture = 0,
                        Mining = 1,
                        MineralProduced = 0,
                        PowerRating = 0

                    };
                    // Set type-specific values
                    switch (chosenType)
                    {
                        case PlanetType.Barren:
                            NewPlanet.PopulationModifier = 0.5m;
                            NewPlanet.AgricultureModifier = 0m;
                            NewPlanet.OreModifier = 0.02m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.AvailableOre = random.Next(150, 500);
                            NewPlanet.TotalLand = random.Next(50, 1100);
                            break;
                        case PlanetType.Icy:
                            NewPlanet.PopulationModifier = 0.75m;
                            NewPlanet.AgricultureModifier = 1m;
                            NewPlanet.OreModifier = 0.005m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.AvailableOre = random.Next(1500, 4500);
                            NewPlanet.TotalLand = random.Next(24, 83);
                            break;
                        case PlanetType.Marshy:
                            NewPlanet.PopulationModifier = 0.8m;
                            NewPlanet.AgricultureModifier = 0.5m;
                            NewPlanet.OreModifier = 0.005m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(50, 150);
                            NewPlanet.AvailableOre = random.Next(0, 150);
                            break;
                        case PlanetType.Forest:
                            NewPlanet.PopulationModifier = 0.9m;
                            NewPlanet.AgricultureModifier = 2m;
                            NewPlanet.OreModifier = 0.005m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(50, 350);
                            NewPlanet.AvailableOre = random.Next(0, 50);
                            break;
                        case PlanetType.Oceanic:
                            NewPlanet.PopulationModifier = 0.8m;
                            NewPlanet.AgricultureModifier = 0m;
                            NewPlanet.OreModifier = 0.005m;
                            NewPlanet.ArtifactModifier = 0.10m;
                            NewPlanet.TotalLand = random.Next(10, 50);
                            NewPlanet.AvailableOre = 0;
                            break;
                        case PlanetType.Rocky:
                            NewPlanet.PopulationModifier = 0.75m;
                            NewPlanet.AgricultureModifier = 1m;
                            NewPlanet.OreModifier = 0.001m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(34, 121);
                            NewPlanet.AvailableOre = random.Next(500, 3300);
                            break;
                        case PlanetType.Desert:
                            NewPlanet.PopulationModifier = 0.75m;
                            NewPlanet.AgricultureModifier = 0.75m;
                            NewPlanet.OreModifier = 0.02m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(100, 250);
                            NewPlanet.AvailableOre = random.Next(100, 350);
                            break;
                        case PlanetType.Balanced:
                            NewPlanet.PopulationModifier = 1.2m;
                            NewPlanet.AgricultureModifier = 1m;
                            NewPlanet.OreModifier = 0.01m;
                            NewPlanet.ArtifactModifier = 0.05m;
                            NewPlanet.TotalLand = random.Next(185, 1050);
                            NewPlanet.AvailableOre = random.Next(750, 1100);
                            break;
                        case PlanetType.Gas:
                            NewPlanet.PopulationModifier = 1m;
                            NewPlanet.AgricultureModifier = 0m;
                            NewPlanet.OreModifier = 0m;
                            NewPlanet.ArtifactModifier = 0.05m;
                            NewPlanet.TotalLand = random.Next(2, 6);
                            NewPlanet.AvailableOre = 0;
                            break;
                        case PlanetType.URich:
                            NewPlanet.PopulationModifier = 0.1m;
                            NewPlanet.AgricultureModifier = 0m;
                            NewPlanet.OreModifier = 0.03m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(11, 28);
                            NewPlanet.AvailableOre = random.Next(50000, 350000);
                            break;
                        case PlanetType.UEden:
                            NewPlanet.PopulationModifier = 10m;
                            NewPlanet.AgricultureModifier = 0.02m;
                            NewPlanet.OreModifier = 0m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(500, 2500);
                            NewPlanet.AvailableOre = 0;
                            break;
                        case PlanetType.USpazial:
                            NewPlanet.PopulationModifier = 0.1m;
                            NewPlanet.AgricultureModifier = 0m;
                            NewPlanet.OreModifier = 0m;
                            NewPlanet.ArtifactModifier = 0.15m;
                            NewPlanet.TotalLand = random.Next(2, 4);
                            NewPlanet.AvailableOre = 0;
                            break;
                        case PlanetType.ULarge:
                            NewPlanet.PopulationModifier = 0.2m;
                            NewPlanet.AgricultureModifier = 0m;
                            NewPlanet.OreModifier = 0m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(7000, 16000);
                            NewPlanet.AvailableOre = 0;
                            break;
                        case PlanetType.UFertile:
                            NewPlanet.PopulationModifier = 0.5m;
                            NewPlanet.AgricultureModifier = 1.75m;
                            NewPlanet.OreModifier = 0m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(950, 2150);
                            NewPlanet.AvailableOre = 0;
                            break;
                        case PlanetType.Dead:
                            NewPlanet.PopulationModifier = 0.05m;
                            NewPlanet.AgricultureModifier = 0m;
                            NewPlanet.OreModifier = 0m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(2, 4);
                            NewPlanet.AvailableOre = 0;
                            break;
                        case PlanetType.ClusterLevel1:
                            NewPlanet.PopulationModifier = 1.1m;
                            NewPlanet.AgricultureModifier = 1.15m;
                            NewPlanet.OreModifier = 0.02m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(1200, 5001);
                            NewPlanet.AvailableOre = 0;
                            break;
                        case PlanetType.ClusterLevel2:
                            NewPlanet.PopulationModifier = 1.2m;
                            NewPlanet.AgricultureModifier = 1.3m;
                            NewPlanet.OreModifier = 0.02m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(12000, 25001);
                            NewPlanet.AvailableOre = 0;
                            break;
                        case PlanetType.ClusterLevel3:
                            NewPlanet.PopulationModifier = 1.3m;
                            NewPlanet.AgricultureModifier = 1.45m;
                            NewPlanet.OreModifier = 0.02m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(55000, 125001);
                            NewPlanet.AvailableOre = 0;
                            break;
                        default:
                            NewPlanet.PopulationModifier = 1m;
                            NewPlanet.AgricultureModifier = 1m;
                            NewPlanet.OreModifier = 1m;
                            NewPlanet.ArtifactModifier = 0.01m;
                            NewPlanet.TotalLand = random.Next(2, 4);
                            NewPlanet.AvailableOre = 0;
                            break;
                    }
                    // Calculate LandAvailable
                    NewPlanet.LandAvailable = NewPlanet.TotalLand - (NewPlanet.Housing + NewPlanet.Commercial + NewPlanet.Industry + NewPlanet.Agriculture + NewPlanet.Mining);
                    // planet 2
                    var NewPlanet2 = new Planets
                    {
                        ApplicationUserId = user.Id,
                        ApplicationUser = user,
                        Name = "E." + random.Next(1000, 9999).ToString(),
                        Type = chosenType,
                        FoodRequired = 1,
                        GoodsRequired = 1,
                        CurrentPopulation = 10,
                        MaxPopulation = 10,
                        Loyalty = 2500,
                        AvailableLabour = 8,
                        Housing = 1,
                        Commercial = 0,
                        Industry = 0,
                        Agriculture = 0,
                        Mining = 1,
                        MineralProduced = 0,
                        PowerRating = 0
                    };
                    // Set type-specific values
                    switch (chosenType)
                    {
                        case PlanetType.Barren:
                            NewPlanet2.PopulationModifier = 0.5m;
                            NewPlanet2.AgricultureModifier = 0m;
                            NewPlanet2.OreModifier = 0.02m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.AvailableOre = random.Next(150, 500);
                            NewPlanet2.TotalLand = random.Next(50, 1100);
                            break;
                        case PlanetType.Icy:
                            NewPlanet2.PopulationModifier = 0.75m;
                            NewPlanet2.AgricultureModifier = 1m;
                            NewPlanet2.OreModifier = 0.005m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.AvailableOre = random.Next(1500, 4500);
                            NewPlanet2.TotalLand = random.Next(24, 83);
                            break;
                        case PlanetType.Marshy:
                            NewPlanet2.PopulationModifier = 0.8m;
                            NewPlanet2.AgricultureModifier = 0.5m;
                            NewPlanet2.OreModifier = 0.005m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(50, 150);
                            NewPlanet2.AvailableOre = random.Next(0, 150);
                            break;
                        case PlanetType.Forest:
                            NewPlanet2.PopulationModifier = 0.9m;
                            NewPlanet2.AgricultureModifier = 2m;
                            NewPlanet2.OreModifier = 0.005m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(50, 350);
                            NewPlanet2.AvailableOre = random.Next(0, 50);
                            break;
                        case PlanetType.Oceanic:
                            NewPlanet2.PopulationModifier = 0.8m;
                            NewPlanet2.AgricultureModifier = 0m;
                            NewPlanet2.OreModifier = 0.005m;
                            NewPlanet2.ArtifactModifier = 0.10m;
                            NewPlanet2.TotalLand = random.Next(10, 50);
                            NewPlanet2.AvailableOre = 0;
                            break;
                        case PlanetType.Rocky:
                            NewPlanet2.PopulationModifier = 0.75m;
                            NewPlanet2.AgricultureModifier = 1m;
                            NewPlanet2.OreModifier = 0.001m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(34, 121);
                            NewPlanet2.AvailableOre = random.Next(500, 3300);
                            break;
                        case PlanetType.Desert:
                            NewPlanet2.PopulationModifier = 0.75m;
                            NewPlanet2.AgricultureModifier = 0.75m;
                            NewPlanet2.OreModifier = 0.02m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(100, 250);
                            NewPlanet2.AvailableOre = random.Next(100, 350);
                            break;
                        case PlanetType.Balanced:
                            NewPlanet2.PopulationModifier = 1.2m;
                            NewPlanet2.AgricultureModifier = 1m;
                            NewPlanet2.OreModifier = 0.01m;
                            NewPlanet2.ArtifactModifier = 0.05m;
                            NewPlanet2.TotalLand = random.Next(185, 1050);
                            NewPlanet2.AvailableOre = random.Next(750, 1100);
                            break;
                        case PlanetType.Gas:
                            NewPlanet2.PopulationModifier = 1m;
                            NewPlanet2.AgricultureModifier = 0m;
                            NewPlanet2.OreModifier = 0m;
                            NewPlanet2.ArtifactModifier = 0.05m;
                            NewPlanet2.TotalLand = random.Next(2, 6);
                            NewPlanet2.AvailableOre = 0;
                            break;
                        case PlanetType.URich:
                            NewPlanet2.PopulationModifier = 0.1m;
                            NewPlanet2.AgricultureModifier = 0m;
                            NewPlanet2.OreModifier = 0.03m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(11, 28);
                            NewPlanet2.AvailableOre = random.Next(50000, 350000);
                            break;
                        case PlanetType.UEden:
                            NewPlanet2.PopulationModifier = 10m;
                            NewPlanet2.AgricultureModifier = 0.02m;
                            NewPlanet2.OreModifier = 0m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(500, 2500);
                            NewPlanet2.AvailableOre = 0;
                            break;
                        case PlanetType.USpazial:
                            NewPlanet2.PopulationModifier = 0.1m;
                            NewPlanet2.AgricultureModifier = 0m;
                            NewPlanet2.OreModifier = 0m;
                            NewPlanet2.ArtifactModifier = 0.15m;
                            NewPlanet2.TotalLand = random.Next(2, 4);
                            NewPlanet2.AvailableOre = 0;
                            break;
                        case PlanetType.ULarge:
                            NewPlanet2.PopulationModifier = 0.2m;
                            NewPlanet2.AgricultureModifier = 0m;
                            NewPlanet2.OreModifier = 0m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(7000, 16000);
                            NewPlanet2.AvailableOre = 0;
                            break;
                        case PlanetType.UFertile:
                            NewPlanet2.PopulationModifier = 0.5m;
                            NewPlanet2.AgricultureModifier = 1.75m;
                            NewPlanet2.OreModifier = 0m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(950, 2150);
                            NewPlanet2.AvailableOre = 0;
                            break;
                        case PlanetType.Dead:
                            NewPlanet2.PopulationModifier = 0.05m;
                            NewPlanet2.AgricultureModifier = 0m;
                            NewPlanet2.OreModifier = 0m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(2, 4);
                            NewPlanet2.AvailableOre = 0;
                            break;
                        case PlanetType.ClusterLevel1:
                            NewPlanet2.PopulationModifier = 1.1m;
                            NewPlanet2.AgricultureModifier = 1.15m;
                            NewPlanet2.OreModifier = 0.02m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(1200, 5001);
                            NewPlanet2.AvailableOre = 0;
                            break;
                        case PlanetType.ClusterLevel2:
                            NewPlanet2.PopulationModifier = 1.2m;
                            NewPlanet2.AgricultureModifier = 1.3m;
                            NewPlanet2.OreModifier = 0.02m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(12000, 25001);
                            NewPlanet2.AvailableOre = 0;
                            break;
                        case PlanetType.ClusterLevel3:
                            NewPlanet2.PopulationModifier = 1.3m;
                            NewPlanet2.AgricultureModifier = 1.45m;
                            NewPlanet2.OreModifier = 0.02m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(55000, 125001);
                            NewPlanet2.AvailableOre = 0;
                            break;
                        default:
                            NewPlanet2.PopulationModifier = 1m;
                            NewPlanet2.AgricultureModifier = 1m;
                            NewPlanet2.OreModifier = 1m;
                            NewPlanet2.ArtifactModifier = 0.01m;
                            NewPlanet2.TotalLand = random.Next(2, 4);
                            NewPlanet2.AvailableOre = 0;
                            break;
                    }
                    // Calculate LandAvailable
                    NewPlanet2.LandAvailable = NewPlanet2.TotalLand - (NewPlanet2.Housing + NewPlanet2.Commercial + NewPlanet2.Industry + NewPlanet2.Agriculture + NewPlanet2.Mining);
                    // planet 3
                    var NewPlanet3 = new Planets
                    {
                        ApplicationUserId = user.Id,
                        ApplicationUser = user,
                        Name = "E." + random.Next(1000, 9999).ToString(),
                        Type = chosenType,
                        FoodRequired = 1,
                        GoodsRequired = 1,
                        CurrentPopulation = 10,
                        MaxPopulation = 10,
                        Loyalty = 2500,
                        AvailableLabour = 8,
                        Housing = 1,
                        Commercial = 0,
                        Industry = 0,
                        Agriculture = 0,
                        Mining = 1,
                        MineralProduced = 0,
                        PowerRating = 0
                    };
                    // Set type-specific values
                    switch (chosenType)
                    {
                        case PlanetType.Barren:
                            NewPlanet3.PopulationModifier = 0.5m;
                            NewPlanet3.AgricultureModifier = 0m;
                            NewPlanet3.OreModifier = 0.02m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.AvailableOre = random.Next(150, 500);
                            NewPlanet3.TotalLand = random.Next(50, 1100);
                            break;
                        case PlanetType.Icy:
                            NewPlanet3.PopulationModifier = 0.75m;
                            NewPlanet3.AgricultureModifier = 1m;
                            NewPlanet3.OreModifier = 0.005m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.AvailableOre = random.Next(1500, 4500);
                            NewPlanet3.TotalLand = random.Next(24, 83);
                            break;
                        case PlanetType.Marshy:
                            NewPlanet3.PopulationModifier = 0.8m;
                            NewPlanet3.AgricultureModifier = 0.5m;
                            NewPlanet3.OreModifier = 0.005m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(50, 150);
                            NewPlanet3.AvailableOre = random.Next(0, 150);
                            break;
                        case PlanetType.Forest:
                            NewPlanet3.PopulationModifier = 0.9m;
                            NewPlanet3.AgricultureModifier = 2m;
                            NewPlanet3.OreModifier = 0.005m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(50, 350);
                            NewPlanet3.AvailableOre = random.Next(0, 50);
                            break;
                        case PlanetType.Oceanic:
                            NewPlanet3.PopulationModifier = 0.8m;
                            NewPlanet3.AgricultureModifier = 0m;
                            NewPlanet3.OreModifier = 0.005m;
                            NewPlanet3.ArtifactModifier = 0.10m;
                            NewPlanet3.TotalLand = random.Next(10, 50);
                            NewPlanet3.AvailableOre = 0;
                            break;
                        case PlanetType.Rocky:
                            NewPlanet3.PopulationModifier = 0.75m;
                            NewPlanet3.AgricultureModifier = 1m;
                            NewPlanet3.OreModifier = 0.001m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(34, 121);
                            NewPlanet3.AvailableOre = random.Next(500, 3300);
                            break;
                        case PlanetType.Desert:
                            NewPlanet3.PopulationModifier = 0.75m;
                            NewPlanet3.AgricultureModifier = 0.75m;
                            NewPlanet3.OreModifier = 0.02m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(100, 250);
                            NewPlanet3.AvailableOre = random.Next(100, 350);
                            break;
                        case PlanetType.Balanced:
                            NewPlanet3.PopulationModifier = 1.2m;
                            NewPlanet3.AgricultureModifier = 1m;
                            NewPlanet3.OreModifier = 0.01m;
                            NewPlanet3.ArtifactModifier = 0.05m;
                            NewPlanet3.TotalLand = random.Next(185, 1050);
                            NewPlanet3.AvailableOre = random.Next(750, 1100);
                            break;
                        case PlanetType.Gas:
                            NewPlanet3.PopulationModifier = 1m;
                            NewPlanet3.AgricultureModifier = 0m;
                            NewPlanet3.OreModifier = 0m;
                            NewPlanet3.ArtifactModifier = 0.05m;
                            NewPlanet3.TotalLand = random.Next(2, 6);
                            NewPlanet3.AvailableOre = 0;
                            break;
                        case PlanetType.URich:
                            NewPlanet3.PopulationModifier = 0.1m;
                            NewPlanet3.AgricultureModifier = 0m;
                            NewPlanet3.OreModifier = 0.03m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(11, 28);
                            NewPlanet3.AvailableOre = random.Next(50000, 350000);
                            break;
                        case PlanetType.UEden:
                            NewPlanet3.PopulationModifier = 10m;
                            NewPlanet3.AgricultureModifier = 0.02m;
                            NewPlanet3.OreModifier = 0m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(500, 2500);
                            NewPlanet3.AvailableOre = 0;
                            break;
                        case PlanetType.USpazial:
                            NewPlanet3.PopulationModifier = 0.1m;
                            NewPlanet3.AgricultureModifier = 0m;
                            NewPlanet3.OreModifier = 0m;
                            NewPlanet3.ArtifactModifier = 0.15m;
                            NewPlanet3.TotalLand = random.Next(2, 4);
                            NewPlanet3.AvailableOre = 0;
                            break;
                        case PlanetType.ULarge:
                            NewPlanet3.PopulationModifier = 0.2m;
                            NewPlanet3.AgricultureModifier = 0m;
                            NewPlanet3.OreModifier = 0m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(7000, 16000);
                            NewPlanet3.AvailableOre = 0;
                            break;
                        case PlanetType.UFertile:
                            NewPlanet3.PopulationModifier = 0.5m;
                            NewPlanet3.AgricultureModifier = 1.75m;
                            NewPlanet3.OreModifier = 0m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(950, 2150);
                            NewPlanet3.AvailableOre = 0;
                            break;
                        case PlanetType.Dead:
                            NewPlanet3.PopulationModifier = 0.05m;
                            NewPlanet3.AgricultureModifier = 0m;
                            NewPlanet3.OreModifier = 0m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(2, 4);
                            NewPlanet3.AvailableOre = 0;
                            break;
                        case PlanetType.ClusterLevel1:
                            NewPlanet3.PopulationModifier = 1.1m;
                            NewPlanet3.AgricultureModifier = 1.15m;
                            NewPlanet3.OreModifier = 0.02m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(1200, 5001);
                            NewPlanet3.AvailableOre = 0;
                            break;
                        case PlanetType.ClusterLevel2:
                            NewPlanet3.PopulationModifier = 1.2m;
                            NewPlanet3.AgricultureModifier = 1.3m;
                            NewPlanet3.OreModifier = 0.02m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(12000, 25001);
                            NewPlanet3.AvailableOre = 0;
                            break;
                        case PlanetType.ClusterLevel3:
                            NewPlanet3.PopulationModifier = 1.3m;
                            NewPlanet3.AgricultureModifier = 1.45m;
                            NewPlanet3.OreModifier = 0.02m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(55000, 125001);
                            NewPlanet3.AvailableOre = 0;
                            break;
                        default:
                            NewPlanet3.PopulationModifier = 1m;
                            NewPlanet3.AgricultureModifier = 1m;
                            NewPlanet3.OreModifier = 1m;
                            NewPlanet3.ArtifactModifier = 0.01m;
                            NewPlanet3.TotalLand = random.Next(2, 4);
                            NewPlanet3.AvailableOre = 0;
                            break;
                    }
                    // Calculate LandAvailable
                    NewPlanet3.LandAvailable = NewPlanet3.TotalLand - (NewPlanet3.Housing + NewPlanet3.Commercial + NewPlanet3.Industry + NewPlanet3.Agriculture + NewPlanet3.Mining);
                    // Add planets to list
                    DefenderPlanets.Add(NewPlanet);
                    DefenderPlanets.Add(NewPlanet2);
                    DefenderPlanets.Add(NewPlanet3);
                }
                else
                {
                    DefenderPlanets = _context.Planets.Where(p => p.ApplicationUserId == TargetUserFleets.FirstOrDefault().ApplicationUserId && !p.Name.Contains("H.")).OrderBy(x => x.DateTimeAcquired).ToList();
                }
                if (DefenderPlanets.Count > 0)
                {
                    if (DefenderPlanets.Count == 1)
                    {
                        DefenderPlanets[0].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                        DefenderPlanets[0].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                        DefenderPlanets[0].DateTimeAcquired = DateTime.UtcNow;
                        currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                        _context.Planets.Update(DefenderPlanets[0]);
                    }
                    else if (DefenderPlanets.Count == 2)
                    {
                        DefenderPlanets[0].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                        DefenderPlanets[0].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                        DefenderPlanets[0].DateTimeAcquired = DateTime.UtcNow;
                        currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                        DefenderPlanets[1].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                        DefenderPlanets[1].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                        DefenderPlanets[1].DateTimeAcquired = DateTime.UtcNow;
                        currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                        _context.Planets.Update(DefenderPlanets[0]);
                        _context.Planets.Update(DefenderPlanets[1]);
                    }
                    else if (DefenderPlanets.Count > 2)
                    {
                        DefenderPlanets[0].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                        DefenderPlanets[0].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                        DefenderPlanets[0].DateTimeAcquired = DateTime.UtcNow;
                        currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                        DefenderPlanets[1].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                        DefenderPlanets[1].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                        DefenderPlanets[1].DateTimeAcquired = DateTime.UtcNow;
                        currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                        DefenderPlanets[2].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                        DefenderPlanets[2].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                        DefenderPlanets[2].DateTimeAcquired = DateTime.UtcNow;
                        currentUser.Exploration.ExplorationPointsNeeded = (int)(currentUser.Exploration.ExplorationPointsNeeded * 1.2);
                        _context.Planets.Update(DefenderPlanets[0]);
                        _context.Planets.Update(DefenderPlanets[1]);
                        _context.Planets.Update(DefenderPlanets[2]);
                    }
                }
                // Fleet Report + Important Events for attacker and defender
                EndFleets = $"<h3>{currentUser.UserName}'s Fleet</h3>";
                EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                foreach (var ship in AttackerFleet)
                {
                    int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                    EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                }
                EndFleets += "</table>";

                EndFleets += $"<h3>{targetUser.UserName}'s Fleet</h3>";
                EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                foreach (var ship in DefenderFleet)
                {
                    int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                    EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                }
                EndFleets += "</table>";
                if (DefenderPlanets.Count > 0)
                {
                    EndFleets += "<h3>Planets Won</h3>";
                    EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Type</th><th>Land</th></tr>";
                    foreach (var planet in DefenderPlanets)
                    {
                        EndFleets += $"<tr><td>{planet.Name}</td><td>{planet.Type}</td><td>{planet.TotalLand}</td></tr>";
                    }
                    EndFleets += "</table>";
                }
                else
                {
                    EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Type</th><th>Land</th></tr>";
                    EndFleets += "<tr><td colspan='3'>Defender has no planets</td></tr>";
                    EndFleets += "/table>";
                }
                ImportantEvents attackerEvent = new ImportantEvents
                {
                    ApplicationUserId = currentUser.Id,
                    ImportantEventTypes = ImportantEventTypes.Battles,
                    Text = $"You have won the battle against {TargetUserName}.<br />{EndFleets}<br />",
                    DateAndTime = DateTime.Now
                };
                _context.ImportantEvents.Add(attackerEvent);
                if (targetUser.IsNPC != true)
                {
                    ImportantEvents DefenderEvent = new ImportantEvents
                    {
                        ApplicationUserId = targetUser.Id,
                        ImportantEventTypes = ImportantEventTypes.Battles,
                        Text = $"You have lost the battle against {currentUser.UserName}.<br />{EndFleets}<br />",
                        DateAndTime = DateTime.Now
                    };
                    _context.ImportantEvents.Add(DefenderEvent);
                }
                // battle logs
                BattleLogs AttackerBattleLog = new BattleLogs
                {
                    ApplicationUserId = currentUser.Id,
                    Attacker = currentUser.UserName,
                    Defender = targetUser.UserName,
                    DateAndTime = DateTime.Now,
                    Outcome = $"Win",
                    FleetReport = EndFleets
                };
                _context.Battlelogs.Add(AttackerBattleLog);
                if (targetUser.IsNPC != true)
                {
                    BattleLogs DefenderBattleLog = new BattleLogs
                    {
                        ApplicationUserId = targetUser.Id,
                        Attacker = currentUser.UserName,
                        Defender = targetUser.UserName,
                        DateAndTime = DateTime.Now,
                        Outcome = $"Loss",
                        FleetReport = EndFleets
                    };
                    _context.Battlelogs.Add(DefenderBattleLog);
                }
                // update database for attacker and defenders fleets
                foreach (var fleet in AttackerFleet)
                {
                    var existingFleet = _context.Fleets.FirstOrDefault(f =>  f.ApplicationUserId == currentUser.Id && f.ShipId == fleet.ShipId);
                    existingFleet.TotalShips = fleet.TotalShips;
                    existingFleet.TotalPowerRating = fleet.PowerRating * fleet.TotalShips;
                    existingFleet.TotalUpkeep = fleet.Upkeep * fleet.TotalShips;
                    if (existingFleet.TotalShips <= 0)
                    {
                        _context.Fleets.Remove(existingFleet); // Remove fleet if no ships left
                    }
                    else
                    {
                        _context.Fleets.Update(existingFleet);
                    }
                }
                if (targetUser.IsNPC != true)
                {
                    foreach (var fleet in DefenderFleet)
                    {
                        var existingFleet = _context.Fleets.FirstOrDefault(f => f.ApplicationUserId == targetUser.Id && f.ShipId == fleet.ShipId);
                        existingFleet.TotalShips = fleet.TotalShips;
                        existingFleet.TotalPowerRating = fleet.PowerRating * fleet.TotalShips;
                        existingFleet.TotalUpkeep = fleet.Upkeep * fleet.TotalShips;
                        _context.Fleets.Update(existingFleet);
                        if (existingFleet.TotalShips <= 0)
                        {
                            _context.Fleets.Remove(existingFleet); // Remove fleet if no ships left
                        }
                        else
                        {
                            _context.Fleets.Update(existingFleet);
                        }
                    }
                }
                // Update Attackers Stats
                currentUser.BattlesWon++;
                // Update Defenders Stats + Damage Protection
                targetUser.BattlesLost++;
                if (DefenderPlanets.Count > 0)
                {
                    if (DefenderPlanets.Count == 1)
                    {

                        targetUser.TotalPlanets -= DefenderPlanets[0].TotalPlanets;
                        targetUser.TotalColonies -= 1;
                        targetUser.ColoniesLost += 1;
                        currentUser.ColoniesWon += 1;
                        currentUser.TotalColonies += 1;
                        currentUser.TotalPlanets += DefenderPlanets[0].TotalPlanets;

                    }
                    else if (DefenderPlanets.Count == 2)
                    {

                        targetUser.TotalPlanets -= DefenderPlanets[0].TotalPlanets + DefenderPlanets[1].TotalPlanets;
                        targetUser.TotalColonies -= 2;
                        targetUser.ColoniesLost += 2;
                        currentUser.ColoniesWon += 2;
                        currentUser.TotalColonies += 2;
                        currentUser.TotalPlanets += DefenderPlanets[0].TotalPlanets + DefenderPlanets[1].TotalPlanets;
                    }
                    else if (DefenderPlanets.Count > 2)
                    {

                        targetUser.TotalPlanets -= DefenderPlanets[0].TotalPlanets + DefenderPlanets[1].TotalPlanets + DefenderPlanets[2].TotalPlanets;
                        targetUser.TotalColonies -= 3;
                        targetUser.ColoniesLost += 3;
                        currentUser.ColoniesWon += 3;
                        currentUser.TotalColonies += 3;
                        currentUser.TotalPlanets += DefenderPlanets[0].TotalPlanets + DefenderPlanets[1].TotalPlanets + DefenderPlanets[2].TotalPlanets;
                    }
                }
                if (targetUser.IsNPC == true)
                {
                    targetUser.TotalPlanets = 1; // NPCs should always have at least one planet
                    targetUser.TotalColonies = 1; // NPCs should always have at least one colony
                    targetUser.DamageProtection = DateTime.Now.AddMinutes(15); // Reset damage protection for defender
                }
                else
                {
                    targetUser.DamageProtection = DateTime.Now.AddDays(1); // Reset damage protection for defender
                }
                var turnsMessage = await _turnService.TryUseTurnsAsync(currentUser.Id, 5);
                StatusMessage = turnsMessage.Message;
                VictoryMessage = $"You have won the battle against {TargetUserName}";
                _context.SaveChanges();
                return Page();
            }
        }


        private (double AttackerMod, double DefenderMod) GetStanceModifier(AttackType type)
        {
            return type switch
            {
                AttackType.Careful => (0.5, 0.5),
                AttackType.Normal => (0.95, 1.0),
                AttackType.Aggressive => (1.74, 1.99),
                _ => (0.95, 1.0)
            };
        }
        private (int DamageDealt, int ShipsKilled) CalulateAttackerDamage(MergedFleet Attacker, MergedFleet Defender, bool RetalPhase)
        {
            var Damage = (Attacker.TotalShips * Attacker.EnergyWeapon) * GetStanceModifier(AttackType).AttackerMod * (double)Defender.EnergyShield
            + (Attacker.TotalShips * Attacker.KineticWeapon) * GetStanceModifier(AttackType).AttackerMod * (double)Defender.KineticShield
            + (Attacker.TotalShips * Attacker.MissileWeapon) * GetStanceModifier(AttackType).AttackerMod * (double)Defender.MissileShield
            + (Attacker.TotalShips * Attacker.ChemicalWeapon) * GetStanceModifier(AttackType).AttackerMod * (double)Defender.ChemicalShield;
            // Placeholder for damage calculation logic
            var shipsKilled = 0;
            if (RetalPhase == true)
            {
                shipsKilled = (int)Math.Floor((Damage / 2) / Defender.Hull);                
            }
            else
            {
                shipsKilled = (int)Math.Floor(Damage / Defender.Hull);
            }
            if(shipsKilled > Defender.TotalShips)
            {
                shipsKilled = Defender.TotalShips;
            }
            return ((int)Math.Floor(Damage), shipsKilled);
        }
        private (int DamageDealt, int ShipsKilled) CalulateDefenderDamage(MergedFleet Attacker, MergedFleet Defender, bool RetalPhase)
        {
            var Damage = (Defender.TotalShips * Defender.EnergyWeapon) * GetStanceModifier(AttackType).DefenderMod * (double)Attacker.EnergyShield
            + (Defender.TotalShips * Defender.KineticWeapon) * GetStanceModifier(AttackType).DefenderMod * (double)Attacker.KineticShield
            + (Defender.TotalShips * Defender.MissileWeapon) * GetStanceModifier(AttackType).DefenderMod * (double)Attacker.MissileShield
            + (Defender.TotalShips * Defender.ChemicalWeapon) * GetStanceModifier(AttackType).DefenderMod * (double)Attacker.ChemicalShield;
            // Placeholder for damage calculation logic
            var shipsKilled = 0;
            if (RetalPhase == true)
            {
                shipsKilled = (int)Math.Floor((Damage / 2) / Attacker.Hull);
            }
            else
            {
                shipsKilled = (int)Math.Floor(Damage / Attacker.Hull);
            }
            if (shipsKilled > Attacker.TotalShips)
            {
                shipsKilled = Attacker.TotalShips;
            }
            return ((int)Math.Floor(Damage), shipsKilled);
        }
        private PlanetType GetRandomPlanetTypeWeighted(List<(PlanetType Type, double Weight)> weights)
        {
            var totalWeight = weights.Sum(w => w.Weight);
            var rand = new Random();
            var roll = rand.NextDouble() * totalWeight;
            double cumulative = 0;
            foreach (var (type, weight) in weights)
            {
                cumulative += weight;
                if (roll < cumulative)
                    return type;
            }
            // Fallback (should not happen)
            return weights.Last().Type;
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
public class BattleResultWave1
{
    public bool IsAttacker { get; set; } // Indicates if the current user is the attacker
    public string AttackerShipName { get; set; }
    public string DefenderShipName { get; set; }
    public int AttackerDamageDelt { get; set; }
    public int DefenderDamageDelt { get; set; }
    public int AttackerShipTotalLoss { get; set; }
    public int DefenderShipTotalLoss { get; set; }
    public string? AttackerCapture { get; set; } // Capture chance for attacker ship
    public string? DefenderCapture { get; set; } // Capture chance for defender ship
}
public class BattleResultWave2
{
    public bool IsAttacker { get; set; } // Indicates if the current user is the attacker
    public string AttackerShipName { get; set; }
    public string DefenderShipName { get; set; }
    public int AttackerDamageDelt { get; set; }
    public int DefenderDamageDelt { get; set; }
    public int AttackerShipTotalLoss { get; set; }
    public int DefenderShipTotalLoss { get; set; }
    public string? AttackerCapture { get; set; } // Capture chance for attacker ship
    public string? DefenderCapture { get; set; } // Capture chance for defender ship
}

public class MergedFleet
{
    public int Id { get; set; } // Primary key for EF Core

    // Foreign key to ApplicationUser
    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public string UserName { get; set; } // Username for display
    public int ShipId { get; set; }
    public int TotalShips { get; set; }
    public int TotalPowerRating { get; set; }
    public int TotalUpkeep { get; set; }

    [Description("Ship Name")]
    public string ShipName { get; set; }

    [Description("Ship Type")]
    public ShipType ShipType { get; set; }

    [Description("Power Rating")]
    public int PowerRating { get; set; }

    [Description("Range")]
    public int Range { get; set; }

    [Description("Weapon")]
    public int Weapon { get; set; }

    [Description("Hull")]
    public int Hull { get; set; }

    [Description("Energy Weapon")]
    public int EnergyWeapon { get; set; }

    [Description("Kinetic Weapon")]
    public int KineticWeapon { get; set; }

    [Description("Missile Weapon")]
    public int MissileWeapon { get; set; }

    [Description("Chemical Weapon")]
    public int ChemicalWeapon { get; set; }

    [Description("Energy Shield")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal EnergyShield { get; set; }

    [Description("Kinetic Shield")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal KineticShield { get; set; }

    [Description("Missile Shield")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal MissileShield { get; set; }

    [Description("Chemical Shield")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal ChemicalShield { get; set; }

    [Description("No Defense")]
    public bool NoDefense { get; set; }

    [Description("No Retal")]
    public bool NoRetal { get; set; }

    [Description("Cap Chance")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal CapChance { get; set; }

    [Description("Cost")]
    public int Cost { get; set; }

    [Description("Upkeep")]
    public int Upkeep { get; set; }

    [Description("Terran Metal")]
    public int TerranMetal { get; set; }

    [Description("Red Crystal")]
    public int RedCrystal { get; set; }

    [Description("White Crystal")]
    public int WhiteCrystal { get; set; }

    [Description("Rutile")]
    public int Rutile { get; set; }

    [Description("Composite")]
    public int Composite { get; set; }

    [Description("Strafez Organism")]
    public int StrafezOrganism { get; set; }

    [Description("Scanning Power")]
    public int ScanningPower { get; set; }
    [Description("Build Rate")]
    public int BuildRate { get; set; }
    [Description("Cost To Build")]
    public int CostToBuild { get; set; }
    [Description("Immune to Capture")]
    public bool ImmuneToCapture { get; set; }
    [Description("CanCapture")]
    public bool CanCapture { get; set; }
    public int TotalShipsStart { get; set; } // Used to store the initial number of ships for the battle report
}