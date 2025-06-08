using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public enum MineralType
    {
        [Description("Terran Metal")]
        TerranMetal,
        [Description("Red Crystal")]
        RedCrystal,
        [Description("White Crystal")]
        WhiteCrystal,
        [Description("Rutile")]
        Rutile,
        [Description("Composite")]
        Composite,
        [Description("Strafez Organism")]
        StrafezOrganism

        // Add more as needed
    }
}