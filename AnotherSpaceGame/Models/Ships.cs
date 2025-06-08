using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnotherSpaceGame.Models
{
    public class Ships
    {
        public int Id { get; set; }

        [Description("Ship Name")]
        public string ShipName { get; set; }

        [Description("Ship Type")]
        public ShipType ShipType { get; set; }

        [Description("Power Rating")]
        public int PowerRating { get; set; }

        [Description("Range")]
        public int Range { get; set; }

        [Description("Weapon")]
        public int Weapon { get; set; }

        [Description("Hull")]
        public int Hull { get; set; }

        [Description("Energy Weapon")]
        public int EnergyWeapon { get; set; }

        [Description("Kinetic Weapon")]
        public int KineticWeapon { get; set; }

        [Description("Missile Weapon")]
        public int MissileWeapon { get; set; }

        [Description("Chemical Weapon")]
        public int ChemicalWeapon { get; set; }

        [Description("Energy Shield")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal EnergyShield { get; set; }

        [Description("Kinetic Shield")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal KineticShield { get; set; }

        [Description("Missile Shield")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MissileShield { get; set; }

        [Description("Chemical Shield")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ChemicalShield { get; set; }

        [Description("No Defense")]
        public bool NoDefense { get; set; }

        [Description("No Retal")]
        public bool NoRetal { get; set; }

        [Description("Cap Chance")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CapChance { get; set; }

        [Description("Cost")]
        public int Cost { get; set; }

        [Description("Upkeep")]
        public int Upkeep { get; set; }

        [Description("Terran Metal")]
        public int TerranMetal { get; set; }

        [Description("Red Crystal")]
        public int RedCrystal { get; set; }

        [Description("White Crystal")]
        public int WhiteCrystal { get; set; }

        [Description("Rutile")]
        public int Rutile { get; set; }

        [Description("Composite")]
        public int Composite { get; set; }

        [Description("Strafez Organism")]
        public int StrafezOrganism { get; set; }

        [Description("Scanning Power")]
        public int ScanningPower { get; set; }
        [Description("Build Rate")]
        public int BuildRate { get; set; }
        [Description("Cost To Build")]
        public int CostToBuild { get; set; }
        [Description("Immune to Capture")]
        public bool ImmuneToCapture { get; set; }
        [Description("CanCapture")] 
        public bool CanCapture { get; set; }
    }
}