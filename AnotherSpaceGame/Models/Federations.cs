using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnotherSpaceGame.Models
{
    public class Federations
    {
        [Key]
        public int Id { get; set; }

        [Description("Federation Name")]
        public string? FederationName { get; set; }

        [Description("Federation Leader")]
        public ApplicationUser FederationLeader { get; set; }
        public string? FederationLeaderId { get; set; }

        [Description("Federation Members")]
        public ICollection<ApplicationUser> FederationMembers { get; set; }

        [Description("Federation Wars")]
        public ICollection<Federations> FederationWars { get; set; }

        [Description("Federation Discussion")]
        public ICollection<FederationMessages> FederationDiscussion { get; set; }

        [Description("Federation Applicants")]
        public ICollection<FederationApplication> FederationApplicants { get; set; }

        [Description("Federation Activity")]
        public ICollection<BattleLogs> FederationActivity { get; set; } 

        [Description("Total Members")]
        public int TotalMembers { get; set; }

        [Description("Maximum Members")]
        public int MaximumMembers { get; set; }

        [Description("Created At")]
        public DateTime CreatedAt { get; set; }

        [Description("Total Powerating")]
        public int TotalPowerating { get; set; }

        [Description("Total Planets")]
        public int TotalPlanets { get; set; }

        [Description("Federation Description")]
        public string? FederationDescription { get; set; }

    }

    public class FederationMessages
    {
        [Key]
        public int Id { get; set; }

        [Description("Sender")]
        public ApplicationUser Sender { get; set; }
        public string? SenderId { get; set; }

        [Description("Message Text")]
        public string? MessageText { get; set; }

        [Description("Sent At")]
        public DateTime SentAt { get; set; }
        public int FederationId { get; set; }
        public Federations Federation { get; set; }
    }
}