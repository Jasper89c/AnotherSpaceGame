using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public enum ResearchType
    {
        [Description("Ship Hull")]
        ShipHull,
        [Description("Ship Class")]
        ShipClass,
        [Description("Genetic")]
        Genetic,
        [Description("Planetary")]
        Planetary,
        [Description("Alien")]
        Alien,
        [Description("Intelligence")]
        Intelligence,
        [Description("Technology")]
        Technology
    }
}