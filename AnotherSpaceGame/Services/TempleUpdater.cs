using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;

namespace AnotherSpaceGame.Services
{

    public class TempleUpdater : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TempleUpdater> _logger;
        private readonly Random _random; // Add a Random instance
        private readonly Random _random1; // Add a Random instance
        private readonly Random _random2; // Add a Random instance

        public TempleUpdater(IServiceProvider serviceProvider, ILogger<TempleUpdater> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _random = new Random(); // Initialize the Random instance
            _random1 = new Random(); // Initialize the Random instance
            _random2 = new Random(); // Initialize the Random instance
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                while (!stoppingToken.IsCancellationRequested)
                {
                    // 1. Calculate the next scheduled time (e.g., 5:00 AM)
                    TimeSpan scheduledTime = new TimeSpan(5, 0, 0); // 5:00 AM
                    DateTime now = DateTime.Now;
                    DateTime nextRun = now.Date + scheduledTime;
                    if (now > nextRun)
                    {
                        nextRun = nextRun.AddDays(1);
                    }
                    TimeSpan delay = nextRun - now;

                    // 2. Wait until the scheduled time
                    if (delay > TimeSpan.Zero)
                    {
                        await Task.Delay(delay, stoppingToken);
                    }
                    try
                    {
                        // Add your logic here  
                        var usersTemples = dbContext.Users.OrderByDescending(u => u.TempleHeight).Take(25).ToList();
                        for (int i = 0; i < usersTemples.Count; i++)
                        {
                            if (i == 0 && usersTemples[i].TempleHeight > 0)
                            {
                                var randomArtifactId1 = _random.Next(52, 65);
                                var randomArtifactAmount1 = _random.Next(3, 6);
                                Artifacts arti1 = dbContext.Artifacts.FirstOrDefault(x => x.ApplicationUserId == usersTemples[i].Id && x.ArtifactId == randomArtifactId1);
                                string arti1Name = "Unknown Artifact";
                                if (arti1 == null)
                                {
                                    Artifacts newArtifact = new Artifacts(randomArtifactId1, randomArtifactAmount1, usersTemples[i].Id);
                                    arti1Name = newArtifact.ArtifactName.ToString();
                                    dbContext.Artifacts.Add(newArtifact);
                                    dbContext.SaveChanges();
                                }
                                else
                                {
                                    arti1.Total += randomArtifactAmount1;
                                    arti1Name = arti1.ArtifactName.ToString();
                                }
                                var randomArtifactId2 = _random1.Next(52, 65);
                                var randomArtifactAmount2 = _random1.Next(3, 6);
                                Artifacts arti2 = dbContext.Artifacts.FirstOrDefault(x => x.ApplicationUserId == usersTemples[i].Id && x.ArtifactId == randomArtifactId2);
                                string arti2Name = "Unknown Artifact";
                                if (arti2 == null)
                                {
                                    Artifacts newArtifact = new Artifacts(randomArtifactId1, randomArtifactAmount1, usersTemples[i].Id);
                                    arti2Name = newArtifact.ArtifactName.ToString();
                                    dbContext.Artifacts.Add(newArtifact);
                                    dbContext.SaveChanges();
                                }
                                else
                                {
                                    arti2.Total += randomArtifactAmount2;
                                    arti2Name = arti2.ArtifactName.ToString();
                                }
                                var randomArtifactId3 = _random2.Next(52, 65);
                                var randomArtifactAmount3 = _random2.Next(3, 6);
                                Artifacts arti3 = dbContext.Artifacts.FirstOrDefault(x => x.ApplicationUserId == usersTemples[i].Id && x.ArtifactId == randomArtifactId3);
                                string arti3Name = "Unknown Artifact";
                                if (arti3 == null)
                                {
                                    Artifacts newArtifact = new Artifacts(randomArtifactId1, randomArtifactAmount1, usersTemples[i].Id);
                                    arti3Name = newArtifact.ArtifactName.ToString();
                                    dbContext.Artifacts.Add(newArtifact);
                                    dbContext.SaveChanges();
                                }
                                else
                                {
                                    arti3.Total += randomArtifactAmount3;
                                    arti3Name = arti3.ArtifactName.ToString();
                                }
                                Commodities com = dbContext.Commodities.FirstOrDefault(x => x.ApplicationUserId == usersTemples[i].Id);
                                long creditsToAdd = (long)(_random.NextDouble() * (5000000000L - 1000000000L)) + 1000000000L;
                                com.Credits += creditsToAdd;
                                long foodToAdd = _random.Next(500000000, 2000000000);
                                com.Food += foodToAdd;
                                ImportantEvents importantEvents = new ImportantEvents
                                {
                                    ApplicationUser = usersTemples[i],
                                    ApplicationUserId = usersTemples[i].Id,
                                    ImportantEventTypes = ImportantEventTypes.Misc,
                                    Text = $"Your temple has been updated with the following <br> {randomArtifactAmount1} of Artifact {arti1Name} <br> {randomArtifactAmount2} of Artifact {arti2Name} <br> {randomArtifactAmount3} of Artifact {arti3Name} <br> Credits + {creditsToAdd.ToString("C0")} <br> Food + {foodToAdd.ToString("N0")}",
                                    DateAndTime = DateTime.Now
                                };
                                dbContext.ImportantEvents.Add(importantEvents);
                            }
                            else if (i >= 1 && i <= 4 && usersTemples[i].TempleHeight > 0)
                            {
                                var randomArtifactId1 = _random1.Next(52, 65);
                                var randomArtifactAmount1 = 2;
                                Artifacts arti1 = dbContext.Artifacts.FirstOrDefault(x => x.ApplicationUserId == usersTemples[i].Id && x.ArtifactId == randomArtifactId1);
                                string arti1Name = "Unknown Artifact";
                                if (arti1 == null)
                                {
                                    Artifacts newArtifact = new Artifacts(randomArtifactId1, randomArtifactAmount1, usersTemples[i].Id);
                                    arti1Name = newArtifact.ArtifactName.ToString();
                                    dbContext.Artifacts.Add(newArtifact);
                                    dbContext.SaveChanges();
                                }
                                else
                                {
                                    arti1.Total += randomArtifactAmount1;
                                    arti1Name = arti1.ArtifactName.ToString();
                                }
                                Commodities com = dbContext.Commodities.FirstOrDefault(x => x.ApplicationUserId == usersTemples[i].Id);
                                long creditsToAdd = _random.Next(500000000, 1000000000);
                                com.Credits += creditsToAdd;
                                long foodToAdd = _random.Next(250000000, 1000000000);
                                com.Food += foodToAdd;
                                ImportantEvents importantEvents = new ImportantEvents
                                {
                                    ApplicationUser = usersTemples[i],
                                    ApplicationUserId = usersTemples[i].Id,
                                    ImportantEventTypes = ImportantEventTypes.Misc,
                                    Text = $"Your temple has been updated with the following <br> {randomArtifactAmount1} of Artifact {arti1Name} <br> Credits + {creditsToAdd.ToString("C0")} <br> Food + {foodToAdd.ToString("N0")}",
                                    DateAndTime = DateTime.Now
                                };
                                dbContext.ImportantEvents.Add(importantEvents);
                            }
                            else if (i >= 5 && usersTemples[i].TempleHeight > 0)
                            {
                                var randomArtifactId1 = _random2.Next(52, 65);
                                var randomArtifactAmount1 = 1;
                                Artifacts arti1 = dbContext.Artifacts.FirstOrDefault(x => x.ApplicationUserId == usersTemples[i].Id && x.ArtifactId == randomArtifactId1);
                                string arti1Name = "Unknown Artifact";
                                if (arti1 == null)
                                {
                                    Artifacts newArtifact = new Artifacts(randomArtifactId1, randomArtifactAmount1, usersTemples[i].Id);
                                    arti1Name = newArtifact.ArtifactName.ToString();
                                    dbContext.Artifacts.Add(newArtifact);
                                    dbContext.SaveChanges();
                                }
                                else
                                {
                                    arti1.Total += randomArtifactAmount1;
                                    arti1Name = arti1.ArtifactName.ToString();
                                }
                                Commodities com = dbContext.Commodities.FirstOrDefault(x => x.ApplicationUserId == usersTemples[i].Id);
                                long creditsToAdd = _random.Next(250000000, 500000000);
                                com.Credits += creditsToAdd;
                                long foodToAdd = _random.Next(100000000, 250000000);
                                com.Food += foodToAdd;
                                ImportantEvents importantEvents = new ImportantEvents
                                {
                                    ApplicationUser = usersTemples[i],
                                    ApplicationUserId = usersTemples[i].Id,
                                    ImportantEventTypes = ImportantEventTypes.Misc,
                                    Text = $"Your temple has been updated with the following <br> {randomArtifactAmount1} of Artifact {arti1Name} <br> Credits + {creditsToAdd.ToString("C0")} <br> Food + {foodToAdd.ToString("N0")}",
                                    DateAndTime = DateTime.Now
                                };
                                dbContext.ImportantEvents.Add(importantEvents);
                            }
                        }
                        _logger.LogInformation("Temple Updater executed successfully.");
                        dbContext.SaveChanges();
                    }
                    catch (Exception ex1)
                    {
                        _logger.LogError(ex1, "Error occurred in Temple Updater.");
                    }

                    await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
                }
            }
        }
    }
}
