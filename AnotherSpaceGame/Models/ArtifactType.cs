using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public enum ArtifactType
    {
        [Description("Common")]
        Common,
        [Description("Uncommon")]
        Uncommon,
        [Description("Rare")]
        Rare,
        [Description("Unique")]
        Unique,
        [Description("Special")]
        Special
    }
}