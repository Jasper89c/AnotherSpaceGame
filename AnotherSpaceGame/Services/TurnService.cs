using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.EntityFrameworkCore;
public class TurnService
{
    private readonly ApplicationDbContext _context;
    public TurnService(ApplicationDbContext context) { _context = context; }

    public async Task<int> GetTurnsAsync(string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        var currentUser = _context.Users.FirstOrDefault(x => x.Id == user.Id);
        var turns = _context.Turns.FirstOrDefault(x => x.ApplicationUserId == currentUser.Id);
        return turns.CurrentTurns;
    }

    public async Task<TurnResult> TryUseTurnsAsync(string userId, int turnsToUse)
    {   
        // Eager-load user and related data
        var user = await _context.Users
            .Include(u => u.Commodities)
            .Include(u => u.Turns)
            .Include(u => u.Fleets)
            .FirstOrDefaultAsync(u => u.Id == userId);
        var currentUser = _context.Users
                .FirstOrDefault(u => u.Id == user.Id);
        if (user == null)
            return new TurnResult { Success = false, Message = "User not found." };
        if (user.Turns.CurrentTurns < turnsToUse)
            return new TurnResult { Success = false, Message = "Not enough turns." };

        // Batch load related entities
        var userPlanets = await _context.Planets.Where(p => p.ApplicationUserId == currentUser.Id).ToListAsync();
        var userInfrastructer = await _context.Infrastructers.FirstOrDefaultAsync(i => i.ApplicationUserId == currentUser.Id);
        var userFleet = await _context.Fleets.Where(f => f.ApplicationUserId == currentUser.Id).ToListAsync();

        var mods = GetFactionModifiers(user.Faction);

        // Initialize income variables
        decimal taxIncome = 0, commercialIncome = 0, industryIncome = 0, agricultureIncome = 0, goodsIncome = 0;
        decimal miningTMIncome = 0, miningRCIncome = 0, miningWCIncome = 0, miningCIncome = 0, miningRIncome = 0, miningSOIncome = 0;
        decimal outOfDamageProtectionBonus = 0;
        decimal creditIncome = 0;

        // check for viral and collective planets
        if (user.Faction == Faction.Viral)
            {
                userPlanets.RemoveAll(p =>
                    p.Type is not PlanetType.TaintedC1 and
                    not PlanetType.TaintedC2 and
                    not PlanetType.TaintedC3 and
                    not PlanetType.TaintedC4 and
                    not PlanetType.InfectedC1 and
                    not PlanetType.InfectedC2 and
                    not PlanetType.InfectedC3);
            }
        if (user.Faction == Faction.Collective)
        {
            userPlanets.RemoveAll(p =>
                p.Type is not PlanetType.SimilareC1 and
                not PlanetType.SimilareC2 and
                not PlanetType.SimilareC3 and
                not PlanetType.SimilareC4 and
                not PlanetType.SimilareC5 and
                not PlanetType.AssimilatedC1 and
                not PlanetType.AssimilatedC2 and
                not PlanetType.AssimilatedC3);
        }
        
        // update users fleets
        foreach (var item in user.Fleets)
        {
            var ship = _context.Ships.FirstOrDefault(x => x.Id == item.ShipId);
            item.TotalUpkeep = item.TotalShips * ship.Upkeep;
            item.TotalPowerRating = item.TotalShips * ship.PowerRating;
        }

        foreach (var planet in userPlanets)
        {
            // Mining
            MineOre(planet, userInfrastructer, user, turnsToUse);

            // Power rating
            planet.PowerRating = CalculatePowerRating(planet);

            // Incomes
            taxIncome += CalculateTaxIncome(planet, mods, turnsToUse);
            commercialIncome += CalculateCommercialIncome(planet, userInfrastructer, mods, turnsToUse);
            agricultureIncome += CalculateAgricultureIncome(user, planet, userInfrastructer, mods, turnsToUse);
            industryIncome += CalculateIndustryIncome(user,planet, userInfrastructer, mods, turnsToUse);
            agricultureIncome = UpdatePlanetPopulation(planet, userInfrastructer, turnsToUse, user.Faction, user, agricultureIncome);
            UpdatePlanetResources(planet, userInfrastructer, mods, turnsToUse, user);
            // Mining by mineral type
            switch (planet.MineralProduced)
            {
                case MineralType.TerranMetal:
                    miningTMIncome += CalculateMiningIncome(planet, userInfrastructer, mods, turnsToUse);
                    break;
                case MineralType.RedCrystal:
                    miningRCIncome += CalculateMiningIncome(planet, userInfrastructer, mods, turnsToUse);
                    break;
                case MineralType.WhiteCrystal:
                    miningWCIncome += CalculateMiningIncome(planet, userInfrastructer, mods, turnsToUse);
                    break;
                case MineralType.Rutile:
                    miningRIncome += CalculateMiningIncome(planet, userInfrastructer, mods, turnsToUse);
                    break;
                case MineralType.Composite:
                    miningCIncome += CalculateMiningIncome(planet, userInfrastructer, mods, turnsToUse);
                    break;
                case MineralType.StrafezOrganism:
                    miningSOIncome += CalculateMiningIncome(planet, userInfrastructer, mods, turnsToUse);
                    break;
            }
            // set total planets based on type
            switch (planet.Type)
                            {
                case PlanetType.AssimilatedC1:
                    planet.TotalPlanets = 5;
                    break;
                case PlanetType.AssimilatedC2:
                    planet.TotalPlanets = 25;
                    break;
                case PlanetType.AssimilatedC3:
                    planet.TotalPlanets = 125;
                    break;
                case PlanetType.Balanced:
                    planet.TotalPlanets = 1;
                    break;
                case PlanetType.Barren:
                    planet.TotalPlanets = 1;
                    break;
                case PlanetType.ClusterLevel1:
                    planet.TotalPlanets = 5;
                    break;
                case PlanetType.ClusterLevel2:
                    planet.TotalPlanets = 25;
                    break;
                case PlanetType.ClusterLevel3:
                    planet.TotalPlanets = 125;
                    break;
                case PlanetType.Dead:
                    planet.TotalPlanets = 1;
                    break;
                case PlanetType.Desert:
                    planet.TotalPlanets = 1;
                    break;
                case PlanetType.Forest:
                    planet.TotalPlanets = 1;
                    break;
                case PlanetType.Gas:
                    planet.TotalPlanets = 1;
                    break;
                case PlanetType.InfectedC1:
                    planet.TotalPlanets = 5;
                    break;
                case PlanetType.InfectedC2:
                    planet.TotalPlanets = 25;
                    break;
                case PlanetType.InfectedC3:
                    planet.TotalPlanets = 125;
                    break;
                case PlanetType.Icy:
                    planet.TotalPlanets = 1;
                    break;
                case PlanetType.Marshy:
                    planet.TotalPlanets = 1;
                    break;
                case PlanetType.Oceanic:
                    planet.TotalPlanets = 1;
                    break;
                case PlanetType.Rocky:
                    planet.TotalPlanets = 1;
                    break;
                case PlanetType.SimilareC1:
                    planet.TotalPlanets = 1;
                    break;
                case PlanetType.SimilareC2:
                    planet.TotalPlanets = 4;
                    break;
                case PlanetType.SimilareC3:
                    planet.TotalPlanets = 16;
                    break;
                case PlanetType.SimilareC4:
                    planet.TotalPlanets = 64;
                    break;
                case PlanetType.SimilareC5:
                    planet.TotalPlanets = 256;
                    break;
                case PlanetType.UEden:
                    planet.TotalPlanets = 1;
                    break;
                case PlanetType.UFertile:
                    planet.TotalPlanets = 1;
                    break;
                case PlanetType.ULarge:
                    planet.TotalPlanets = 1;
                    break;
                case PlanetType.URich:
                    planet.TotalPlanets = 1;
                    break;
                case PlanetType.USpazial:
                    planet.TotalPlanets = 1;
                    break;
                case PlanetType.TaintedC1:
                    planet.TotalPlanets = 1;
                    break;
                case PlanetType.TaintedC2:
                    planet.TotalPlanets = 4;
                    break;
                case PlanetType.TaintedC3:
                    planet.TotalPlanets = 16;
                    break;
                case PlanetType.TaintedC4:
                    planet.TotalPlanets = 64;
                    break;
            }

            // Goods and food logic
            var goodsIncomeResult = HandleGoodsIncome(planet, user, industryIncome, creditIncome, turnsToUse);
            goodsIncome += goodsIncomeResult.Item1;
            creditIncome += goodsIncomeResult.Item2;

        }

        // Combine incomes
        creditIncome = commercialIncome + taxIncome + goodsIncome;

        // Power rating
        user.PowerRating = userPlanets.Sum(p => p.PowerRating) + userFleet.Sum(v => v.TotalPowerRating);

        // Maintenance and fleet costs
        decimal infraCost = CalculateInfraCost(userPlanets, mods);
        creditIncome -= infraCost * turnsToUse;
        decimal fleetCost = userFleet.Sum(f => f.TotalUpkeep);
        creditIncome -= fleetCost * turnsToUse;

        // Out of Damage Protection Bonus
        if (user.DamageProtection < DateTime.Now)
        {
            outOfDamageProtectionBonus = Math.Floor(creditIncome * 0.05m) * turnsToUse;
            creditIncome += outOfDamageProtectionBonus;
        }

        // Update user commodities
        UpdateUserCommodities(user, creditIncome, agricultureIncome, goodsIncome, miningTMIncome, miningRCIncome, miningWCIncome, miningCIncome, miningRIncome, miningSOIncome);
        // update cols + planet counts
        user.TotalColonies = userPlanets.Count;
        user.TotalPlanets = userPlanets.Sum(x => x.TotalPlanets);

        if(user.Commodities.Food < 0)
        {
            user.Commodities.Food = 0;
        }

        // Update turns and last action
        user.Turns.CurrentTurns -= turnsToUse;
        user.LastAction = DateTime.Now;
        SetMaxCommodities(user);
        SetMaxPowerRating(user, userPlanets);
        await _context.SaveChangesAsync();

        // Fix for CS1002 and CS0201 errors in the problematic line
        var message = "Turns Used: (" + turnsToUse.ToString() +
                      ") Credits: " + creditIncome.ToString("C0") +
                      " Food: " + agricultureIncome.ToString("N0") +
                      " Consumer Goods: " + goodsIncome.ToString("N0");
        if (outOfDamageProtectionBonus > 0)
            message += $" Bonus: {outOfDamageProtectionBonus.ToString("C0")}";

        return new TurnResult { Success = true, Message = message };
    }

