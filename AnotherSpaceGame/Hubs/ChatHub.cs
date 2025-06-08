using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Hubs
{
    public class ChatHub : Hub
    {
        [Authorize]
        public async Task SendMessage(string message)
        {
            var userName = Context.User?.Identity?.IsAuthenticated == true
                ? Context.User.Identity.Name
                : "Guest";
            await Clients.All.SendAsync("ReceiveMessage", userName, message);
        }
    }
}