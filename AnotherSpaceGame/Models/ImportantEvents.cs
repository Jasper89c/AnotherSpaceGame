using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnotherSpaceGame.Models
{
    public class ImportantEvents
    {
        [Key]
        public int Id { get; set; }

        // Foreign key to ApplicationUser
        [Description("User Id")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Description("Date and Time")]
        public DateTime DateAndTime { get; set; }

        [Description("Event Text")]
        public string Text { get; set; }

        [Description("Event Type")]
        public ImportantEventTypes ImportantEventTypes { get; set; }
    }
}