    // --- Helper methods below ---
    private void SetMaxPowerRating(ApplicationUser user, List<Planets> userPlanets)
    {
        while (user.PowerRating > 1250000000)
        {
            foreach (var Ship in user.Fleets)
            {
                if(Ship.TotalShips > 1)
                {
                    Ships refShip = _context.Ships.FirstOrDefault(x => x.Id == Ship.ShipId);
                    Ship.TotalShips = (int)Math.Ceiling(Ship.TotalShips * 0.97);
                    Ship.TotalPowerRating = refShip.PowerRating * Ship.TotalShips;
                    Ship.TotalUpkeep = refShip.Upkeep * Ship.TotalShips;
                }
            }
            user.PowerRating = user.Fleets.Sum(x => x.TotalPowerRating) + userPlanets.Sum(p => p.PowerRating);
        }
    }
    private void SetMaxCommodities(ApplicationUser user)
    {
        if(user.Commodities.Credits > 5000000000000)
            user.Commodities.Credits = 5000000000000;
        if(user.Commodities.Food > 25000000000)
            user.Commodities.Food = 25000000000;
        if(user.Commodities.ConsumerGoods > 25000000000)
            user.Commodities.ConsumerGoods = 25000000000;
        if(user.Commodities.TerranMetal > 2000000000)
            user.Commodities.TerranMetal = 2000000000;
        if(user.Commodities.RedCrystal > 2000000000)
            user.Commodities.RedCrystal = 2000000000;
        if(user.Commodities.WhiteCrystal > 2000000000)
            user.Commodities.WhiteCrystal = 2000000000;
        if(user.Commodities.Rutile > 2000000000)
            user.Commodities.Rutile = 2000000000;
        if(user.Commodities.Composite > 2000000000)
            user.Commodities.Composite = 2000000000;
        if(user.Commodities.StrafezOrganism > 2000000000)
            user.Commodities.StrafezOrganism = 2000000000;
        if(user.Commodities.RawMaterial > 5000000000)
            user.Commodities.RawMaterial = 5000000000;
        if(user.Commodities.Ore > 5000000000)
            user.Commodities.Ore = 5000000000;
    }
    private decimal UpdatePlanetPopulation(Planets planet, Infrastructer infra, int turnsToUse, Faction faction, ApplicationUser user, decimal agricultureIncome)
    {
        // set max population
        planet.MaxPopulation = (int)Math.Ceiling((double)(planet.Housing * 10 + (planet.Housing * (infra.Housing + 1))));
        if (faction == Faction.Collective)
        {
            planet.MaxPopulation = (int)Math.Ceiling((double)(planet.Housing * 10 + (planet.Housing * (infra.Housing + 1))) * 2);
        }
        // Set Food Required
        var foodNeeded = planet.FoodRequired * turnsToUse;
        // Set Guardian food requirements
        if (user.Faction == Faction.Guardian)
        {
            agricultureIncome = 0;
            foodNeeded = 0;
        }
        // Check if population can grow
        if (planet.CurrentPopulation < planet.MaxPopulation && (user.Commodities.Food + agricultureIncome) > foodNeeded)
        {
            planet.CurrentPopulation += (int)Math.Floor(((planet.PopulationModifier * (planet.MaxPopulation - planet.CurrentPopulation)) * turnsToUse) / 2);

        }
        // Check if population should decrease
        else if ((user.Commodities.Food + agricultureIncome) < foodNeeded)
        {
            planet.Loyalty -= (int)Math.Floor(planet.Loyalty * 0.05m);
            planet.CurrentPopulation -= (int)Math.Floor(planet.CurrentPopulation * 0.1m);
        }
        // check if population exceeds max population
        if (planet.CurrentPopulation > planet.MaxPopulation)
        {
            planet.CurrentPopulation = planet.MaxPopulation;
        }
        
        agricultureIncome -= foodNeeded;
        return agricultureIncome;
    }

