using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnotherSpaceGame.Models
{
    public class NPCs
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; } 
        [Description("Federation Id")]
        public int? FederationId { get; set; } = null;
        [Description("Federation")]
        public Federations? Federation { get; set; } = null;
        public int PowerRating { get; set; } = Random.Shared.Next(15000, 2000000);
        public DateTime PlayingSince { get; set; } = DateTime.Now;
        public double EmpireAge { get; set; } = 0.0;
        public int BattlesWon { get; set; } = 0;
        public int BattlesLost { get; set; } = 0;
        public int ColoniesWon { get; set; } = 0;
        public int ColoniesLost { get; set; } = 0;
        public int ColoniesExplored { get; set; } = 0;
        public int PlanetsPlundered { get; set; } = 0;
        public Faction Faction { get; set; } = GetRandomFaction();
        public int TotalColonies { get; set; } = 0;
        public int TotalPlanets { get; set; } = 0;
        public DateTime DamageProtection { get; set; } = DateTime.Now;
        public DateTime LastAction { get; set; } = DateTime.Now;
        public decimal ArtifactShield { get; set; } = 0.0m;
        public bool IsNPC { get; set; } = true;

        private static Faction GetRandomFaction()
        {
            // Generate a random Faction value
            var values = Enum.GetValues(typeof(Faction));
            return (Faction)values.GetValue(Random.Shared.Next(values.Length))!;
        }
    }
}
