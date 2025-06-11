// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

namespace AnotherSpaceGame.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        public SelectList FactionOptions { get; set; }

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Faction")]
            public Faction Faction { get; set; }
        }
        

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            FactionOptions = new SelectList(Enum.GetValues(typeof(Faction)).Cast<Faction>());
            FactionOptions = new SelectList(
            Enum.GetValues(typeof(Faction))
            .Cast<Faction>()
            .Where(f => f != Faction.KalZul && f != Faction.DarkMarauder)
            .Select(f => new { Value = f, Text = f.ToDescription() }),
            "Value",
            "Text"
                );
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            FactionOptions = new SelectList(Enum.GetValues(typeof(Faction)).Cast<Faction>());
            FactionOptions = new SelectList(
            Enum.GetValues(typeof(Faction))
            .Cast<Faction>()
            .Where(f => f != Faction.KalZul && f != Faction.DarkMarauder)
            .Select(f => new { Value = f, Text = f.ToDescription() }),
            "Value",
            "Text"
                );

            // --- Username and Email Uniqueness Check ---
            var existingUserByUsername = await _userManager.FindByNameAsync(Input.Username);
            if (existingUserByUsername != null)
            {
                ModelState.AddModelError("Input.Username", "This username is already taken.");
            }

            var existingUserByEmail = await _userManager.FindByEmailAsync(Input.Email);
            if (existingUserByEmail != null)
            {
                ModelState.AddModelError("Input.Email", "This email is already registered.");
            }

            if (!ModelState.IsValid)
            {
                // If we got this far, something failed, redisplay form
                return Page();
            }

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Username, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    // Set the users Fraction to Terran
                    user.Faction = Input.Faction; // Initialize Fraction to Terran

                    // --- Commodities creation and linking ---
                    var commodities = new Commodities
                    {
                        ApplicationUserId = user.Id
                        // Default values are set in the constructor
                    };
                    user.Commodities = commodities;
                    _context.Commodities.Add(commodities);
                    // --- End Commodities logic ---
                    // --- Planets ---
                    
                        user.Planets = new List<Planets>();
                        var planets = new List<Planets>
                        {
                        new Planets()
                        {
                            ApplicationUserId = user.Id,
                            Name = "H." + Random.Shared.RandomString(4), // Random name for the planet

                            // Default values are set in the constructor
                        }
                        };
                    
                    if (user.Faction == Faction.Viral)
                    {
                        planets[0].Type = PlanetType.TaintedC1;
                        planets[0].Name = "C." + Random.Shared.RandomString(4); // Set the planet type for Viral faction
                    }
                    if (user.Faction == Faction.Collective)
                    {
                        planets[0].Type = PlanetType.SimilareC1;
                        planets[0].Name = "B." + Random.Shared.RandomString(4);
                        // Set the planet type for Collective faction
                    }

                    user.Planets = planets;
                    _context.Planets.AddRange(planets);
                    // --- End Planets logic ---
                    // --- Turns ---    
                    var turns = new Turns
                    {
                        ApplicationUserId = user.Id
                        // Default values are set in the constructor
                    };
                    user.Turns = turns;
                    _context.Turns.Add(turns);
                    // --- End Turns logic ---
                    // --- Power Rating ---
                    user.PowerRating = 1000; // Initialize Power Rating
                    // --- End Power Rating logic ---
                    // --- Playing Since ---
                    user.PlayingSince = DateTime.UtcNow; // Set Playing Since to current time
                    // --- End Playing Since logic ---
                    // --- Empire Age ---
                    // Fixing the EmpireAge assignment to correctly calculate the age in days
                    user.EmpireAge = (DateTime.UtcNow - user.PlayingSince).TotalDays; // Set Empire Age to current time
                    // --- End Empire Age logic ---
                    // --- Battles Won ---
                    user.BattlesWon = 0; // Initialize Battles Won
                    // --- End Battles Won logic ---
                    // --- Battles Lost ---
                    user.BattlesLost = 0; // Initialize Battles Lost
                    // --- End Battles Lost logic ---
                    // --- Colonies Won ---
                    user.ColoniesWon = 0; // Initialize Colonies Won
                    // --- End Colonies Won logic ---
                    // --- Colonies Lost ---
                    user.ColoniesLost = 0; // Initialize Colonies Lost
                    // --- End Colonies Lost logic ---
                    // --- Colonies Explored ---
                    user.ColoniesExplored = 0; // Initialize Colonies Explored
                    // --- End Colonies Explored logic ---
                    // --- Planets Plundered ---
                    user.PlanetsPlundered = 0; // Initialize Planets Plundered
                    // --- End Planets Plundered logic ---
                    // --- Fraction ---
                    user.Faction = Faction.Terran; // Initialize Fraction
                    // --- End Fraction logic ---
                    // --- Total Colonies ---
                    user.TotalColonies = 1; // Initialize Total Colonies
                    // --- End Total Colonies logic ---
                    // --- Total Planets ---
                    user.TotalPlanets = 1; // Initialize Total Planets
                    // --- End Total Planets logic ---
                    // --- Damage Protection ---
                    user.DamageProtection = DateTime.Now.AddDays(1); // Initialize Damage Protection
                    // --- End Damage Protection logic ---
                    // --- Last Activity ---
                    user.LastAction = DateTime.UtcNow; // Set Last Activity to current time
                    // --- End Last Activity logic ---
                    // --- Artifact Shield ---
                    user.ArtifactShield = 0m; // Initialize Artifact Shield
                    // --- End Artifact Shield logic ---
                    // --- isNPC ---
                    user.IsNPC = false; // Set IsNPC to false for new users 
                    // --- End isNPC logic ---
                    // --- Important Events ---
                    var importantEvents = new List<ImportantEvents>
                    {
                        new ImportantEvents
                        {
                            ApplicationUserId = user.Id,
                            Text = "Welcome to Another Space Game! Your journey begins now.",
                            DateAndTime = DateTime.UtcNow
                        }
                    };  
                    user.ImportantEvents = importantEvents;
                    _context.ImportantEvents.AddRange(importantEvents);
                    // --- End Important Events logic ---
                    // --- Battle Logs ---
                    var battleLogs = new List<BattleLogs>
                    {
                        new BattleLogs
                        {
                            ApplicationUserId = user.Id,
                            DateAndTime = DateTime.UtcNow,
                            Attacker = "System",
                            Defender = "System",
                            FleetReport = "Sytem",
                            Outcome = "Void"
                        }
                    };
                    user.Battlelogs = battleLogs;
                    _context.Battlelogs.AddRange(battleLogs);
                    // --- End Battle Logs logic ---
                    // --- Exploration ---
                    var exploration = new Exploration
                    {
                        ApplicationUserId = user.Id
                        // Default values are set in the constructor
                    };
                    user.Exploration = exploration;
                    _context.Explorations.Add(exploration);
                    // --- End Exploration logic ---
                    // --- Infrastructer ---
                    var infrastructer = new Infrastructer
                    {
                        ApplicationUserId = user.Id
                        // Default values are set in the constructor
                    };
                    user.Infrastructer = infrastructer;
                    _context.Infrastructers.Add(infrastructer);
                    // --- End Infrastructer logic ---
                    // --- EClassResearch ---
                    var eClassResearch = new EClassResearch
                    {
                        ApplicationUserId = user.Id
                        // Default values are set in the constructor
                    };
                    user.EClassResearch = eClassResearch;
                    _context.EClassResearches.Add(eClassResearch);
                    // --- End EClassResearch logic ---
                    // --- CyrilClassResearch ---
                    var cyrilClassResearch = new CyrilClassResearch
                    {
                        ApplicationUserId = user.Id
                        // Default values are set in the constructor
                    };
                    user.CyrilClassResearch = cyrilClassResearch;
                    _context.CyrilClassResearches.Add(cyrilClassResearch);
                    // --- End CyrilClassResearch logic ---
                    // --- StrafezResearch ---
                    var strafezResearch = new StrafezResearch
                    {
                        ApplicationUserId = user.Id
                        // Default values are set in the constructor
                    };
                    user.StrafezResearch = strafezResearch;
                    _context.StrafezResearches.Add(strafezResearch);
                    // --- End StrafezResearch logic ---
                    // --- FClassResearch ---
                    var fClassResearch = new FClassResearch
                    {
                        ApplicationUserId = user.Id
                        // Default values are set in the constructor
                    };
                    user.FClassResearch = fClassResearch;
                    _context.FClassResearches.Add(fClassResearch);
                    // --- End FClassResearch logic ---
                    // --- ProjectsResearch ---
                    var projectsResearch = new ProjectsResearch
                    {
                        ApplicationUserId = user.Id
                        // Default values are set in the constructor
                    };
                    user.ProjectsResearch = projectsResearch;
                    _context.ProjectsResearches.Add(projectsResearch);
                    // --- End ProjectsResearch logic ---
                    // --- ViralSpecificResearch ---
                    if (user.Faction == Faction.Viral)
                    {
                        var viralSpecificResearch = new ViralSpecificResearch
                        {
                            ApplicationUserId = user.Id
                            // Default values are set in the constructor
                        };
                        user.ViralSpecificResearch = viralSpecificResearch;
                        _context.ViralSpecificResearches.Add(viralSpecificResearch);
                        var viralResearch = new ViralResearch
                        {
                            ApplicationUserId = user.Id
                            // Default values are set in the constructor
                        };
                        user.ViralResearch = viralResearch;
                        _context.ViralResearches.Add(viralResearch);
                    }
                    // --- End ViralSpecificResearch logic ---
                    // --- CollectiveSpecificResearch ---
                    if (user.Faction == Faction.Collective)
                    {
                        var collectiveSpecificResearch = new CollectiveSpecificResearch
                        {
                            ApplicationUserId = user.Id
                            // Default values are set in the constructor
                        };
                        user.CollectiveSpecificResearch = collectiveSpecificResearch;
                        _context.CollectiveSpecificResearches.Add(collectiveSpecificResearch);
                        var collectiveResearch = new CollectiveResearch
                        {
                            ApplicationUserId = user.Id
                            // Default values are set in the constructor
                        };
                        user.CollectiveResearch = collectiveResearch;
                        _context.CollectiveResearches.Add(collectiveResearch);
                    }
                    // --- End CollectiveSpecificResearch logic ---
                    // --- TerranResearch ---
                    if (user.Faction == Faction.Terran)
                    {
                        var terranResearch = new TerranResearch
                        {
                            ApplicationUserId = user.Id
                            // Default values are set in the constructor
                        };
                        user.TerranResearch = terranResearch;
                        _context.TerranResearches.Add(terranResearch);
                        var clusterResearch = new ClusterResearch
                        {
                            ApplicationUserId = user.Id
                            // Default values are set in the constructor
                        };
                        user.ClusterResearch = clusterResearch;
                        _context.ClusterResearches.Add(clusterResearch);
                    }
                    // --- End TerranResearch logic ---
                    // --- AMinerResearch ---
                    if (user.Faction == Faction.AMiner)
                    {
                        var aMinerResearch = new AMinerResearch
                        {
                            ApplicationUserId = user.Id
                            // Default values are set in the constructor
                        };
                        user.AMinerResearch = aMinerResearch;
                        _context.AMinerResearches.Add(aMinerResearch);
                        var clusterResearch = new ClusterResearch
                        {
                            ApplicationUserId = user.Id
                            // Default values are set in the constructor
                        };
                        user.ClusterResearch = clusterResearch;
                        _context.ClusterResearches.Add(clusterResearch);
                    }
                    // --- End AMinerResearch logic ---
                    // --- MarauderResearch ---
                    if (user.Faction == Faction.Marauder)
                    {
                        var marauderResearch = new MarauderResearch
                        {
                            ApplicationUserId = user.Id
                            // Default values are set in the constructor
                        };
                        user.MarauderResearch = marauderResearch;
                        _context.MarauderResearches.Add(marauderResearch);
                        var clusterResearch = new ClusterResearch
                        {
                            ApplicationUserId = user.Id
                            // Default values are set in the constructor
                        };
                        user.ClusterResearch = clusterResearch;
                        _context.ClusterResearches.Add(clusterResearch);
                    }
                    // --- End MarauderResearch logic ---
                    // --- GuardianResearch ---
                    if (user.Faction == Faction.Guardian)
                    {
                        var guardianResearch = new GuardianResearch
                        {
                            ApplicationUserId = user.Id
                            // Default values are set in the constructor
                        };
                        user.GuardianResearch = guardianResearch;
                        _context.GuardianResearches.Add(guardianResearch);
                        var clusterResearch = new ClusterResearch
                        {
                            ApplicationUserId = user.Id
                            // Default values are set in the constructor
                        };
                        user.ClusterResearch = clusterResearch;
                        _context.ClusterResearches.Add(clusterResearch);
                    }
                    // --- End GuardianResearch logic ---
                    // --- UserProjects ---
                    var userProjects = new UserProjects
                    {
                        ApplicationUserId = user.Id
                        // Default values are set in the constructor
                    };
                    user.UserProjects = userProjects;
                    _context.UserProjects.Add(userProjects);
                    // --- End UserProjects logic ---
                    // --- Missons ---
                    var missions = new Missions
                    {
                        ApplicationUserId = user.Id
                        // Default values are set in the constructor
                    };
                    user.Missions = missions;
                    _context.Missions.Add(missions);
                    // --- End Missons logic ---
                    // --- viralreversedships ---
                    if(user.Faction == Faction.Viral)
                    {
                        var viralReversedShips = new ViralReversedShips
                        {
                            ApplicationUserId = user.Id
                            // Default values are set in the constructor
                        };
                        user.ViralReversedShips = viralReversedShips;
                        _context.ViralReversedShips.Add(viralReversedShips);
                    }
                    // --- End viralreversedships logic ---
                    user.ITechCooldown = DateTime.UtcNow.AddDays(2); // Initialize ITechCooldown to current time

                    await _context.SaveChangesAsync();

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