    private void UpdatePlanetResources(Planets planet, Infrastructer infra, (decimal FactionTaxModifier, decimal FactionCommercialModifier, decimal FactionIndustryModifier, decimal FactionAgricultureModifier, decimal FactionMiningModifier, decimal FactionDemandForGoods, decimal InfrastructreMaintenanceCost) mods, int turnsToUse, ApplicationUser user)
    {
        planet.AvailableLabour = (int)Math.Floor((double)(planet.CurrentPopulation - (planet.Housing + planet.Commercial + planet.Industry + planet.Agriculture + planet.Mining)));
        planet.LandAvailable = planet.TotalLand - (planet.Housing + planet.Commercial + planet.Industry + planet.Agriculture + planet.Mining);
        planet.FoodRequired = (int)Math.Floor((double)(planet.CurrentPopulation / 10));
        if(user.Faction == Faction.Guardian)
        {
           planet.FoodRequired = 0; // Guardians do not require food
        }
        planet.GoodsRequired = (int)Math.Floor((double)((planet.CurrentPopulation / 10) * mods.FactionDemandForGoods));
    }

    private void MineOre(Planets planet, Infrastructer infra, ApplicationUser user, int turnsToUse)
    {
            var oreToMine = (int)Math.Floor((double)((planet.Mining * (infra.Mining + 1) * GetFactionModifiers(user.Faction).FactionMiningModifier)) * turnsToUse);
            if (oreToMine > planet.AvailableOre)
            {
                oreToMine = planet.AvailableOre;
            }
            planet.AvailableOre -= oreToMine;
            user.Commodities.Ore += oreToMine;
    }

