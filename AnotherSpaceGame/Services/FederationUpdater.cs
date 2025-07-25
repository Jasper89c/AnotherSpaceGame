using AnotherSpaceGame.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Services
{
    public class FederationUpdater : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);

        public FederationUpdater(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var federations = await db.Federations
                        .Include(f => f.FederationMembers)
                        .ToListAsync(stoppingToken);

                    foreach (var federation in federations)
                    {
                        federation.TotalPlanets = federation.FederationMembers.Sum(m => m.TotalPlanets);
                        federation.TotalPowerating = federation.FederationMembers.Sum(m => m.PowerRating);
                        federation.TotalMembers = federation.FederationMembers.Count;
                        if (federation.TotalMembers <= 0)
                        {
                            db.Federations.Remove(federation);
                        }
                    }

                    await db.SaveChangesAsync(stoppingToken);
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}