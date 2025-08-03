using AnotherSpaceGame.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnotherSpaceGame.Services;

public class FederationUpdater : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<FederationUpdater> _logger;

    public FederationUpdater(IServiceProvider serviceProvider, ILogger<FederationUpdater> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            // Use dbContext here
            var server = dbContext.ServerStats.FirstOrDefault();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {

                        var federations = await dbContext.Federations
                            .Include(f => f.FederationMembers)
                            .ToListAsync(stoppingToken);
                        var fedsUpdated = new List<string>();
                        foreach (var federation in federations)
                        {
                            federation.TotalPlanets = federation.FederationMembers.Sum(m => m.TotalPlanets);
                            federation.TotalPowerating = federation.FederationMembers.Sum(m => m.PowerRating);
                            federation.TotalMembers = federation.FederationMembers.Count;
                            if (federation.TotalMembers <= 0)
                            {
                                // Remove related FederationWars where this federation is a defender
                                var warsAsDefender = dbContext.FederationWars
                                    .Where(w => w.DefenderFederationId == federation.Id);
                            dbContext.FederationWars.RemoveRange(warsAsDefender);

                                // (Optional) Remove related FederationWars where this federation is an attacker
                                var warsAsAttacker = dbContext.FederationWars
                                    .Where(w => w.AttackerFederationId == federation.Id);
                                dbContext.FederationWars.RemoveRange(warsAsAttacker);

                                // Remove related FederationApplications where this federation is referenced
                                var applications = dbContext.FederationApplications
                                    .Where(a => a.FederationId == federation.Id);
                                dbContext.FederationApplications.RemoveRange(applications);

                                // Remove related FederationMessages where this federation is referenced
                                var messages = dbContext.FederationMessages
                                    .Where(m => m.FederationId == federation.Id);
                            dbContext.FederationMessages.RemoveRange(messages);

                            dbContext.Federations.Remove(federation);
                                _logger.LogInformation("Removed empty federation.");
                            }
                            fedsUpdated.Add(federation.FederationName);
                        }

                        await dbContext.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation("Federations updated at {Time}.", DateTime.Now);
                        foreach (var fed in fedsUpdated)
                        {
                            _logger.LogInformation("Federation updated: {FederationName}", fed);
                        }
                    
                }
                catch (Exception ex1)
                {
                    _logger.LogError(ex1, "Error occurred in FederationUpdater.");
                }

                await Task.Delay(TimeSpan.FromMinutes(server.FederationTimer), stoppingToken);
            }
        }
    }
}
