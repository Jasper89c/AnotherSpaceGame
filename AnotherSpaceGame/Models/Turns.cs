namespace AnotherSpaceGame.Models
{
    public class Turns
    {
        public int Id { get; set; } // Primary key for EF Core

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int CurrentTurns { get; set; }
        public int MaxTurns { get; set; }

        public Turns()
        {
            CurrentTurns = 45;
            MaxTurns = 45;
        }
    }
}