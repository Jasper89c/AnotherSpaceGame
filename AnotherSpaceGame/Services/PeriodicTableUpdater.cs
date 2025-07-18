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

public class PeriodicTableUpdater : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHubContext<TurnHub> _turnHub;

    public PeriodicTableUpdater(IServiceProvider serviceProvider, IHubContext<TurnHub> turnHub)
    {
        _serviceProvider = serviceProvider;
        _turnHub = turnHub;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var logger = _serviceProvider.GetRequiredService<ILogger<PeriodicTableUpdater>>();
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

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
                        logger.LogInformation($"Updated {turnsToUpdate.Count} turn(s) at {DateTime.UtcNow}.");
                    }
                    else
                    {
                        logger.LogInformation("No turns to update.");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating turns.");
            }
            await Task.Delay(TimeSpan.FromSeconds(8), stoppingToken);
        }
    }
}