    private int CalculatePowerRating(Planets planet)
    {
        return (planet.CurrentPopulation / 10) + planet.Housing + planet.Commercial + planet.Agriculture + planet.Industry + planet.Mining + (planet.TotalPlanets * 1000);
    }

    private decimal CalculateTaxIncome(Planets planet, (decimal FactionTaxModifier, decimal FactionCommercialModifier, decimal FactionIndustryModifier, decimal FactionAgricultureModifier, decimal FactionMiningModifier, decimal FactionDemandForGoods, decimal InfrastructreMaintenanceCost) mods, int turnsToUse)
    {
        return Math.Floor(((planet.CurrentPopulation * (planet.Loyalty / 5000m)) + (planet.CurrentPopulation / 2m)) * mods.FactionTaxModifier) * turnsToUse;
    }

    private decimal CalculateCommercialIncome(Planets planet, Infrastructer infra, (decimal FactionTaxModifier, decimal FactionCommercialModifier, decimal FactionIndustryModifier, decimal FactionAgricultureModifier, decimal FactionMiningModifier, decimal FactionDemandForGoods, decimal InfrastructreMaintenanceCost) mods, int turnsToUse)
    {
        return Math.Floor((planet.Commercial * ((infra.Commercial * 0.5m) + 5)) * mods.FactionCommercialModifier) * turnsToUse;
    }

