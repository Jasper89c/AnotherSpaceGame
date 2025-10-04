using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class BattleSimModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public string StatusMessage { get; set; } = string.Empty;
        public BattleSimModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool IsBattleSimulated { get; set; }
        public List<Faction> Factions { get; set; }

        [BindProperty(SupportsGet = true)]
        public int AttackerFactionId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int DefenderFactionId { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<MergedFleet>? AttackerInputFleet { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<MergedFleet>? DefenderInputFleet { get; set; }
        // wave stuff
        [BindProperty(SupportsGet = true)]
        public AttackType AttackType { get; set; }
        public List<SelectListItem> AttackTypeList { get; set; } = new();
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
        public async Task OnGetAsync() 
        {
            Factions = Enum.GetValues(typeof(Faction)).Cast<Faction>().ToList();
            Factions.Remove(Faction.DarkMarauder);
            Factions.Remove(Faction.KalZul);

            AttackTypeList = Enum.GetValues(typeof(AttackType))
                .Cast<AttackType>()
                .Select(a => new SelectListItem
                {
                    Value = a.ToString(),
                    Text = a.ToString()
                }).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // AttackerFleet and DefenderFleet will be bound automatically
            // You can process them here, e.g. simulate battle, validate, etc.
            IsBattleSimulated = true;
            // Example: Validate input
            if (AttackerInputFleet != null)
            {
                // Remove empty rows, etc.
                AttackerInputFleet = AttackerInputFleet.Where(f => f.ShipId > 0 && f.TotalShips > 0).ToList();
                
            }
            if (DefenderInputFleet != null)
            {
                // Remove empty rows, etc.
                DefenderInputFleet = DefenderInputFleet.Where(f => f.ShipId > 0 && f.TotalShips > 0).ToList();
                
            }
            // If both fleets are empty, return with an error
            if ((AttackerInputFleet == null || !AttackerInputFleet.Any()) && (DefenderInputFleet == null || !DefenderInputFleet.Any()))
            {
                StatusMessage = "Both fleets cannot be empty.";
                await OnGetAsync();
                return Page();
            }
            // Merge Fleets with ShipType information
            List<MergedFleet> AttackerFleet = new List<MergedFleet>();
            List<MergedFleet> DefenderFleet = new List<MergedFleet>();
            var Ships = _context.Ships.ToList();
            foreach (var item in AttackerInputFleet)
            {
                AttackerFleet.Add(new MergedFleet
                {
                    Id = item.Id,
                    ApplicationUserId = "0",
                    UserName = "Attacker",
                    ShipId = item.ShipId,
                    TotalShips = item.TotalShips,
                    PowerRating = Ships.FirstOrDefault(s => s.Id == item.ShipId).PowerRating,
                    TotalPowerRating = Ships.FirstOrDefault(s => s.Id == item.ShipId).PowerRating * item.TotalShips,
                    TotalUpkeep = item.TotalShips * item.TotalUpkeep,
                    ShipName = Ships.FirstOrDefault(s => s.Id == item.ShipId).ShipName,
                    ShipType = Ships.FirstOrDefault(s => s.Id == item.ShipId).ShipType,
                    Range = Ships.FirstOrDefault(s => s.Id == item.ShipId).Range,
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
            foreach (var item in DefenderInputFleet)
            {
                DefenderFleet.Add(new MergedFleet
                {
                    Id = item.Id,
                    ApplicationUserId = "1",
                    UserName = "Defender",
                    ShipId = item.ShipId,
                    TotalShips = item.TotalShips,
                    PowerRating = Ships.FirstOrDefault(s => s.Id == item.ShipId).PowerRating,
                    TotalPowerRating = Ships.FirstOrDefault(s => s.Id == item.ShipId).PowerRating * item.TotalShips,
                    TotalUpkeep = item.TotalShips * item.TotalUpkeep,
                    ShipName = Ships.FirstOrDefault(s => s.Id == item.ShipId).ShipName,
                    ShipType = Ships.FirstOrDefault(s => s.Id == item.ShipId).ShipType,
                    Range = Ships.FirstOrDefault(s => s.Id == item.ShipId).Range,
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
            AttackerFleet = AttackerFleet.OrderByDescending(s => s.PowerRating).ToList();
            DefenderFleet = DefenderFleet.OrderByDescending(s => s.PowerRating).ToList();
            // Add your simulation logic here
            if (DefenderFleet.Count() > 0)
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
                        if (defender.TotalShips > 0)
                        {
                            // Defender Attacks First
                            // Defender Attacks
                            DamageAttackersStackTakes = CalulateDefenderDamage(attacker, defender, false).DamageDealt;
                            HowManyAttackersShipsKilled = CalulateDefenderDamage(attacker, defender, false).ShipsKilled;
                        }
                        else
                        {
                            DamageAttackersStackTakes = 0;
                            HowManyAttackersShipsKilled = 0;
                        }
                        //// Attacker Retals (if Defender does not have NoRetal && Attacker does not have NoDefense)
                        if (defender.NoRetal == false && attacker.NoDefense == false && attacker.TotalShips > 0)
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
                        if (defender.CanCapture == true && HowManyAttackersShipsKilled > 0 && attacker.ImmuneToCapture == false && attacker.CanCapture == false)
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
                        if (attacker.TotalShips > 0)
                        {
                            // Attacker Attacks
                            DamageDefendersStackTakes = CalulateAttackerDamage(attacker, defender, false).DamageDealt;
                            HowManyDefendersShipsKilled = CalulateAttackerDamage(attacker, defender, false).ShipsKilled;
                        }
                        else
                        {
                            DamageDefendersStackTakes = 0;
                            HowManyDefendersShipsKilled = 0;
                        }
                        //// Defender Retals (if Attacker does not have NoRetal && Defender does not have NoDefense)
                        if (attacker.NoRetal == false && defender.NoDefense == false && defender.TotalShips > 0)
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
                        if (attacker.TotalShips > 0)
                        {
                            // Attacker Attacks First
                            // Attacker Attacks
                            DamageDefendersStackTakes = CalulateAttackerDamage(attacker, defender, false).DamageDealt;
                            HowManyDefendersShipsKilled = CalulateAttackerDamage(attacker, defender, false).ShipsKilled;
                        }
                        else
                        {
                            DamageDefendersStackTakes = 0;
                            HowManyDefendersShipsKilled = 0;
                        }
                        //// Defender Retals (if Attacker does not have NoRetal && Defender does not have NoDefense)
                        if (attacker.NoRetal == false && defender.NoDefense == false && defender.TotalShips > 0)
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
                        if (defender.TotalShips > 0)
                        {
                            // Defender Attacks
                            DamageAttackersStackTakes = CalulateDefenderDamage(attacker, defender, false).DamageDealt;
                            HowManyAttackersShipsKilled = CalulateDefenderDamage(attacker, defender, false).ShipsKilled;
                        }
                        else
                        {
                            DamageAttackersStackTakes = 0;
                            HowManyAttackersShipsKilled = 0;
                        }
                        //// Attacker Retals (if Defender does not have NoRetal && Attacker does not have NoDefense)
                        if (defender.NoRetal == false && attacker.NoDefense == false && attacker.TotalShips > 0)
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
                        
                        VictoryMessage = $"Attacker Wins the battle!";
                        await OnGetAsync();
                        return Page();
                    }
                    // attacker no fleet
                    else if (AttackerFleet.All(f => f.TotalShips <= 0))
                    {
                        // Attacker has no fleet left
                        // Defender wins by default
                        // Fleet Report + Important Events for attacker and defender
                        EndFleets = $"<h3>Attacker's Fleet</h3>";
                        EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                        foreach (var ship in AttackerFleet)
                        {
                            int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                            EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                        }
                        EndFleets += "</table>";

                        EndFleets += $"<h3>Defender's Fleet</h3>";
                        EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                        foreach (var ship in DefenderFleet)
                        {
                            int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                            EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                        }
                        EndFleets += "</table>";
                        
                        VictoryMessage = $"Defender Wins the battle!";
                        await OnGetAsync();
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
                        if (defender.TotalShips > 0)
                        {
                            // Defender Attacks First
                            // Defender Attacks
                            DamageAttackersStackTakes = CalulateDefenderDamage(attacker, defender, false).DamageDealt;
                            HowManyAttackersShipsKilled = CalulateDefenderDamage(attacker, defender, false).ShipsKilled;
                        }
                        else
                        {
                            DamageAttackersStackTakes = 0;
                            HowManyAttackersShipsKilled = 0;
                        }
                        //// Attacker Retals (if Defender does not have NoRetal && Attacker does not have NoDefense)
                        if (defender.NoRetal == false && attacker.NoDefense == false && attacker.TotalShips > 0)
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
                        if (attacker.TotalShips > 0)
                        {
                            // Attacker Attacks
                            DamageDefendersStackTakes = CalulateAttackerDamage(attacker, defender, false).DamageDealt;
                            HowManyDefendersShipsKilled = CalulateAttackerDamage(attacker, defender, false).ShipsKilled;
                        }
                        else
                        {
                            DamageDefendersStackTakes = 0;
                            HowManyDefendersShipsKilled = 0;
                        }
                        //// Defender Retals (if Attacker does not have NoRetal && Defender does not have NoDefense)
                        if (attacker.NoRetal == false && defender.NoDefense == false && defender.TotalShips > 0)
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
                        if (defender.CanCapture == true && HowManyAttackersShipsKilled > 0 && attacker.ImmuneToCapture == false && attacker.CanCapture == false)
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
                        if (attacker.TotalShips > 0)
                        {
                            // Attacker Attacks First
                            // Attacker Attacks
                            DamageDefendersStackTakes = CalulateAttackerDamage(attacker, defender, false).DamageDealt;
                            HowManyDefendersShipsKilled = CalulateAttackerDamage(attacker, defender, false).ShipsKilled;
                        }
                        else
                        {
                            DamageDefendersStackTakes = 0;
                            HowManyDefendersShipsKilled = 0;
                        }
                        //// Defender Retals (if Attacker does not have NoRetal && Defender does not have NoDefense)
                        if (attacker.NoRetal == false && defender.NoDefense == false && defender.TotalShips > 0)
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
                        if (defender.TotalShips > 0)
                        {
                            // Defender Attacks
                            DamageAttackersStackTakes = CalulateDefenderDamage(attacker, defender, false).DamageDealt;
                            HowManyAttackersShipsKilled = CalulateDefenderDamage(attacker, defender, false).ShipsKilled;
                        }
                        else
                        {
                            DamageAttackersStackTakes = 0;
                            HowManyAttackersShipsKilled = 0;
                        }
                        //// Attacker Retals (if Defender does not have NoRetal && Attacker does not have NoDefense)
                        if (defender.NoRetal == false && attacker.NoDefense == false && attacker.TotalShips > 0)
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
                        // Fleet Report + Important Events for attacker and defender
                        EndFleets = $"<h3>Attacker's Fleet</h3>";
                        EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                        foreach (var ship in AttackerFleet)
                        {
                            int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                            EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                        }
                        EndFleets += "</table>";

                        EndFleets += $"<h3>Defender's Fleet</h3>";
                        EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                        foreach (var ship in DefenderFleet)
                        {
                            int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                            EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                        }
                        EndFleets += "</table>";
                        
                        VictoryMessage = $"Attacker Wins the battle!";
                        return Page();
                    }
                    // attacker no fleet
                    else if (AttackerFleet.All(f => f.TotalShips <= 0))
                    {
                        // Attacker has no fleet left
                        // Defender wins by default
                        // Fleet Report + Important Events for attacker and defender
                        EndFleets = $"<h3>Attacker's Fleet</h3>";
                        EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                        foreach (var ship in AttackerFleet)
                        {
                            int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                            EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                        }
                        EndFleets += "</table>";

                        EndFleets += $"<h3>Defender's Fleet</h3>";
                        EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                        foreach (var ship in DefenderFleet)
                        {
                            int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                            EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                        }
                        EndFleets += "</table>";
                        
                        VictoryMessage = $"Defender Wins the battle!";
                        await OnGetAsync();
                        return Page();
                    }
                }

            }
            else
            {
                // Defender has no fleet left
                // Attacker wins by default
                // Fleet Report + Important Events for attacker and defender
                EndFleets = $"<h3>Attacker's Fleet</h3>";
                EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";


                foreach (var ship in AttackerFleet)
                {
                    int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                    EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                }
                EndFleets += "</table>";

                EndFleets += $"<h3>Defender's Fleet</h3>";
                EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                foreach (var ship in DefenderFleet)
                {
                    int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                    EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                }
                EndFleets += "</table>";
                VictoryMessage = $"Attacker Wins the battle!";
                await OnGetAsync();
                return Page();
            }
            if (DefenderTotalPowerRatingLoss <= AttackerTotalPowerRatingLoss)
            {
                // Defender wins by default
                // Fleet Report + Important Events for attacker and defender
                EndFleets = $"<h3>Attacker's Fleet</h3>";
                EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                foreach (var ship in AttackerFleet)
                {
                    int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                    EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                }
                EndFleets += "</table>";

                EndFleets += $"<h3>Defender's Fleet</h3>";
                EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                foreach (var ship in DefenderFleet)
                {
                    int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                    EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                }
                EndFleets += "</table>";                
                VictoryMessage = $"Defender Wins the battle!";
                await OnGetAsync();
                return Page();
            }
            else
            {
                // Attacker wins by default
               
                // Fleet Report + Important Events for attacker and defender
                EndFleets = $"<h3>Attacker's Fleet</h3>";
                EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                foreach (var ship in AttackerFleet)
                {
                    int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                    EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                }
                EndFleets += "</table>";

                EndFleets += $"<h3>Defender's Fleet</h3>";
                EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Total Ships</th><th>Killed</th><th>Remaining</th></tr>";

                foreach (var ship in DefenderFleet)
                {
                    int killed = ship.TotalShipsStart - ship.TotalShips; // You need to track starting ships
                    EndFleets += $"<tr><td>{ship.ShipName}</td><td>{ship.TotalShipsStart}</td><td>{killed}</td><td>{ship.TotalShips}</td></tr>";
                }
                EndFleets += "</table>";
                VictoryMessage = $"Attacker Wins the Battle!";
                await OnGetAsync();
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
            if (shipsKilled > Defender.TotalShips)
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
        public async Task<JsonResult> OnGetShipsForFactionAsync(int factionId)
        {
            var ships = new List<Ships>();

            switch (factionId)
            {
                case 0:
                    ships = _context.Set<Ships>()
                    .Where(s => s.Id >= 219 && s.Id <= 236 && s.Id >= 305 && s.Id <= 327)
                    .ToList();
                    break;
                case 1:
                    ships = _context.Set<Ships>()
                    .Where(s => s.Id >= 237 && s.Id <= 256 && s.Id >= 305 && s.Id <= 327)
                    .ToList();
                    break;
                case 2:
                    ships = _context.Set<Ships>()
                    .Where(s => s.Id >= 275 && s.Id <= 288 && s.Id >= 305 && s.Id <= 327)
                    .ToList();
                    break;
                case 3:
                    ships = _context.Set<Ships>()
                    .Where(s => s.Id >= 219 && s.Id <= 327)
                    .ToList();
                    break;
                case 4:
                    ships = _context.Set<Ships>()
                   .Where(s => s.Id >= 257 && s.Id <= 269 && s.Id >= 305 && s.Id <= 327)
                   .ToList();
                    break;
                case 5:
                    ships = _context.Set<Ships>()
                   .Where(s => s.Id >= 289 && s.Id <= 304 && s.Id >= 305 && s.Id <= 327)
                   .ToList();
                    break;
                default:
                    break;
            }

            return new JsonResult(ships);
        }
    }
}
