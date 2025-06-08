using System.ComponentModel.DataAnnotations;

namespace AnotherSpaceGame.Models
{
    public class CounterAttacks
    {
        [Key]
        public int Id { get; set; }

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string TargetUserId { get; set; }
        public DateTime DateGranted { get; set; }
    }
}