    private decimal CalculateAgricultureIncome(ApplicationUser user, Planets planet, Infrastructer infra, (decimal FactionTaxModifier, decimal FactionCommercialModifier, decimal FactionIndustryModifier, decimal FactionAgricultureModifier, decimal FactionMiningModifier, decimal FactionDemandForGoods, decimal InfrastructreMaintenanceCost) mods, int turnsToUse)
    {
        var agri = Math.Floor(((planet.Agriculture * ((infra.Agriculture * 0.1m) + 1)) * mods.FactionAgricultureModifier) * planet.AgricultureModifier) * turnsToUse;
        var RmGenerated = agri * 1.2m;
        user.Commodities.RawMaterial += (int)RmGenerated;
        if(user.Faction == Faction.Guardian)
        {
            agri = 0;
            RmGenerated = 0;
        }
        return agri;
    }

    private decimal CalculateIndustryIncome(ApplicationUser user,Planets planet, Infrastructer infra, (decimal FactionTaxModifier, decimal FactionCommercialModifier, decimal FactionIndustryModifier, decimal FactionAgricultureModifier, decimal FactionMiningModifier, decimal FactionDemandForGoods, decimal InfrastructreMaintenanceCost) mods, int turnsToUse)
    {
        var industry = Math.Floor((planet.Industry * ((infra.Industry * 0.1m) + 1)) * mods.FactionIndustryModifier) * turnsToUse;
        var RMneeded = industry * 0.1m;
        if (user.Commodities.RawMaterial < RMneeded)
        {
            industry = user.Commodities.RawMaterial * 10;
            user.Commodities.RawMaterial = 0;
        }
        else
        {
            // Fix for CS0266: Explicitly cast 'decimal' to 'int' when subtracting from 'user.Commodities.RawMaterial'
            user.Commodities.RawMaterial -= (int)RMneeded;
        }
        return industry;
    }

    private decimal CalculateMiningIncome(Planets planet, Infrastructer infra, (decimal FactionTaxModifier, decimal FactionCommercialModifier, decimal FactionIndustryModifier, decimal FactionAgricultureModifier, decimal FactionMiningModifier, decimal FactionDemandForGoods, decimal InfrastructreMaintenanceCost) mods, int turnsToUse)
    {
        return Math.Floor((planet.TotalPlanets * ((0.13m * infra.Mining) + 1)) * mods.FactionMiningModifier) * turnsToUse;
    }

