using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Numerics;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class IncomeSimModel : PageModel
    {
        [BindProperty]
        public List<IncomeSimPlanetInput> Planets { get; set; } = new();
        [BindProperty]
        public InfrastructureInput Infrastructure { get; set; }
        public decimal TaxCreditsPerTurn { get; set; }
        public decimal TaxCreditsPerTurnWithGoods { get; set; }
        public decimal CommercialCreditsPerTurn { get; set; }
        public decimal AgriculturePerTurn { get; set; }
        public decimal AgriculturePerTurnMinusFood { get; set; }
        public decimal IndustryPerTurn { get; set; }
        public decimal IndustryPerTurnMinusGoodsEaten { get; set; }
        public decimal MiningPerTurn { get; set; }
        public decimal FoodNeeded { get; set; }
        public decimal GoodsNeeded { get; set; }
        [BindProperty]
        public Faction Faction { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var _planets = new List<Planets>();
            foreach (var p in Planets)
            {
                var planet = new Planets
                {
                    PlanetId = 1,
                    Name = "a",
                    Type = p.PlanetType,
                    AvailableOre = 0,
                    MineralProduced = p.MineralType,
                    TotalLand = 1,
                    FoodRequired = p.Population / 10,
                    GoodsRequired = p.Population / 10,
                    CurrentPopulation = p.Population,
                    MaxPopulation = p.Population,
                    Loyalty = p.Loyalty,
                    AvailableLabour = 1,
                    LandAvailable = 1,
                    Housing = 1,
                    Commercial = p.Commercial,
                    Industry = p.Industry,
                    Agriculture = p.Agriculture,
                    Mining = p.Mining,
                    PowerRating = 1,
                    DateTimeAcquired = DateTime.Now
                };

                SetPlanetModifiers(planet); // <-- Set modifiers based on type

                _planets.Add(planet);
            }

            foreach (var planet in _planets)
            {
                // Calculate income from each planet
                TaxCreditsPerTurn += Math.Floor(((planet.CurrentPopulation * (planet.Loyalty / 5000m)) + (planet.CurrentPopulation / 2m)) * GetFactionModifiers(Faction).FactionTaxModifier);
                CommercialCreditsPerTurn += Math.Floor((planet.Commercial * ((Infrastructure.Commercial * 0.5m) + 5)) * GetFactionModifiers(Faction).FactionCommercialModifier);
                AgriculturePerTurn += Math.Floor(((planet.Agriculture * ((Infrastructure.Agriculture * 0.1m) + 1)) * GetFactionModifiers(Faction).FactionAgricultureModifier) * planet.AgricultureModifier);
                IndustryPerTurn += Math.Floor((planet.Industry * ((Infrastructure.Industry * 0.1m) + 1)) * GetFactionModifiers(Faction).FactionIndustryModifier);
                MiningPerTurn += Math.Floor((planet.TotalPlanets * ((0.13m * Infrastructure.Mining) + 1)) * GetFactionModifiers(Faction).FactionMiningModifier);
                FoodNeeded += planet.FoodRequired;
                GoodsNeeded += planet.GoodsRequired;
            }
            // Calulate goods income
            TaxCreditsPerTurnWithGoods = Math.Floor(TaxCreditsPerTurn + (GoodsNeeded * 5.5m));
            IndustryPerTurnMinusGoodsEaten = IndustryPerTurn - GoodsNeeded;
            // Calculate food income
            AgriculturePerTurnMinusFood = AgriculturePerTurn - FoodNeeded;


            return Page();
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
        private void SetPlanetModifiers(Planets planet)
        {
            switch (planet.Type)
            {
                case PlanetType.Barren:
                    planet.PopulationModifier = 0.5m;
                    planet.AgricultureModifier = 0;
                    planet.OreModifier = 0.02m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.Icy:
                    planet.PopulationModifier = 0.75m;
                    planet.AgricultureModifier = 1;
                    planet.OreModifier = 0.005m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.Marshy:
                    planet.PopulationModifier = 0.8m;
                    planet.AgricultureModifier = 0.5m;
                    planet.OreModifier = 0.005m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.Forest:
                    planet.PopulationModifier = 0.9m;
                    planet.AgricultureModifier = 2;
                    planet.OreModifier = 0.005m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.Oceanic:
                    planet.PopulationModifier = 0.8m;
                    planet.AgricultureModifier = 0m;
                    planet.OreModifier = 0.005m;
                    planet.ArtifactModifier = 0.10m;
                    break;
                case PlanetType.Rocky:
                    planet.PopulationModifier = 0.75m;
                    planet.AgricultureModifier = 1;
                    planet.OreModifier = 0.001m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.Desert:
                    planet.PopulationModifier = 0.75m;
                    planet.AgricultureModifier = 0.75m;
                    planet.OreModifier = 0.02m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.Balanced:
                    planet.PopulationModifier = 1.2m;
                    planet.AgricultureModifier = 1m;
                    planet.OreModifier = 0.01m;
                    planet.ArtifactModifier = 0.05m;
                    break;
                case PlanetType.Gas:
                    planet.PopulationModifier = 1m;
                    planet.AgricultureModifier = 0m;
                    planet.OreModifier = 0m;
                    planet.ArtifactModifier = 0.05m;
                    break;
                case PlanetType.ClusterLevel1:
                    planet.PopulationModifier = 1.1m;
                    planet.AgricultureModifier = 1.15m;
                    planet.OreModifier = 0.02m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.ClusterLevel2:
                    planet.PopulationModifier = 1.2m;
                    planet.AgricultureModifier = 1.3m;
                    planet.OreModifier = 0.02m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.ClusterLevel3:
                    planet.PopulationModifier = 1.3m;
                    planet.AgricultureModifier = 1.45m;
                    planet.OreModifier = 0.02m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.URich:
                    planet.PopulationModifier = 0.1m;
                    planet.AgricultureModifier = 0m;
                    planet.OreModifier = 0.03m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.UEden:
                    planet.PopulationModifier = 10m;
                    planet.AgricultureModifier = 0.02m;
                    planet.OreModifier = 0m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.USpazial:
                    planet.PopulationModifier = 0.1m;
                    planet.AgricultureModifier = 0m;
                    planet.OreModifier = 0m;
                    planet.ArtifactModifier = 0.15m;
                    break;
                case PlanetType.ULarge:
                    planet.PopulationModifier = 0.2m;
                    planet.AgricultureModifier = 0m;
                    planet.OreModifier = 0m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.UFertile:
                    planet.PopulationModifier = 0.5m;
                    planet.AgricultureModifier = 1.75m;
                    planet.OreModifier = 0m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.Dead:
                    planet.PopulationModifier = 0.05m;
                    planet.AgricultureModifier = 0m;
                    planet.OreModifier = 0m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.SimilareC1:
                    planet.PopulationModifier = 1.2m;
                    planet.AgricultureModifier = 0.3m;
                    planet.OreModifier = 0.02m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.SimilareC2:
                    planet.PopulationModifier = 1.2m;
                    planet.AgricultureModifier = 0.35m;
                    planet.OreModifier = 0.02m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.SimilareC3:
                    planet.PopulationModifier = 1.2m;
                    planet.AgricultureModifier = 0.4m;
                    planet.OreModifier = 0.02m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.SimilareC4:
                    planet.PopulationModifier = 1.2m;
                    planet.AgricultureModifier = 0.45m;
                    planet.OreModifier = 0.02m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.SimilareC5:
                    planet.PopulationModifier = 1.2m;
                    planet.AgricultureModifier = 0.48m;
                    planet.OreModifier = 0.02m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.AssimilatedC1:
                    planet.PopulationModifier = 1m;
                    planet.AgricultureModifier = 0.8m;
                    planet.OreModifier = 0.02m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.AssimilatedC2:
                    planet.PopulationModifier = 1m;
                    planet.AgricultureModifier = 0.9m;
                    planet.OreModifier = 0.02m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.AssimilatedC3:
                    planet.PopulationModifier = 1m;
                    planet.AgricultureModifier = 1m;
                    planet.OreModifier = 0.02m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.TaintedC1:
                    planet.PopulationModifier = 0.8m;
                    planet.AgricultureModifier = 0.8m;
                    planet.OreModifier = 0.02m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.TaintedC2:
                    planet.PopulationModifier = 0.7m;
                    planet.AgricultureModifier = 0.75m;
                    planet.OreModifier = 0.02m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.TaintedC3:
                    planet.PopulationModifier = 0.6m;
                    planet.AgricultureModifier = 0.7m;
                    planet.OreModifier = 0.02m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.TaintedC4:
                    planet.PopulationModifier = 0.5m;
                    planet.AgricultureModifier = 0.65m;
                    planet.OreModifier = 0.02m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.InfectedC1:
                    planet.PopulationModifier = 0.7m;
                    planet.AgricultureModifier = 0.9m;
                    planet.OreModifier = 0.02m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.InfectedC2:
                    planet.PopulationModifier = 0.65m;
                    planet.AgricultureModifier = 1m;
                    planet.OreModifier = 0.02m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                case PlanetType.InfectedC3:
                    planet.PopulationModifier = 0.6m;
                    planet.AgricultureModifier = 1.1m;
                    planet.OreModifier = 0.02m;
                    planet.ArtifactModifier = 0.01m;
                    break;
                default:
                    planet.PopulationModifier = 1m;
                    planet.AgricultureModifier = 1m;
                    planet.OreModifier = 1m;
                    planet.ArtifactModifier = 0.01m;
                    break;
            }
        }
        public class InfrastructureInput
        {
            public int Housing { get; set; }
            public int Commercial { get; set; }
            public int Agriculture { get; set; }
            public int Industry { get; set; }
            public int Mining { get; set; }
        }
        public class IncomeSimPlanetInput
        {
            public PlanetType PlanetType { get; set; }
            public int Population { get; set; }
            public int Commercial { get; set; }
            public int Agriculture { get; set; }
            public int Industry { get; set; }
            public int Mining { get; set; }
            public int Loyalty { get; set; }
            public MineralType MineralType { get; set; }
        }
    }
}
