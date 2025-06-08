using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using AnotherSpaceGame.Data;
using Microsoft.EntityFrameworkCore;
using AnotherSpaceGame.Models;

public class PeriodicTableUpdater : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public PeriodicTableUpdater(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Only select turns that need updating
                var turnsToUpdate = await dbContext.Turns
                    .Where(t => t.CurrentTurns < t.MaxTurns)
                    .ToListAsync(stoppingToken);

                foreach (var turn in turnsToUpdate)
                {
                    turn.CurrentTurns++;
                }

                if (turnsToUpdate.Count > 0)
                {
                    await dbContext.SaveChangesAsync(stoppingToken);
                }
            }
            await Task.Delay(TimeSpan.FromSeconds(8), stoppingToken);
        }
    }
}