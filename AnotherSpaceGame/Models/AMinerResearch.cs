using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public class AMinerResearch
    {
        public int Id { get; set; } // Primary key for EF Core

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Description("Starship Engineering")]
        public bool StarshipEngineering { get; set; }

        [Description("Starship Engineering Turns Required")]
        public int StarshipEngineeringTurnsRequired { get; set; }

        [Description("Flagship Engineering")]
        public bool FlagshipEngineering { get; set; }

        [Description("Flagship Engineering Turns Required")]
        public int FlagshipEngineeringTurnsRequired { get; set; }

        [Description("Advanced Engineering")]
        public bool AdvancedEngineering { get; set; }

        [Description("Advanced Engineering Turns Required")]
        public int AdvancedEngineeringTurnsRequired { get; set; }

        [Description("Sensor Engineering")]
        public bool SensorEngineering { get; set; }

        [Description("Sensor Engineering Turns Required")]
        public int SensorEngineeringTurnsRequired { get; set; }

        [Description("Starbase Engineering")]
        public bool StarbaseEngineering { get; set; }

        [Description("Starbase Engineering Turns Required")]
        public int StarbaseEngineeringTurnsRequired { get; set; }

        [Description("Aspha Corvette")]
        public bool AsphaCorvette { get; set; }

        [Description("Aspha Corvette Turns Required")]
        public int AsphaCorvetteTurnsRequired { get; set; }

        [Description("Aspha Frigate")]
        public bool AsphaFrigate { get; set; }

        [Description("Aspha Frigate Turns Required")]
        public int AsphaFrigateTurnsRequired { get; set; }

        [Description("Aspha Destroyer")]
        public bool AsphaDestroyer { get; set; }

        [Description("Aspha Destroyer Turns Required")]
        public int AsphaDestroyerTurnsRequired { get; set; }

        [Description("Aspha Cruiser")]
        public bool AsphaCruiser { get; set; }

        [Description("Aspha Cruiser Turns Required")]
        public int AsphaCruiserTurnsRequired { get; set; }

        [Description("Aspha Battleship")]
        public bool AsphaBattleship { get; set; }

        [Description("Aspha Battleship Turns Required")]
        public int AsphaBattleshipTurnsRequired { get; set; }

        [Description("Aspha Dreadnought")]
        public bool AsphaDreadnought { get; set; }

        [Description("Aspha Dreadnought Turns Required")]
        public int AsphaDreadnoughtTurnsRequired { get; set; }

        [Description("Aspha Seeker")]
        public bool AsphaSeeker { get; set; }

        [Description("Aspha Seeker Turns Required")]
        public int AsphaSeekerTurnsRequired { get; set; }

        [Description("Aspha Ranger")]
        public bool AsphaRanger { get; set; }

        [Description("Aspha Ranger Turns Required")]
        public int AsphaRangerTurnsRequired { get; set; }

        [Description("Aspha Light Starbase")]
        public bool AsphaLightStarbase { get; set; }

        [Description("Aspha Light Starbase Turns Required")]
        public int AsphaLightStarbaseTurnsRequired { get; set; }

        [Description("Aspha Heavy Starbase")]
        public bool AsphaHeavyStarbase { get; set; }

        [Description("Aspha Heavy Starbase Turns Required")]
        public int AsphaHeavyStarbaseTurnsRequired { get; set; }

        [Description("G-Class Ship")]
        public bool GClassShip { get; set; }

        [Description("G-Class Ship Turns Required")]
        public int GClassShipTurnsRequired { get; set; }

        [Description("G-Class Juggernaught")]
        public bool GClassJuggernaught { get; set; }

        [Description("G-Class Juggernaught Turns Required")]
        public int GClassJuggernaughtTurnsRequired { get; set; }

        public AMinerResearch()
        {
            // Initialize default values if necessary
            StarshipEngineeringTurnsRequired = 40;
            FlagshipEngineeringTurnsRequired = 60;
            AdvancedEngineeringTurnsRequired = 70;
            SensorEngineeringTurnsRequired = 50;
            StarbaseEngineeringTurnsRequired = 80;
            AsphaCorvetteTurnsRequired = 30;
            AsphaFrigateTurnsRequired = 40;
            AsphaDestroyerTurnsRequired = 50;
            AsphaCruiserTurnsRequired = 60;
            AsphaBattleshipTurnsRequired = 80;
            AsphaDreadnoughtTurnsRequired = 120;
            AsphaSeekerTurnsRequired = 40;
            AsphaRangerTurnsRequired = 90;
            AsphaLightStarbaseTurnsRequired = 80;
            AsphaHeavyStarbaseTurnsRequired = 180;
            GClassShipTurnsRequired = 500;
            GClassJuggernaughtTurnsRequired = 350;
        }
    }
}