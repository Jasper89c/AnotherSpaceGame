using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnotherSpaceGame.Models
{
    public class MarketPosts
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public MarketType MarketType { get; set; } 

        [Required]
        public int Amount { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public int Price { get; set; }
        

        // Foreign key to ApplicationUser
        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}