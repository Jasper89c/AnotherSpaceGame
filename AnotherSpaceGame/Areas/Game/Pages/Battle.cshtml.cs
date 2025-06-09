using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class BattleModel : PageModel
    {
        public List<int> CurrentUserFleetIds { get; set; }

        public List<int> TargetUserFleetIds { get; set; }
        [BindProperty]
        public string TargetUserName { get; set; }
        [BindProperty]
        public ApplicationUser TargetUser { get; set; }
        [BindProperty]
        public string CurrentUserName { get; set; }
        public AttackType AttackType { get; set; }
        public List<Fleet> CurrentUserFleets { get; set; }
        public List<MergedFleet> CurrentUserFleetsStart { get; set; }
        public List<Fleet> TargetUserFleets { get; set; }
        public List<MergedFleet> TargetUserFleetsStart { get; set; }

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
            CurrentUserName = user.UserName;
            var fleetIdsJson = HttpContext.Session.GetString("SelectedFleetIds");
            List<int> currentUserFleetIds = new List<int>();
            if (!string.IsNullOrEmpty(fleetIdsJson))
            {
                currentUserFleetIds = JsonSerializer.Deserialize<List<int>>(fleetIdsJson);
            }
            CurrentUserFleetIds = currentUserFleetIds;
            var fleetIdsJson1 = HttpContext.Session.GetString("SelectedFleetIds2");
            List<int> targetUserFleetIds = new List<int>();
            if (!string.IsNullOrEmpty(fleetIdsJson1))
            {
                targetUserFleetIds = JsonSerializer.Deserialize<List<int>>(fleetIdsJson1);
            }
            TargetUserFleetIds = targetUserFleetIds;
            // Load fleets from DB
            CurrentUserFleets = _context.Fleets.Where(f => CurrentUserFleetIds.Contains(f.Id)).OrderByDescending(f => f.TotalPowerRating).Take(10).ToList();
            TargetUserFleets = _context.Fleets.Where(f => TargetUserFleetIds.Contains(f.Id)).OrderByDescending(f => f.TotalPowerRating).Take(10).ToList();
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
                    ShipId = item.ShipId,
                    TotalShips = item.TotalShips,
                    TotalPowerRating = item.TotalPowerRating,
                    TotalUpkeep = item.TotalUpkeep,
                    ShipName = Ships.FirstOrDefault(s => s.Id == item.ShipId).ShipName,
                    ShipType = Ships.FirstOrDefault(s => s.Id == item.ShipId).ShipType,
                    PowerRating = Ships.FirstOrDefault(s => s.Id == item.ShipId).PowerRating,
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
                    ShipId = item.ShipId,
                    TotalShips = item.TotalShips,
                    TotalPowerRating = item.TotalPowerRating,
                    TotalUpkeep = item.TotalUpkeep,
                    ShipName = Ships.FirstOrDefault(s => s.Id == item.ShipId).ShipName,
                    ShipType = Ships.FirstOrDefault(s => s.Id == item.ShipId).ShipType,
                    PowerRating = Ships.FirstOrDefault(s => s.Id == item.ShipId).PowerRating,
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
            CurrentUserFleetsStart = AttackerFleet;
            TargetUserFleetsStart = DefenderFleet;
            if (TargetUserFleets.Count() > 0)
            {
                // Wave 1
                // Set Stacks for Wave 1
                int stacks = Math.Max(AttackerFleet.Count, DefenderFleet.Count);
                for (int i = 0; i < stacks; i++)
                {
                    var attacker = AttackerFleet[i];
                    var defender = DefenderFleet[i];
                    // Defender Attacks First
                    if (defender.Range >= attacker.Range)
                    {
                        // Check if Attacker has no fleet in this stack
                        if (attacker == null)
                        {
                            var max = AttackerFleet.Count + 1;
                            attacker = AttackerFleet[Random.Shared.Next(0, max)];
                        }
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
                                    DefenderCapture = $"{defender.ApplicationUser.UserName} captured {totalCapturedShips} {ShipName} ships from {attacker.ApplicationUser.UserName}."
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
                                    AttackerCapture = $"{attacker.ApplicationUser.UserName} captured {totalCapturedShips} {ShipName} ships from {defender.ApplicationUser.UserName}."
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
                        if (TargetUser.IsNPC == true)
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
                            DefenderPlanets[0].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                            DefenderPlanets[0].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                            DefenderPlanets[0].DateTimeAcquired = DateTime.UtcNow;
                            DefenderPlanets[1].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                            DefenderPlanets[1].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                            DefenderPlanets[1].DateTimeAcquired = DateTime.UtcNow;
                            DefenderPlanets[2].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                            DefenderPlanets[2].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                            DefenderPlanets[2].DateTimeAcquired = DateTime.UtcNow;
                            _context.Planets.Update(DefenderPlanets[0]);
                            _context.Planets.Update(DefenderPlanets[1]);
                            _context.Planets.Update(DefenderPlanets[2]);
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
                            EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Type</th><th>Class</th><th>Land</th></tr>";
                            foreach (var planet in DefenderPlanets)
                            {
                                EndFleets += $"<tr><td>{planet.Name}</td><td>{planet.Type}</td><td>{planet.Type}</td><td>{planet.TotalLand}</td></tr>";
                            }
                            EndFleets += "</table>";
                        }
                        else
                        {
                            EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Type</th><th>Class</th><th>Land</th></tr>";
                            EndFleets += "<td colspan='3'>Defender has no planets</h3>";
                            EndFleets += "/table>";
                        }
                        ImportantEvents attackerEvent = new ImportantEvents
                        {
                            ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId,
                            ImportantEventTypes = ImportantEventTypes.Battles,
                            Text = $"You have won the battle against {TargetUserName}.<br />{EndFleets}<br />",
                            DateAndTime = DateTime.UtcNow
                        };
                        ImportantEvents DefenderEvent = new ImportantEvents
                        {
                            ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId,
                            ImportantEventTypes = ImportantEventTypes.Battles,
                            Text = $"You have lost the battle against {user.UserName}.<br />{EndFleets}<br />",
                            DateAndTime = DateTime.UtcNow
                        };
                        _context.ImportantEvents.Add(attackerEvent);
                        _context.ImportantEvents.Add(DefenderEvent);
                        // battle logs
                        BattleLogs AttackerBattleLog = new BattleLogs
                        {
                            ApplicationUserId = user.Id,
                            Defender = TargetUser.Id,
                            DateAndTime = DateTime.UtcNow,
                            Outcome = "Win",
                            FleetReport = EndFleets
                        };
                        BattleLogs DefenderBattleLog = new BattleLogs
                        {
                            ApplicationUserId = TargetUser.Id,
                            Defender = user.Id,
                            DateAndTime = DateTime.UtcNow,
                            Outcome = $"Loss",
                            FleetReport = EndFleets
                        };
                        _context.Battlelogs.Add(AttackerBattleLog);
                        _context.Battlelogs.Add(DefenderBattleLog);
                        // update database for attacker and defenders fleets
                        foreach (var fleet in AttackerFleet)
                        {
                            var existingFleet = _context.Fleets.FirstOrDefault(f => f.Id == fleet.Id && f.ApplicationUserId == attacker.ApplicationUserId && f.ShipId == attacker.ShipId);
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
                        foreach (var fleet in DefenderFleet)
                        {
                            var existingFleet = _context.Fleets.FirstOrDefault(f => f.Id == fleet.Id && f.ApplicationUserId == defender.ApplicationUserId && f.ShipId == defender.ShipId);
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
                        // Update Attackers Stats
                        user.BattlesWon++;
                        user.ColoniesWon += 3;
                        user.TotalColonies += 3;
                        user.TotalPlanets += 3;
                        // Update Defenders Stats + Damage Protection
                        TargetUser.BattlesLost++;
                        TargetUser.ColoniesLost += 3;
                        TargetUser.TotalColonies -= 3;
                        TargetUser.TotalPlanets -= DefenderPlanets[0].TotalPlanets + DefenderPlanets[1].TotalPlanets + DefenderPlanets[2].TotalPlanets;
                        if (TargetUser.IsNPC == true)
                        {
                            TargetUser.TotalPlanets = 1; // NPCs should always have at least one planet
                            TargetUser.TotalColonies = 1; // NPCs should always have at least one colony
                            TargetUser.DamageProtection = DateTime.Now.AddMinutes(15); // Reset damage protection for defender
                        }
                        else
                        {
                            TargetUser.DamageProtection = DateTime.Now.AddDays(1); // Reset damage protection for defender
                        }
                        var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, 5);
                        StatusMessage = turnsMessage.Message;
                        return Page();
                    }
                    // attacker no fleet
                    else if (AttackerFleet.All(f => f.TotalShips <= 0))
                    {
                        // Attacker has no fleet left
                        // Defender wins by default
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
                        ImportantEvents attackerEvent = new ImportantEvents
                        {
                            ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId,
                            ImportantEventTypes = ImportantEventTypes.Battles,
                            Text = $"You have Lost the battle against {TargetUserName}.<br />{EndFleets}<br />",
                            DateAndTime = DateTime.UtcNow
                        };
                        ImportantEvents DefenderEvent = new ImportantEvents
                        {
                            ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId,
                            ImportantEventTypes = ImportantEventTypes.Battles,
                            Text = $"You have Won the battle against {user.UserName}.<br />{EndFleets}<br />",
                            DateAndTime = DateTime.UtcNow
                        };
                        _context.ImportantEvents.Add(attackerEvent);
                        _context.ImportantEvents.Add(DefenderEvent);
                        // battle logs
                        BattleLogs AttackerBattleLog = new BattleLogs
                        {
                            ApplicationUserId = user.Id,
                            Defender = TargetUser.Id,
                            DateAndTime = DateTime.UtcNow,
                            Outcome = $"Loss",
                            FleetReport = EndFleets
                        };
                        BattleLogs DefenderBattleLog = new BattleLogs
                        {
                            ApplicationUserId = TargetUser.Id,
                            Defender = user.Id,
                            DateAndTime = DateTime.UtcNow,
                            Outcome = $"Win",
                            FleetReport = EndFleets
                        };
                        _context.Battlelogs.Add(AttackerBattleLog);
                        _context.Battlelogs.Add(DefenderBattleLog);
                        // update database for attacker and defenders fleets
                        // update database for attacker and defenders fleets
                        foreach (var fleet in AttackerFleet)
                        {
                            var existingFleet = _context.Fleets.FirstOrDefault(f => f.Id == fleet.Id && f.ApplicationUserId == attacker.ApplicationUserId && f.ShipId == attacker.ShipId);
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
                        foreach (var fleet in DefenderFleet)
                        {
                            var existingFleet = _context.Fleets.FirstOrDefault(f => f.Id == fleet.Id && f.ApplicationUserId == defender.ApplicationUserId && f.ShipId == defender.ShipId);
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
                        // Update Attackers Stats
                        user.BattlesLost += 1;
                        // Update Defenders Stats
                        TargetUser.BattlesWon++;
                        if (TargetUser.IsNPC == true)
                        {
                            TargetUser.TotalPlanets = 1; // NPCs should always have at least one planet
                            TargetUser.TotalColonies = 1; // NPCs should always have at least one colony
                            TargetUser.DamageProtection = DateTime.Now.AddMinutes(15); // Reset damage protection for defender
                        }
                        else
                        {
                            TargetUser.DamageProtection = DateTime.Now.AddDays(1); // Reset damage protection for defender
                        }
                        var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, 5);
                        StatusMessage = turnsMessage.Message;
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
                    var attacker = AttackerFleet[i];
                    var defender = DefenderFleet[i];
                    // Defender Attacks First
                    if (defender.Range >= attacker.Range)
                    {
                        // Check if Attacker has no fleet in this stack
                        if (attacker == null)
                        {
                            var max = AttackerFleet.Count + 1;
                            attacker = AttackerFleet[Random.Shared.Next(0, max)];
                        }
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
                                    DefenderCapture = $"{defender.ApplicationUser.UserName} captured {totalCapturedShips} {ShipName} ships from {attacker.ApplicationUser.UserName}."
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
                                    AttackerCapture = $"{attacker.ApplicationUser.UserName} captured {totalCapturedShips} {ShipName} ships from {defender.ApplicationUser.UserName}."
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
                        if (TargetUser.IsNPC == true)
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
                            DefenderPlanets[0].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                            DefenderPlanets[0].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                            DefenderPlanets[0].DateTimeAcquired = DateTime.UtcNow;
                            DefenderPlanets[1].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                            DefenderPlanets[1].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                            DefenderPlanets[1].DateTimeAcquired = DateTime.UtcNow;
                            DefenderPlanets[2].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                            DefenderPlanets[2].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                            DefenderPlanets[2].DateTimeAcquired = DateTime.UtcNow;
                            _context.Planets.Update(DefenderPlanets[0]);
                            _context.Planets.Update(DefenderPlanets[1]);
                            _context.Planets.Update(DefenderPlanets[2]);
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
                            EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Type</th><th>Class</th><th>Land</th></tr>";
                            foreach (var planet in DefenderPlanets)
                            {
                                EndFleets += $"<tr><td>{planet.Name}</td><td>{planet.Type}</td><td>{planet.Type}</td><td>{planet.TotalLand}</td></tr>";
                            }
                            EndFleets += "</table>";
                        }
                        else
                        {
                            EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Type</th><th>Class</th><th>Land</th></tr>";
                            EndFleets += "<td colspan='3'>Defender has no planets</h3>";
                            EndFleets += "/table>";
                        }
                        ImportantEvents attackerEvent = new ImportantEvents
                        {
                            ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId,
                            ImportantEventTypes = ImportantEventTypes.Battles,
                            Text = $"You have won the battle against {TargetUserName}.<br />{EndFleets}<br />" + EndFleets,
                            DateAndTime = DateTime.UtcNow
                        };
                        ImportantEvents DefenderEvent = new ImportantEvents
                        {
                            ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId,
                            ImportantEventTypes = ImportantEventTypes.Battles,
                            Text = $"You have lost the battle against {user.UserName}.<br />{EndFleets}<br />" + EndFleets,
                            DateAndTime = DateTime.UtcNow
                        };
                        _context.ImportantEvents.Add(attackerEvent);
                        _context.ImportantEvents.Add(DefenderEvent);
                        // battle logs
                        BattleLogs AttackerBattleLog = new BattleLogs
                        {
                            ApplicationUserId = user.Id,
                            Defender = TargetUser.Id,
                            DateAndTime = DateTime.UtcNow,
                            Outcome = $"Win",
                            FleetReport = EndFleets
                        };
                        BattleLogs DefenderBattleLog = new BattleLogs
                        {
                            ApplicationUserId = TargetUser.Id,
                            Defender = user.Id,
                            DateAndTime = DateTime.UtcNow,
                            Outcome = $"Loss",
                            FleetReport = EndFleets
                        };
                        _context.Battlelogs.Add(AttackerBattleLog);
                        _context.Battlelogs.Add(DefenderBattleLog);
                        // update database for attacker and defenders fleets
                        foreach (var fleet in AttackerFleet)
                        {
                            var existingFleet = _context.Fleets.FirstOrDefault(f => f.Id == fleet.Id && f.ApplicationUserId == attacker.ApplicationUserId && f.ShipId == attacker.ShipId);
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
                        foreach (var fleet in DefenderFleet)
                        {
                            var existingFleet = _context.Fleets.FirstOrDefault(f => f.Id == fleet.Id && f.ApplicationUserId == defender.ApplicationUserId && f.ShipId == defender.ShipId);
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
                        // Update Attackers Stats
                        user.BattlesWon++;
                        user.ColoniesWon += 3;
                        user.TotalColonies += 3;
                        user.TotalPlanets += 3;
                        // Update Defenders Stats + Damage Protection
                        TargetUser.BattlesLost++;
                        TargetUser.ColoniesLost += 3;
                        TargetUser.TotalColonies -= 3;
                        TargetUser.TotalPlanets -= DefenderPlanets[0].TotalPlanets + DefenderPlanets[1].TotalPlanets + DefenderPlanets[2].TotalPlanets;
                        if (TargetUser.IsNPC == true)
                        {
                            TargetUser.TotalPlanets = 1; // NPCs should always have at least one planet
                            TargetUser.TotalColonies = 1; // NPCs should always have at least one colony
                            TargetUser.DamageProtection = DateTime.Now.AddMinutes(15); // Reset damage protection for defender
                        }
                        else
                        {
                            TargetUser.DamageProtection = DateTime.Now.AddDays(1); // Reset damage protection for defender
                        }
                        var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, 5);
                        StatusMessage = turnsMessage.Message;
                        return Page();
                    }
                    // attacker no fleet
                    else if (AttackerFleet.All(f => f.TotalShips <= 0))
                    {
                        // Attacker has no fleet left
                        // Defender wins by default
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
                        ImportantEvents attackerEvent = new ImportantEvents
                        {
                            ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId,
                            ImportantEventTypes = ImportantEventTypes.Battles,
                            Text = $"You have Lost the battle against {TargetUserName}.<br />{EndFleets}<br />" + EndFleets,
                            DateAndTime = DateTime.UtcNow
                        };
                        ImportantEvents DefenderEvent = new ImportantEvents
                        {
                            ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId,
                            ImportantEventTypes = ImportantEventTypes.Battles,
                            Text = $"You have Won the battle against {user.UserName}.<br />{EndFleets}<br />" + EndFleets,
                            DateAndTime = DateTime.UtcNow
                        };
                        _context.ImportantEvents.Add(attackerEvent);
                        _context.ImportantEvents.Add(DefenderEvent);
                        // battle logs
                        BattleLogs AttackerBattleLog = new BattleLogs
                        {
                            ApplicationUserId = user.Id,
                            Defender = TargetUser.Id,
                            DateAndTime = DateTime.UtcNow,
                            Outcome = $"Loss",
                            FleetReport = EndFleets
                        };
                        BattleLogs DefenderBattleLog = new BattleLogs
                        {
                            ApplicationUserId = TargetUser.Id,
                            Defender = user.Id,
                            DateAndTime = DateTime.UtcNow,
                            Outcome = $"Win",
                            FleetReport = EndFleets
                        };
                        _context.Battlelogs.Add(AttackerBattleLog);
                        _context.Battlelogs.Add(DefenderBattleLog);
                        // update database for attacker and defenders fleets
                        foreach (var fleet in AttackerFleet)
                        {
                            var existingFleet = _context.Fleets.FirstOrDefault(f => f.Id == fleet.Id && f.ApplicationUserId == attacker.ApplicationUserId && f.ShipId == attacker.ShipId);
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
                        foreach (var fleet in DefenderFleet)
                        {
                            var existingFleet = _context.Fleets.FirstOrDefault(f => f.Id == fleet.Id && f.ApplicationUserId == defender.ApplicationUserId && f.ShipId == defender.ShipId);
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
                        // Update Attackers Stats
                        user.BattlesLost += 1;
                        // Update Defenders Stats
                        TargetUser.BattlesWon++;
                        if (TargetUser.IsNPC == true)
                        {
                            TargetUser.TotalPlanets = 1; // NPCs should always have at least one planet
                            TargetUser.TotalColonies = 1; // NPCs should always have at least one colony
                            TargetUser.DamageProtection = DateTime.Now.AddMinutes(15); // Reset damage protection for defender
                        }
                        else
                        {
                            TargetUser.DamageProtection = DateTime.Now.AddDays(1); // Reset damage protection for defender
                        }
                        var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, 5);
                        StatusMessage = turnsMessage.Message;
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
                if (TargetUser.IsNPC == true)
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
                    DefenderPlanets[0].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                    DefenderPlanets[0].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                    DefenderPlanets[0].DateTimeAcquired = DateTime.UtcNow;
                    DefenderPlanets[1].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                    DefenderPlanets[1].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                    DefenderPlanets[1].DateTimeAcquired = DateTime.UtcNow;
                    DefenderPlanets[2].ApplicationUser = CurrentUserFleets.FirstOrDefault().ApplicationUser;
                    DefenderPlanets[2].ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId;
                    DefenderPlanets[2].DateTimeAcquired = DateTime.UtcNow;
                    _context.Planets.Update(DefenderPlanets[0]);
                    _context.Planets.Update(DefenderPlanets[1]);
                    _context.Planets.Update(DefenderPlanets[2]);
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
                    EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Type</th><th>Class</th><th>Land</th></tr>";
                    foreach (var planet in DefenderPlanets)
                    {
                        EndFleets += $"<tr><td>{planet.Name}</td><td>{planet.Type}</td><td>{planet.Type}</td><td>{planet.TotalLand}</td></tr>";
                    }
                    EndFleets += "</table>";
                }
                else
                {
                    EndFleets += "<table border='1' cellpadding='4' cellspacing='0'><tr><th>Name</th><th>Type</th><th>Class</th><th>Land</th></tr>";
                    EndFleets += "<td colspan='3'>Defender has no planets</h3>";
                    EndFleets += "/table>";
                }
                ImportantEvents attackerEvent = new ImportantEvents
                {
                    ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId,
                    ImportantEventTypes = ImportantEventTypes.Battles,
                    Text = $"You have won the battle against {TargetUserName}.<br />{EndFleets}<br />" + EndFleets,
                    DateAndTime = DateTime.UtcNow
                };
                ImportantEvents DefenderEvent = new ImportantEvents
                {
                    ApplicationUserId = CurrentUserFleets.FirstOrDefault().ApplicationUserId,
                    ImportantEventTypes = ImportantEventTypes.Battles,
                    Text = $"You have lost the battle against {user.UserName}.<br />{EndFleets}<br />" + EndFleets,
                    DateAndTime = DateTime.UtcNow
                };
                _context.ImportantEvents.Add(attackerEvent);
                _context.ImportantEvents.Add(DefenderEvent);
                // battle logs
                BattleLogs AttackerBattleLog = new BattleLogs
                {
                    ApplicationUserId = user.Id,
                    Defender = TargetUser.Id,
                    DateAndTime = DateTime.UtcNow,
                    Outcome = $"Win",
                    FleetReport = EndFleets
                };
                BattleLogs DefenderBattleLog = new BattleLogs
                {
                    ApplicationUserId = TargetUser.Id,
                    Defender = user.Id,
                    DateAndTime = DateTime.UtcNow,
                    Outcome = $"Loss",
                    FleetReport = EndFleets
                };
                _context.Battlelogs.Add(AttackerBattleLog);
                _context.Battlelogs.Add(DefenderBattleLog);
                // update database for attacker and defenders fleets
                foreach (var fleet in AttackerFleet)
                {
                    var existingFleet = _context.Fleets.FirstOrDefault(f => f.Id == fleet.Id && f.ApplicationUserId == user.Id && f.ShipId == fleet.ShipId);
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
                foreach (var fleet in DefenderFleet)
                {
                    var existingFleet = _context.Fleets.FirstOrDefault(f => f.Id == fleet.Id && f.ApplicationUserId == TargetUser.Id && f.ShipId == fleet.ShipId);
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
                // Update Attackers Stats
                user.BattlesWon++;
                user.ColoniesWon += 3;
                user.TotalColonies += 3;
                user.TotalPlanets += 3;
                // Update Defenders Stats + Damage Protection
                TargetUser.BattlesLost++;
                TargetUser.ColoniesLost += 3;
                TargetUser.TotalColonies -= 3;
                TargetUser.TotalPlanets -= DefenderPlanets[0].TotalPlanets + DefenderPlanets[1].TotalPlanets + DefenderPlanets[2].TotalPlanets;
                if (TargetUser.IsNPC == true)
                {
                    TargetUser.TotalPlanets = 1; // NPCs should always have at least one planet
                    TargetUser.TotalColonies = 1; // NPCs should always have at least one colony
                    TargetUser.DamageProtection = DateTime.Now.AddMinutes(15); // Reset damage protection for defender
                }
                else
                {
                    TargetUser.DamageProtection = DateTime.Now.AddDays(1); // Reset damage protection for defender
                }
                var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, 5);
                StatusMessage = turnsMessage.Message;
                return Page();
            }
            return Page();
        }


        private (double DamageDealt, double DamageTaken) GetStanceModifier(AttackType type)
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
            var Damage = ((Attacker.TotalShips * (Attacker.EnergyWeapon * Attacker.TotalShips) * GetStanceModifier(AttackType).DamageDealt) * (double)Defender.EnergyShield)
            + ((Attacker.TotalShips * (Attacker.KineticWeapon * Attacker.TotalShips) * GetStanceModifier(AttackType).DamageDealt) * (double)Defender.KineticShield)
            + ((Attacker.TotalShips * (Attacker.MissileWeapon * Attacker.TotalShips) * GetStanceModifier(AttackType).DamageDealt) * (double)Defender.MissileShield)
            + ((Attacker.TotalShips * (Attacker.ChemicalWeapon * Attacker.TotalShips) * GetStanceModifier(AttackType).DamageDealt) * (double)Defender.ChemicalShield);
            // Placeholder for damage calculation logic
            var shipsKilled = 0;
            if (RetalPhase == true)
            {
                shipsKilled = (int)Math.Floor((Damage / 2) / (Defender.TotalShips * Defender.Hull));
            }
            else
            {
                shipsKilled = (int)Math.Floor(Damage / (Defender.TotalShips * Defender.Hull));
            }
            return ((int)(Math.Floor(Damage)), shipsKilled);
        }
        private (int DamageDealt, int ShipsKilled) CalulateDefenderDamage(MergedFleet Attacker, MergedFleet Defender, bool RetalPhase)
        {
            var Damage = ((Defender.TotalShips * (Defender.EnergyWeapon * Defender.TotalShips) * GetStanceModifier(AttackType).DamageTaken) * (double)Attacker.EnergyShield)
            + ((Defender.TotalShips * (Defender.KineticWeapon * Defender.TotalShips) * GetStanceModifier(AttackType).DamageTaken) * (double)Attacker.KineticShield)
            + ((Defender.TotalShips * (Defender.MissileWeapon * Defender.TotalShips) * GetStanceModifier(AttackType).DamageTaken) * (double)Attacker.MissileShield)
            + ((Defender.TotalShips * (Defender.ChemicalWeapon * Defender.TotalShips) * GetStanceModifier(AttackType).DamageTaken) * (double)Attacker.ChemicalShield);
            // Placeholder for damage calculation logic
            var shipsKilled = 0;
            if (RetalPhase == true)
            {
                shipsKilled = (int)Math.Floor((Damage / 2) / (Attacker.TotalShips * Attacker.Hull));
            }
            else
            {
                shipsKilled = (int)Math.Floor(Damage / (Attacker.TotalShips * Attacker.Hull));
            }
            return ((int)(Math.Floor(Damage)), shipsKilled);
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