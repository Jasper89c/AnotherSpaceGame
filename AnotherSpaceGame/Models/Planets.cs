namespace AnotherSpaceGame.Models
{
    public class Planets
    {

        public int Id { get; set; } // Primary key for EF Core

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int PlanetId { get; set; }
        public string Name { get; set; }
        public PlanetType Type { get; set; }
        public int AvailableOre { get; set; }
        public MineralType MineralProduced { get; set; }
        public int TotalLand { get; set; }
        public int FoodRequired { get; set; }
        public int GoodsRequired { get; set; }
        public int CurrentPopulation { get; set; }
        public int MaxPopulation { get; set; }
        public int Loyalty { get; set; }
        public int AvailableLabour { get; set; }
        public int LandAvailable { get; set; }
        public int Housing { get; set; }
        public int Commercial { get; set; }
        public int Industry { get; set; }
        public int Agriculture { get; set; }
        public int Mining { get; set; }
        public decimal ArtifactModifier { get; set; }
        public decimal AgricultureModifier { get; set; }
        public decimal PopulationModifier { get; set; }
        public decimal OreModifier { get; set; }
        public int PowerRating { get; set; } // Power rating for the planet
        public int TotalPlanets { get; set; } // Total number of planets
        public DateTime DateTimeAcquired { get; set; } // Date and time when the planet was acquired

        public Planets()
        {
            // Default values
            AvailableOre = 1500;
            TotalLand = 1500; 
            FoodRequired = 1;
            GoodsRequired = 1;
            CurrentPopulation = 10;
            MaxPopulation = 10; 
            Loyalty = 2500; 
            AvailableLabour = 8;
            Housing = 1;
            Commercial = 0;
            Industry = 0;
            Agriculture = 0;
            Mining = 1;
            LandAvailable = TotalLand - (Housing + Commercial + Industry + Agriculture + Mining);
            Type = PlanetType.Balanced;
            MineralProduced = MineralType.TerranMetal;
            PowerRating = 1000; // Default power rating for the planet
            TotalPlanets = 1; // Default total planets count
            DateTimeAcquired = DateTime.Now; // Default to current time
            switch (Type)   
            {
                case PlanetType.Barren:
                    PopulationModifier = 0.5m;
                    AgricultureModifier = 0;
                    OreModifier = 0.02m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.Icy:
                    PopulationModifier = 0.75m;
                    AgricultureModifier = 1;
                    OreModifier = 0.005m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.Marshy:
                    PopulationModifier = 0.8m;
                    AgricultureModifier = 0.5m;
                    OreModifier = 0.005m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.Forest:
                    PopulationModifier = 0.9m;
                    AgricultureModifier = 2;
                    OreModifier = 0.005m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.Oceanic:
                    PopulationModifier = 0.8m;
                    AgricultureModifier = 0m;
                    OreModifier = 0.005m;
                    ArtifactModifier = 0.10m;
                    break;
                case PlanetType.Rocky:
                    PopulationModifier = 0.75m;
                    AgricultureModifier = 1;
                    OreModifier = 0.001m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.Desert:
                    PopulationModifier = 0.75m;
                    AgricultureModifier = 0.75m;
                    OreModifier = 0.02m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.Balanced:
                    PopulationModifier = 1.2m;
                    AgricultureModifier = 1m;
                    OreModifier = 0.01m;
                    ArtifactModifier = 0.05m;
                    break;
                case PlanetType.Gas:
                    PopulationModifier = 1m;
                    AgricultureModifier = 0m;
                    OreModifier = 0m;
                    ArtifactModifier = 0.05m;
                    break;
                case PlanetType.ClusterLevel1:
                    PopulationModifier = 1.1m;
                    AgricultureModifier = 1.15m;
                    OreModifier = 0.02m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.ClusterLevel2:
                    PopulationModifier = 1.2m;
                    AgricultureModifier = 1.3m;
                    OreModifier = 0.02m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.ClusterLevel3:
                    PopulationModifier = 1.3m;
                    AgricultureModifier = 1.45m;
                    OreModifier = 0.02m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.URich:
                    PopulationModifier = 0.1m;
                    AgricultureModifier = 0m;
                    OreModifier = 0.03m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.UEden:
                    PopulationModifier = 10m;
                    AgricultureModifier = 0.02m;
                    OreModifier = 0m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.USpazial:
                    PopulationModifier = 0.1m;
                    AgricultureModifier = 0m;
                    OreModifier = 0m;
                    ArtifactModifier = 0.15m;
                    break;
                case PlanetType.ULarge:
                    PopulationModifier = 0.2m;
                    AgricultureModifier = 0m;
                    OreModifier = 0m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.UFertile:
                    PopulationModifier = 0.5m;
                    AgricultureModifier = 1.75m;
                    OreModifier = 0m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.Dead:
                    PopulationModifier = 0.05m;
                    AgricultureModifier = 0m;
                    OreModifier = 0m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.SimilareC1:
                    PopulationModifier = 1.2m;
                    AgricultureModifier = 0.3m;
                    OreModifier = 0.02m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.SimilareC2:
                    PopulationModifier = 1.2m;
                    AgricultureModifier = 0.35m;
                    OreModifier = 0.02m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.SimilareC3:
                    PopulationModifier = 1.2m;
                    AgricultureModifier = 0.4m;
                    OreModifier = 0.02m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.SimilareC4:
                    PopulationModifier = 1.2m;
                    AgricultureModifier = 0.45m;
                    OreModifier = 0.02m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.SimilareC5:
                    PopulationModifier = 1.2m;
                    AgricultureModifier = 0.48m;
                    OreModifier = 0.02m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.AssimilatedC1:
                    PopulationModifier = 1m;
                    AgricultureModifier = 0.8m;
                    OreModifier = 0.02m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.AssimilatedC2:
                    PopulationModifier = 1m;
                    AgricultureModifier = 0.9m;
                    OreModifier = 0.02m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.AssimilatedC3:
                    PopulationModifier = 1m;
                    AgricultureModifier = 1m;
                    OreModifier = 0.02m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.TaintedC1:
                    PopulationModifier = 0.8m;
                    AgricultureModifier = 0.8m;
                    OreModifier = 0.02m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.TaintedC2:
                    PopulationModifier = 0.7m;
                    AgricultureModifier = 0.75m;
                    OreModifier = 0.02m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.TaintedC3:
                    PopulationModifier = 0.6m;
                    AgricultureModifier = 0.7m;
                    OreModifier = 0.02m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.TaintedC4:
                    PopulationModifier = 0.5m;
                    AgricultureModifier = 0.65m;
                    OreModifier = 0.02m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.InfectedC1:
                    PopulationModifier = 0.7m;
                    AgricultureModifier = 0.9m;
                    OreModifier = 0.02m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.InfectedC2:
                    PopulationModifier = 0.65m;
                    AgricultureModifier = 1m;
                    OreModifier = 0.02m;
                    ArtifactModifier = 0.01m;
                    break;
                case PlanetType.InfectedC3:
                    PopulationModifier = 0.6m;
                    AgricultureModifier = 1.1m;
                    OreModifier = 0.02m;
                    ArtifactModifier = 0.01m;
                    break;
                default:
                    PopulationModifier = 1m;
                    AgricultureModifier = 1m;
                    OreModifier = 1m;
                    ArtifactModifier = 0.01m;
                    break;
            }
        }
    }
    
}