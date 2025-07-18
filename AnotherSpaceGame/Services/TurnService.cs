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
        // Fix for CS0131: The left-hand side of an assignment must be a variable, property or indexer
        decimal creditIncome = 0;

        foreach (var planet in userPlanets)
        {
            UpdatePlanetPopulation(planet, userInfrastructer, turnsToUse,user.Faction);
            UpdatePlanetResources(planet, userInfrastructer, mods, turnsToUse,user);

            // Mining
            MineOre(planet, userInfrastructer, user, turnsToUse);

            // Power rating
            planet.PowerRating = CalculatePowerRating(planet);

            // Incomes
            taxIncome += CalculateTaxIncome(planet, mods, turnsToUse);
            commercialIncome += CalculateCommercialIncome(planet, userInfrastructer, mods, turnsToUse);
            agricultureIncome += CalculateAgricultureIncome(user, planet, userInfrastructer, mods, turnsToUse);
            industryIncome += CalculateIndustryIncome(user,planet, userInfrastructer, mods, turnsToUse);

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

            // Goods and food logic
            var goodsIncomeResult = HandleGoodsIncome(planet, user, ref industryIncome, creditIncome, turnsToUse);
            goodsIncome += goodsIncomeResult.Item1;
            creditIncome += goodsIncomeResult.Item2;

            HandleFoodLogic(planet, user, ref agricultureIncome, turnsToUse);
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

        // Update turns and last action
        user.Turns.CurrentTurns -= turnsToUse;
        user.LastAction = DateTime.Now;

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

    private void UpdatePlanetPopulation(Planets planet, Infrastructer infra, int turnsToUse, Faction faction)
    {
        planet.MaxPopulation = (int)Math.Floor((double)(planet.Housing * 10 + (planet.Housing * infra.Housing)));
        if (faction == Faction.Collective)
        {
            planet.MaxPopulation = (int)Math.Floor(planet.MaxPopulation * 2m);
        }
        if (planet.CurrentPopulation < planet.MaxPopulation)
        {
            planet.CurrentPopulation += (int)Math.Floor(((planet.PopulationModifier * (planet.MaxPopulation - planet.CurrentPopulation)) * turnsToUse) / 2 );
            if (planet.CurrentPopulation > planet.MaxPopulation)
                planet.CurrentPopulation = planet.MaxPopulation;
        }
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
        if (planet.AvailableOre > 0 && planet.Mining > 0)
        {
            var oreToMine = (int)Math.Floor((double)(planet.Mining * infra.Mining) * turnsToUse);
            if (oreToMine < 1) oreToMine = 1;
            if (oreToMine > planet.AvailableOre) oreToMine = planet.AvailableOre;
            planet.AvailableOre -= oreToMine;
            user.Commodities.Ore += oreToMine;
        }
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

     private (decimal,decimal) HandleGoodsIncome(Planets planet, ApplicationUser user, ref decimal industryIncome, decimal taxIncome, int turnsToUse)
    {
        var goodsNeeded = planet.GoodsRequired * turnsToUse;
        if ((user.Commodities.ConsumerGoods + industryIncome) >= goodsNeeded)
        {
            industryIncome -= goodsNeeded;
            taxIncome += Math.Floor(goodsNeeded * 5.5m) * turnsToUse;
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

    private void HandleFoodLogic(Planets planet, ApplicationUser user, ref decimal agricultureIncome, int turnsToUse)
    {
        var foodNeeded = planet.FoodRequired * turnsToUse;
        if(user.Faction == Faction.Guardian)
        {
            agricultureIncome = 0;
            foodNeeded = 0;
        }
        if ((user.Commodities.Food + agricultureIncome) < foodNeeded && user.Faction != Faction.Guardian)
        {
            planet.Loyalty -= (int)Math.Floor(planet.Loyalty * 0.1m);
            planet.CurrentPopulation -= (int)Math.Floor(planet.CurrentPopulation * 0.1m);
        }
        agricultureIncome -= foodNeeded;
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
            Faction.Terran => (1.0m, 1.1m, 2.2m, 1.2m, 1.0m, 3.5m, 1.0m),
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

