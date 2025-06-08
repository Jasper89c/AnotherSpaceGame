using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public class ViralResearch
    {
        public int Id { get; set; } // Primary key for EF Core

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Description("Class 1 Viral Hull")]
        public bool Class1ViralHull { get; set; }

        [Description("Class 1 Viral Hull Turns Required")]
        public int Class1ViralHullTurnsRequired { get; set; }

        [Description("Class 2 Viral Hull")]
        public bool Class2ViralHull { get; set; }

        [Description("Class 2 Viral Hull Turns Required")]
        public int Class2ViralHullTurnsRequired { get; set; }

        [Description("Advanced Exploration")]
        public bool AdvancedExploration { get; set; }

        [Description("Advanced Exploration Turns Required")]
        public int AdvancedExplorationTurnsRequired { get; set; }

        [Description("Kohoutek Scout")]
        public bool KohoutekScout { get; set; }

        [Description("Kohoutek Scout Turns Required")]
        public int KohoutekScoutTurnsRequired { get; set; }

        [Description("V-Class Destroyer")]
        public bool VClassDestroyer { get; set; }

        [Description("V-Class Destroyer Turns Required")]
        public int VClassDestroyerTurnsRequired { get; set; }

        [Description("V-Class Cruiser")]
        public bool VClassCruiser { get; set; }

        [Description("V-Class Cruiser Turns Required")]
        public int VClassCruiserTurnsRequired { get; set; }

        [Description("B-Class Cruiser")]
        public bool BClassCruiser { get; set; }

        [Description("B-Class Cruiser Turns Required")]
        public int BClassCruiserTurnsRequired { get; set; }

        [Description("B-Class Starbase")]
        public bool BClassStarbase { get; set; }

        [Description("B-Class Starbase Turns Required")]
        public int BClassStarbaseTurnsRequired { get; set; }

        public ViralResearch()
        {
            // Initialize default values if necessary
            Class1ViralHull = false;
            Class1ViralHullTurnsRequired = 125;
            Class2ViralHull = false;
            Class2ViralHullTurnsRequired = 225;
            AdvancedExploration = false;
            AdvancedExplorationTurnsRequired = 120;
            KohoutekScout = false;
            KohoutekScoutTurnsRequired = 90;
            VClassDestroyer = false;
            VClassDestroyerTurnsRequired = 80;
            VClassCruiser = false;
            VClassCruiserTurnsRequired = 90;
            BClassCruiser = false;
            BClassCruiserTurnsRequired = 110;
            BClassStarbase = false;
            BClassStarbaseTurnsRequired = 130;
        }
    }
}