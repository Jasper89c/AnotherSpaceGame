using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class ShipSimModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ShipSimModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public AttackType AttackType { get; set; }
        public List<Ships> AllShips { get; set; } = new();
        public List<SelectListItem> AttackTypeList { get; set; } = new();
        [BindProperty]
        public int SelectedShipId { get; set; }

        [BindProperty]
        public string SelectedAttackType { get; set; }
        public List<Result> Results { get; set; } = new();
        public bool IsSimulated { get; set; } = false;
        public async Task OnGetAsync()
        {
            AllShips = await _context.Set<Ships>().ToListAsync();
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
            AllShips = await _context.Set<Ships>().ToListAsync();
            // Handle simulation logic here using SelectedShipId and SelectedAttackType
            var selectedShip = AllShips.FirstOrDefault(s => s.Id == SelectedShipId);
            AttackType = Enum.Parse<AttackType>(SelectedAttackType);


            foreach (var ship in AllShips)
            {
                var defender = new MergedFleet
                {
                    Id = ship.Id,
                    ShipId = ship.Id,
                    PowerRating = ship.PowerRating,
                    TotalShips = (int)Math.Ceiling(1000000 / (double)ship.PowerRating),
                    TotalPowerRating = ship.PowerRating * (int)Math.Ceiling(1000000 / (double)ship.PowerRating),
                    TotalUpkeep = (int)Math.Ceiling(1000000 / (double)ship.PowerRating) * ship.Upkeep,
                    ShipName = ship.ShipName,
                    ShipType = ship.ShipType,
                    Range = ship.Range,
                    Weapon = ship.Weapon,
                    Hull = ship.Hull,
                    EnergyWeapon = ship.EnergyWeapon,
                    KineticWeapon = ship.KineticWeapon,
                    MissileWeapon = ship.MissileWeapon,
                    ChemicalWeapon = ship.ChemicalWeapon,
                    EnergyShield = ship.EnergyShield,
                    KineticShield = ship.KineticShield,
                    MissileShield = ship.MissileShield,
                    ChemicalShield = ship.ChemicalShield,
                    NoDefense = ship.NoDefense,
                    NoRetal = ship.NoRetal,
                    CanCapture = ship.CanCapture,
                    CapChance = ship.CapChance,
                    Composite = ship.Composite,
                    RedCrystal = ship.RedCrystal,
                    Rutile = ship.Rutile,
                    WhiteCrystal = ship.WhiteCrystal,
                    StrafezOrganism = ship.StrafezOrganism,
                    TerranMetal = ship.TerranMetal,
                    CostToBuild = ship.CostToBuild,
                    ScanningPower = ship.ScanningPower,
                    BuildRate = ship.BuildRate,
                    Cost = ship.Cost,
                    ImmuneToCapture = ship.ImmuneToCapture,
                    Upkeep = ship.Upkeep,
                    TotalShipsStart = (int)Math.Ceiling(1000000 / (double)ship.PowerRating) // Store the initial number of ships for the battle report
                };
                var attacker = new MergedFleet
                {
                    Id = selectedShip.Id,
                    ShipId = selectedShip.Id,
                    PowerRating = selectedShip.PowerRating,
                    TotalShips = (int)Math.Ceiling(1000000 / (double)selectedShip.PowerRating),
                    TotalPowerRating = selectedShip.PowerRating * (int)Math.Ceiling(1000000 / (double)selectedShip.PowerRating),
                    TotalUpkeep = (int)Math.Ceiling(1000000 / (double)selectedShip.PowerRating) * selectedShip.Upkeep,
                    ShipName = selectedShip.ShipName,
                    ShipType = selectedShip.ShipType,
                    Range = selectedShip.Range,
                    Weapon = selectedShip.Weapon,
                    Hull = selectedShip.Hull,
                    EnergyWeapon = selectedShip.EnergyWeapon,
                    KineticWeapon = selectedShip.KineticWeapon,
                    MissileWeapon = selectedShip.MissileWeapon,
                    ChemicalWeapon = selectedShip.ChemicalWeapon,
                    EnergyShield = selectedShip.EnergyShield,
                    KineticShield = selectedShip.KineticShield,
                    MissileShield = selectedShip.MissileShield,
                    ChemicalShield = selectedShip.ChemicalShield,
                    NoDefense = selectedShip.NoDefense,
                    NoRetal = selectedShip.NoRetal,
                    CanCapture = selectedShip.CanCapture,
                    CapChance = selectedShip.CapChance,
                    Composite = selectedShip.Composite,
                    RedCrystal = selectedShip.RedCrystal,
                    Rutile = selectedShip.Rutile,
                    WhiteCrystal = selectedShip.WhiteCrystal,
                    StrafezOrganism = selectedShip.StrafezOrganism,
                    TerranMetal = selectedShip.TerranMetal,
                    CostToBuild = selectedShip.CostToBuild,
                    ScanningPower = selectedShip.ScanningPower,
                    BuildRate = selectedShip.BuildRate,
                    Cost = selectedShip.Cost,
                    ImmuneToCapture = selectedShip.ImmuneToCapture,
                    Upkeep = selectedShip.Upkeep,
                    TotalShipsStart = (int)Math.Ceiling(1000000 / (double)selectedShip.PowerRating) // Store the initial number of ships for the battle report
                };
                var result = new Result
                {
                    AttackerShipCountStart = (int)Math.Ceiling(1000000 / (double)attacker.PowerRating),
                    DefenderShipCountStart = (int)Math.Ceiling(1000000 / (double)defender.PowerRating),
                    AttackerShipName = attacker.ShipName,
                    DefenderShipName = defender.ShipName
                };
                // wave 1
                var attackerShipsKilledWave1 = 0;
                var attackerShipsKilledWave2 = 0;
                var defenderShipsKilledWave1 = 0;
                var defenderShipsKilledWave2 = 0;
                // defender attacks first
                if (defender.Range >= attacker.Range)
                {
                    // defender attacks first
                    attackerShipsKilledWave1 = CalulateDefenderDamage(attacker, defender, false).ShipsKilled;
                    // attacker retaliates
                    if (defender.NoRetal == false && attacker.NoDefense == false)
                    {
                        defenderShipsKilledWave1 = CalulateAttackerDamage(attacker, defender, true).ShipsKilled;
                    }
                }
                // attacker attacks first
                else
                {
                    // attacker attacks first
                    defenderShipsKilledWave1 = CalulateAttackerDamage(attacker, defender, false).ShipsKilled;
                    // defender retaliates
                    if (attacker.NoRetal == false && defender.NoDefense == false)
                    {
                        attackerShipsKilledWave1 = CalulateDefenderDamage(attacker, defender, true).ShipsKilled;
                    }
                }
                attacker.TotalShips -= attackerShipsKilledWave1;
                defender.TotalShips -= defenderShipsKilledWave1;
                // wave 2
                if (attacker.TotalShips > 0 && defender.TotalShips > 0)
                {
                    // defender attacks first
                    if (defender.Range >= attacker.Range)
                    {
                        // defender attacks first
                        attackerShipsKilledWave2 = CalulateDefenderDamage(attacker, defender, false).ShipsKilled;
                        // attacker retaliates
                        if (defender.NoRetal == false && attacker.NoDefense == false)
                        {
                            defenderShipsKilledWave2 = CalulateAttackerDamage(attacker, defender, true).ShipsKilled;
                        }
                    }
                    // attacker attacks first
                    else
                    {
                        // attacker attacks first
                        defenderShipsKilledWave2 = CalulateAttackerDamage(attacker, defender, false).ShipsKilled;
                        // defender retaliates
                        if (attacker.NoRetal == false && defender.NoDefense == false)
                        {
                            attackerShipsKilledWave2 = CalulateDefenderDamage(attacker, defender, true).ShipsKilled;
                        }
                    }
                    attacker.TotalShips -= attackerShipsKilledWave2;
                    defender.TotalShips -= defenderShipsKilledWave2;
                }
                // create result
                result.AttackerShipCountEnd = attacker.TotalShips;
                result.DefenderShipCountEnd = defender.TotalShips;
                defender.TotalPowerRating = defender.PowerRating * defender.TotalShips;
                attacker.TotalPowerRating = attacker.PowerRating * attacker.TotalShips;
                if (attacker.TotalPowerRating > defender.TotalPowerRating)
                {
                    result.ResultText = "Win";
                }
                else if (attacker.TotalPowerRating <= defender.TotalPowerRating)
                {
                    result.ResultText = "Lose";
                }
                Results.Add(result);
            }
            IsSimulated = true;
            await OnGetAsync();
            return Page();
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

    }

    public class Result()
    {
        public string AttackerShipName { get; set; }
        public string DefenderShipName { get; set; }
        public int AttackerShipCountStart { get; set; }
        public int AttackerShipCountEnd { get; set; }
        public int DefenderShipCountStart { get; set; }
        public int DefenderShipCountEnd { get; set; }
        public string ResultText { get; set; }
    }
}
