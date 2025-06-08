using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public class StrafezResearch
    {
        public int Id { get; set; } // Primary key for EF Core

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Description("Basic Genetic Engineering")]
        public bool BasicGeneticEngineering { get; set; }

        [Description("Basic Genetic Engineering Turns Required")]
        public int BasicGeneticEngineeringTurnsRequired { get; set; }

        [Description("Small Genetic Engineering")]
        public bool SmallGeneticEngineering { get; set; }

        [Description("Small Genetic Engineering Turns Required")]
        public int SmallGeneticEngineeringTurnsRequired { get; set; }

        [Description("Large Genetic Engineering")]
        public bool LargeGeneticEngineering { get; set; }

        [Description("Large Genetic Engineering Turns Required")]
        public int LargeGeneticEngineeringTurnsRequired { get; set; }

        [Description("Small Strafez And Runner Fodder")]
        public bool SmallStrafezRunnerFodder { get; set; }

        [Description("Small Strafez Runner And Fodder Turns Required")]
        public int SmallStrafezRunnerFodderTurnsRequired { get; set; }

        [Description("Large Strafez Runner And Fodder")]
        public bool LargeStrafezRunnerFodder { get; set; }

        [Description("Large Strafez Runner And Fodder Turns Required")]
        public int LargeStrafezRunnerFodderTurnsRequired { get; set; }

        [Description("Strafez And Queen King")]
        public bool StrafezQueenKing { get; set; }

        [Description("Strafez Queen And King Turns Required")]
        public int StrafezQueenKingTurnsRequired { get; set; }

        public StrafezResearch()
        {
            // Initialize default values if necessary
            BasicGeneticEngineering = false;
            BasicGeneticEngineeringTurnsRequired = 25;
            SmallGeneticEngineering = false;
            SmallGeneticEngineeringTurnsRequired = 20;
            LargeGeneticEngineering = false;
            LargeGeneticEngineeringTurnsRequired = 20;
            SmallStrafezRunnerFodder = false;
            SmallStrafezRunnerFodderTurnsRequired = 20;
            LargeStrafezRunnerFodder = false;
            LargeStrafezRunnerFodderTurnsRequired = 60;
            StrafezQueenKing = false;
            StrafezQueenKingTurnsRequired = 80;
        }
    }
}