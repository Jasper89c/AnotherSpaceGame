using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public enum PlanetType
    {
        [Description("Barren")]
        Barren,
        [Description("Icy")]
        Icy,
        [Description("Marshy")]
        Marshy,
        [Description("Forest")]
        Forest,
        [Description("Oceanic")]
        Oceanic,
        [Description("Rocky")]
        Rocky,
        [Description("Desert")]
        Desert,
        [Description("Balanced")]
        Balanced,
        [Description("Gas")]
        Gas,
        [Description("Cluster Level 1")]
        ClusterLevel1,
        [Description("Cluster Level 2")]
        ClusterLevel2,
        [Description("Cluster Level 3")]
        ClusterLevel3,
        [Description("U.Rich")]
        URich,
        [Description("U.Eden")]
        UEden,
        [Description("U.Spazial")]
        USpazial,
        [Description("U.Large")]
        ULarge,
        [Description("U.Fertile")]
        UFertile,
        [Description("Dead")]
        Dead,
        [Description("Similare C.1")]
        SimilareC1,
        [Description("Similare C.2")]
        SimilareC2,
        [Description("Similare C.3")]
        SimilareC3,
        [Description("Similare C.4")]
        SimilareC4,
        [Description("Similare C.5")]
        SimilareC5,
        [Description("Assimilated C.1")]
        AssimilatedC1,
        [Description("Assimilated C.2")]
        AssimilatedC2,
        [Description("Assimilated C.3")]
        AssimilatedC3,
        [Description("Tainted C.1")]
        TaintedC1,
        [Description("Tainted C.2")]
        TaintedC2,
        [Description("Tainted C.3")]
        TaintedC3,
        [Description("Tainted C.4")]
        TaintedC4,
        [Description("Infected C.1")]
        InfectedC1,
        [Description("Infected C.2")]
        InfectedC2,
        [Description("Infected C.3")]
        InfectedC3

        // Add more as needed
    }
}