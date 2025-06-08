using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public class ViralSpecificResearch
    {
        public int Id { get; set; } // Primary key for EF Core

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Description("Viral Clustering")]
        public bool ViralClustering { get; set; }

        [Description("Viral Clustering Turns Required")]
        public int ViralClusteringTurnsRequired { get; set; }

        [Description("Unreverse Engineering")]
        public bool UnreverseEngineering { get; set; }

        [Description("Unreverse Engineering Turns Required")]
        public int UnreverseEngineeringTurnsRequired { get; set; }

        public ViralSpecificResearch()
        {
            // Initialize properties with default values
            ViralClustering = false;
            ViralClusteringTurnsRequired = 150;
            UnreverseEngineering = false;
            UnreverseEngineeringTurnsRequired = 666;
        }
    }
}