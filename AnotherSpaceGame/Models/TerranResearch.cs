using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public class TerranResearch
    {
        public int Id { get; set; } // Primary key for EF Core

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Description("Small Terran Hull")]
        public bool SmallTerranHull { get; set; }

        [Description("Small Terran Hull Turns Required")]
        public int SmallTerranHullTurnsRequired { get; set; }

        [Description("Medium Terran Hull")]
        public bool MediumTerranHull { get; set; }

        [Description("Medium Terran Hull Turns Required")]
        public int MediumTerranHullTurnsRequired { get; set; }

        [Description("Large Terran Hull")]
        public bool LargeTerranHull { get; set; }

        [Description("Large Terran Hull Turns Required")]
        public int LargeTerranHullTurnsRequired { get; set; }

        [Description("Terran Corvette")]
        public bool TerranCorvette { get; set; }

        [Description("Terran Corvette Turns Required")]
        public int TerranCorvetteTurnsRequired { get; set; }

        [Description("Terran Frigate")]
        public bool TerranFrigate { get; set; }

        [Description("Terran Frigate Turns Required")]
        public int TerranFrigateTurnsRequired { get; set; }

        [Description("Terran Destroyer")]
        public bool TerranDestroyer { get; set; }

        [Description("Terran Destroyer Turns Required")]
        public int TerranDestroyerTurnsRequired { get; set; }

        [Description("Terran Cruiser")]
        public bool TerranCruiser { get; set; }

        [Description("Terran Cruiser Turns Required")]
        public int TerranCruiserTurnsRequired { get; set; }

        [Description("Terran Battleship")]
        public bool TerranBattleship { get; set; }

        [Description("Terran Battleship Turns Required")]
        public int TerranBattleshipTurnsRequired { get; set; }

        [Description("Terran Advanced Scout")]
        public bool TerranAdvancedScout { get; set; }

        [Description("Terran Advanced Scout Turns Required")]
        public int TerranAdvancedScoutTurnsRequired { get; set; }

        [Description("Terran Dreadnaught")]
        public bool TerranDreadnaught { get; set; }

        [Description("Terran Dreadnaught Turns Required")]
        public int TerranDreadnaughtTurnsRequired { get; set; }

        [Description("Terran Starbase")]
        public bool TerranStarbase { get; set; }

        [Description("Terran Starbase Turns Required")]
        public int TerranStarbaseTurnsRequired { get; set; }

        [Description("Terran Juggernaught")]
        public bool TerranJuggernaught { get; set; }

        [Description("Terran Juggernaught Turns Required")]
        public int TerranJuggernaughtTurnsRequired { get; set; }

        [Description("Advanced Radar")]
        public bool AdvancedRadar { get; set; }

        [Description("Advanced Radar Turns Required")]
        public int AdvancedRadarTurnsRequired { get; set; }

        [Description("Crystal Lenses")]
        public bool CrystalLenses { get; set; }

        [Description("Crystal Lenses Turns Required")]
        public int CrystalLensesTurnsRequired { get; set; }

        [Description("Photon Torpedo")]
        public bool PhotonTorpedo { get; set; }

        [Description("Photon Torpedo Turns Required")]
        public int PhotonTorpedoTurnsRequired { get; set; }

        [Description("Photon Corvette")]
        public bool PhotonCorvette { get; set; }

        [Description("Photon Corvette Turns Required")]
        public int PhotonCorvetteTurnsRequired { get; set; }

        [Description("Photon Frigate")]
        public bool PhotonFrigate { get; set; }

        [Description("Photon Frigate Turns Required")]
        public int PhotonFrigateTurnsRequired { get; set; }

        [Description("Photon Destroyer")]
        public bool PhotonDestroyer { get; set; }

        [Description("Photon Destroyer Turns Required")]
        public int PhotonDestroyerTurnsRequired { get; set; }

        [Description("Photon Cruiser")]
        public bool PhotonCruiser { get; set; }

        [Description("Photon Cruiser Turns Required")]
        public int PhotonCruiserTurnsRequired { get; set; }

        public TerranResearch()
        {
            SmallTerranHull = false;
            SmallTerranHullTurnsRequired = 20;
            MediumTerranHull = false;
            MediumTerranHullTurnsRequired = 40;
            LargeTerranHull = false;
            LargeTerranHullTurnsRequired = 60;
            TerranCorvette = false;
            TerranCorvetteTurnsRequired = 40;
            TerranFrigate = false;
            TerranFrigateTurnsRequired = 60;
            TerranDestroyer = false;
            TerranDestroyerTurnsRequired = 80;
            TerranCruiser = false;
            TerranCruiserTurnsRequired = 90;
            TerranBattleship = false;
            TerranBattleshipTurnsRequired = 135;
            TerranAdvancedScout = false;
            TerranAdvancedScoutTurnsRequired = 40;
            TerranDreadnaught = false;
            TerranDreadnaughtTurnsRequired = 160;
            TerranStarbase = false;
            TerranStarbaseTurnsRequired = 160;
            TerranJuggernaught = false;
            TerranJuggernaughtTurnsRequired = 400;
            AdvancedRadar = false;
            AdvancedRadarTurnsRequired = 25;
            CrystalLenses = false;
            CrystalLensesTurnsRequired = 40;
            PhotonTorpedo = false;
            PhotonTorpedoTurnsRequired = 35;
            PhotonCorvette = false;
            PhotonCorvetteTurnsRequired = 50;
            PhotonFrigate = false;
            PhotonFrigateTurnsRequired = 70;
            PhotonDestroyer = false;
            PhotonDestroyerTurnsRequired = 120;
            PhotonCruiser = false;
            PhotonCruiserTurnsRequired = 120;
        }
    }
}