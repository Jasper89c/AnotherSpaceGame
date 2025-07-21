namespace AnotherSpaceGame.Models
{
    public class Commodities
    {       
        public int Id { get; set; } // Primary key for EF Core

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public long Credits { get; set; }
        public long Food { get; set; }
        public long Ore { get; set; }
        public long RawMaterial { get; set; }
        public long ConsumerGoods { get; set; }
        public long TerranMetal { get; set; }
        public long RedCrystal { get; set; }
        public long WhiteCrystal { get; set; }
        public long Rutile { get; set; }
        public long Composite { get; set; }
        public long StrafezOrganism { get; set; }

        public Commodities()
        {
            Credits = 250000;
            Food = 250000;
            Ore = 2000;
            RawMaterial = 10000;
            ConsumerGoods = 10000;
            TerranMetal = 10000;
            RedCrystal = 10000;
            WhiteCrystal = 10000;
            Rutile = 10000;
            Composite = 10000;
            StrafezOrganism = 10000;
        }
    }
}