using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public class CollectiveSpecificResearch
    {
        public int Id { get; set; } // Primary key for EF Core

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Description("Collective Clustering")]
        public bool CollectiveClustering { get; set; }

        [Description("Collective Clustering Turns Required")]
        public int CollectiveClusteringTurnsRequired { get; set; }

        public CollectiveSpecificResearch()
        {
            CollectiveClustering = false;
            CollectiveClusteringTurnsRequired = 150;
        }
    }
}