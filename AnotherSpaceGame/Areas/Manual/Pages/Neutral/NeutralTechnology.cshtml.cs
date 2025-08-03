using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnotherSpaceGame.Areas.Manual.Pages.Neutral
{
    public class NeutralTechnologyModel : PageModel
    {
        public ProjectsResearch ProjectsResearch { get; set; } = new ProjectsResearch();
        public ClusterResearch ClusterResearch { get; set; } = new ClusterResearch();
        public CyrilClassResearch CyrilClassResearch { get; set; } = new CyrilClassResearch();
        public EClassResearch EClassResearch { get; set; } = new EClassResearch();
        public FClassResearch FClassResearch { get; set; } = new FClassResearch();

        public List<ResearchItem> ProjectsResearchItems { get; set; }
        public List<ResearchItem> ClusterResearchItems { get; set; }
        public List<ResearchItem> CyrilClassResearchItems { get; set; }
        public List<ResearchItem> EClassResearchItems { get; set; }
        public List<ResearchItem> FClassResearchItems { get; set; }

        public void OnGet()
        {
            ProjectsResearchItems = GetProjectsResearchItems(ProjectsResearch);
            ClusterResearchItems = GetClusterResearchItems(ClusterResearch);
            CyrilClassResearchItems = GetCyrilClassResearchItems(CyrilClassResearch);
            EClassResearchItems = GetEClassResearchItems(EClassResearch);
            FClassResearchItems = GetFClassResearchItems(FClassResearch);
        }

        public static List<ResearchItem> GetProjectsResearchItems(ProjectsResearch research)
        {
            return new List<ResearchItem>
            {
                new ResearchItem { Name = "Capsule Laboratory", IsResearched = research.CapsuleLaboratory, TurnsRequired = research.CapsuleLaboratoryTurnsRequired },
                new ResearchItem { Name = "ITech", IsResearched = research.ITechLaboratory, TurnsRequired = research.ITechLaboratoryTurnsRequired },
                new ResearchItem { Name = "Kal-Zul Overmind", IsResearched = research.KalZulOvermind, TurnsRequired = research.KalZulOvermindTurnsRequired },
                new ResearchItem { Name = "Kal-Zul Artifact", IsResearched = research.KalZulArtifact, TurnsRequired = research.KalZulArtifactTurnsRequired }
            };
        }

        public static List<ResearchItem> GetClusterResearchItems(ClusterResearch research)
        {
            return new List<ResearchItem>
            {
                new ResearchItem { Name = "Colony Cluster Level 1", IsResearched = research.ColonyClusterLevel1, TurnsRequired = research.ColonyClusterLevel1TurnsRequired },
                new ResearchItem { Name = "Colony Cluster Level 2", IsResearched = research.ColonyClusterLevel2, TurnsRequired = research.ColonyClusterLevel2TurnsRequired },
                new ResearchItem { Name = "Colony Cluster Level 3", IsResearched = research.ColonyClusterLevel3, TurnsRequired = research.ColonyClusterLevel3TurnsRequired }
            };
        }

        public static List<ResearchItem> GetCyrilClassResearchItems(CyrilClassResearch research)
        {
            return new List<ResearchItem>
            {
                new ResearchItem { Name = "Cyril Alloy", IsResearched = research.CyrilAlloy, TurnsRequired = research.CyrilAlloyTurnsRequired },
                new ResearchItem { Name = "Small Cyril Hull", IsResearched = research.SmallCyrilHull, TurnsRequired = research.SmallCyrilHullTurnsRequired },
                new ResearchItem { Name = "Medium Cyril Hull", IsResearched = research.MediumCyrilHull, TurnsRequired = research.MediumCyrilHullTurnsRequired },
                new ResearchItem { Name = "Large Cyril Hull", IsResearched = research.LargeCyrilHull, TurnsRequired = research.LargeCyrilHullTurnsRequired },
                new ResearchItem { Name = "Cyril Corvette", IsResearched = research.CyrilCorvette, TurnsRequired = research.CyrilCorvetteTurnsRequired },
                new ResearchItem { Name = "Cyril Frigate", IsResearched = research.CyrilFrigate, TurnsRequired = research.CyrilFrigateTurnsRequired },
                new ResearchItem { Name = "Cyril Destroyer", IsResearched = research.CyrilDestroyer, TurnsRequired = research.CyrilDestroyerTurnsRequired },
                new ResearchItem { Name = "Cyril Cruiser", IsResearched = research.CyrilCruiser, TurnsRequired = research.CyrilCruiserTurnsRequired }
            };
        }

        public static List<ResearchItem> GetEClassResearchItems(EClassResearch research)
        {
            return new List<ResearchItem>
            {
                new ResearchItem { Name = "ExaHertz Class Railgun", IsResearched = research.ExaHertzClassRailgun, TurnsRequired = research.ExaHertzClassRailgunTurnsRequired },
                new ResearchItem { Name = "ZettaHertz Class Railgun", IsResearched = research.ZettaHertzClassRailgun, TurnsRequired = research.ZettaHertzClassRailgunTurnsRequired },
                new ResearchItem { Name = "E-Class Fighter", IsResearched = research.EClassFighter, TurnsRequired = research.EClassFighterTurnsRequired },
                new ResearchItem { Name = "E-Class Frigate", IsResearched = research.EClassFrigate, TurnsRequired = research.EClassFrigateTurnsRequired },
                new ResearchItem { Name = "E-Class Destroyer", IsResearched = research.EClassDestroyer, TurnsRequired = research.EClassDestroyerTurnsRequired }
            };
        }

        public static List<ResearchItem> GetFClassResearchItems(FClassResearch research)
        {
            return new List<ResearchItem>
            {
                new ResearchItem { Name = "F-Class Engineering", IsResearched = research.FClassEngineering, TurnsRequired = research.FClassEngineeringTurnsRequired },
                new ResearchItem { Name = "F-Class Frigate", IsResearched = research.FClassFrigate, TurnsRequired = research.FClassFrigateTurnsRequired },
                new ResearchItem { Name = "F-Class Destroyer", IsResearched = research.FClassDestroyer, TurnsRequired = research.FClassDestroyerTurnsRequired },
                new ResearchItem { Name = "F-Class Cruiser", IsResearched = research.FClassCruiser, TurnsRequired = research.FClassCruiserTurnsRequired }
            };
        }
    }

    public class ResearchItem
    {
        public string Name { get; set; }
        public bool IsResearched { get; set; }
        public int? TurnsRequired { get; set; } // Nullable for items that don't have a turns property
    }
}
