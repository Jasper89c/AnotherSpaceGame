using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public class FClassResearch
    {
        public int Id { get; set; } // Primary key for EF Core

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Description("F-Class Engineering")]
        public bool FClassEngineering { get; set; }

        [Description("F-Class Engineering Turns Required")]
        public int FClassEngineeringTurnsRequired { get; set; }

        [Description("F-Class Frigate")]
        public bool FClassFrigate { get; set; }

        [Description("F-Class Frigate Turns Required")]
        public int FClassFrigateTurnsRequired { get; set; }

        [Description("F-Class Destroyer")]
        public bool FClassDestroyer { get; set; }

        [Description("F-Class Destroyer Turns Required")]
        public int FClassDestroyerTurnsRequired { get; set; }

        [Description("F-Class Cruiser")]
        public bool FClassCruiser { get; set; }

        [Description("F-Class Cruiser Turns Required")]
        public int FClassCruiserTurnsRequired { get; set; }

        public FClassResearch()
        {
            // Initialize default values if needed
            FClassEngineering = false;
            FClassEngineeringTurnsRequired = 70;
            FClassFrigate = false;
            FClassFrigateTurnsRequired = 50;
            FClassDestroyer = false;
            FClassDestroyerTurnsRequired = 60;
            FClassCruiser = false;
            FClassCruiserTurnsRequired = 70;
        }
    }
}