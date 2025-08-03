using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class CapsuleLabModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private static readonly List<(ArtifactName Result, ArtifactName[] Recipe)> ArtifactCombinations = new()
        {
            // Existing combinations
            (ArtifactName.AmberDinero,     new[] { ArtifactName.YellowOrb, ArtifactName.BrownOrb, ArtifactName.PurpleOrb, ArtifactName.GrayOrb }),
            (ArtifactName.AmethystDinero,  new[] { ArtifactName.EnergyPod, ArtifactName.GoldenOrb, ArtifactName.OrangeOrb, ArtifactName.GreenOrb }),
            (ArtifactName.BronzeDinero,    new[] { ArtifactName.BlackOrb, ArtifactName.OrangeOrb, ArtifactName.GreenOrb, ArtifactName.BlueOrb }),
            (ArtifactName.CuartoMapa,      new[] { ArtifactName.WhiteOrb, ArtifactName.BlackOrb, ArtifactName.BlueOrb, ArtifactName.GreenOrb }),
            (ArtifactName.GarnetDinero,    new[] { ArtifactName.GreenOrb, ArtifactName.MoccasinOrb, ArtifactName.TurquoiseOrb, ArtifactName.GoldenOrb }),
            (ArtifactName.GoldDinero,      new[] { ArtifactName.GrayOrb, ArtifactName.PinkOrb, ArtifactName.AquaOrb, ArtifactName.TurquoiseOrb }),
            (ArtifactName.OpalDinero,      new[] { ArtifactName.EnergyPod, ArtifactName.YellowOrb, ArtifactName.AquaOrb, ArtifactName.BlueOrb }),
            (ArtifactName.PlatinumDinero,  new[] { ArtifactName.AquaOrb, ArtifactName.PlumOrb, ArtifactName.PinkOrb, ArtifactName.PinkOrb }),
            (ArtifactName.SilverDinero,    new[] { ArtifactName.YellowOrb, ArtifactName.WhiteOrb, ArtifactName.GrayOrb, ArtifactName.PurpleOrb }),
            (ArtifactName.TopazDinero,     new[] { ArtifactName.BlueOrb, ArtifactName.GoldenOrb, ArtifactName.OrangeOrb, ArtifactName.TurquoiseOrb }),

            // Previous new combinations
            (ArtifactName.MajorSuerte,     new[] { ArtifactName.AssimilatedBase, ArtifactName.SilverDinero, ArtifactName.BronzeDinero, ArtifactName.GoldDinero }),
            (ArtifactName.MinorAlimento,   new[] { ArtifactName.OpalDinero, ArtifactName.GarnetDinero, ArtifactName.TopazDinero, ArtifactName.AmberDinero }),
            (ArtifactName.MinorBarrera,    new[] { ArtifactName.CuartoMapa, ArtifactName.OpalDinero, ArtifactName.CuartoMapa, ArtifactName.OpalDinero }),
            (ArtifactName.MinorCosecha,    new[] { ArtifactName.AmberDinero, ArtifactName.GarnetDinero, ArtifactName.TopazDinero, ArtifactName.OpalDinero }),
            (ArtifactName.MinorGente,      new[] { ArtifactName.GarnetDinero, ArtifactName.PlatinumDinero, ArtifactName.PlatinumDinero, ArtifactName.GoldDinero }),
            (ArtifactName.MinorRequerido,  new[] { ArtifactName.SilverDinero, ArtifactName.OrganicBase, ArtifactName.GoldDinero, ArtifactName.PlatinumDinero }),
            (ArtifactName.MinorSuerte,     new[] { ArtifactName.CuartoMapa, ArtifactName.OrganicBase, ArtifactName.SilverDinero, ArtifactName.BronzeDinero }),
            (ArtifactName.MinorTierra,     new[] { ArtifactName.GoldDinero, ArtifactName.SilverDinero, ArtifactName.CuartoMapa, ArtifactName.SilverDinero }),
            (ArtifactName.SmallTimeCapsule,new[] { ArtifactName.MinorGordo, ArtifactName.MinorBarrera, ArtifactName.MinorTierra, ArtifactName.Traicione }),
            (ArtifactName.Traicione,       new[] { ArtifactName.OpalDinero, ArtifactName.AmberDinero, ArtifactName.BronzeDinero, ArtifactName.CuartoMapa }),

            // Newest combinations from your latest message
            (ArtifactName.BigTimeCapsule,  new[] { ArtifactName.SmallTimeCapsule, ArtifactName.Regalo, ArtifactName.MajorProducto, ArtifactName.SmallTimeCapsule }),
            (ArtifactName.MinorGordo,      new[] { ArtifactName.MinorCosecha, ArtifactName.MinorCosecha, ArtifactName.MinorTierra, ArtifactName.MinorAlimento }),
            (ArtifactName.Historia,        new[] { ArtifactName.MinorSuerte, ArtifactName.SmallTimeCapsule, ArtifactName.MinorRequerido, ArtifactName.MajorSuerte }),
            (ArtifactName.MinorAfortunado, new[] { ArtifactName.MinorRequerido, ArtifactName.MajorSuerte, ArtifactName.MinorSuerte, ArtifactName.MinorGente }),
            (ArtifactName.MajorAfortunado, new[] { ArtifactName.MajorSuerte, ArtifactName.MinorRequerido, ArtifactName.MinorGente, ArtifactName.MinorAlimento }),
            (ArtifactName.MinorEstructura, new[] { ArtifactName.MinorGente, ArtifactName.MinorRequerido, ArtifactName.SmallTimeCapsule, ArtifactName.MinorCosecha }),
            (ArtifactName.MajorAlimento,   new[] { ArtifactName.MinorBarrera, ArtifactName.MinorGordo, ArtifactName.MinorBarrera, ArtifactName.MinorGordo }),
            (ArtifactName.MajorCosecha,    new[] { ArtifactName.MinorGordo, ArtifactName.Traicione, ArtifactName.Traicione, ArtifactName.MinorGordo }),
            (ArtifactName.MajorTierra,     new[] { ArtifactName.MinorCosecha, ArtifactName.MinorTierra, ArtifactName.MinorTierra, ArtifactName.MinorCosecha }),
            (ArtifactName.Persiana,        new[] { ArtifactName.MinorRequerido, ArtifactName.SmallTimeCapsule, ArtifactName.MinorGente, ArtifactName.SmallTimeCapsule }),
            (ArtifactName.MajorBarrera,    new[] { ArtifactName.MinorRequerido, ArtifactName.MinorGente, ArtifactName.MinorAlimento, ArtifactName.MinorGordo }),
            (ArtifactName.Regalo,          new[] { ArtifactName.MinorGordo, ArtifactName.GarnetDinero, ArtifactName.Traicione, ArtifactName.MinorAlimento }),
            (ArtifactName.MajorProducto,   new[] { ArtifactName.MinorCosecha, ArtifactName.MinorTierra, ArtifactName.Traicione, ArtifactName.MinorGente }),
            (ArtifactName.MajorDinero,     new[] { ArtifactName.MinorAlimento, ArtifactName.MinorCosecha, ArtifactName.MinorTierra, ArtifactName.AmberDinero }),

            (ArtifactName.GrandAlimenter, new[] { ArtifactName.MajorAfortunado, ArtifactName.MinorAfortunado, ArtifactName.MinorEstructura, ArtifactName.MajorCosecha }),
            (ArtifactName.GrandAlimento, new[] { ArtifactName.MajorGordo, ArtifactName.Historia, ArtifactName.MajorBarrera, ArtifactName.MajorProducto }),
            (ArtifactName.GrandBarrera, new[] { ArtifactName.Persiana, ArtifactName.MajorTierra, ArtifactName.MajorBarrera, ArtifactName.MajorGordo }),
            (ArtifactName.GrandCosecha, new[] { ArtifactName.MajorAfortunado, ArtifactName.MajorAlimento, ArtifactName.MinorEstructura, ArtifactName.MajorTierra }),
            (ArtifactName.GrandDinero, new[] { ArtifactName.MajorCosecha, ArtifactName.MajorAlimento, ArtifactName.MinorEstructura, ArtifactName.MinorRequerido }),
            (ArtifactName.GrandEstructura, new[] { ArtifactName.MinorGordo, ArtifactName.MinorEstructura, ArtifactName.Historia, ArtifactName.MinorAfortunado }),
            (ArtifactName.GrandGente, new[] { ArtifactName.MajorCosecha, ArtifactName.BigTimeCapsule, ArtifactName.MajorTierra, ArtifactName.SmallTimeCapsule }),
            (ArtifactName.GrandProducto, new[] { ArtifactName.Regalo, ArtifactName.MajorAlimento, ArtifactName.MinorAlimento, ArtifactName.Regalo }),
            (ArtifactName.GrandRequerido, new[] { ArtifactName.MajorGordo, ArtifactName.Persiana, ArtifactName.MajorTierra, ArtifactName.MajorCosecha }),
            (ArtifactName.GrandTierra, new[] { ArtifactName.MajorAlimento, ArtifactName.MajorTierra, ArtifactName.MajorCosecha, ArtifactName.Persiana }),
            (ArtifactName.MajorGordo, new[] { ArtifactName.Regalo, ArtifactName.Persiana, ArtifactName.MinorGordo, ArtifactName.MajorAfortunado }),
            (ArtifactName.PlanetaryCore, new[] { ArtifactName.BronzeDinero, ArtifactName.GrandAlimenter, ArtifactName.GrandGente, ArtifactName.GrandCosecha }),
        };
        public List<ArtifactCombinationDisplay> DisplayCombinations { get; set; } = new();
        public bool CanAccessCapsuleLab { get; set; }
        public List<Artifacts> UserArtifacts { get; set; } = new();
        [BindProperty]
        public int SelectedArtifact1 { get; set; }
        [BindProperty]
        public int SelectedArtifact2 { get; set; }
        [BindProperty]
        public int SelectedArtifact3 { get; set; }
        [BindProperty]
        public int SelectedArtifact4 { get; set; }
        public string CreationMessage { get; set; }
        public List<ArtifactSelectOption> ArtifactOptions { get; set; } = new();

        public CapsuleLabModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var userProjects = _context.UserProjects.FirstOrDefault(up => up.ApplicationUserId == user.Id);

            if (userProjects != null && userProjects.CapsuleLab && userProjects.CapsuleLabUnlockTimer < DateTime.Now)
            {
                CanAccessCapsuleLab = true;
            }
            else
            {
                CanAccessCapsuleLab = false;
            }

            UserArtifacts = _context.Artifacts
                .Where(a => a.ApplicationUserId == user.Id)
                .ToList();

            ArtifactOptions = UserArtifacts
        .Where(a => a.Total > 0)
        .OrderBy(a => a.ArtifactType) // Order by type first
        .ThenBy(a => a.ArtifactName)  // Then by name (optional)
        .Select(a => new ArtifactSelectOption
        {
            ArtifactId = a.ArtifactId,
            DisplayName = $"{a.ArtifactName} (x{a.Total})"
        })
        .ToList();
            ServerStats serverStats = _context.ServerStats.FirstOrDefault();
            if (serverStats.UWEnabled == true)
            {
                return RedirectToPage("/Projects");
            }

            BuildDisplayCombinations();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            // Reload artifacts for the select menus
            UserArtifacts = _context.Artifacts
                .Where(a => a.ApplicationUserId == user.Id)
                .ToList();

            ArtifactOptions = UserArtifacts
        .Where(a => a.Total > 0)
        .OrderBy(a => a.ArtifactType) // Order by type first
        .ThenBy(a => a.ArtifactName)  // Then by name (optional)
        .Select(a => new ArtifactSelectOption
        {
            ArtifactId = a.ArtifactId,
            DisplayName = $"{a.ArtifactName} (x{a.Total})"
        })
        .ToList();

            var selectedIds = new[] { SelectedArtifact1, SelectedArtifact2, SelectedArtifact3, SelectedArtifact4 };

            // Get the selected artifacts in order
            var selectedArtifacts = UserArtifacts
                .Where(a => selectedIds.Contains(a.ArtifactId))
                .OrderBy(a => Array.IndexOf(selectedIds, a.ArtifactId))
                .ToList();
            // Check if the user actually owns all selected artifacts
            if (selectedArtifacts[0].Total < 1 && selectedArtifacts[1].Total < 1 && selectedArtifacts[2].Total < 1 && selectedArtifacts[3].Total < 1)
            {
                if (selectedArtifacts[0].Total < 1)
                {
                    CreationMessage += $"insufficient {selectedArtifacts[0].ArtifactName} </br >";
                }
                if (selectedArtifacts[1].Total < 1)
                {
                    CreationMessage += $"insufficient {selectedArtifacts[1].ArtifactName} </br >";
                }
                if (selectedArtifacts[2].Total < 1)
                {
                    CreationMessage += $"insufficient {selectedArtifacts[2].ArtifactName} </br >";
                }
                if (selectedArtifacts[3].Total < 1)
                {
                    CreationMessage += $"insufficient {selectedArtifacts[3].ArtifactName} </br >";
                }
                BuildDisplayCombinations();
                return Page();
            }

            // Get their names in order
            var selectedNames = selectedArtifacts.Select(a => a.ArtifactName).ToArray();

            // Find a matching recipe
            var match = ArtifactCombinations.FirstOrDefault(combo =>
                combo.Recipe.SequenceEqual(selectedNames)
            );

            if (match.Result != default)
            {
                var existingArtifact = _context.Artifacts
                    .FirstOrDefault(a => a.ApplicationUserId == user.Id && a.ArtifactName == match.Result);
                if (existingArtifact.ArtifactName == match.Result && user.Id == existingArtifact.ApplicationUserId)
                {
                    if (existingArtifact.Total >= existingArtifact.MaxTotal)
                    {
                        CreationMessage = $"You have reached the maximum limit for {match.Result}. You cannot create more.";
                        BuildDisplayCombinations();
                        return Page();
                    }
                    existingArtifact.Total += 1; // Increment the total count of the existing artifact
                    CreationMessage = $"You have successfully created a {match.Result}!";
                    // Update the existing artifact
                    _context.Artifacts.Update(existingArtifact);
                    // No need to create a new artifact, just update the existing one
                }
                else
                {
                    var newArtifact = new Artifacts
                    {
                        Total = 1, // Assuming one artifact is created per combination
                        ApplicationUserId = user.Id,
                        ApplicationUser = user,
                        ArtifactName = match.Result
                    };
                    // Create the new artifact
                    switch (match.Result)
                    {
                        case ArtifactName.AquaOrb:
                            newArtifact.ArtifactId = 1;
                            newArtifact.ArtifactName = ArtifactName.AquaOrb;
                            newArtifact.ArtifactType = ArtifactType.Common;
                            newArtifact.MaxTotal = 255;
                            break;
                        case ArtifactName.BlackOrb:
                            newArtifact.ArtifactId = 2;
                            newArtifact.ArtifactName = ArtifactName.BlackOrb;
                            newArtifact.ArtifactType = ArtifactType.Common;
                            newArtifact.MaxTotal = 255;
                            break;
                        case ArtifactName.BlueOrb:
                            newArtifact.ArtifactId = 3;
                            newArtifact.ArtifactName = ArtifactName.BlueOrb;
                            newArtifact.ArtifactType = ArtifactType.Common;
                            newArtifact.MaxTotal = 255;
                            break;
                        case ArtifactName.BrownOrb:
                            newArtifact.ArtifactId = 4;
                            newArtifact.ArtifactName = ArtifactName.BrownOrb;
                            newArtifact.ArtifactType = ArtifactType.Common;
                            newArtifact.MaxTotal = 255;
                            break;
                        case ArtifactName.EnergyPod:
                            newArtifact.ArtifactId = 5;
                            newArtifact.ArtifactName = ArtifactName.EnergyPod;
                            newArtifact.ArtifactType = ArtifactType.Common;
                            newArtifact.MaxTotal = 255;
                            break;
                        case ArtifactName.GoldenOrb:
                            newArtifact.ArtifactId = 6;
                            newArtifact.ArtifactName = ArtifactName.GoldenOrb;
                            newArtifact.ArtifactType = ArtifactType.Common;
                            newArtifact.MaxTotal = 255;
                            break;
                        case ArtifactName.GrayOrb:
                            newArtifact.ArtifactId = 7;
                            newArtifact.ArtifactName = ArtifactName.GrayOrb;
                            newArtifact.ArtifactType = ArtifactType.Common;
                            newArtifact.MaxTotal = 255;
                            break;
                        case ArtifactName.GreenOrb:
                            newArtifact.ArtifactId = 8;
                            newArtifact.ArtifactName = ArtifactName.GreenOrb;
                            newArtifact.ArtifactType = ArtifactType.Common;
                            newArtifact.MaxTotal = 255;
                            break;
                        case ArtifactName.MoccasinOrb:
                            newArtifact.ArtifactId = 9;
                            newArtifact.ArtifactName = ArtifactName.MoccasinOrb;
                            newArtifact.ArtifactType = ArtifactType.Common;
                            newArtifact.MaxTotal = 255;
                            break;
                        case ArtifactName.OrangeOrb:
                            newArtifact.ArtifactId = 10;
                            newArtifact.ArtifactName = ArtifactName.OrangeOrb;
                            newArtifact.ArtifactType = ArtifactType.Common;
                            newArtifact.MaxTotal = 255;
                            break;
                        case ArtifactName.PinkOrb:
                            newArtifact.ArtifactId = 11;
                            newArtifact.ArtifactName = ArtifactName.PinkOrb;
                            newArtifact.ArtifactType = ArtifactType.Common;
                            newArtifact.MaxTotal = 255;
                            break;
                        case ArtifactName.PlumOrb:
                            newArtifact.ArtifactId = 12;
                            newArtifact.ArtifactName = ArtifactName.PlumOrb;
                            newArtifact.ArtifactType = ArtifactType.Common;
                            newArtifact.MaxTotal = 255;
                            break;
                        case ArtifactName.PurpleOrb:
                            newArtifact.ArtifactId = 13;
                            newArtifact.ArtifactName = ArtifactName.PurpleOrb;
                            newArtifact.ArtifactType = ArtifactType.Common;
                            newArtifact.MaxTotal = 255;
                            break;
                        case ArtifactName.TurquoiseOrb:
                            newArtifact.ArtifactId = 14;
                            newArtifact.ArtifactName = ArtifactName.TurquoiseOrb;
                            newArtifact.ArtifactType = ArtifactType.Common;
                            newArtifact.MaxTotal = 255;
                            break;
                        case ArtifactName.WhiteOrb:
                            newArtifact.ArtifactId = 15;
                            newArtifact.ArtifactName = ArtifactName.WhiteOrb;
                            newArtifact.ArtifactType = ArtifactType.Common;
                            newArtifact.MaxTotal = 255;
                            break;
                        case ArtifactName.YellowOrb:
                            newArtifact.ArtifactId = 16;
                            newArtifact.ArtifactName = ArtifactName.YellowOrb;
                            newArtifact.ArtifactType = ArtifactType.Common;
                            newArtifact.MaxTotal = 255;
                            break;
                        case ArtifactName.AmberDinero:
                            newArtifact.ArtifactId = 17;
                            newArtifact.ArtifactName = ArtifactName.AmberDinero;
                            newArtifact.ArtifactType = ArtifactType.Uncommon;
                            newArtifact.MaxTotal = 100;
                            break;
                        case ArtifactName.AmethystDinero:
                            newArtifact.ArtifactId = 18;
                            newArtifact.ArtifactName = ArtifactName.AmethystDinero;
                            newArtifact.ArtifactType = ArtifactType.Uncommon;
                            newArtifact.MaxTotal = 100;
                            break;
                        case ArtifactName.AssimilatedBase:
                            newArtifact.ArtifactId = 19;
                            newArtifact.ArtifactName = ArtifactName.AssimilatedBase;
                            newArtifact.ArtifactType = ArtifactType.Uncommon;
                            newArtifact.MaxTotal = 100;
                            break;
                        case ArtifactName.BronzeDinero:
                            newArtifact.ArtifactId = 20;
                            newArtifact.ArtifactName = ArtifactName.BronzeDinero;
                            newArtifact.ArtifactType = ArtifactType.Uncommon;
                            newArtifact.MaxTotal = 100;
                            break;
                        case ArtifactName.CuartoMapa:
                            newArtifact.ArtifactId = 21;
                            newArtifact.ArtifactName = ArtifactName.CuartoMapa;
                            newArtifact.ArtifactType = ArtifactType.Uncommon;
                            newArtifact.MaxTotal = 100;
                            break;
                        case ArtifactName.GarnetDinero:
                            newArtifact.ArtifactId = 22;
                            newArtifact.ArtifactName = ArtifactName.GarnetDinero;
                            newArtifact.ArtifactType = ArtifactType.Uncommon;
                            newArtifact.MaxTotal = 100;
                            break;
                        case ArtifactName.GoldDinero:
                            newArtifact.ArtifactId = 23;
                            newArtifact.ArtifactName = ArtifactName.GoldDinero;
                            newArtifact.ArtifactType = ArtifactType.Uncommon;
                            newArtifact.MaxTotal = 100;
                            break;
                        case ArtifactName.OpalDinero:
                            newArtifact.ArtifactId = 24;
                            newArtifact.ArtifactName = ArtifactName.OpalDinero;
                            newArtifact.ArtifactType = ArtifactType.Uncommon;
                            newArtifact.MaxTotal = 100;
                            break;
                        case ArtifactName.OrganicBase:
                            newArtifact.ArtifactId = 25;
                            newArtifact.ArtifactName = ArtifactName.OrganicBase;
                            newArtifact.ArtifactType = ArtifactType.Uncommon;
                            newArtifact.MaxTotal = 100;
                            break;
                        case ArtifactName.PlatinumDinero:
                            newArtifact.ArtifactId = 26;
                            newArtifact.ArtifactName = ArtifactName.PlatinumDinero;
                            newArtifact.ArtifactType = ArtifactType.Uncommon;
                            newArtifact.MaxTotal = 100;
                            break;
                        case ArtifactName.SilverDinero:
                            newArtifact.ArtifactId = 27;
                            newArtifact.ArtifactName = ArtifactName.SilverDinero;
                            newArtifact.ArtifactType = ArtifactType.Uncommon;
                            newArtifact.MaxTotal = 100;
                            break;
                        case ArtifactName.TopazDinero:
                            newArtifact.ArtifactId = 28;
                            newArtifact.ArtifactName = ArtifactName.TopazDinero;
                            newArtifact.ArtifactType = ArtifactType.Uncommon;
                            newArtifact.MaxTotal = 100;
                            break;
                        case ArtifactName.MajorSuerte:
                            newArtifact.ArtifactId = 29;
                            newArtifact.ArtifactName = ArtifactName.MajorSuerte;
                            newArtifact.ArtifactType = ArtifactType.Rare;
                            newArtifact.MaxTotal = 50;
                            break;
                        case ArtifactName.MinorAlimento:
                            newArtifact.ArtifactId = 30;
                            newArtifact.ArtifactName = ArtifactName.MinorAlimento;
                            newArtifact.ArtifactType = ArtifactType.Rare;
                            newArtifact.MaxTotal = 50;
                            break;
                        case ArtifactName.MinorBarrera:
                            newArtifact.ArtifactId = 31;
                            newArtifact.ArtifactName = ArtifactName.MinorBarrera;
                            newArtifact.ArtifactType = ArtifactType.Rare;
                            newArtifact.MaxTotal = 50;
                            break;
                        case ArtifactName.MinorCosecha:
                            newArtifact.ArtifactId = 32;
                            newArtifact.ArtifactName = ArtifactName.MinorCosecha;
                            newArtifact.ArtifactType = ArtifactType.Rare;
                            newArtifact.MaxTotal = 50;
                            break;
                        case ArtifactName.MinorGente:
                            newArtifact.ArtifactId = 33;
                            newArtifact.ArtifactName = ArtifactName.MinorGente;
                            newArtifact.ArtifactType = ArtifactType.Rare;
                            newArtifact.MaxTotal = 50;
                            break;
                        case ArtifactName.MinorRequerido:
                            newArtifact.ArtifactId = 34;
                            newArtifact.ArtifactName = ArtifactName.MinorRequerido;
                            newArtifact.ArtifactType = ArtifactType.Rare;
                            newArtifact.MaxTotal = 50;
                            break;
                        case ArtifactName.MinorSuerte:
                            newArtifact.ArtifactId = 35;
                            newArtifact.ArtifactName = ArtifactName.MinorSuerte;
                            newArtifact.ArtifactType = ArtifactType.Rare;
                            newArtifact.MaxTotal = 50;
                            break;
                        case ArtifactName.MinorTierra:
                            newArtifact.ArtifactId = 36;
                            newArtifact.ArtifactName = ArtifactName.MinorTierra;
                            newArtifact.ArtifactType = ArtifactType.Rare;
                            newArtifact.MaxTotal = 50;
                            break;
                        case ArtifactName.SmallTimeCapsule:
                            newArtifact.ArtifactId = 37;
                            newArtifact.ArtifactName = ArtifactName.SmallTimeCapsule;
                            newArtifact.ArtifactType = ArtifactType.Rare;
                            newArtifact.MaxTotal = 50;
                            break;
                        case ArtifactName.Traicione:
                            newArtifact.ArtifactId = 38;
                            newArtifact.ArtifactName = ArtifactName.Traicione;
                            newArtifact.ArtifactType = ArtifactType.Rare;
                            newArtifact.MaxTotal = 50;
                            break;
                        case ArtifactName.BigTimeCapsule:
                            newArtifact.ArtifactId = 39;
                            newArtifact.ArtifactName = ArtifactName.BigTimeCapsule;
                            newArtifact.ArtifactType = ArtifactType.Unique;
                            newArtifact.MaxTotal = 25;
                            break;
                        case ArtifactName.Historia:
                            newArtifact.ArtifactId = 40;
                            newArtifact.ArtifactName = ArtifactName.Historia;
                            newArtifact.ArtifactType = ArtifactType.Unique;
                            newArtifact.MaxTotal = 25;
                            break;
                        case ArtifactName.MajorAfortunado:
                            newArtifact.ArtifactId = 41;
                            newArtifact.ArtifactName = ArtifactName.MajorAfortunado;
                            newArtifact.ArtifactType = ArtifactType.Unique;
                            newArtifact.MaxTotal = 25;
                            break;
                        case ArtifactName.MajorAlimento:
                            newArtifact.ArtifactId = 42;
                            newArtifact.ArtifactName = ArtifactName.MajorAlimento;
                            newArtifact.ArtifactType = ArtifactType.Unique;
                            newArtifact.MaxTotal = 25;
                            break;
                        case ArtifactName.MajorBarrera:
                            newArtifact.ArtifactId = 43;
                            newArtifact.ArtifactName = ArtifactName.MajorBarrera;
                            newArtifact.ArtifactType = ArtifactType.Unique;
                            newArtifact.MaxTotal = 25;
                            break;
                        case ArtifactName.MajorCosecha:
                            newArtifact.ArtifactId = 44;
                            newArtifact.ArtifactName = ArtifactName.MajorCosecha;
                            newArtifact.ArtifactType = ArtifactType.Unique;
                            newArtifact.MaxTotal = 25;
                            break;
                        case ArtifactName.MajorDinero:
                            newArtifact.ArtifactId = 45;
                            newArtifact.ArtifactName = ArtifactName.MajorDinero;
                            newArtifact.ArtifactType = ArtifactType.Unique;
                            newArtifact.MaxTotal = 25;
                            break;
                        case ArtifactName.MajorProducto:
                            newArtifact.ArtifactId = 46;
                            newArtifact.ArtifactName = ArtifactName.MajorProducto;
                            newArtifact.ArtifactType = ArtifactType.Unique;
                            newArtifact.MaxTotal = 25;
                            break;
                        case ArtifactName.MajorTierra:
                            newArtifact.ArtifactId = 47;
                            newArtifact.ArtifactName = ArtifactName.MajorTierra;
                            newArtifact.ArtifactType = ArtifactType.Unique;
                            newArtifact.MaxTotal = 25;
                            break;
                        case ArtifactName.MinorAfortunado:
                            newArtifact.ArtifactId = 48;
                            newArtifact.ArtifactName = ArtifactName.MinorAfortunado;
                            newArtifact.ArtifactType = ArtifactType.Unique;
                            newArtifact.MaxTotal = 25;
                            break;
                        case ArtifactName.MinorEstructura:
                            newArtifact.ArtifactId = 49;
                            newArtifact.ArtifactName = ArtifactName.MinorEstructura;
                            newArtifact.ArtifactType = ArtifactType.Unique;
                            newArtifact.MaxTotal = 25;
                            break;
                        case ArtifactName.MinorGordo:
                            newArtifact.ArtifactId = 50;
                            newArtifact.ArtifactName = ArtifactName.MinorGordo;
                            newArtifact.ArtifactType = ArtifactType.Unique;
                            newArtifact.MaxTotal = 25;
                            break;
                        case ArtifactName.Persiana:
                            newArtifact.ArtifactId = 51;
                            newArtifact.ArtifactName = ArtifactName.Persiana;
                            newArtifact.ArtifactType = ArtifactType.Unique;
                            newArtifact.MaxTotal = 25;
                            break;
                        case ArtifactName.Regalo:
                            newArtifact.ArtifactId = 52;
                            newArtifact.ArtifactName = ArtifactName.Regalo;
                            newArtifact.ArtifactType = ArtifactType.Unique;
                            newArtifact.MaxTotal = 25;
                            break;
                        case ArtifactName.GrandAlimenter:
                            newArtifact.ArtifactId = 53;
                            newArtifact.ArtifactName = ArtifactName.GrandAlimenter;
                            newArtifact.ArtifactType = ArtifactType.Special;
                            newArtifact.MaxTotal = 10;
                            break;
                        case ArtifactName.GrandAlimento:
                            newArtifact.ArtifactId = 54;
                            newArtifact.ArtifactName = ArtifactName.GrandAlimento;
                            newArtifact.ArtifactType = ArtifactType.Special;
                            newArtifact.MaxTotal = 10;
                            break;
                        case ArtifactName.GrandBarrera:
                            newArtifact.ArtifactId = 55;
                            newArtifact.ArtifactName = ArtifactName.GrandBarrera;
                            newArtifact.ArtifactType = ArtifactType.Special;
                            newArtifact.MaxTotal = 10;
                            break;
                        case ArtifactName.GrandCosecha:
                            newArtifact.ArtifactId = 56;
                            newArtifact.ArtifactName = ArtifactName.GrandCosecha;
                            newArtifact.ArtifactType = ArtifactType.Special;
                            newArtifact.MaxTotal = 10;
                            break;
                        case ArtifactName.GrandDinero:
                            newArtifact.ArtifactId = 57;
                            newArtifact.ArtifactName = ArtifactName.GrandDinero;
                            newArtifact.ArtifactType = ArtifactType.Special;
                            newArtifact.MaxTotal = 10;
                            break;
                        case ArtifactName.GrandEstructura:
                            newArtifact.ArtifactId = 58;
                            newArtifact.ArtifactName = ArtifactName.GrandEstructura;
                            newArtifact.ArtifactType = ArtifactType.Special;
                            newArtifact.MaxTotal = 10;
                            break;
                        case ArtifactName.GrandGente:
                            newArtifact.ArtifactId = 59;
                            newArtifact.ArtifactName = ArtifactName.GrandGente;
                            newArtifact.ArtifactType = ArtifactType.Special;
                            newArtifact.MaxTotal = 10;
                            break;
                        case ArtifactName.GrandProducto:
                            newArtifact.ArtifactId = 60;
                            newArtifact.ArtifactName = ArtifactName.GrandProducto;
                            newArtifact.ArtifactType = ArtifactType.Special;
                            newArtifact.MaxTotal = 10;
                            break;
                        case ArtifactName.GrandRequerido:
                            newArtifact.ArtifactId = 61;
                            newArtifact.ArtifactName = ArtifactName.GrandRequerido;
                            newArtifact.ArtifactType = ArtifactType.Special;
                            newArtifact.MaxTotal = 10;
                            break;
                        case ArtifactName.GrandTierra:
                            newArtifact.ArtifactId = 62;
                            newArtifact.ArtifactName = ArtifactName.GrandTierra;
                            newArtifact.ArtifactType = ArtifactType.Special;
                            newArtifact.MaxTotal = 10;
                            break;
                        case ArtifactName.MajorGordo:
                            newArtifact.ArtifactId = 63;
                            newArtifact.ArtifactName = ArtifactName.MajorGordo;
                            newArtifact.ArtifactType = ArtifactType.Special;
                            newArtifact.MaxTotal = 10;
                            break;
                        case ArtifactName.PlanetaryCore:
                            newArtifact.ArtifactId = 64;
                            newArtifact.ArtifactName = ArtifactName.PlanetaryCore;
                            newArtifact.ArtifactType = ArtifactType.Special;
                            newArtifact.MaxTotal = 10;
                            break;
                        default:
                            break;
                    }
                    _context.Artifacts.Add(newArtifact);
                    CreationMessage = $"You have successfully created a {match.Result}!";
                }
                // Remove the used artifacts
                // selectedIds: the array of 4 selected artifact IDs, in order
                foreach (var artifactId in selectedIds)
                {
                    var artifact = UserArtifacts.FirstOrDefault(a => a.ArtifactId == artifactId);
                    if (artifact != null)
                    {
                        artifact.Total -= 1;
                        if (artifact.Total <= 0)
                        {
                            _context.Artifacts.Remove(artifact);
                        }
                    }
                }
                await _context.SaveChangesAsync();

                CreationMessage = $"You have successfully created {match.Result}!";
            }
            else
            {
                CreationMessage = "No valid combination found for the selected artifacts and order.";
            }
            BuildDisplayCombinations();
            UserArtifacts = _context.Artifacts
                .Where(a => a.ApplicationUserId == user.Id)
                .ToList();

            ArtifactOptions = UserArtifacts
        .Where(a => a.Total > 0)
        .OrderBy(a => a.ArtifactType) // Order by type first
        .ThenBy(a => a.ArtifactName)  // Then by name (optional)
        .Select(a => new ArtifactSelectOption
        {
            ArtifactId = a.ArtifactId,
            DisplayName = $"{a.ArtifactName} (x{a.Total})"
        })
        .ToList();
            return Page();
        }
        private string GetArtifactDisplayName(ArtifactName name)
        {
            var field = typeof(ArtifactName).GetField(name.ToString());
            var attr = (System.ComponentModel.DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(System.ComponentModel.DescriptionAttribute));
            return attr != null ? attr.Description : name.ToString();
        }

        private void BuildDisplayCombinations()
        {
            DisplayCombinations = ArtifactCombinations
                .Select(combo => new ArtifactCombinationDisplay
                {
                    ResultName = GetArtifactDisplayName(combo.Result),
                    RecipeNames = combo.Recipe.Select(GetArtifactDisplayName).ToList()
                })
                .ToList();
        }
    }

    public class ArtifactSelectOption
    {
        public int ArtifactId { get; set; }
        public string DisplayName { get; set; }
    }
    public class ArtifactCombinationDisplay
    {
        public string ResultName { get; set; }
        public List<string> RecipeNames { get; set; }
    }
}
