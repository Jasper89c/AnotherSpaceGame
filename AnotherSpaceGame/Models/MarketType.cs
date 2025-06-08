using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public enum MarketType
    {
        [Description("Food")]
        Food,
        [Description("Ore")]
        Ore,
        [Description("Raw Material")]
        RawMaterial,
        [Description("Consumer Goods")]
        ConsumerGoods,
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

    }
}