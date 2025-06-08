using System.ComponentModel.DataAnnotations;

namespace AnotherSpaceGame.Models
{
    public class Missions
    {
        [Key]
        public int Id { get; set; }
        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public bool Mission1 { get; set; }
        public bool Mission2 { get; set; }
        public bool Mission3 { get; set; }
        public bool Mission4 { get; set; }
        public bool Mission5 { get; set; }
        public bool Mission6 { get; set; }
        public bool Mission7 { get; set; }
        public bool Mission8 { get; set; }
        public bool Mission9 { get; set; }
        public bool Mission10 { get; set; }

    }
}
