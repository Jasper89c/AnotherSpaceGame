using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public class MarauderResearch
    {
        public int Id { get; set; } // Primary key for EF Core

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Description("Class 3 Marauder Hull")]
        public bool Class3MarauderHull { get; set; }

        [Description("Class 3 Marauder Hull Turns Required")]
        public int Class3MarauderHullTurnsRequired { get; set; }
        [Description("Class 4 Marauder Hull")]
        public bool Class4MarauderHull { get; set; }

        [Description("Class 4 Marauder Hull Turns Required")]
        public int Class4MarauderHullTurnsRequired { get; set; }

        [Description("Class 5 Marauder Hull")]
        public bool Class5MarauderHull { get; set; }

        [Description("Class 5 Marauder Hull Turns Required")]
        public int Class5MarauderHullTurnsRequired { get; set; }

        [Description("Class 6 Marauder Hull")]
        public bool Class6MarauderHull { get; set; }

        [Description("Class 6 Marauder Hull Turns Required")]
        public int Class6MarauderHullTurnsRequired { get; set; }

        [Description("Advanced Engineering")]
        public bool AdvancedEngineering { get; set; }

        [Description("Advanced Engineering Turns Required")]
        public int AdvancedEngineeringTurnsRequired { get; set; }

        [Description("Marauder Corvette")]
        public bool MarauderCorvette { get; set; }

        [Description("Marauder Corvette Turns Required")]
        public int MarauderCorvetteTurnsRequired { get; set; }

        [Description("Marauder Frigate")]
        public bool MarauderFrigate { get; set; }

        [Description("Marauder Frigate Turns Required")]
        public int MarauderFrigateTurnsRequired { get; set; }

        [Description("Marauder Destroyer")]
        public bool MarauderDestroyer { get; set; }

        [Description("Marauder Destroyer Turns Required")]
        public int MarauderDestroyerTurnsRequired { get; set; }

        [Description("Marauder Cruiser")]
        public bool MarauderCruiser { get; set; }

        [Description("Marauder Cruiser Turns Required")]
        public int MarauderCruiserTurnsRequired { get; set; }

        [Description("Marauder Battleship")]
        public bool MarauderBattleship { get; set; }

        [Description("Marauder Battleship Turns Required")]
        public int MarauderBattleshipTurnsRequired { get; set; }

        [Description("Type D Frigate")]
        public bool TypeDFrigate { get; set; }

        [Description("Type D Frigate Turns Required")]
        public int TypeDFrigateTurnsRequired { get; set; }

        [Description("Type D Destroyer")]
        public bool TypeDDestroyer { get; set; }

        [Description("Type D Destroyer Turns Required")]
        public int TypeDDestroyerTurnsRequired { get; set; }

        [Description("Type D Cruiser")]
        public bool TypeDCruiser { get; set; }

        [Description("Type D Cruiser Turns Required")]
        public int TypeDCruiserTurnsRequired { get; set; }
        [Description("Type D Battleship")]
        public bool TypeDBattleship { get; set; }

        [Description("Type D Battleship Turns Required")]
        public int TypeDBattleshipTurnsRequired { get; set; }

        public MarauderResearch()
        {
            // Initialize default values
            Class3MarauderHull = false;
            Class3MarauderHullTurnsRequired = 140;
            Class4MarauderHull = false;
            Class4MarauderHullTurnsRequired = 160;
            Class5MarauderHull = false;
            Class5MarauderHullTurnsRequired = 180;
            Class6MarauderHull = false;
            Class6MarauderHullTurnsRequired = 180;
            AdvancedEngineering = false;
            AdvancedEngineeringTurnsRequired = 320;
            MarauderCorvette = false;
            MarauderCorvetteTurnsRequired = 70;
            MarauderFrigate = false;
            MarauderFrigateTurnsRequired = 90;
            MarauderDestroyer = false;
            MarauderDestroyerTurnsRequired = 110;
            MarauderCruiser = false;
            MarauderCruiserTurnsRequired = 130;
            MarauderBattleship = false;
            MarauderBattleshipTurnsRequired = 150;
            TypeDFrigate = false;
            TypeDFrigateTurnsRequired = 140;
            TypeDDestroyer = false;
            TypeDDestroyerTurnsRequired = 160;
            TypeDCruiser = false;
            TypeDCruiserTurnsRequired = 200;
            TypeDBattleship = false;
            TypeDBattleshipTurnsRequired = 240;
        }
    }
}