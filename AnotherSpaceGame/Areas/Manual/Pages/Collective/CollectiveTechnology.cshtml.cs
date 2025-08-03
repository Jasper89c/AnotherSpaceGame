using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnotherSpaceGame.Areas.Manual.Pages.Collective
{
    public class CollectiveTechnologyModel : PageModel
    {
        public CollectiveResearch CollectiveResearch { get; set; } = new CollectiveResearch();
        public CollectiveSpecificResearch CollectiveSpecificResearch { get; set; } = new CollectiveSpecificResearch();
        public List<ResearchItem> CollectiveResearchItems { get; set; }
        public List<ResearchItem> CollectiveSpecificResearchItems { get; set; }

        public void OnGet()
        {
            CollectiveResearchItems = GetCollectiveResearchItems(CollectiveResearch);
            CollectiveSpecificResearchItems = GetCollectiveSpecificResearchItems(CollectiveSpecificResearch);
        }

        public static List<ResearchItem> GetCollectiveSpecificResearchItems(CollectiveSpecificResearch research)
        {
            return new List<ResearchItem>
            {
                new ResearchItem { Name = "Collective Clustering", IsResearched = research.CollectiveClustering, TurnsRequired = research.CollectiveClusteringTurnsRequired }
            };
        }

        public static List<ResearchItem> GetCollectiveResearchItems(CollectiveResearch research)
        {
            return new List<ResearchItem>
        {
        new ResearchItem { Name = "Fighter Class", IsResearched = research.FighterClass, TurnsRequired = research.FighterClassTurnsRequired },
        new ResearchItem { Name = "Frigate Class", IsResearched = research.FrigateClass, TurnsRequired = research.FrigateClassTurnsRequired },
        new ResearchItem { Name = "R-Class Frigate", IsResearched = research.RClassFrigate, TurnsRequired = research.RClassFrigateTurnsRequired },
        new ResearchItem { Name = "Destroyer Class", IsResearched = research.DestroyerClass, TurnsRequired = research.DestroyerClassTurnsRequired },
        new ResearchItem { Name = "Cruiser Class", IsResearched = research.CruiserClass, TurnsRequired = research.CruiserClassTurnsRequired },
        new ResearchItem { Name = "H-Class Ships", IsResearched = research.HClassShips, TurnsRequired = research.HClassShipsTurnsRequired },
        new ResearchItem { Name = "R-Class Corvette", IsResearched = research.RClassCorvette, TurnsRequired = research.RClassCorvetteTurnsRequired },
        new ResearchItem { Name = "H-Class Destroyer", IsResearched = research.HClassDestroyer, TurnsRequired = research.HClassDestroyerTurnsRequired },
        new ResearchItem { Name = "R-Class Destroyer", IsResearched = research.RClassDestroyer, TurnsRequired = research.RClassDestroyerTurnsRequired },
        new ResearchItem { Name = "H-Class Cruiser", IsResearched = research.HClassCruiser, TurnsRequired = research.HClassCruiserTurnsRequired },
        new ResearchItem { Name = "R-Class Battleship", IsResearched = research.RClassBattleship, TurnsRequired = research.RClassBattleshipTurnsRequired },
        new ResearchItem { Name = "H-Class Dreadnought", IsResearched = research.HClassDreadnought, TurnsRequired = research.HClassDreadnoughtTurnsRequired },
        new ResearchItem { Name = "R-Class Dreadnought", IsResearched = research.RClassDreadnought, TurnsRequired = research.RClassDreadnoughtTurnsRequired },
        new ResearchItem { Name = "R-Class Juggernaught", IsResearched = research.RClassJuggernaught, TurnsRequired = research.RClassJuggernaughtTurnsRequired },
        new ResearchItem { Name = "H-Class Leviathan", IsResearched = research.HClassLeviathan, TurnsRequired = research.HClassLeviathanTurnsRequired },
        new ResearchItem { Name = "H-Class Starbase", IsResearched = research.HClassStarbase, TurnsRequired = research.HClassStarbaseTurnsRequired }
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
