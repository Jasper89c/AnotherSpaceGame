using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public enum ShipType
    {
        [Description("Fighter")]
        Fighter,
        [Description("Corvette")]
        Corvette,
        [Description("Frigate")]
        Frigate,
        [Description("Destroyer")]
        Destroyer,
        [Description("Cruiser")]
        Cruiser,
        [Description("Battleship")]
        Battleship,
        [Description("Dreadnought")]
        Dreadnought,
        [Description("Starbase")]
        Starbase,
        [Description("Juggernaught")]
        Juggernaught,
        [Description("Scout")]
        Scout,
        [Description("Special")]
        Special,
        [Description("Leviathan")]
        Leviathan
    }
}