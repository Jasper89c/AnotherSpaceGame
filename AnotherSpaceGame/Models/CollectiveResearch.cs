using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public class CollectiveResearch
    {
        public int Id { get; set; } // Primary key for EF Core

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Description("Fighter Class")]
        public bool FighterClass { get; set; }

        [Description("Fighter Class Turns Required")]
        public int FighterClassTurnsRequired { get; set; }

        [Description("Frigate Class")]
        public bool FrigateClass { get; set; }

        [Description("Frigate Class Turns Required")]
        public int FrigateClassTurnsRequired { get; set; }
        [Description("R-Class Frigate")]
        public bool RClassFrigate { get; set; }

        [Description("R-Class Frigate Turns Required")]
        public int RClassFrigateTurnsRequired { get; set; }

        [Description("Destroyer Class")]
        public bool DestroyerClass { get; set; }

        [Description("Destroyer Class Turns Required")]
        public int DestroyerClassTurnsRequired { get; set; }

        [Description("Cruiser Class")]
        public bool CruiserClass { get; set; }

        [Description("Cruiser Class Turns Required")]
        public int CruiserClassTurnsRequired { get; set; }

        [Description("H-Class Ships")]
        public bool HClassShips { get; set; }

        [Description("H-Class Ships Turns Required")]
        public int HClassShipsTurnsRequired { get; set; }

        [Description("R-Class Corvette")]
        public bool RClassCorvette { get; set; }

        [Description("R-Class Corvette Turns Required")]
        public int RClassCorvetteTurnsRequired { get; set; }

        [Description("H-Class Destroyer")]
        public bool HClassDestroyer { get; set; }

        [Description("H-Class Destroyer Turns Required")]
        public int HClassDestroyerTurnsRequired { get; set; }

        [Description("R-Class Destroyer")]
        public bool RClassDestroyer { get; set; }

        [Description("R-Class Destroyer Turns Required")]
        public int RClassDestroyerTurnsRequired { get; set; }

        [Description("H-Class Cruiser")]
        public bool HClassCruiser { get; set; }

        [Description("H-Class Cruiser Turns Required")]
        public int HClassCruiserTurnsRequired { get; set; }

        [Description("R-Class Battleship")]
        public bool RClassBattleship { get; set; }

        [Description("R-Class Battleship Turns Required")]
        public int RClassBattleshipTurnsRequired { get; set; }

        [Description("H-Class Dreadnought")]
        public bool HClassDreadnought { get; set; }

        [Description("H-Class Dreadnought Turns Required")]
        public int HClassDreadnoughtTurnsRequired { get; set; }

        [Description("R-Class Dreadnought")]
        public bool RClassDreadnought { get; set; }

        [Description("R-Class Dreadnought Turns Required")]
        public int RClassDreadnoughtTurnsRequired { get; set; }

        [Description("R-Class Juggernaught")]
        public bool RClassJuggernaught { get; set; }

        [Description("R-Class Juggernaught Turns Required")]
        public int RClassJuggernaughtTurnsRequired { get; set; }

        [Description("H-Class Leviathan")]
        public bool HClassLeviathan { get; set; }

        [Description("H-Class Leviathan Turns Required")]
        public int HClassLeviathanTurnsRequired { get; set; }

        [Description("H-Class Starbase")]
        public bool HClassStarbase { get; set; }

        [Description("H-Class Starbase Turns Required")]
        public int HClassStarbaseTurnsRequired { get; set; }

        public CollectiveResearch()
        {
            // Initialize default values
            FighterClass = false;
            FighterClassTurnsRequired = 60;
            FrigateClass = false;
            FrigateClassTurnsRequired = 100;
            DestroyerClass = false;
            DestroyerClassTurnsRequired = 120;
            CruiserClass = false;
            CruiserClassTurnsRequired = 140;
            HClassShips = false;
            HClassShipsTurnsRequired = 210;
            RClassFrigate = false;
            RClassFrigateTurnsRequired = 80;
            HClassDestroyer = false;
            HClassDestroyerTurnsRequired = 90;
            RClassDestroyer = false;
            RClassDestroyerTurnsRequired = 90;
            HClassCruiser = false;
            HClassCruiserTurnsRequired = 110;
            RClassBattleship = false;
            RClassBattleshipTurnsRequired = 180;
            HClassDreadnought = false;
            HClassDreadnoughtTurnsRequired = 210;
            RClassDreadnought = false;
            RClassDreadnoughtTurnsRequired = 210;
            RClassJuggernaught = false;
            RClassJuggernaughtTurnsRequired = 250;
            HClassLeviathan = false;
            HClassLeviathanTurnsRequired = 500;
            HClassStarbase = false;
            HClassStarbaseTurnsRequired = 190;
        }
    }
}