using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnotherSpaceGame.Areas.Manual.Pages.Marauder
{
    public class MarauderTechnologyModel : PageModel
    {
        public MarauderResearch MarauderResearch { get; set; } = new MarauderResearch();
        public List<MarauderResearchItem> ResearchItems { get; set; }

        public void OnGet()
        {
            ResearchItems = GetMarauderResearchItems(MarauderResearch);
        }

        public static List<MarauderResearchItem> GetMarauderResearchItems(MarauderResearch research)
        {
            return new List<MarauderResearchItem>
            {
                new MarauderResearchItem { Name = "Class 3 Marauder Hull", IsResearched = research.Class3MarauderHull, TurnsRequired = research.Class3MarauderHullTurnsRequired },
                new MarauderResearchItem { Name = "Class 4 Marauder Hull", IsResearched = research.Class4MarauderHull, TurnsRequired = research.Class4MarauderHullTurnsRequired },
                new MarauderResearchItem { Name = "Class 5 Marauder Hull", IsResearched = research.Class5MarauderHull, TurnsRequired = research.Class5MarauderHullTurnsRequired },
                new MarauderResearchItem { Name = "Class 6 Marauder Hull", IsResearched = research.Class6MarauderHull, TurnsRequired = research.Class6MarauderHullTurnsRequired },
                new MarauderResearchItem { Name = "Advanced Engineering", IsResearched = research.AdvancedEngineering, TurnsRequired = research.AdvancedEngineeringTurnsRequired },
                new MarauderResearchItem { Name = "Marauder Corvette", IsResearched = research.MarauderCorvette, TurnsRequired = research.MarauderCorvetteTurnsRequired },
                new MarauderResearchItem { Name = "Marauder Frigate", IsResearched = research.MarauderFrigate, TurnsRequired = research.MarauderFrigateTurnsRequired },
                new MarauderResearchItem { Name = "Marauder Destroyer", IsResearched = research.MarauderDestroyer, TurnsRequired = research.MarauderDestroyerTurnsRequired },
                new MarauderResearchItem { Name = "Marauder Cruiser", IsResearched = research.MarauderCruiser, TurnsRequired = research.MarauderCruiserTurnsRequired },
                new MarauderResearchItem { Name = "Marauder Battleship", IsResearched = research.MarauderBattleship, TurnsRequired = research.MarauderBattleshipTurnsRequired },
                new MarauderResearchItem { Name = "Type D Frigate", IsResearched = research.TypeDFrigate, TurnsRequired = research.TypeDFrigateTurnsRequired },
                new MarauderResearchItem { Name = "Type D Destroyer", IsResearched = research.TypeDDestroyer, TurnsRequired = research.TypeDDestroyerTurnsRequired },
                new MarauderResearchItem { Name = "Type D Cruiser", IsResearched = research.TypeDCruiser, TurnsRequired = research.TypeDCruiserTurnsRequired },
                new MarauderResearchItem { Name = "Type D Battleship", IsResearched = research.TypeDBattleship, TurnsRequired = research.TypeDBattleshipTurnsRequired }
            };
        }
    }

    public class MarauderResearchItem
    {
        public string Name { get; set; }
        public bool IsResearched { get; set; }
        public int TurnsRequired { get; set; }
    }
}
