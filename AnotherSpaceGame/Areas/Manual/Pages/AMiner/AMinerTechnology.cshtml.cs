using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnotherSpaceGame.Areas.Manual.Pages.AMiner
{
    public class AMinerTechnologyModel : PageModel
    {
        public AMinerResearch AMinerResearch { get; set; } = new AMinerResearch();
        public List<AMinerResearchItem> ResearchItems { get; set; }

        public void OnGet()
        {
            // Assuming AMinerResearch is already populated
            ResearchItems = GetResearchItems(AMinerResearch);
        }

        public static List<AMinerResearchItem> GetResearchItems(AMinerResearch research)
        {
            return new List<AMinerResearchItem>
            {
                new AMinerResearchItem { Name = "Starship Engineering", IsResearched = research.StarshipEngineering, TurnsRequired = research.StarshipEngineeringTurnsRequired },
                new AMinerResearchItem { Name = "Flagship Engineering", IsResearched = research.FlagshipEngineering, TurnsRequired = research.FlagshipEngineeringTurnsRequired },
                new AMinerResearchItem { Name = "Advanced Engineering", IsResearched = research.AdvancedEngineering, TurnsRequired = research.AdvancedEngineeringTurnsRequired },
                new AMinerResearchItem { Name = "Sensor Engineering", IsResearched = research.SensorEngineering, TurnsRequired = research.SensorEngineeringTurnsRequired },
                new AMinerResearchItem { Name = "Starbase Engineering", IsResearched = research.StarbaseEngineering, TurnsRequired = research.StarbaseEngineeringTurnsRequired },
                new AMinerResearchItem { Name = "Aspha Corvette", IsResearched = research.AsphaCorvette, TurnsRequired = research.AsphaCorvetteTurnsRequired },
                new AMinerResearchItem { Name = "Aspha Frigate", IsResearched = research.AsphaFrigate, TurnsRequired = research.AsphaFrigateTurnsRequired },
                new AMinerResearchItem { Name = "Aspha Destroyer", IsResearched = research.AsphaDestroyer, TurnsRequired = research.AsphaDestroyerTurnsRequired },
                new AMinerResearchItem { Name = "Aspha Cruiser", IsResearched = research.AsphaCruiser, TurnsRequired = research.AsphaCruiserTurnsRequired },
                new AMinerResearchItem { Name = "Aspha Battleship", IsResearched = research.AsphaBattleship, TurnsRequired = research.AsphaBattleshipTurnsRequired },
                new AMinerResearchItem { Name = "Aspha Dreadnought", IsResearched = research.AsphaDreadnought, TurnsRequired = research.AsphaDreadnoughtTurnsRequired },
                new AMinerResearchItem { Name = "Aspha Seeker", IsResearched = research.AsphaSeeker, TurnsRequired = research.AsphaSeekerTurnsRequired },
                new AMinerResearchItem { Name = "Aspha Ranger", IsResearched = research.AsphaRanger, TurnsRequired = research.AsphaRangerTurnsRequired },
                new AMinerResearchItem { Name = "Aspha Light Starbase", IsResearched = research.AsphaLightStarbase, TurnsRequired = research.AsphaLightStarbaseTurnsRequired },
                new AMinerResearchItem { Name = "Aspha Heavy Starbase", IsResearched = research.AsphaHeavyStarbase, TurnsRequired = research.AsphaHeavyStarbaseTurnsRequired },
                new AMinerResearchItem { Name = "G-Class Ship", IsResearched = research.GClassShip, TurnsRequired = research.GClassShipTurnsRequired },
                new AMinerResearchItem { Name = "G-Class Juggernaught", IsResearched = research.GClassJuggernaught, TurnsRequired = research.GClassJuggernaughtTurnsRequired }
            };
        }
    }

    public class AMinerResearchItem
    {
        public string Name { get; set; }
        public bool IsResearched { get; set; }
        public int TurnsRequired { get; set; }
    }
}
