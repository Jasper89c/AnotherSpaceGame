using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel;

namespace AnotherSpaceGame.Models
{
    public class ApplicationUser : IdentityUser
    {        
        // navigation property for ViralReversedShips
        public ViralReversedShips ViralReversedShips { get; set; }
        // navigation property for UWShips
        public UWShips UWShips { get; set; }
        // navigation property for counterattacks
        public ICollection<CounterAttacks> CounterAttacks { get; set; }
        // navigation property for missions
        public Missions Missions { get; set; }
        // Navigation property for Commodities
        public Commodities Commodities { get; set; }
        // Navigation property for Fleets (one-to-many)
        public ICollection<Fleet> Fleets { get; set; }
        // Navigation property for Planets (one-to-many)
        public ICollection<Planets> Planets { get; set; }
        // Navigation property for Turns (one-to-one)
        public Turns Turns { get; set; }
        // Navigation property for Artifacts (one-to-many)
        public ICollection<Artifacts> Artifacts { get; set; }
        // Navigation property for Infrstructer (one-to-one)
        public Infrastructer Infrastructer { get; set; }
        // Navigation property for EClassResearch (one-to-one)
        public EClassResearch EClassResearch { get; set; }
        // Navigation property for CyrilClassResearch (one-to-one)
        public CyrilClassResearch CyrilClassResearch { get; set; }
        // Navigation property for StrafezResearch (one-to-one)
        public StrafezResearch StrafezResearch { get; set; }
        // Navigation property for FClassResearch (one-to-one)
        public FClassResearch FClassResearch { get; set; }
        // Navigation property for ProjectsResearch (one-to-one)
        public ProjectsResearch ProjectsResearch { get; set; }
        // Navigation property for ViralSpecificResearch (one-to-one)
        public ViralSpecificResearch ViralSpecificResearch { get; set; }
        // Navigation property for CollectiveSpecificResearch (one-to-one)
        public CollectiveSpecificResearch CollectiveSpecificResearch { get; set; }
        // Navigation property for TerranResearch (one-to-one)
        public TerranResearch TerranResearch { get; set; }
        // Navigation property for AMinerResearch (one-to-one)
        public AMinerResearch AMinerResearch { get; set; }
        // Navigation property for MarauderResearch (one-to-one)
        public MarauderResearch MarauderResearch { get; set; }
        // navigation property for viralresearch
        public ViralResearch ViralResearch { get; set; }
        // Navigation property for CollectiveResearch (one-to-one)
        public CollectiveResearch CollectiveResearch { get; set; }
        // Navigation property for GuardianResearch (one-to-one)
        public GuardianResearch GuardianResearch { get; set; }
        // Navigation property for ClusterResearch (one-to-one)
        public ClusterResearch ClusterResearch { get; set; }
        // Navigation property for Exploration (one-to-one)
        public Exploration Exploration { get; set; }
        // Navigation property for ImportantEvents (one-to-many)
        public ICollection<ImportantEvents> ImportantEvents { get; set; }
        // Navigation property for Battlelogs (one-to-many)
        public ICollection<BattleLogs> Battlelogs { get; set; }
        // Federation relationship
        [Description("Federation Id")]
        public int? FederationId { get; set; }
        [Description("Federation")]
        public Federations? Federation { get; set; }
        public FederationApplication FederationApplication { get; set; }
        // Navigation property for MarketPosts (one-to-many)
        public ICollection<MarketPosts> MarketPosts { get; set; }

        // Navigation property for UserProjects (one-to-one)
        public UserProjects UserProjects { get; set; }

        // Additional properties for ApplicationUser

        public int PowerRating { get; set; }
        public DateTime PlayingSince { get; set; }
        public double EmpireAge { get; set; }
        public int BattlesWon { get; set; }
        public int BattlesLost { get; set; }
        public int ColoniesWon { get; set; }
        public int ColoniesLost { get; set; }
        public int ColoniesExplored { get; set; }
        public int PlanetsPlundered { get; set; }
        public Faction Faction { get; set; }
        public int TotalColonies { get; set; }
        public int TotalPlanets { get; set; }
        public DateTime DamageProtection { get; set; }
        public DateTime LastAction { get; set; }
        public decimal ArtifactShield { get; set; }
        public bool IsNPC { get; set; }
        public DateTime ITechCooldown { get; set; }
        public long TempleHeight { get; set; }
    }
}
