using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;
using AnotherSpaceGame.Data;
using Microsoft.EntityFrameworkCore;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Services;
using Medallion.Threading;
using Medallion.Threading.SqlServer;

public class PeriodicTableUpdater : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHubContext<TurnHub> _turnHub;
    private readonly string _connectionString;
    private readonly ILogger<PeriodicTableUpdater> _logger;

    public PeriodicTableUpdater(
        IServiceProvider serviceProvider,
        IHubContext<TurnHub> turnHub,
        IConfiguration configuration,
        ILogger<PeriodicTableUpdater> logger)
    {
        _serviceProvider = serviceProvider;
        _turnHub = turnHub;
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var distributedLock = new SqlDistributedLock("PeriodicTableUpdaterLock", _connectionString);
        // Ensure the DistributedLock library is installed via NuGet.
        // Run the following command in the Package Manager Console:
        // Install-Package DistributedLock.SqlServer

        while (!stoppingToken.IsCancellationRequested)
        {
            // Try to acquire the lock for up to 5 seconds
            using (var handle = await distributedLock.TryAcquireAsync(TimeSpan.FromSeconds(5), stoppingToken))
            {
                if (handle != null)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        var server = dbContext.ServerStats.FirstOrDefault();

                        try
                        {
                            var turnsToUpdate = await dbContext.Turns
                                .Where(t => t.CurrentTurns < t.MaxTurns)
                                .ToListAsync(stoppingToken);

                            foreach (var turn in turnsToUpdate)
                            {
                                turn.CurrentTurns++;
                                await _turnHub.Clients.User(turn.ApplicationUserId).SendAsync("UpdateTurns", turn.CurrentTurns);
                            }

                            if (turnsToUpdate.Count > 0)
                            {
                                await dbContext.SaveChangesAsync(stoppingToken);
                                _logger.LogInformation($"Updated {turnsToUpdate.Count} turn(s) at {DateTime.Now}.");
                            }
                            else
                            {
                                _logger.LogInformation("No turns to update.");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error updating turns.");
                        }
                    }
                }
                else
                {
                    _logger.LogInformation("Could not acquire distributed lock. Another instance is running the updater.");
                }
            }
            await Task.Delay(TimeSpan.FromSeconds(7), stoppingToken);
        }
    }
}