     private (decimal,decimal) HandleGoodsIncome(Planets planet, ApplicationUser user, decimal industryIncome, decimal taxIncome, int turnsToUse)
    {
        var goodsNeeded = planet.GoodsRequired * turnsToUse;
        if ((user.Commodities.ConsumerGoods + industryIncome) >= goodsNeeded)
        {
            industryIncome -= goodsNeeded;
            taxIncome += Math.Floor(goodsNeeded * 5.5m);
            return (industryIncome, taxIncome);
        }
        else if ((user.Commodities.ConsumerGoods + industryIncome) < goodsNeeded)
        {
            goodsNeeded = (int)(user.Commodities.ConsumerGoods + industryIncome);
            taxIncome += Math.Floor(goodsNeeded * 5.5m) * turnsToUse;
            return (industryIncome, taxIncome);
        }
            return (0, 0);
    }

    private decimal CalculateInfraCost(List<Planets> planets, (decimal FactionTaxModifier, decimal FactionCommercialModifier, decimal FactionIndustryModifier, decimal FactionAgricultureModifier, decimal FactionMiningModifier, decimal FactionDemandForGoods, decimal InfrastructreMaintenanceCost) mods)
    {
        return Math.Floor(planets.Sum(h => h.Housing) + planets.Sum(c => c.Commercial) + planets.Sum(i => i.Industry) + planets.Sum(a => a.Agriculture) + planets.Sum(m => m.Mining) * mods.InfrastructreMaintenanceCost);
    }

    private static void UpdateUserCommodities(ApplicationUser user, decimal creditIncome, decimal agricultureIncome, decimal goodsIncome, decimal miningTMIncome, decimal miningRCIncome, decimal miningWCIncome, decimal miningCIncome, decimal miningRIncome, decimal miningSOIncome)
    {
        user.Commodities.Credits += (int)creditIncome;
        user.Commodities.Food += (int)agricultureIncome;
        user.Commodities.ConsumerGoods += (int)goodsIncome;
        user.Commodities.TerranMetal += (int)miningTMIncome;
        user.Commodities.RedCrystal += (int)miningRCIncome;
        user.Commodities.WhiteCrystal += (int)miningWCIncome;
        user.Commodities.Rutile += (int)miningRIncome;
        user.Commodities.Composite += (int)miningCIncome;
        user.Commodities.StrafezOrganism += (int)miningSOIncome;
    }

    // Helper for faction modifiers
    private (decimal FactionTaxModifier, decimal FactionCommercialModifier, decimal FactionIndustryModifier,
             decimal FactionAgricultureModifier, decimal FactionMiningModifier, decimal FactionDemandForGoods, decimal InfrastructreMaintenanceCost)
        GetFactionModifiers(Faction faction)
    {
        return faction switch
        {
            Faction.Terran => (1.0m, 1.2m, 2.2m, 1.2m, 1.0m, 3.5m, 1.0m),
            Faction.AMiner => (2.2m, 0.05m, 3.5m, 0.5m, 29m, 1.0m, 0.8m),
            Faction.Marauder => (0.5m, 0.05m, 0.5m, 0.5m, 0.5m, 1.0m, 0.05m),
            Faction.Viral => (1.2m, 0.95m, 0.5m, 0.8m, 1.0m, 2.0m, 1.0m),
            Faction.Collective => (0.5m, 0.05m, 0.1m, 1.5m, 0.5m, 0.05m, 1.0m),
            Faction.Guardian => (0.15m, 0.01m, 0.01m, 0.05m, 0.01m, 0.1m, 0.75m),
            Faction.KalZul => (1.0m, 1.0m, 1.0m, 1.0m, 1.0m, 3.5m, 1.0m),
            Faction.DarkMarauder => (1.0m, 1.0m, 1.0m, 1.0m, 1.0m, 3.5m, 1.0m),
            _ => (1.0m, 1.0m, 1.0m, 1.0m, 1.0m, 1.0m, 1.0m)
        };
    }
}

public class TurnResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
}

