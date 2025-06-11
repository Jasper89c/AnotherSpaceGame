using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public class CyrilClassResearch
    {
        public int Id { get; set; } // Primary key for EF Core

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Description("Cyril Alloy")]
        public bool CyrilAlloy { get; set; }

        [Description("Cyril Alloy Turns Required")]
        public int CyrilAlloyTurnsRequired { get; set; }

        [Description("Small Cyril Hull")]
        public bool SmallCyrilHull { get; set; }

        [Description("Small Cyril Hull Turns Required")]
        public int SmallCyrilHullTurnsRequired { get; set; }

        [Description("Medium Cyril Hull")]
        public bool MediumCyrilHull { get; set; }

        [Description("Medium Cyril Hull Turns Required")]
        public int MediumCyrilHullTurnsRequired { get; set; }

        [Description("Large Cyril Hull")]
        public bool LargeCyrilHull { get; set; }

        [Description("Large Cyril Hull Turns Required")]
        public int LargeCyrilHullTurnsRequired { get; set; }

        [Description("Cyril Corvette")]
        public bool CyrilCorvette { get; set; }

        [Description("Cyril Corvette Turns Required")]
        public int CyrilCorvetteTurnsRequired { get; set; }

        [Description("Cyril Frigate")]
        public bool CyrilFrigate { get; set; }

        [Description("Cyril Frigate Turns Required")]
        public int CyrilFrigateTurnsRequired { get; set; }

        [Description("Cyril Destroyer")]
        public bool CyrilDestroyer { get; set; }

        [Description("Cyril Destroyer Turns Required")]
        public int CyrilDestroyerTurnsRequired { get; set; }

        [Description("Cyril Cruiser")]
        public bool CyrilCruiser { get; set; }

        [Description("Cyril Cruiser Turns Required")]
        public int CyrilCruiserTurnsRequired { get; set; }

        public CyrilClassResearch()
        {
            // Initialize default values
            CyrilAlloy = false;
            CyrilAlloyTurnsRequired = 20;
            SmallCyrilHull = false;
            SmallCyrilHullTurnsRequired = 40;
            MediumCyrilHull = false;
            MediumCyrilHullTurnsRequired = 60;
            LargeCyrilHull = false;
            LargeCyrilHullTurnsRequired = 80;
            CyrilCorvette = false;
            CyrilCorvetteTurnsRequired = 40;
            CyrilFrigate = false;
            CyrilFrigateTurnsRequired = 60;
            CyrilDestroyer = false;
            CyrilDestroyerTurnsRequired = 80;
            CyrilCruiser = false;
            CyrilCruiserTurnsRequired = 90;
        }
    }
}