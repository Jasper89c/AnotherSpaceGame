using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public enum Faction
    {
        [Description("Terran")]
        Terran,
        [Description("A-Miner")]
        AMiner,
        [Description("Marauder")]
        Marauder,
        [Description("Viral")]
        Viral,
        [Description("Collective")]
        Collective,
        [Description("Guardian")]
        Guardian,
        [Description("Kal-Zul")]
        KalZul,
        [Description("Dark Marauder")]
        DarkMarauder
    }
}