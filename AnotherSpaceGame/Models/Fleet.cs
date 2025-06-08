namespace AnotherSpaceGame.Models
{
    public class Fleet
    {
        public int Id { get; set; } // Primary key for EF Core

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int ShipId { get; set; }
        public int TotalShips { get; set; }
        public int TotalPowerRating { get; set; }
        public int TotalUpkeep { get; set; }

    }
}