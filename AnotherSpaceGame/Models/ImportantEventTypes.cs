using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public enum ImportantEventTypes
    {
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