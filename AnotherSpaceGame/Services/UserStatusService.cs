using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Linq;
using AnotherSpaceGame.Data;

namespace AnotherSpaceGame.Services
{
    public class UserStatus
    {
        public string Username { get; set; } = "Guest";
        public long Credits { get; set; }
        public long Food { get; set; }
        public int PowerRating { get; set; }
        public int Turns { get; set; }
    }

    public interface IUserStatusService
    {
        UserStatus GetCurrentStatus(ClaimsPrincipal user);
    }

    public class UserStatusService : IUserStatusService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserStatusService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public UserStatus GetCurrentStatus(ClaimsPrincipal user)
        {
            if (user.Identity?.IsAuthenticated == true)
            {
                var username = user.Identity.Name ?? "Unknown";

                // Fix: Retrieve the ApplicationUser object using the username
                var applicationUser = _dbContext.Users
                    .FirstOrDefault(u => u.UserName == username);

                if (applicationUser == null)
                {
                    return new UserStatus();
                }

                // Example: Find commodities for the current user
                var commodities = _dbContext.Commodities
                    .FirstOrDefault(c => c.ApplicationUserId == applicationUser.Id);
                var turns = _dbContext.Turns
                    .FirstOrDefault(t => t.ApplicationUserId == applicationUser.Id); // Assuming you want the first turn record for the user

                return new UserStatus
                {
                    Username = username,
                    Credits = (commodities?.Credits ?? 0),
                    Food = commodities?.Food ?? 0,
                    PowerRating = applicationUser?.PowerRating ?? 0,
                    Turns = turns?.CurrentTurns ?? 0 // Fix: Use null conditional operator to avoid the CS0019 error
                };
            }
            return new UserStatus();
        }
    }
}