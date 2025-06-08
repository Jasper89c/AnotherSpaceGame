using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public class EClassResearch
    {
        public int Id { get; set; } // Primary key for EF Core

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Description("ExaHertz Class Railgun")]
        public bool ExaHertzClassRailgun { get; set; }

        [Description("ExaHertz Class Railgun Turns Required")]
        public int ExaHertzClassRailgunTurnsRequired { get; set; }

        [Description("ZettaHertz Class Railgun")]
        public bool ZettaHertzClassRailgun { get; set; }

        [Description("ZettaHertz Class Railgun Turns Required")]
        public int ZettaHertzClassRailgunTurnsRequired { get; set; }

        [Description("E-Class Fighter")]
        public bool EClassFighter { get; set; }

        [Description("E-Class Fighter Turns Required")]
        public int EClassFighterTurnsRequired { get; set; }

        [Description("E-Class Frigate")]
        public bool EClassFrigate { get; set; }

        [Description("E-Class Frigate Turns Required")]
        public int EClassFrigateTurnsRequired { get; set; }

        [Description("E-Class Destroyer")]
        public bool EClassDestroyer { get; set; }

        [Description("E-Class Destroyer Turns Required")]
        public int EClassDestroyerTurnsRequired { get; set; }

        public EClassResearch()
        {
            // Initialize default values
            ExaHertzClassRailgun = false;
            ExaHertzClassRailgunTurnsRequired = 40;
            ZettaHertzClassRailgun = false;
            ZettaHertzClassRailgunTurnsRequired = 70;
            EClassFighter = false;
            EClassFighterTurnsRequired = 20;
            EClassFrigate = false;
            EClassFrigateTurnsRequired = 35;
            EClassDestroyer = false;
            EClassDestroyerTurnsRequired = 50;
        }
    }
}
