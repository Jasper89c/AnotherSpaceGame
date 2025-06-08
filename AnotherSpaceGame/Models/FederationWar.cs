using System;
using System.ComponentModel.DataAnnotations;

namespace AnotherSpaceGame.Models
{
    public class FederationWar
    {
        [Key]
        public int Id { get; set; }
        public int AttackerFederationId { get; set; }
        public Federations AttackerFederation { get; set; }
        public int DefenderFederationId { get; set; }
        public Federations DefenderFederation { get; set; }
        public DateTime DeclaredAt { get; set; }
        // Add more properties as needed (e.g., status)
    }
}