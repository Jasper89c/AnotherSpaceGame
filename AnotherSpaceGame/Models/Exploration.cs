using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnotherSpaceGame.Models
{
    public class Exploration
    {
        [Key]
        public int Id { get; set; }

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Description("Exploration Points Needed")]
        public long ExplorationPointsNeeded { get; set; }

        [Description("Exploration Completed")]
        public decimal ExplorationCompleted { get; set; }

        [Description("Turns Required")]
        public int TurnsRequired { get; set; }

        [Description("Ships In Fleet")]
        public int ShipsInFleet { get; set; }

        [Description("Scanning Power")]
        public int ScanningPower { get; set; }

        [Description("Total Planets")]
        public int TotalPlanets { get; set; }

        [Description("Total Colonies")]
        public int TotalColonies { get; set; }
        public bool EnableFeature { get; set; }
        public Exploration()
        {
            ExplorationPointsNeeded = 5000;
            ExplorationCompleted = 0m;
            TurnsRequired = 1;
            ShipsInFleet = 0;
            ScanningPower = 0;
            TotalPlanets = 1;
            TotalColonies = 1;
        }

    }
}