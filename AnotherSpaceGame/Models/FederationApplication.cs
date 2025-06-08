using System;
using System.ComponentModel.DataAnnotations;

namespace AnotherSpaceGame.Models
{
    public class FederationApplication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public int FederationId { get; set; }
        public Federations Federation { get; set; }

        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
        // Optionally, add status, message, etc.
    }
}