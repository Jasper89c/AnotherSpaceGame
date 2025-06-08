namespace AnotherSpaceGame.Models
{
    public class Commodities
    {       
        public int Id { get; set; } // Primary key for EF Core

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int Credits { get; set; }
        public int Food { get; set; }
        public int Ore { get; set; }
        public int RawMaterial { get; set; }
        public int ConsumerGoods { get; set; }
        public int TerranMetal { get; set; }
        public int RedCrystal { get; set; }
        public int WhiteCrystal { get; set; }
        public int Rutile { get; set; }
        public int Composite { get; set; }
        public int StrafezOrganism { get; set; }

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