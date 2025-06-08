using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public class GuardianResearch
    {
        public int Id { get; set; } // Primary key for EF Core

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Description("Fighter Class")]
        public bool FighterClass { get; set; }

        [Description("Fighter Class Turns Required")]
        public int FighterClassTurnsRequired { get; set; }

        [Description("Corvette Class")]
        public bool CorvetteClass { get; set; }

        [Description("Corvette Class Turns Required")]
        public int CorvetteClassTurnsRequired { get; set; }

        [Description("Frigate Class")]
        public bool FrigateClass { get; set; }

        [Description("Frigate Class Turns Required")]
        public int FrigateClassTurnsRequired { get; set; }

        [Description("Destroyer Class")]
        public bool DestroyerClass { get; set; }

        [Description("Destroyer Class Turns Required")]
        public int DestroyerClassTurnsRequired { get; set; }

        [Description("Cruiser Class")]
        public bool CruiserClass { get; set; }

        [Description("Cruiser Class Turns Required")]
        public int CruiserClassTurnsRequired { get; set; }

        [Description("Weapons Platform")]
        public bool WeaponsPlatform { get; set; }

        [Description("Weapons Platform Turns Required")]
        public int WeaponsPlatformTurnsRequired { get; set; }

        [Description("Advanced Scouts")]
        public bool AdvancedScouts { get; set; }

        [Description("Advanced Scouts Turns Required")]
        public int AdvancedScoutsTurnsRequired { get; set; }

        [Description("L-Class Frigate")]
        public bool LClassFrigate { get; set; }

        [Description("L-Class Frigate Turns Required")]
        public int LClassFrigateTurnsRequired { get; set; }

        [Description("L-Class Destroyer")]
        public bool LClassDestroyer { get; set; }

        [Description("L-Class Destroyer Turns Required")]
        public int LClassDestroyerTurnsRequired { get; set; }

        [Description("L-Class Cruiser")]
        public bool LClassCruiser { get; set; }

        [Description("L-Class Cruiser Turns Required")]
        public int LClassCruiserTurnsRequired { get; set; }

        [Description("K-Class Destroyer")]
        public bool KClassDestroyer { get; set; }

        [Description("K-Class Destroyer Turns Required")]
        public int KClassDestroyerTurnsRequired { get; set; }

        [Description("K-Class Cruiser")]
        public bool KClassCruiser { get; set; }

        [Description("K-Class Cruiser Turns Required")]
        public int KClassCruiserTurnsRequired { get; set; }

        [Description("Advanced Engineering")]
        public bool AdvancedEngineering { get; set; }

        [Description("Advanced Engineering Turns Required")]
        public int AdvancedEngineeringTurnsRequired { get; set; }

        [Description("Focus Crystal")]
        public bool FocusCrystal { get; set; }

        [Description("Focus Crystal Turns Required")]
        public int FocusCrystalTurnsRequired { get; set; }

        [Description("Organic Sensors")]
        public bool OrganicSensors { get; set; }

        [Description("Organic Sensors Turns Required")]
        public int OrganicSensorsTurnsRequired { get; set; }

        [Description("Lance Torpodoes")]
        public bool LanceTorpodoes { get; set; }

        [Description("Lance Torpodoes Turns Required")]
        public int LanceTorpodoesTurnsRequired { get; set; }

        public GuardianResearch()
        {
            // Initialize default values if needed
            FighterClassTurnsRequired = 40;
            FighterClass = false;
            CorvetteClassTurnsRequired = 80;
            CorvetteClass = false;
            FrigateClassTurnsRequired = 120;
            FrigateClass = false;
            DestroyerClassTurnsRequired = 160;
            DestroyerClass = false;
            CruiserClassTurnsRequired = 200;
            CruiserClass = false;
            WeaponsPlatformTurnsRequired = 160;
            WeaponsPlatform = false;
            AdvancedScoutsTurnsRequired = 120;
            AdvancedScouts = false;
            LClassFrigateTurnsRequired = 50;
            LClassFrigate = false;
            LClassDestroyerTurnsRequired = 70;
            LClassDestroyer = false;
            LClassCruiserTurnsRequired = 90;
            LClassCruiser = false;
            KClassDestroyerTurnsRequired = 5000;
            KClassDestroyer = false;
            KClassCruiserTurnsRequired = 2500;
            KClassCruiser = false;
            AdvancedEngineeringTurnsRequired = 320;
            AdvancedEngineering = false;
            FocusCrystalTurnsRequired = 40;
            FocusCrystal = false;
            OrganicSensorsTurnsRequired = 70;
            OrganicSensors = false;
            LanceTorpodoesTurnsRequired = 120;
            LanceTorpodoes = false;
        }
    }
}