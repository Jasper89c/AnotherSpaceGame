using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public class ClusterResearch
    {
        public int Id { get; set; } // Primary key for EF Core
        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Description("Colony Cluster Level 1")]
        public bool ColonyClusterLevel1 { get; set; }

        [Description("Colony Cluster Level 1 Turns Required")]
        public int ColonyClusterLevel1TurnsRequired { get; set; }

        [Description("Colony Cluster Level 2")]
        public bool ColonyClusterLevel2 { get; set; }

        [Description("Colony Cluster Level 2 Turns Required")]
        public int ColonyClusterLevel2TurnsRequired { get; set; }

        [Description("Colony Cluster Level 3")]
        public bool ColonyClusterLevel3 { get; set; }

        [Description("Colony Cluster Level 3 Turns Required")]
        public int ColonyClusterLevel3TurnsRequired { get; set; }

        public ClusterResearch()
        {
            // Initialize default values if needed
            ColonyClusterLevel1 = false;
            ColonyClusterLevel1TurnsRequired = 100;
            ColonyClusterLevel2 = false;
            ColonyClusterLevel2TurnsRequired = 150;
            ColonyClusterLevel3 = false;
            ColonyClusterLevel3TurnsRequired = 200;
        }
    }
}
