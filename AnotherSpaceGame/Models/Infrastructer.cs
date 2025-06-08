namespace AnotherSpaceGame.Models
{
    public class Infrastructer
    {
        public int Id { get; set; } // Primary key for EF Core

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int Housing { get; set; }
        public int Commercial { get; set; }
        public int Agriculture { get; set; }
        public int Industry { get; set; }
        public int Mining { get; set; }
        public int TurnsRequired { get; set; }
        public int TurnsRemaining { get; set; }
        public int TotalLevels { get; set; }
        public int UnusedLevels { get ; set; }


        public Infrastructer()
        {
            Housing = 0;
            Commercial = 0;
            Agriculture = 0;
            Industry = 0;
            Mining = 0;
            TotalLevels = 0;
            UnusedLevels = 0; // Initialize unused levels to zero
            TurnsRequired = 2; // Default or fallback value
            TurnsRemaining = 2; // Default or fallback value
            
        }
    }
}