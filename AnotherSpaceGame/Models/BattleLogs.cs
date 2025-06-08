using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnotherSpaceGame.Models
{
    public class BattleLogs
    {
        [Key]
        public int Id { get; set; }

        // Foreign key to ApplicationUser
        [Description("User Id")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Description("Date and Time")]
        public DateTime DateAndTime { get; set; }

        [Description("Attacker")]
        public string Attacker { get; set; }

        [Description("Defender")]
        public string Defender { get; set; }

        [Description("Outcome")]
        public string Outcome { get; set; }

        [Description("Fleet Report")]
        public string FleetReport { get; set; }
    }
}