using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnotherSpaceGame.Areas.Manual.Pages.AMiner
{
    public class AMinerModel : PageModel
    {
        public string FactionTaxModifier { get; private set; }
        public string FactionCommercialModifier { get; private set; }
        public string FactionIndustryModifier { get; private set; }
        public string FactionAgricultureModifier { get; private set; }
        public string FactionMiningModifier { get; private set; }
        public string FactionDemandForGoods { get; private set; }
        public string InfrastructreMaintenanceCost { get; private set; }
        public string MaxmumColonies { get; private set; }

        public void OnGet()
        {
            // Get the faction modifiers for viral
            var (factionTaxModifier, factionCommercialModifier, factionIndustryModifier,
                 factionAgricultureModifier, factionMiningModifier, factionDemandForGoods, infrastructureMaintenanceCost) = GetFactionModifiers(Faction.AMiner);
            // Convert to string for display
            FactionTaxModifier = FormatModifier(factionTaxModifier);
            FactionCommercialModifier = FormatModifier(factionCommercialModifier);
            FactionIndustryModifier = FormatModifier(factionIndustryModifier);
            FactionAgricultureModifier = FormatModifier(factionAgricultureModifier);
            FactionMiningModifier = FormatModifier(factionMiningModifier);
            FactionDemandForGoods = FormatModifier(factionDemandForGoods);
            InfrastructreMaintenanceCost = FormatModifier(infrastructureMaintenanceCost);
            MaxmumColonies = GetColonyCapForFaction(Faction.AMiner).ToString();
        }
        private string FormatModifier(decimal value)
        {
            if (value == 1.0m)
            {
                return $"0%";
            }
            if (value == 1.1m)
            {
                return $"+10%";
            }
            if (value == 1.2m)
            {
                return $"+20%";
            }
            if (value == 1.5m)
            {
                return $"+50%";
            }
            if (value == 2.2m)
            {
                return $"+220%";
            }
            if (value == 3.5m)
            {
                return $"+350%";
            }
            if (value == 29m)
            {
                return $"+2900%";
            }
            if (value == 0.05m)
            {
                return $"-95%";
            }
            if (value == 0.5m)
            {
                return $"-50%";
            }
            if (value == 0.8m)
            {
                return $"-20%";
            }
            if (value == 0.1m)
            {
                return $"-90%";
            }
            if (value == 0.01m)
            {
                return $"-99%";
            }
            if (value == 0.95m)
            {
                return $"-5%";
            }
            if (value == 0.15m)
            {
                return $"-85%";
            }
            if (value == 0.75m)
            {
                return $"-25%";
            }
            return $"{value}";
        }
        // Helper for faction modifiers
        private (decimal FactionTaxModifier, decimal FactionCommercialModifier, decimal FactionIndustryModifier,
                 decimal FactionAgricultureModifier, decimal FactionMiningModifier, decimal FactionDemandForGoods, decimal InfrastructreMaintenanceCost)
            GetFactionModifiers(Faction faction)
        {
            return faction switch
            {
                Faction.Terran => (1.0m, 1.1m, 2.2m, 1.2m, 1.0m, 3.5m, 1.0m),
                Faction.AMiner => (2.2m, 0.05m, 3.5m, 0.5m, 29m, 1.0m, 0.8m),
                Faction.Marauder => (0.5m, 0.05m, 0.5m, 0.5m, 0.5m, 1.0m, 0.05m),
                Faction.Viral => (1.2m, 0.95m, 0.5m, 0.8m, 1.0m, 2.0m, 1.0m),
                Faction.Collective => (0.5m, 0.05m, 0.1m, 1.5m, 0.5m, 0.05m, 1.0m),
                Faction.Guardian => (0.15m, 0.01m, 0.01m, 0.05m, 0.01m, 0.1m, 0.75m),
                Faction.KalZul => (1.0m, 1.0m, 1.0m, 1.0m, 1.0m, 3.5m, 1.0m),
                Faction.DarkMarauder => (1.0m, 1.0m, 1.0m, 1.0m, 1.0m, 3.5m, 1.0m),
                _ => (1.0m, 1.0m, 1.0m, 1.0m, 1.0m, 1.0m, 1.0m)
            };
        }

        private int GetColonyCapForFaction(Faction faction)
        {
            var maxColoniesByFaction = new Dictionary<Faction, int>
    {
        { Faction.Terran, 16 },
        { Faction.AMiner, 13 },
        { Faction.Collective, 14 },
        { Faction.Marauder, 13 },
        { Faction.Guardian, 11 },
        { Faction.Viral, 13 }
    };
            return maxColoniesByFaction.TryGetValue(faction, out int cap) ? cap : 10;
        }
    }
}
