using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public enum ImportantEventTypes
    {
        [Description("Show All")]
        ShowAll,

        [Description("Battles")]
        Battles,

        [Description("Market")]
        Market,

        [Description("Artifacts")]
        Artifacts,

        [Description("Miscellaneous")]
        Misc
    }
}