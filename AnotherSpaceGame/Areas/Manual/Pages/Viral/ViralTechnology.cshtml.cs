using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnotherSpaceGame.Areas.Manual.Pages.Viral
{
    public class ViralTechnologyModel : PageModel
    {
        public ViralResearch ViralResearch { get; set; } = new ViralResearch();
        public ViralSpecificResearch ViralSpecificResearch { get; set; } = new ViralSpecificResearch();
        public List<ResearchItem> ViralResearchItems { get; set; }
        public List<ResearchItem> ViralSpecificResearchItems { get; set; }

        public void OnGet()
        {
            ViralResearchItems = GetViralResearchItems(ViralResearch);
            ViralSpecificResearchItems = GetViralSpecificResearchItems(ViralSpecificResearch);
        }

        public static List<ResearchItem> GetViralResearchItems(ViralResearch research)
        {
            return new List<ResearchItem>
            {
                new ResearchItem { Name = "Class 1 Viral Hull", IsResearched = research.Class1ViralHull, TurnsRequired = research.Class1ViralHullTurnsRequired },
                new ResearchItem { Name = "Class 2 Viral Hull", IsResearched = research.Class2ViralHull, TurnsRequired = research.Class2ViralHullTurnsRequired },
                new ResearchItem { Name = "Advanced Exploration", IsResearched = research.AdvancedExploration, TurnsRequired = research.AdvancedExplorationTurnsRequired },
                new ResearchItem { Name = "Kohoutek Scout", IsResearched = research.KohoutekScout, TurnsRequired = research.KohoutekScoutTurnsRequired },
                new ResearchItem { Name = "V-Class Destroyer", IsResearched = research.VClassDestroyer, TurnsRequired = research.VClassDestroyerTurnsRequired },
                new ResearchItem { Name = "V-Class Cruiser", IsResearched = research.VClassCruiser, TurnsRequired = research.VClassCruiserTurnsRequired },
                new ResearchItem { Name = "B-Class Cruiser", IsResearched = research.BClassCruiser, TurnsRequired = research.BClassCruiserTurnsRequired },
                new ResearchItem { Name = "B-Class Starbase", IsResearched = research.BClassStarbase, TurnsRequired = research.BClassStarbaseTurnsRequired }
            };
        }

        public static List<ResearchItem> GetViralSpecificResearchItems(ViralSpecificResearch research)
        {
            return new List<ResearchItem>
            {
                new ResearchItem { Name = "Viral Clustering", IsResearched = research.ViralClustering, TurnsRequired = research.ViralClusteringTurnsRequired },
                new ResearchItem { Name = "Unreverse Engineering", IsResearched = research.UnreverseEngineering, TurnsRequired = research.UnreverseEngineeringTurnsRequired }
            };
        }
    }

    public class ResearchItem
    {
        public string Name { get; set; }
        public bool IsResearched { get; set; }
        public int TurnsRequired { get; set; }
    }
}
