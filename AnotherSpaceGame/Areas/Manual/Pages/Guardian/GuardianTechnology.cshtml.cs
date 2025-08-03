using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnotherSpaceGame.Areas.Manual.Pages.Guardian
{
    public class GuardianTechnologyModel : PageModel
    {
        public GuardianResearch GuardianResearch { get; set; } = new GuardianResearch();
        public List<GuardianResearchItem> ResearchItems { get; set; }

        public void OnGet()
        {
            ResearchItems = GetResearchItems(GuardianResearch);
        }

        public static List<GuardianResearchItem> GetResearchItems(GuardianResearch research)
        {
            return new List<GuardianResearchItem>
            {
                new GuardianResearchItem { Name = "Fighter Class", IsResearched = research.FighterClass, TurnsRequired = research.FighterClassTurnsRequired },
                new GuardianResearchItem { Name = "Corvette Class", IsResearched = research.CorvetteClass, TurnsRequired = research.CorvetteClassTurnsRequired },
                new GuardianResearchItem { Name = "Frigate Class", IsResearched = research.FrigateClass, TurnsRequired = research.FrigateClassTurnsRequired },
                new GuardianResearchItem { Name = "Destroyer Class", IsResearched = research.DestroyerClass, TurnsRequired = research.DestroyerClassTurnsRequired },
                new GuardianResearchItem { Name = "Cruiser Class", IsResearched = research.CruiserClass, TurnsRequired = research.CruiserClassTurnsRequired },
                new GuardianResearchItem { Name = "Weapons Platform", IsResearched = research.WeaponsPlatform, TurnsRequired = research.WeaponsPlatformTurnsRequired },
                new GuardianResearchItem { Name = "Advanced Scouts", IsResearched = research.AdvancedScouts, TurnsRequired = research.AdvancedScoutsTurnsRequired },
                new GuardianResearchItem { Name = "L-Class Frigate", IsResearched = research.LClassFrigate, TurnsRequired = research.LClassFrigateTurnsRequired },
                new GuardianResearchItem { Name = "L-Class Destroyer", IsResearched = research.LClassDestroyer, TurnsRequired = research.LClassDestroyerTurnsRequired },
                new GuardianResearchItem { Name = "L-Class Cruiser", IsResearched = research.LClassCruiser, TurnsRequired = research.LClassCruiserTurnsRequired },
                new GuardianResearchItem { Name = "K-Class Destroyer", IsResearched = research.KClassDestroyer, TurnsRequired = research.KClassDestroyerTurnsRequired },
                new GuardianResearchItem { Name = "K-Class Cruiser", IsResearched = research.KClassCruiser, TurnsRequired = research.KClassCruiserTurnsRequired },
                new GuardianResearchItem { Name = "Advanced Engineering", IsResearched = research.AdvancedEngineering, TurnsRequired = research.AdvancedEngineeringTurnsRequired },
                new GuardianResearchItem { Name = "Focus Crystal", IsResearched = research.FocusCrystal, TurnsRequired = research.FocusCrystalTurnsRequired },
                new GuardianResearchItem { Name = "Organic Sensors", IsResearched = research.OrganicSensors, TurnsRequired = research.OrganicSensorsTurnsRequired },
                new GuardianResearchItem { Name = "Lance Torpodoes", IsResearched = research.LanceTorpodoes, TurnsRequired = research.LanceTorpodoesTurnsRequired }
            };
        }
    }

    public class GuardianResearchItem
    {
        public string Name { get; set; }
        public bool IsResearched { get; set; }
        public int TurnsRequired { get; set; }
    }
}
