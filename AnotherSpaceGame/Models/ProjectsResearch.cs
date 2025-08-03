using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public class ProjectsResearch
    {
        public int Id { get; set; } // Primary key for EF Core

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Description("Capsule Laboratory")]
        public bool CapsuleLaboratory { get; set; }

        [Description("Capsule Laboratory Turns Required")]
        public int CapsuleLaboratoryTurnsRequired { get; set; }

        [Description("Kal-Zul Overmind")]
        public bool KalZulOvermind { get; set; }

        [Description("Kal-Zul Overmind Turns Required")]
        public int KalZulOvermindTurnsRequired { get; set; }

        [Description("Kal-Zul Artifact")]
        public bool KalZulArtifact { get; set; }

        [Description("Kal-Zul Artifact Turns Required")]
        public int KalZulArtifactTurnsRequired { get; set; }

        [Description("I-Tech Laboratory")]
        public bool ITechLaboratory { get; set; }

        [Description("I-Tech Laboratory Turns Required")]
        public int ITechLaboratoryTurnsRequired { get; set; }
        [Description("Advanced Exploration")]
        public bool AdvancedExploration { get; set; }

        [Description("Advanced Exploration Turns Required")]
        public int AdvancedExplorationTurnsRequired { get; set; }

        public ProjectsResearch()
        {
            // Initialize default values if needed
            
            CapsuleLaboratory = false;
            CapsuleLaboratoryTurnsRequired = 600;
            KalZulOvermind = false;
            KalZulOvermindTurnsRequired = 999;
            KalZulArtifact = false;
            KalZulArtifactTurnsRequired = 90;
            ITechLaboratory = false;
            ITechLaboratoryTurnsRequired = 1200;
            AdvancedExploration = false;
            AdvancedExplorationTurnsRequired = 800;

        }
    }
}