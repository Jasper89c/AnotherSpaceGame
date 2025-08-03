using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnotherSpaceGame.Areas.Manual.Pages.Terran
{
    public class TerranTechnologyModel : PageModel
    {
        public TerranResearch TerranResearch { get; set; } = new TerranResearch();
        public List<TerranResearchItem> ResearchItems { get; set; }

        public void OnGet()
        {
            ResearchItems = GetTerranResearchItems(TerranResearch);
        }

        public static List<TerranResearchItem> GetTerranResearchItems(TerranResearch research)
        {
            return new List<TerranResearchItem>
            {
                new TerranResearchItem { Name = "Small Terran Hull", IsResearched = research.SmallTerranHull, TurnsRequired = research.SmallTerranHullTurnsRequired },
                new TerranResearchItem { Name = "Medium Terran Hull", IsResearched = research.MediumTerranHull, TurnsRequired = research.MediumTerranHullTurnsRequired },
                new TerranResearchItem { Name = "Large Terran Hull", IsResearched = research.LargeTerranHull, TurnsRequired = research.LargeTerranHullTurnsRequired },
                new TerranResearchItem { Name = "Terran Corvette", IsResearched = research.TerranCorvette, TurnsRequired = research.TerranCorvetteTurnsRequired },
                new TerranResearchItem { Name = "Terran Frigate", IsResearched = research.TerranFrigate, TurnsRequired = research.TerranFrigateTurnsRequired },
                new TerranResearchItem { Name = "Terran Destroyer", IsResearched = research.TerranDestroyer, TurnsRequired = research.TerranDestroyerTurnsRequired },
                new TerranResearchItem { Name = "Terran Cruiser", IsResearched = research.TerranCruiser, TurnsRequired = research.TerranCruiserTurnsRequired },
                new TerranResearchItem { Name = "Terran Battleship", IsResearched = research.TerranBattleship, TurnsRequired = research.TerranBattleshipTurnsRequired },
                new TerranResearchItem { Name = "Terran Advanced Scout", IsResearched = research.TerranAdvancedScout, TurnsRequired = research.TerranAdvancedScoutTurnsRequired },
                new TerranResearchItem { Name = "Terran Dreadnaught", IsResearched = research.TerranDreadnaught, TurnsRequired = research.TerranDreadnaughtTurnsRequired },
                new TerranResearchItem { Name = "Terran Starbase", IsResearched = research.TerranStarbase, TurnsRequired = research.TerranStarbaseTurnsRequired },
                new TerranResearchItem { Name = "Terran Juggernaught", IsResearched = research.TerranJuggernaught, TurnsRequired = research.TerranJuggernaughtTurnsRequired },
                new TerranResearchItem { Name = "Advanced Radar", IsResearched = research.AdvancedRadar, TurnsRequired = research.AdvancedRadarTurnsRequired },
                new TerranResearchItem { Name = "Crystal Lenses", IsResearched = research.CrystalLenses, TurnsRequired = research.CrystalLensesTurnsRequired },
                new TerranResearchItem { Name = "Photon Torpedo", IsResearched = research.PhotonTorpedo, TurnsRequired = research.PhotonTorpedoTurnsRequired },
                new TerranResearchItem { Name = "Photon Corvette", IsResearched = research.PhotonCorvette, TurnsRequired = research.PhotonCorvetteTurnsRequired },
                new TerranResearchItem { Name = "Photon Frigate", IsResearched = research.PhotonFrigate, TurnsRequired = research.PhotonFrigateTurnsRequired },
                new TerranResearchItem { Name = "Photon Destroyer", IsResearched = research.PhotonDestroyer, TurnsRequired = research.PhotonDestroyerTurnsRequired },
                new TerranResearchItem { Name = "Photon Cruiser", IsResearched = research.PhotonCruiser, TurnsRequired = research.PhotonCruiserTurnsRequired }
            };
        }
    }

    public class TerranResearchItem
    {
        public string Name { get; set; }
        public bool IsResearched { get; set; }
        public int TurnsRequired { get; set; }
    }
}
