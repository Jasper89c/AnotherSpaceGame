using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class TechnologyResearchModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly TurnService _turnService;
        public TechnologyResearchModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context, TurnService turnService)
        {
            _userManager = userManager;
            _context = context;
            _turnService = turnService;
        }

        public List<AvailableResearch> ListOfAvailableResearch { get; set; }
        [BindProperty]
        public string SelectedResearchName { get; set; }

        [BindProperty]
        public int TurnsToInvest { get; set; }
        public string StatusMessage { get; set; }
        public string ResearchMessage { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            ListOfAvailableResearch = new List<AvailableResearch>();
            GetAvailableResearch(user);
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            var currentUser = _context.Users.FirstOrDefault(x => x.Id == user.Id);
            if (string.IsNullOrEmpty(SelectedResearchName) || TurnsToInvest <= 0)
            {
                GetAvailableResearch(currentUser);
                return Page();
            }

            int availableTurns = await _turnService.GetTurnsAsync(currentUser.Id);

            // Check if user has enough turns
            if (TurnsToInvest > availableTurns)
            {
                TurnsToInvest = availableTurns;
            }
            if (TurnsToInvest > 45)
            {
                TurnsToInvest = 45;
            }

            // Find the research model and property
            object researchModel = null;
            PropertyInfo turnsRequiredProp = null;

            // List all possible research models for the user
            var researchModels = new object[]
            {
        _context.TerranResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id),
        _context.EClassResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id),
        _context.CyrilClassResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id),
        _context.StrafezResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id),
        _context.FClassResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id),
        _context.ProjectsResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id),
        _context.AMinerResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id),
        _context.MarauderResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id),
        _context.ViralResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id),
        _context.ViralSpecificResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id),
        _context.CollectiveResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id),
        _context.CollectiveSpecificResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id),
        _context.GuardianResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id),
        _context.ClusterResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id)
            };

            // Property name pattern: e.g., "SmallTerranHullTurnsRequired"
            string turnsRequiredPropName = SelectedResearchName
            .Replace(" ", "")
            .Replace("-", "")
            .Replace("'", "") + "TurnsRequired";

            foreach (var model in researchModels)
            {
                if (model == null) continue;
                turnsRequiredProp = model.GetType()
                .GetProperties()
                .FirstOrDefault(p =>
                    string.Equals(p.Name.Replace("_", ""), turnsRequiredPropName, StringComparison.OrdinalIgnoreCase));
                if (turnsRequiredProp != null)
                {
                    researchModel = model;
                    break;
                }
            }

            if (researchModel == null || turnsRequiredProp == null)
            {
                ModelState.AddModelError("", "Research not found.");
                GetAvailableResearch(currentUser);
                return Page();
            }

            // Deduct turns from the TurnsRequired property
            int currentTurnsRequired = (int)turnsRequiredProp.GetValue(researchModel);
            int newTurnsRequired = Math.Max(0, currentTurnsRequired - TurnsToInvest);
            turnsRequiredProp.SetValue(researchModel, newTurnsRequired);

            // If TurnsRequired is 0 or less, set the associated bool property to true
            if (newTurnsRequired <= 0)
            {
                string boolPropName = turnsRequiredPropName.Replace("TurnsRequired", "");
                var boolProp = researchModel.GetType()
                    .GetProperties()
                    .FirstOrDefault(p =>
                    string.Equals(p.Name.Replace("_", ""), boolPropName, StringComparison.OrdinalIgnoreCase)
                    && p.PropertyType == typeof(bool));
                if (boolProp != null && boolProp.PropertyType == typeof(bool))
                {
                    boolProp.SetValue(researchModel, true);
                    ResearchMessage = $"Research '{boolPropName}' completed!";
                }
            }

            // Deduct the turns from the user's available turns
            var turnMessage = await _turnService.TryUseTurnsAsync(currentUser.Id, TurnsToInvest);
            StatusMessage = $"{turnMessage.Message}";
            await _context.SaveChangesAsync();

            GetAvailableResearch(currentUser);
            return Page();
        }


        public void GetAvailableResearch(ApplicationUser user)
        {
            var currentUser = _context.Users.FirstOrDefault(x => x.Id == user.Id);
            // Get the user's research data
            var EClassData = _context.EClassResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id);
            var CClassData = _context.CyrilClassResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id);
            var SClassData = _context.StrafezResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id);
            var FClassData = _context.FClassResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id);
            var PClassData = _context.ProjectsResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id);
            ClusterResearch ClusterData = null;
            TerranResearch TerranData = null;
            AMinerResearch AMinerData = null;
            MarauderResearch MarauderData = null;
            ViralResearch ViralData = null;
            ViralSpecificResearch ViralSpecificData = null;
            CollectiveResearch CollectiveData = null;
            CollectiveSpecificResearch CollectiveSpecificData = null;
            GuardianResearch GuardianData = null;
            ListOfAvailableResearch = new List<AvailableResearch>();
            switch (user.Faction)
            {
                case Faction.Terran:
                    TerranData = _context.TerranResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id);
                    ClusterData = _context.ClusterResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id);
                    // Check if the user has completed the prerequisite research
                    // Terran research is dependent on the Terran hulls and ships
                    if (TerranData.SmallTerranHull != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Small Terran Hull",
                            TurnsRequired = TerranData.SmallTerranHullTurnsRequired
                        });
                    }
                    if (TerranData.CrystalLenses != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Crystal Lenses",
                            TurnsRequired = TerranData.CrystalLensesTurnsRequired
                        });
                    }
                    if (TerranData.SmallTerranHull == true)
                    {
                        if (TerranData.MediumTerranHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Medium Terran Hull",
                                TurnsRequired = TerranData.MediumTerranHullTurnsRequired
                            });
                        }
                        if (TerranData.TerranCorvette != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Terran Corvette",
                                TurnsRequired = TerranData.TerranCorvetteTurnsRequired
                            });
                        }
                    }
                    if (TerranData.MediumTerranHull == true)
                    {
                        if (TerranData.LargeTerranHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Large Terran Hull",
                                TurnsRequired = TerranData.LargeTerranHullTurnsRequired
                            });
                        }
                        if (TerranData.TerranFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Terran Frigate",
                                TurnsRequired = TerranData.TerranFrigateTurnsRequired
                            });
                        }
                    }
                    if (TerranData.LargeTerranHull == true)
                    {
                        if (TerranData.TerranDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Terran Destroyer",
                                TurnsRequired = TerranData.TerranDestroyerTurnsRequired
                            });
                        }
                    }
                    if (TerranData.TerranDestroyer == true)
                    {
                        if (TerranData.TerranCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Terran Cruiser",
                                TurnsRequired = TerranData.TerranCruiserTurnsRequired
                            });
                        }
                    }
                    if (TerranData.TerranCruiser == true)
                    {
                        if (TerranData.TerranBattleship != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Terran Battleship",
                                TurnsRequired = TerranData.TerranBattleshipTurnsRequired
                            });
                        }
                    }
                    if (TerranData.TerranBattleship == true && TerranData.CrystalLenses == true)
                    {
                        if (TerranData.AdvancedRadar != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Advanced Radar",
                                TurnsRequired = TerranData.AdvancedRadarTurnsRequired
                            });
                        }
                        if (TerranData.PhotonTorpedo != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Photon Torpedo",
                                TurnsRequired = TerranData.PhotonTorpedoTurnsRequired
                            });
                        }
                    }
                    if (TerranData.AdvancedRadar == true)
                    {
                        if (TerranData.TerranAdvancedScout != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Terran Advanced Scout",
                                TurnsRequired = TerranData.TerranAdvancedScoutTurnsRequired
                            });
                        }
                    }
                    if (TerranData.PhotonTorpedo == true)
                    {
                        if (TerranData.PhotonCorvette != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Photon Corvette",
                                TurnsRequired = TerranData.PhotonCorvetteTurnsRequired
                            });
                        }
                    }
                    if (TerranData.PhotonCorvette == true)
                    {
                        if (TerranData.PhotonFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Photon Frigate",
                                TurnsRequired = TerranData.PhotonFrigateTurnsRequired
                            });
                        }
                    }
                    if (TerranData.PhotonFrigate == true)
                    {
                        if (TerranData.PhotonDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Photon Destroyer",
                                TurnsRequired = TerranData.PhotonDestroyerTurnsRequired
                            });
                        }
                    }
                    if (TerranData.PhotonDestroyer == true)
                    {
                        if (TerranData.PhotonCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Photon Cruiser",
                                TurnsRequired = TerranData.PhotonCruiserTurnsRequired
                            });
                        }
                    }
                    if (TerranData.PhotonCruiser == true)
                    {
                        if (TerranData.TerranDreadnaught != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Terran Dreadnaught",
                                TurnsRequired = TerranData.TerranDreadnaughtTurnsRequired
                            });
                        }
                    }
                    if (TerranData.TerranDreadnaught == true)
                    {
                        if (TerranData.TerranStarbase != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Terran Starbase",
                                TurnsRequired = TerranData.TerranStarbaseTurnsRequired
                            });
                        }
                    }
                    if (TerranData.TerranStarbase == true)
                    {
                        if (TerranData.TerranJuggernaught != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Terran Juggernaught",
                                TurnsRequired = TerranData.TerranJuggernaughtTurnsRequired
                            });
                        }
                    }
                    // SClass Data
                    if (SClassData.BasicGeneticEngineering != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Basic Genetic Engineering",
                            TurnsRequired = SClassData.BasicGeneticEngineeringTurnsRequired
                        });
                    }
                    if (SClassData.BasicGeneticEngineering == true)
                    {
                        if (SClassData.SmallGeneticEngineering != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Small Genetic Engineering",
                                TurnsRequired = SClassData.SmallGeneticEngineeringTurnsRequired
                            });
                        }
                    }
                    if (SClassData.SmallGeneticEngineering == true)
                    {
                        if (SClassData.SmallStrafezRunnerFodder != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Small Strafez Runner Fodder",
                                TurnsRequired = SClassData.SmallStrafezRunnerFodderTurnsRequired
                            });
                        }
                    }
                    if (SClassData.SmallStrafezRunnerFodder == true)
                    {
                        if (SClassData.LargeGeneticEngineering != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Large Genetic Engineering",
                                TurnsRequired = SClassData.LargeGeneticEngineeringTurnsRequired
                            });
                        }
                    }
                    if (SClassData.LargeGeneticEngineering == true)
                    {
                        if (SClassData.LargeStrafezRunnerFodder != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Large Strafez Runner Fodder",
                                TurnsRequired = SClassData.LargeStrafezRunnerFodderTurnsRequired
                            });
                        }
                    }
                    if (SClassData.LargeStrafezRunnerFodder == true)
                    {
                        if (SClassData.StrafezQueenKing != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Strafez Queen King",
                                TurnsRequired = SClassData.StrafezQueenKingTurnsRequired
                            });
                        }
                    }
                    // CClass Data
                    if (CClassData.CyrilAlloy != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Cyril Alloy",
                            TurnsRequired = CClassData.CyrilAlloyTurnsRequired
                        });
                    }
                    if (CClassData.CyrilAlloy == true)
                    {
                        if (CClassData.SmallCyrilHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Small Cyril Hull",
                                TurnsRequired = CClassData.SmallCyrilHullTurnsRequired
                            });
                        }
                    }
                    if (CClassData.SmallCyrilHull == true)
                    {
                        if (CClassData.MediumCyrilHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Medium Cyril Hull",
                                TurnsRequired = CClassData.MediumCyrilHullTurnsRequired
                            });
                        }
                        if (CClassData.CyrilCorvette != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Corvette",
                                TurnsRequired = CClassData.CyrilCorvetteTurnsRequired
                            });
                        }
                    }
                    if (CClassData.MediumCyrilHull == true)
                    {
                        if (CClassData.LargeCyrilHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Large Cyril Hull",
                                TurnsRequired = CClassData.LargeCyrilHullTurnsRequired
                            });
                        }
                        if (CClassData.CyrilFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Frigate",
                                TurnsRequired = CClassData.CyrilFrigateTurnsRequired
                            });
                        }
                    }
                    if (CClassData.LargeCyrilHull == true)
                    {
                        if (CClassData.CyrilDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Destroyer",
                                TurnsRequired = CClassData.CyrilDestroyerTurnsRequired
                            });
                        }
                    }
                    if (CClassData.CyrilDestroyer == true)
                    {
                        if (CClassData.CyrilCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Cruiser",
                                TurnsRequired = CClassData.CyrilCruiserTurnsRequired
                            });
                        }
                    }
                    // FClass Data
                    if (FClassData.FClassEngineering != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "F-Class Engineering",
                            TurnsRequired = FClassData.FClassEngineeringTurnsRequired
                        });
                    }
                    if (FClassData.FClassEngineering == true)
                    {
                        if (FClassData.FClassFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "F-Class Frigate",
                                TurnsRequired = FClassData.FClassFrigateTurnsRequired
                            });
                        }
                    }
                    if (FClassData.FClassFrigate == true)
                    {
                        if (FClassData.FClassDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "F-Class Destroyer",
                                TurnsRequired = FClassData.FClassDestroyerTurnsRequired
                            });
                        }
                    }
                    if (FClassData.FClassDestroyer == true)
                    {
                        if (FClassData.FClassCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "F-Class Cruiser",
                                TurnsRequired = FClassData.FClassCruiserTurnsRequired
                            });
                        }
                    }
                    // Cluster Data
                    if (ClusterData.ColonyClusterLevel1 != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Colony Cluster Level 1",
                            TurnsRequired = ClusterData.ColonyClusterLevel1TurnsRequired
                        });
                    }
                    if (ClusterData.ColonyClusterLevel1 == true)
                    {
                        if (ClusterData.ColonyClusterLevel2 != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Colony Cluster Level 2",
                                TurnsRequired = ClusterData.ColonyClusterLevel2TurnsRequired
                            });
                        }
                    }
                    if (ClusterData.ColonyClusterLevel2 == true)
                    {
                        if (ClusterData.ColonyClusterLevel3 != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Colony Cluster Level 3",
                                TurnsRequired = ClusterData.ColonyClusterLevel3TurnsRequired
                            });
                        }
                    }
                    // EClass Data
                    if (FClassData.FClassCruiser == true)
                    {
                        if (EClassData.ExaHertzClassRailgun != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "ExaHertz Class Railgun",
                                TurnsRequired = EClassData.ExaHertzClassRailgunTurnsRequired
                            });
                        }
                    }
                    if (EClassData.ExaHertzClassRailgun == true)
                    {
                        if (EClassData.EClassFighter != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "E-Class Fighter",
                                TurnsRequired = EClassData.EClassFighterTurnsRequired
                            });
                        }
                    }
                    if (EClassData.EClassFighter == true)
                    {
                        if (EClassData.EClassFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "E-Class Frigate",
                                TurnsRequired = EClassData.EClassFrigateTurnsRequired
                            });
                        }
                    }
                    if (EClassData.EClassFrigate == true)
                    {
                        if (EClassData.EClassDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "E-Class Destroyer",
                                TurnsRequired = EClassData.EClassDestroyerTurnsRequired
                            });
                        }
                    }
                    // Projects Data
                    if (PClassData.ITechLaboratory != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "I-Tech Laboratory",
                            TurnsRequired = PClassData.ITechLaboratoryTurnsRequired
                        });
                    }
                    if (PClassData.CapsuleLaboratory != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Capsule Laboratory",
                            TurnsRequired = PClassData.CapsuleLaboratoryTurnsRequired
                        });
                    }
                    if (PClassData.KalZulOvermind != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Kal'Zul Overmind",
                            TurnsRequired = PClassData.KalZulOvermindTurnsRequired
                        });
                    }
                    if (PClassData.KalZulOvermind == true)
                    {
                        if (PClassData.KalZulArtifact != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Kal'Zul Artifact",
                                TurnsRequired = PClassData.KalZulArtifactTurnsRequired
                            });
                        }
                    }
                    if (PClassData.AdvancedExploration != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Advanced Exploration",
                            TurnsRequired = PClassData.AdvancedExplorationTurnsRequired
                        });
                    }

                    break;
                case Faction.AMiner:
                    AMinerData = _context.AMinerResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id);
                    ClusterData = _context.ClusterResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id);
                    // AMiner Data
                    if (AMinerData.StarshipEngineering != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Starship Engineering",
                            TurnsRequired = AMinerData.StarshipEngineeringTurnsRequired
                        });
                    }
                    if (AMinerData.StarbaseEngineering != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Starbase Engineering",
                            TurnsRequired = AMinerData.StarbaseEngineeringTurnsRequired
                        });
                    }
                    if (AMinerData.StarshipEngineering == true)
                    {
                        if (AMinerData.AsphaCorvette != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Aspha Corvette",
                                TurnsRequired = AMinerData.AsphaCorvetteTurnsRequired
                            });
                        }
                        if (AMinerData.SensorEngineering != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Sensor Engineering",
                                TurnsRequired = AMinerData.SensorEngineeringTurnsRequired
                            });
                        }
                    }
                    if (AMinerData.AsphaCorvette == true)
                    {
                        if (AMinerData.AsphaFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Aspha Frigate",
                                TurnsRequired = AMinerData.AsphaFrigateTurnsRequired
                            });
                        }
                    }
                    if (AMinerData.AsphaFrigate == true)
                    {
                        if (AMinerData.FlagshipEngineering != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Flagship Engineering",
                                TurnsRequired = AMinerData.FlagshipEngineeringTurnsRequired
                            });
                        }
                    }
                    if (AMinerData.FlagshipEngineering == true)
                    {
                        if (AMinerData.AsphaDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Aspha Destroyer",
                                TurnsRequired = AMinerData.AsphaDestroyerTurnsRequired
                            });
                        }
                    }
                    if (AMinerData.AsphaDestroyer == true)
                    {
                        if (AMinerData.AsphaCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Aspha Cruiser",
                                TurnsRequired = AMinerData.AsphaCruiserTurnsRequired
                            });
                        }
                    }
                    if (AMinerData.AsphaCruiser == true)
                    {
                        if (AMinerData.AdvancedEngineering != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Advanced Engineering",
                                TurnsRequired = AMinerData.AdvancedEngineeringTurnsRequired
                            });
                        }
                    }
                    if (AMinerData.AdvancedEngineering == true)
                    {
                        if (AMinerData.AsphaBattleship != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Aspha Battleship",
                                TurnsRequired = AMinerData.AsphaBattleshipTurnsRequired
                            });
                        }
                    }
                    if (AMinerData.AsphaBattleship == true)
                    {
                        if (AMinerData.AsphaDreadnought != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Aspha Dreadnought",
                                TurnsRequired = AMinerData.AsphaDreadnoughtTurnsRequired
                            });
                        }
                    }
                    if (AMinerData.SensorEngineering == true)
                    {
                        if (AMinerData.AsphaSeeker != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Aspha Seeker",
                                TurnsRequired = AMinerData.AsphaSeekerTurnsRequired
                            });
                        }
                    }
                    if (AMinerData.AsphaSeeker == true && AMinerData.AdvancedEngineering == true)
                    {
                        if (AMinerData.AsphaRanger != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Aspha Ranger",
                                TurnsRequired = AMinerData.AsphaRangerTurnsRequired
                            });
                        }
                    }
                    if (AMinerData.StarbaseEngineering == true)
                    {
                        if (AMinerData.AsphaLightStarbase != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Aspha Light Starbase",
                                TurnsRequired = AMinerData.AsphaLightStarbaseTurnsRequired
                            });
                        }
                        if (AMinerData.AsphaHeavyStarbase != true && AMinerData.AdvancedEngineering == true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Aspha Heavy Starbase",
                                TurnsRequired = AMinerData.AsphaHeavyStarbaseTurnsRequired
                            });
                        }
                    }
                    if (AMinerData.AsphaHeavyStarbase == true)
                    {
                        if (AMinerData.GClassShip != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "G-Class Ship",
                                TurnsRequired = AMinerData.GClassShipTurnsRequired
                            });
                        }
                    }
                    if (AMinerData.GClassShip == true)
                    {
                        if (AMinerData.GClassJuggernaught != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "G-Class Juggernaught",
                                TurnsRequired = AMinerData.GClassJuggernaughtTurnsRequired
                            });
                        }
                    }
                    // SClass Data
                    if (SClassData.BasicGeneticEngineering != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Basic Genetic Engineering",
                            TurnsRequired = SClassData.BasicGeneticEngineeringTurnsRequired
                        });
                    }
                    if (SClassData.BasicGeneticEngineering == true)
                    {
                        if (SClassData.SmallGeneticEngineering != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Small Genetic Engineering",
                                TurnsRequired = SClassData.SmallGeneticEngineeringTurnsRequired
                            });
                        }
                    }
                    if (SClassData.SmallGeneticEngineering == true)
                    {
                        if (SClassData.SmallStrafezRunnerFodder != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Small Strafez Runner Fodder",
                                TurnsRequired = SClassData.SmallStrafezRunnerFodderTurnsRequired
                            });
                        }
                    }
                    if (SClassData.SmallStrafezRunnerFodder == true)
                    {
                        if (SClassData.LargeGeneticEngineering != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Large Genetic Engineering",
                                TurnsRequired = SClassData.LargeGeneticEngineeringTurnsRequired
                            });
                        }
                    }
                    if (SClassData.LargeGeneticEngineering == true)
                    {
                        if (SClassData.LargeStrafezRunnerFodder != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Large Strafez Runner Fodder",
                                TurnsRequired = SClassData.LargeStrafezRunnerFodderTurnsRequired
                            });
                        }
                    }
                    if (SClassData.LargeStrafezRunnerFodder == true)
                    {
                        if (SClassData.StrafezQueenKing != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Strafez Queen King",
                                TurnsRequired = SClassData.StrafezQueenKingTurnsRequired
                            });
                        }
                    }
                    // CClass Data
                    if (CClassData.CyrilAlloy != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Cyril Alloy",
                            TurnsRequired = CClassData.CyrilAlloyTurnsRequired
                        });
                    }
                    if (CClassData.CyrilAlloy == true)
                    {
                        if (CClassData.SmallCyrilHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Small Cyril Hull",
                                TurnsRequired = CClassData.SmallCyrilHullTurnsRequired
                            });
                        }
                    }
                    if (CClassData.SmallCyrilHull == true)
                    {
                        if (CClassData.MediumCyrilHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Medium Cyril Hull",
                                TurnsRequired = CClassData.MediumCyrilHullTurnsRequired
                            });
                        }
                        if (CClassData.CyrilCorvette != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Corvette",
                                TurnsRequired = CClassData.CyrilCorvetteTurnsRequired
                            });
                        }
                    }
                    if (CClassData.MediumCyrilHull == true)
                    {
                        if (CClassData.LargeCyrilHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Large Cyril Hull",
                                TurnsRequired = CClassData.LargeCyrilHullTurnsRequired
                            });
                        }
                        if (CClassData.CyrilFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Frigate",
                                TurnsRequired = CClassData.CyrilFrigateTurnsRequired
                            });
                        }
                    }
                    if (CClassData.LargeCyrilHull == true)
                    {
                        if (CClassData.CyrilDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Destroyer",
                                TurnsRequired = CClassData.CyrilDestroyerTurnsRequired
                            });
                        }
                    }
                    if (CClassData.CyrilDestroyer == true)
                    {
                        if (CClassData.CyrilCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Cruiser",
                                TurnsRequired = CClassData.CyrilCruiserTurnsRequired
                            });
                        }
                    }
                    // FClass Data
                    if (FClassData.FClassEngineering != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "F-Class Engineering",
                            TurnsRequired = FClassData.FClassEngineeringTurnsRequired
                        });
                    }
                    if (FClassData.FClassEngineering == true)
                    {
                        if (FClassData.FClassFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "F-Class Frigate",
                                TurnsRequired = FClassData.FClassFrigateTurnsRequired
                            });
                        }
                    }
                    if (FClassData.FClassFrigate == true)
                    {
                        if (FClassData.FClassDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "F-Class Destroyer",
                                TurnsRequired = FClassData.FClassDestroyerTurnsRequired
                            });
                        }
                    }
                    if (FClassData.FClassDestroyer == true)
                    {
                        if (FClassData.FClassCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "F-Class Cruiser",
                                TurnsRequired = FClassData.FClassCruiserTurnsRequired
                            });
                        }
                    }
                    // Cluster Data
                    if (ClusterData.ColonyClusterLevel1 != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Colony Cluster Level 1",
                            TurnsRequired = ClusterData.ColonyClusterLevel1TurnsRequired
                        });
                    }
                    if (ClusterData.ColonyClusterLevel1 == true)
                    {
                        if (ClusterData.ColonyClusterLevel2 != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Colony Cluster Level 2",
                                TurnsRequired = ClusterData.ColonyClusterLevel2TurnsRequired
                            });
                        }
                    }
                    if (ClusterData.ColonyClusterLevel2 == true)
                    {
                        if (ClusterData.ColonyClusterLevel3 != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Colony Cluster Level 3",
                                TurnsRequired = ClusterData.ColonyClusterLevel3TurnsRequired
                            });
                        }
                    }
                    // EClass Data
                    if (FClassData.FClassCruiser == true)
                    {
                        if (EClassData.ExaHertzClassRailgun != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "ExaHertz Class Railgun",
                                TurnsRequired = EClassData.ExaHertzClassRailgunTurnsRequired
                            });
                        }
                    }
                    if (EClassData.ExaHertzClassRailgun == true)
                    {
                        if (EClassData.EClassFighter != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "E-Class Fighter",
                                TurnsRequired = EClassData.EClassFighterTurnsRequired
                            });
                        }
                    }
                    if (EClassData.EClassFighter == true)
                    {
                        if (EClassData.EClassFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "E-Class Frigate",
                                TurnsRequired = EClassData.EClassFrigateTurnsRequired
                            });
                        }
                    }
                    if (EClassData.EClassFrigate == true)
                    {
                        if (EClassData.EClassDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "E-Class Destroyer",
                                TurnsRequired = EClassData.EClassDestroyerTurnsRequired
                            });
                        }
                    }
                    // Projects Data
                    if (PClassData.ITechLaboratory != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "I-Tech Laboratory",
                            TurnsRequired = PClassData.ITechLaboratoryTurnsRequired
                        });
                    }
                    if (PClassData.CapsuleLaboratory != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Capsule Laboratory",
                            TurnsRequired = PClassData.CapsuleLaboratoryTurnsRequired
                        });
                    }
                    if (PClassData.KalZulOvermind != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Kal'Zul Overmind",
                            TurnsRequired = PClassData.KalZulOvermindTurnsRequired
                        });
                    }
                    if (PClassData.KalZulOvermind == true)
                    {
                        if (PClassData.KalZulArtifact != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Kal'Zul Artifact",
                                TurnsRequired = PClassData.KalZulArtifactTurnsRequired
                            });
                        }
                    }
                    if (PClassData.AdvancedExploration != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Advanced Exploration",
                            TurnsRequired = PClassData.AdvancedExplorationTurnsRequired
                        });
                    }
                    break;
                case Faction.Marauder:
                    MarauderData = _context.MarauderResearches.FirstOrDefault(r => r.ApplicationUserId == user.Id);
                    ClusterData = _context.ClusterResearches.FirstOrDefault(r => r.ApplicationUserId == user.Id);
                    // Marauder Data
                    if (MarauderData.Class3MarauderHull != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Class 3 Marauder Hull",
                            TurnsRequired = MarauderData.Class3MarauderHullTurnsRequired
                        });
                    }
                    if (MarauderData.Class3MarauderHull == true)
                    {
                        if (MarauderData.Class4MarauderHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Class 4 Marauder Hull",
                                TurnsRequired = MarauderData.Class4MarauderHullTurnsRequired
                            });
                        }
                        if (MarauderData.Class5MarauderHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Class 5 Marauder Hull",
                                TurnsRequired = MarauderData.Class5MarauderHullTurnsRequired
                            });
                        }
                        if (MarauderData.MarauderCorvette != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Marauder Corvette",
                                TurnsRequired = MarauderData.MarauderCorvetteTurnsRequired
                            });
                        }
                        if (MarauderData.MarauderFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Marauder Frigate",
                                TurnsRequired = MarauderData.MarauderFrigateTurnsRequired
                            });
                        }
                    }
                    if (MarauderData.Class4MarauderHull == true)
                    {
                        if (MarauderData.MarauderDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Marauder Destroyer",
                                TurnsRequired = MarauderData.MarauderDestroyerTurnsRequired
                            });
                        }
                        if (MarauderData.MarauderCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Marauder Cruiser",
                                TurnsRequired = MarauderData.MarauderCruiserTurnsRequired
                            });
                        }
                    }
                    if (MarauderData.Class5MarauderHull == true)
                    {
                        if (MarauderData.MarauderBattleship != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Marauder Battleship",
                                TurnsRequired = MarauderData.MarauderBattleshipTurnsRequired
                            });
                        }
                    }
                    if (MarauderData.MarauderBattleship == true)
                    {
                        if (MarauderData.AdvancedEngineering != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Advanced Engineering",
                                TurnsRequired = MarauderData.AdvancedEngineeringTurnsRequired
                            });
                        }
                    }
                    if (MarauderData.AdvancedEngineering == true)
                    {
                        if (MarauderData.TypeDFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Type D Frigate",
                                TurnsRequired = MarauderData.TypeDFrigateTurnsRequired
                            });
                        }
                    }
                    if (MarauderData.TypeDFrigate == true)
                    {
                        if (MarauderData.TypeDDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Type D Destroyer",
                                TurnsRequired = MarauderData.TypeDDestroyerTurnsRequired
                            });
                        }
                    }
                    if (MarauderData.TypeDDestroyer == true)
                    {
                        if (MarauderData.TypeDCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Type D Cruiser",
                                TurnsRequired = MarauderData.TypeDCruiserTurnsRequired
                            });
                        }
                    }
                    if (MarauderData.TypeDCruiser == true)
                    {
                        if (MarauderData.Class6MarauderHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Class 6 Marauder Hull",
                                TurnsRequired = MarauderData.Class6MarauderHullTurnsRequired
                            });
                        }
                    }
                    if (MarauderData.Class6MarauderHull == true)
                    {
                        if (MarauderData.TypeDBattleship != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Type D Battleship",
                                TurnsRequired = MarauderData.TypeDBattleshipTurnsRequired
                            });
                        }
                    }
                    // SClass Data
                    if (SClassData.BasicGeneticEngineering != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Basic Genetic Engineering",
                            TurnsRequired = SClassData.BasicGeneticEngineeringTurnsRequired
                        });
                    }
                    if (SClassData.BasicGeneticEngineering == true)
                    {
                        if (SClassData.SmallGeneticEngineering != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Small Genetic Engineering",
                                TurnsRequired = SClassData.SmallGeneticEngineeringTurnsRequired
                            });
                        }
                    }
                    if (SClassData.SmallGeneticEngineering == true)
                    {
                        if (SClassData.SmallStrafezRunnerFodder != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Small Strafez Runner Fodder",
                                TurnsRequired = SClassData.SmallStrafezRunnerFodderTurnsRequired
                            });
                        }
                    }
                    if (SClassData.SmallStrafezRunnerFodder == true)
                    {
                        if (SClassData.LargeGeneticEngineering != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Large Genetic Engineering",
                                TurnsRequired = SClassData.LargeGeneticEngineeringTurnsRequired
                            });
                        }
                    }
                    if (SClassData.LargeGeneticEngineering == true)
                    {
                        if (SClassData.LargeStrafezRunnerFodder != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Large Strafez Runner Fodder",
                                TurnsRequired = SClassData.LargeStrafezRunnerFodderTurnsRequired
                            });
                        }
                    }
                    if (SClassData.LargeStrafezRunnerFodder == true)
                    {
                        if (SClassData.StrafezQueenKing != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Strafez Queen King",
                                TurnsRequired = SClassData.StrafezQueenKingTurnsRequired
                            });
                        }
                    }
                    // CClass Data
                    if (CClassData.CyrilAlloy != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Cyril Alloy",
                            TurnsRequired = CClassData.CyrilAlloyTurnsRequired
                        });
                    }
                    if (CClassData.CyrilAlloy == true)
                    {
                        if (CClassData.SmallCyrilHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Small Cyril Hull",
                                TurnsRequired = CClassData.SmallCyrilHullTurnsRequired
                            });
                        }
                    }
                    if (CClassData.SmallCyrilHull == true)
                    {
                        if (CClassData.MediumCyrilHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Medium Cyril Hull",
                                TurnsRequired = CClassData.MediumCyrilHullTurnsRequired
                            });
                        }
                        if (CClassData.CyrilCorvette != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Corvette",
                                TurnsRequired = CClassData.CyrilCorvetteTurnsRequired
                            });
                        }
                    }
                    if (CClassData.MediumCyrilHull == true)
                    {
                        if (CClassData.LargeCyrilHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Large Cyril Hull",
                                TurnsRequired = CClassData.LargeCyrilHullTurnsRequired
                            });
                        }
                        if (CClassData.CyrilFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Frigate",
                                TurnsRequired = CClassData.CyrilFrigateTurnsRequired
                            });
                        }
                    }
                    if (CClassData.LargeCyrilHull == true)
                    {
                        if (CClassData.CyrilDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Destroyer",
                                TurnsRequired = CClassData.CyrilDestroyerTurnsRequired
                            });
                        }
                    }
                    if (CClassData.CyrilDestroyer == true)
                    {
                        if (CClassData.CyrilCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Cruiser",
                                TurnsRequired = CClassData.CyrilCruiserTurnsRequired
                            });
                        }
                    }
                    // FClass Data
                    if (FClassData.FClassEngineering != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "F-Class Engineering",
                            TurnsRequired = FClassData.FClassEngineeringTurnsRequired
                        });
                    }
                    if (FClassData.FClassEngineering == true)
                    {
                        if (FClassData.FClassFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "F-Class Frigate",
                                TurnsRequired = FClassData.FClassFrigateTurnsRequired
                            });
                        }
                    }
                    if (FClassData.FClassFrigate == true)
                    {
                        if (FClassData.FClassDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "F-Class Destroyer",
                                TurnsRequired = FClassData.FClassDestroyerTurnsRequired
                            });
                        }
                    }
                    if (FClassData.FClassDestroyer == true)
                    {
                        if (FClassData.FClassCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "F-Class Cruiser",
                                TurnsRequired = FClassData.FClassCruiserTurnsRequired
                            });
                        }
                    }
                    // Cluster Data
                    if (ClusterData.ColonyClusterLevel1 != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Colony Cluster Level 1",
                            TurnsRequired = ClusterData.ColonyClusterLevel1TurnsRequired
                        });
                    }
                    if (ClusterData.ColonyClusterLevel1 == true)
                    {
                        if (ClusterData.ColonyClusterLevel2 != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Colony Cluster Level 2",
                                TurnsRequired = ClusterData.ColonyClusterLevel2TurnsRequired
                            });
                        }
                    }
                    if (ClusterData.ColonyClusterLevel2 == true)
                    {
                        if (ClusterData.ColonyClusterLevel3 != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Colony Cluster Level 3",
                                TurnsRequired = ClusterData.ColonyClusterLevel3TurnsRequired
                            });
                        }
                    }
                    // EClass Data
                    if (FClassData.FClassCruiser == true)
                    {
                        if (EClassData.ExaHertzClassRailgun != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "ExaHertz Class Railgun",
                                TurnsRequired = EClassData.ExaHertzClassRailgunTurnsRequired
                            });
                        }
                    }
                    if (EClassData.ExaHertzClassRailgun == true)
                    {
                        if (EClassData.EClassFighter != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "E-Class Fighter",
                                TurnsRequired = EClassData.EClassFighterTurnsRequired
                            });
                        }
                    }
                    if (EClassData.EClassFighter == true)
                    {
                        if (EClassData.EClassFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "E-Class Frigate",
                                TurnsRequired = EClassData.EClassFrigateTurnsRequired
                            });
                        }
                    }
                    if (EClassData.EClassFrigate == true)
                    {
                        if (EClassData.EClassDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "E-Class Destroyer",
                                TurnsRequired = EClassData.EClassDestroyerTurnsRequired
                            });
                        }
                    }
                    // Projects Data
                    if (PClassData.ITechLaboratory != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "I-Tech Laboratory",
                            TurnsRequired = PClassData.ITechLaboratoryTurnsRequired
                        });
                    }
                    if (PClassData.CapsuleLaboratory != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Capsule Laboratory",
                            TurnsRequired = PClassData.CapsuleLaboratoryTurnsRequired
                        });
                    }
                    if (PClassData.KalZulOvermind != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Kal'Zul Overmind",
                            TurnsRequired = PClassData.KalZulOvermindTurnsRequired
                        });
                    }
                    if (PClassData.KalZulOvermind == true)
                    {
                        if (PClassData.KalZulArtifact != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Kal'Zul Artifact",
                                TurnsRequired = PClassData.KalZulArtifactTurnsRequired
                            });
                        }
                    }
                    if (PClassData.AdvancedExploration != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Advanced Exploration",
                            TurnsRequired = PClassData.AdvancedExplorationTurnsRequired
                        });
                    }
                    break;
                case Faction.Viral:
                    ViralData = _context.ViralResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id);
                    ViralSpecificData = _context.ViralSpecificResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id);
                    // viral Data
                    if (ViralData.Class1ViralHull != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Class 1 Viral Hull",
                            TurnsRequired = ViralData.Class1ViralHullTurnsRequired
                        });
                    }
                    if (ViralData.Class1ViralHull == true)
                    {
                        if (ViralData.Class2ViralHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Class 2 Viral Hull",
                                TurnsRequired = ViralData.Class2ViralHullTurnsRequired
                            });
                        }
                        if (ViralData.VClassDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "V-Class Destroyer",
                                TurnsRequired = ViralData.VClassDestroyerTurnsRequired
                            });
                        }
                    }
                    if (ViralData.Class2ViralHull == true)
                    {
                        if (ViralData.BClassCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "B-Class Cruiser",
                                TurnsRequired = ViralData.BClassCruiserTurnsRequired
                            });
                        }
                        if (ViralData.VClassCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "V-Class Cruiser",
                                TurnsRequired = ViralData.VClassCruiserTurnsRequired
                            });
                        }
                    }
                    if (ViralData.BClassCruiser == true)
                    {
                        if (ViralData.BClassStarbase != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "B-Class Starbase",
                                TurnsRequired = ViralData.BClassStarbaseTurnsRequired
                            });
                        }
                    }
                    if (ViralData.Class2ViralHull == true)
                    {
                        if (ViralData.AdvancedExploration != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Advanced Exploration",
                                TurnsRequired = ViralData.AdvancedExplorationTurnsRequired
                            });
                        }
                    }
                    if (ViralData.AdvancedExploration == true)
                    {
                        if (ViralData.KohoutekScout != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Kohoutek Scout",
                                TurnsRequired = ViralData.KohoutekScoutTurnsRequired
                            });
                        }
                    }
                    // Viral Specific Data
                    if (ViralSpecificData.ViralClustering != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Viral Clustering",
                            TurnsRequired = ViralSpecificData.ViralClusteringTurnsRequired
                        });
                    }
                    if (ViralSpecificData.UnreverseEngineering != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Unreverse Engineering",
                            TurnsRequired = ViralSpecificData.UnreverseEngineeringTurnsRequired
                        });
                    }
                    // SClass Data
                    if (SClassData.BasicGeneticEngineering != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Basic Genetic Engineering",
                            TurnsRequired = SClassData.BasicGeneticEngineeringTurnsRequired
                        });
                    }
                    if (SClassData.BasicGeneticEngineering == true)
                    {
                        if (SClassData.SmallGeneticEngineering != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Small Genetic Engineering",
                                TurnsRequired = SClassData.SmallGeneticEngineeringTurnsRequired
                            });
                        }
                    }
                    if (SClassData.SmallGeneticEngineering == true)
                    {
                        if (SClassData.SmallStrafezRunnerFodder != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Small Strafez Runner Fodder",
                                TurnsRequired = SClassData.SmallStrafezRunnerFodderTurnsRequired
                            });
                        }
                    }
                    if (SClassData.SmallStrafezRunnerFodder == true)
                    {
                        if (SClassData.LargeGeneticEngineering != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Large Genetic Engineering",
                                TurnsRequired = SClassData.LargeGeneticEngineeringTurnsRequired
                            });
                        }
                    }
                    if (SClassData.LargeGeneticEngineering == true)
                    {
                        if (SClassData.LargeStrafezRunnerFodder != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Large Strafez Runner Fodder",
                                TurnsRequired = SClassData.LargeStrafezRunnerFodderTurnsRequired
                            });
                        }
                    }
                    if (SClassData.LargeStrafezRunnerFodder == true)
                    {
                        if (SClassData.StrafezQueenKing != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Strafez Queen King",
                                TurnsRequired = SClassData.StrafezQueenKingTurnsRequired
                            });
                        }
                    }
                    // CClass Data
                    if (CClassData.CyrilAlloy != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Cyril Alloy",
                            TurnsRequired = CClassData.CyrilAlloyTurnsRequired
                        });
                    }
                    if (CClassData.CyrilAlloy == true)
                    {
                        if (CClassData.SmallCyrilHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Small Cyril Hull",
                                TurnsRequired = CClassData.SmallCyrilHullTurnsRequired
                            });
                        }
                    }
                    if (CClassData.SmallCyrilHull == true)
                    {
                        if (CClassData.MediumCyrilHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Medium Cyril Hull",
                                TurnsRequired = CClassData.MediumCyrilHullTurnsRequired
                            });
                        }
                        if (CClassData.CyrilCorvette != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Corvette",
                                TurnsRequired = CClassData.CyrilCorvetteTurnsRequired
                            });
                        }
                    }
                    if (CClassData.MediumCyrilHull == true)
                    {
                        if (CClassData.LargeCyrilHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Large Cyril Hull",
                                TurnsRequired = CClassData.LargeCyrilHullTurnsRequired
                            });
                        }
                        if (CClassData.CyrilFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Frigate",
                                TurnsRequired = CClassData.CyrilFrigateTurnsRequired
                            });
                        }
                    }
                    if (CClassData.LargeCyrilHull == true)
                    {
                        if (CClassData.CyrilDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Destroyer",
                                TurnsRequired = CClassData.CyrilDestroyerTurnsRequired
                            });
                        }
                    }
                    if (CClassData.CyrilDestroyer == true)
                    {
                        if (CClassData.CyrilCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Cruiser",
                                TurnsRequired = CClassData.CyrilCruiserTurnsRequired
                            });
                        }
                    }
                    // FClass Data
                    if (FClassData.FClassEngineering != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "F-Class Engineering",
                            TurnsRequired = FClassData.FClassEngineeringTurnsRequired
                        });
                    }
                    if (FClassData.FClassEngineering == true)
                    {
                        if (FClassData.FClassFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "F-Class Frigate",
                                TurnsRequired = FClassData.FClassFrigateTurnsRequired
                            });
                        }
                    }
                    if (FClassData.FClassFrigate == true)
                    {
                        if (FClassData.FClassDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "F-Class Destroyer",
                                TurnsRequired = FClassData.FClassDestroyerTurnsRequired
                            });
                        }
                    }
                    if (FClassData.FClassDestroyer == true)
                    {
                        if (FClassData.FClassCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "F-Class Cruiser",
                                TurnsRequired = FClassData.FClassCruiserTurnsRequired
                            });
                        }
                    }
                    // EClass Data
                    if (FClassData.FClassCruiser == true)
                    {
                        if (EClassData.ExaHertzClassRailgun != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "ExaHertz Class Railgun",
                                TurnsRequired = EClassData.ExaHertzClassRailgunTurnsRequired
                            });
                        }
                    }
                    if (EClassData.ExaHertzClassRailgun == true)
                    {
                        if (EClassData.EClassFighter != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "E-Class Fighter",
                                TurnsRequired = EClassData.EClassFighterTurnsRequired
                            });
                        }
                    }
                    if (EClassData.EClassFighter == true)
                    {
                        if (EClassData.EClassFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "E-Class Frigate",
                                TurnsRequired = EClassData.EClassFrigateTurnsRequired
                            });
                        }
                    }
                    if (EClassData.EClassFrigate == true)
                    {
                        if (EClassData.EClassDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "E-Class Destroyer",
                                TurnsRequired = EClassData.EClassDestroyerTurnsRequired
                            });
                        }
                    }
                    // Projects Data
                    if (PClassData.ITechLaboratory != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "I-Tech Laboratory",
                            TurnsRequired = PClassData.ITechLaboratoryTurnsRequired
                        });
                    }
                    if (PClassData.CapsuleLaboratory != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Capsule Laboratory",
                            TurnsRequired = PClassData.CapsuleLaboratoryTurnsRequired
                        });
                    }
                    if (PClassData.KalZulOvermind != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Kal'Zul Overmind",
                            TurnsRequired = PClassData.KalZulOvermindTurnsRequired
                        });
                    }
                    if (PClassData.KalZulOvermind == true)
                    {
                        if (PClassData.KalZulArtifact != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Kal'Zul Artifact",
                                TurnsRequired = PClassData.KalZulArtifactTurnsRequired
                            });
                        }
                    }
                    if (PClassData.AdvancedExploration != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Advanced Exploration",
                            TurnsRequired = PClassData.AdvancedExplorationTurnsRequired
                        });
                    }

                    break;
                case Faction.Collective:
                    CollectiveData = _context.CollectiveResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id);
                    CollectiveSpecificData = _context.CollectiveSpecificResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id);
                    // collective Data
                    if (CollectiveData.FighterClass != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Fighter Class",
                            TurnsRequired = CollectiveData.FighterClassTurnsRequired
                        });
                    }
                    if (CollectiveData.FighterClass == true)
                    {
                        if (CollectiveData.FrigateClass != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Frigate Class",
                                TurnsRequired = CollectiveData.FrigateClassTurnsRequired
                            });
                        }
                        if (CollectiveData.RClassFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "R-Class Frigate",
                                TurnsRequired = CollectiveData.RClassFrigateTurnsRequired
                            });
                        }
                    }
                    if (CollectiveData.FrigateClass == true)
                    {
                        if (CollectiveData.DestroyerClass != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Destroyer Class",
                                TurnsRequired = CollectiveData.DestroyerClassTurnsRequired
                            });
                        }
                        if (CollectiveData.RClassDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "R-Class Destroyer",
                                TurnsRequired = CollectiveData.RClassDestroyerTurnsRequired
                            });
                        }
                    }
                    if (CollectiveData.DestroyerClass == true)
                    {
                        if (CollectiveData.CruiserClass != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cruiser Class",
                                TurnsRequired = CollectiveData.CruiserClassTurnsRequired
                            });
                        }
                    }
                    if (CollectiveData.CruiserClass == true)
                    {
                        if (CollectiveData.HClassShips != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "H-Class Ships",
                                TurnsRequired = CollectiveData.HClassShipsTurnsRequired
                            });
                        }
                    }
                    if (CollectiveData.HClassShips == true)
                    {
                        if (CollectiveData.HClassDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "H-Class Destroyer",
                                TurnsRequired = CollectiveData.HClassDestroyerTurnsRequired
                            });
                        }
                    }
                    if (CollectiveData.HClassDestroyer == true)
                    {
                        if (CollectiveData.HClassCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "H-Class Cruiser",
                                TurnsRequired = CollectiveData.HClassCruiserTurnsRequired
                            });
                        }
                    }
                    if (CollectiveData.HClassCruiser == true)
                    {
                        if (CollectiveData.RClassBattleship != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "R-Class Battleship",
                                TurnsRequired = CollectiveData.RClassBattleshipTurnsRequired
                            });
                        }
                        if (CollectiveData.HClassStarbase != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "H-Class Starbase",
                                TurnsRequired = CollectiveData.HClassStarbaseTurnsRequired
                            });
                        }
                    }
                    if (CollectiveData.HClassStarbase == true)
                    {
                        if (CollectiveData.HClassDreadnought != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "H-Class Dreadnought",
                                TurnsRequired = CollectiveData.HClassDreadnoughtTurnsRequired
                            });
                        }
                        if (CollectiveData.RClassDreadnought != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "R-Class Dreadnought",
                                TurnsRequired = CollectiveData.RClassDreadnoughtTurnsRequired
                            });
                        }
                    }
                    if (CollectiveData.HClassDreadnought == true)
                    {
                        if (CollectiveData.HClassLeviathan != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "H-Class Leviathan",
                                TurnsRequired = CollectiveData.HClassLeviathanTurnsRequired
                            });
                        }
                        if (CollectiveData.RClassJuggernaught != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "R-Class Juggernaught",
                                TurnsRequired = CollectiveData.RClassJuggernaughtTurnsRequired
                            });
                        }
                    }
                    // collective Specific Data
                    if (CollectiveSpecificData.CollectiveClustering != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Collective Clustering",
                            TurnsRequired = CollectiveSpecificData.CollectiveClusteringTurnsRequired
                        });
                    }
                    // SClass Data
                    if (SClassData.BasicGeneticEngineering != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Basic Genetic Engineering",
                            TurnsRequired = SClassData.BasicGeneticEngineeringTurnsRequired
                        });
                    }
                    if (SClassData.BasicGeneticEngineering == true)
                    {
                        if (SClassData.SmallGeneticEngineering != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Small Genetic Engineering",
                                TurnsRequired = SClassData.SmallGeneticEngineeringTurnsRequired
                            });
                        }
                    }
                    if (SClassData.SmallGeneticEngineering == true)
                    {
                        if (SClassData.SmallStrafezRunnerFodder != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Small Strafez Runner Fodder",
                                TurnsRequired = SClassData.SmallStrafezRunnerFodderTurnsRequired
                            });
                        }
                    }
                    if (SClassData.SmallStrafezRunnerFodder == true)
                    {
                        if (SClassData.LargeGeneticEngineering != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Large Genetic Engineering",
                                TurnsRequired = SClassData.LargeGeneticEngineeringTurnsRequired
                            });
                        }
                    }
                    if (SClassData.LargeGeneticEngineering == true)
                    {
                        if (SClassData.LargeStrafezRunnerFodder != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Large Strafez Runner Fodder",
                                TurnsRequired = SClassData.LargeStrafezRunnerFodderTurnsRequired
                            });
                        }
                    }
                    if (SClassData.LargeStrafezRunnerFodder == true)
                    {
                        if (SClassData.StrafezQueenKing != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Strafez Queen King",
                                TurnsRequired = SClassData.StrafezQueenKingTurnsRequired
                            });
                        }
                    }
                    // CClass Data
                    if (CClassData.CyrilAlloy != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Cyril Alloy",
                            TurnsRequired = CClassData.CyrilAlloyTurnsRequired
                        });
                    }
                    if (CClassData.CyrilAlloy == true)
                    {
                        if (CClassData.SmallCyrilHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Small Cyril Hull",
                                TurnsRequired = CClassData.SmallCyrilHullTurnsRequired
                            });
                        }
                    }
                    if (CClassData.SmallCyrilHull == true)
                    {
                        if (CClassData.MediumCyrilHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Medium Cyril Hull",
                                TurnsRequired = CClassData.MediumCyrilHullTurnsRequired
                            });
                        }
                        if (CClassData.CyrilCorvette != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Corvette",
                                TurnsRequired = CClassData.CyrilCorvetteTurnsRequired
                            });
                        }
                    }
                    if (CClassData.MediumCyrilHull == true)
                    {
                        if (CClassData.LargeCyrilHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Large Cyril Hull",
                                TurnsRequired = CClassData.LargeCyrilHullTurnsRequired
                            });
                        }
                        if (CClassData.CyrilFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Frigate",
                                TurnsRequired = CClassData.CyrilFrigateTurnsRequired
                            });
                        }
                    }
                    if (CClassData.LargeCyrilHull == true)
                    {
                        if (CClassData.CyrilDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Destroyer",
                                TurnsRequired = CClassData.CyrilDestroyerTurnsRequired
                            });
                        }
                    }
                    if (CClassData.CyrilDestroyer == true)
                    {
                        if (CClassData.CyrilCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Cruiser",
                                TurnsRequired = CClassData.CyrilCruiserTurnsRequired
                            });
                        }
                    }
                    // FClass Data
                    if (FClassData.FClassEngineering != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "F-Class Engineering",
                            TurnsRequired = FClassData.FClassEngineeringTurnsRequired
                        });
                    }
                    if (FClassData.FClassEngineering == true)
                    {
                        if (FClassData.FClassFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "F-Class Frigate",
                                TurnsRequired = FClassData.FClassFrigateTurnsRequired
                            });
                        }
                    }
                    if (FClassData.FClassFrigate == true)
                    {
                        if (FClassData.FClassDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "F-Class Destroyer",
                                TurnsRequired = FClassData.FClassDestroyerTurnsRequired
                            });
                        }
                    }
                    if (FClassData.FClassDestroyer == true)
                    {
                        if (FClassData.FClassCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "F-Class Cruiser",
                                TurnsRequired = FClassData.FClassCruiserTurnsRequired
                            });
                        }
                    }
                    // EClass Data
                    if (FClassData.FClassCruiser == true)
                    {
                        if (EClassData.ExaHertzClassRailgun != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "ExaHertz Class Railgun",
                                TurnsRequired = EClassData.ExaHertzClassRailgunTurnsRequired
                            });
                        }
                    }
                    if (EClassData.ExaHertzClassRailgun == true)
                    {
                        if (EClassData.EClassFighter != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "E-Class Fighter",
                                TurnsRequired = EClassData.EClassFighterTurnsRequired
                            });
                        }
                    }
                    if (EClassData.EClassFighter == true)
                    {
                        if (EClassData.EClassFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "E-Class Frigate",
                                TurnsRequired = EClassData.EClassFrigateTurnsRequired
                            });
                        }
                    }
                    if (EClassData.EClassFrigate == true)
                    {
                        if (EClassData.EClassDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "E-Class Destroyer",
                                TurnsRequired = EClassData.EClassDestroyerTurnsRequired
                            });
                        }
                    }
                    // Projects Data
                    if (PClassData.ITechLaboratory != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "I-Tech Laboratory",
                            TurnsRequired = PClassData.ITechLaboratoryTurnsRequired
                        });
                    }
                    if (PClassData.CapsuleLaboratory != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Capsule Laboratory",
                            TurnsRequired = PClassData.CapsuleLaboratoryTurnsRequired
                        });
                    }
                    if (PClassData.KalZulOvermind != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Kal'Zul Overmind",
                            TurnsRequired = PClassData.KalZulOvermindTurnsRequired
                        });
                    }
                    if (PClassData.KalZulOvermind == true)
                    {
                        if (PClassData.KalZulArtifact != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Kal'Zul Artifact",
                                TurnsRequired = PClassData.KalZulArtifactTurnsRequired
                            });
                        }
                    }
                    if (PClassData.AdvancedExploration != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Advanced Exploration",
                            TurnsRequired = PClassData.AdvancedExplorationTurnsRequired
                        });
                    }

                    break;
                case Faction.Guardian:
                    GuardianData = _context.GuardianResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id);
                    ClusterData = _context.ClusterResearches.FirstOrDefault(r => r.ApplicationUserId == currentUser.Id);
                    // Guardian Data
                    if (GuardianData.FighterClass != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Fighter Class",
                            TurnsRequired = GuardianData.FighterClassTurnsRequired
                        });
                    }
                    if (GuardianData.FocusCrystal != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Focus Crystal",
                            TurnsRequired = GuardianData.FocusCrystalTurnsRequired
                        });
                    }
                    if (GuardianData.OrganicSensors != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Organic Sensors",
                            TurnsRequired = GuardianData.OrganicSensorsTurnsRequired
                        });
                    }
                    if (GuardianData.FighterClass == true)
                    {
                        if (GuardianData.CorvetteClass != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Corvette Class",
                                TurnsRequired = GuardianData.CorvetteClassTurnsRequired
                            });
                        }
                    }
                    if (GuardianData.CorvetteClass == true)
                    {
                        if (GuardianData.FrigateClass != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Frigate Class",
                                TurnsRequired = GuardianData.FrigateClassTurnsRequired
                            });
                        }
                    }
                    if (GuardianData.FrigateClass == true)
                    {
                        if (GuardianData.DestroyerClass != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Destroyer Class",
                                TurnsRequired = GuardianData.DestroyerClassTurnsRequired
                            });
                        }
                        if (GuardianData.WeaponsPlatform != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Weapons Platform",
                                TurnsRequired = GuardianData.WeaponsPlatformTurnsRequired
                            });
                        }
                    }
                    if (GuardianData.DestroyerClass == true)
                    {
                        if (GuardianData.CruiserClass != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cruiser Class",
                                TurnsRequired = GuardianData.CruiserClassTurnsRequired
                            });
                        }
                    }
                    if (GuardianData.CruiserClass == true)
                    {
                        if (GuardianData.AdvancedEngineering != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Advanced Engineering",
                                TurnsRequired = GuardianData.AdvancedEngineeringTurnsRequired
                            });
                        }
                    }
                    if (GuardianData.AdvancedEngineering == true)
                    {
                        if (GuardianData.AdvancedScouts != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Advanced Scouts",
                                TurnsRequired = GuardianData.AdvancedScoutsTurnsRequired
                            });
                        }
                    }
                    if (GuardianData.FocusCrystal == true && GuardianData.OrganicSensors == true)
                    {
                        if (GuardianData.LanceTorpodoes != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Lance Torpodoes",
                                TurnsRequired = GuardianData.LanceTorpodoesTurnsRequired
                            });
                        }
                    }
                    if (GuardianData.LanceTorpodoes == true)
                    {
                        if (GuardianData.LClassFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "L-Class Frigate",
                                TurnsRequired = GuardianData.LClassFrigateTurnsRequired
                            });
                        }
                    }
                    if (GuardianData.LClassFrigate == true)
                    {
                        if (GuardianData.LClassDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "L-Class Destroyer",
                                TurnsRequired = GuardianData.LClassDestroyerTurnsRequired
                            });
                        }
                    }
                    if (GuardianData.LClassDestroyer == true)
                    {
                        if (GuardianData.LClassCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "L-Class Cruiser",
                                TurnsRequired = GuardianData.LClassCruiserTurnsRequired
                            });
                        }
                    }
                    if (GuardianData.LClassCruiser == true)
                    {
                        if (GuardianData.KClassDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "K-Class Destroyer",
                                TurnsRequired = GuardianData.KClassDestroyerTurnsRequired
                            });
                        }
                    }
                    if (GuardianData.KClassDestroyer == true)
                    {
                        if (GuardianData.KClassCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "K-Class Cruiser",
                                TurnsRequired = GuardianData.KClassCruiserTurnsRequired
                            });
                        }
                    }
                    // SClass Data
                    if (SClassData.BasicGeneticEngineering != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Basic Genetic Engineering",
                            TurnsRequired = SClassData.BasicGeneticEngineeringTurnsRequired
                        });
                    }
                    if (SClassData.BasicGeneticEngineering == true)
                    {
                        if (SClassData.SmallGeneticEngineering != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Small Genetic Engineering",
                                TurnsRequired = SClassData.SmallGeneticEngineeringTurnsRequired
                            });
                        }
                    }
                    if (SClassData.SmallGeneticEngineering == true)
                    {
                        if (SClassData.SmallStrafezRunnerFodder != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Small Strafez Runner Fodder",
                                TurnsRequired = SClassData.SmallStrafezRunnerFodderTurnsRequired
                            });
                        }
                    }
                    if (SClassData.SmallStrafezRunnerFodder == true)
                    {
                        if (SClassData.LargeGeneticEngineering != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Large Genetic Engineering",
                                TurnsRequired = SClassData.LargeGeneticEngineeringTurnsRequired
                            });
                        }
                    }
                    if (SClassData.LargeGeneticEngineering == true)
                    {
                        if (SClassData.LargeStrafezRunnerFodder != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Large Strafez Runner Fodder",
                                TurnsRequired = SClassData.LargeStrafezRunnerFodderTurnsRequired
                            });
                        }
                    }
                    if (SClassData.LargeStrafezRunnerFodder == true)
                    {
                        if (SClassData.StrafezQueenKing != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Strafez Queen King",
                                TurnsRequired = SClassData.StrafezQueenKingTurnsRequired
                            });
                        }
                    }
                    // CClass Data
                    if (CClassData.CyrilAlloy != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Cyril Alloy",
                            TurnsRequired = CClassData.CyrilAlloyTurnsRequired
                        });
                    }
                    if (CClassData.CyrilAlloy == true)
                    {
                        if (CClassData.SmallCyrilHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Small Cyril Hull",
                                TurnsRequired = CClassData.SmallCyrilHullTurnsRequired
                            });
                        }
                    }
                    if (CClassData.SmallCyrilHull == true)
                    {
                        if (CClassData.MediumCyrilHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Medium Cyril Hull",
                                TurnsRequired = CClassData.MediumCyrilHullTurnsRequired
                            });
                        }
                        if (CClassData.CyrilCorvette != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Corvette",
                                TurnsRequired = CClassData.CyrilCorvetteTurnsRequired
                            });
                        }
                    }
                    if (CClassData.MediumCyrilHull == true)
                    {
                        if (CClassData.LargeCyrilHull != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Large Cyril Hull",
                                TurnsRequired = CClassData.LargeCyrilHullTurnsRequired
                            });
                        }
                        if (CClassData.CyrilFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Frigate",
                                TurnsRequired = CClassData.CyrilFrigateTurnsRequired
                            });
                        }
                    }
                    if (CClassData.LargeCyrilHull == true)
                    {
                        if (CClassData.CyrilDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Destroyer",
                                TurnsRequired = CClassData.CyrilDestroyerTurnsRequired
                            });
                        }
                    }
                    if (CClassData.CyrilDestroyer == true)
                    {
                        if (CClassData.CyrilCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Cyril Cruiser",
                                TurnsRequired = CClassData.CyrilCruiserTurnsRequired
                            });
                        }
                    }
                    // FClass Data
                    if (FClassData.FClassEngineering != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "F-Class Engineering",
                            TurnsRequired = FClassData.FClassEngineeringTurnsRequired
                        });
                    }
                    if (FClassData.FClassEngineering == true)
                    {
                        if (FClassData.FClassFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "F-Class Frigate",
                                TurnsRequired = FClassData.FClassFrigateTurnsRequired
                            });
                        }
                    }
                    if (FClassData.FClassFrigate == true)
                    {
                        if (FClassData.FClassDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "F-Class Destroyer",
                                TurnsRequired = FClassData.FClassDestroyerTurnsRequired
                            });
                        }
                    }
                    if (FClassData.FClassDestroyer == true)
                    {
                        if (FClassData.FClassCruiser != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "F-Class Cruiser",
                                TurnsRequired = FClassData.FClassCruiserTurnsRequired
                            });
                        }
                    }
                    // Cluster Data
                    if (ClusterData.ColonyClusterLevel1 != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Colony Cluster Level 1",
                            TurnsRequired = ClusterData.ColonyClusterLevel1TurnsRequired
                        });
                    }
                    if (ClusterData.ColonyClusterLevel1 == true)
                    {
                        if (ClusterData.ColonyClusterLevel2 != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Colony Cluster Level 2",
                                TurnsRequired = ClusterData.ColonyClusterLevel2TurnsRequired
                            });
                        }
                    }
                    if (ClusterData.ColonyClusterLevel2 == true)
                    {
                        if (ClusterData.ColonyClusterLevel3 != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Colony Cluster Level 3",
                                TurnsRequired = ClusterData.ColonyClusterLevel3TurnsRequired
                            });
                        }
                    }
                    // EClass Data
                    if (FClassData.FClassCruiser == true)
                    {
                        if (EClassData.ExaHertzClassRailgun != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "ExaHertz Class Railgun",
                                TurnsRequired = EClassData.ExaHertzClassRailgunTurnsRequired
                            });
                        }
                    }
                    if (EClassData.ExaHertzClassRailgun == true)
                    {
                        if (EClassData.EClassFighter != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "E-Class Fighter",
                                TurnsRequired = EClassData.EClassFighterTurnsRequired
                            });
                        }
                    }
                    if (EClassData.EClassFighter == true)
                    {
                        if (EClassData.EClassFrigate != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "E-Class Frigate",
                                TurnsRequired = EClassData.EClassFrigateTurnsRequired
                            });
                        }
                    }
                    if (EClassData.EClassFrigate == true)
                    {
                        if (EClassData.EClassDestroyer != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "E-Class Destroyer",
                                TurnsRequired = EClassData.EClassDestroyerTurnsRequired
                            });
                        }
                    }
                    // Projects Data
                    if (PClassData.ITechLaboratory != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "I-Tech Laboratory",
                            TurnsRequired = PClassData.ITechLaboratoryTurnsRequired
                        });
                    }
                    if (PClassData.CapsuleLaboratory != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Capsule Laboratory",
                            TurnsRequired = PClassData.CapsuleLaboratoryTurnsRequired
                        });
                    }
                    if (PClassData.KalZulOvermind != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Kal'Zul Overmind",
                            TurnsRequired = PClassData.KalZulOvermindTurnsRequired
                        });
                    }
                    if (PClassData.KalZulOvermind == true)
                    {
                        if (PClassData.KalZulArtifact != true)
                        {
                            ListOfAvailableResearch.Add(new AvailableResearch
                            {
                                Name = "Kal'Zul Artifact",
                                TurnsRequired = PClassData.KalZulArtifactTurnsRequired
                            });
                        }
                    }
                    if (PClassData.AdvancedExploration != true)
                    {
                        ListOfAvailableResearch.Add(new AvailableResearch
                        {
                            Name = "Advanced Exploration",
                            TurnsRequired = PClassData.AdvancedExplorationTurnsRequired
                        });
                    }
                    break;
                case Faction.KalZul:
                    break;
                case Faction.DarkMarauder:
                    break;
                default:
                    break;
            }

        }
        public class AvailableResearch
        {
            public string Name { get; set; }
            public int TurnsRequired { get; set; }
        }

    }
}
