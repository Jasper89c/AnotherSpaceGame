using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using System;
using System.Linq;

namespace AnotherSpaceGame.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task SendMessage(string message)
        {
            var userName = Context.User?.Identity?.IsAuthenticated == true
                ? Context.User.Identity.Name
                : "Guest";

            // Save message to database
            var chatMessage = new ChatMessage
            {
                UserName = userName,
                Message = message,
                SentAt = DateTime.Now
            };
            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            await Clients.All.SendAsync("ReceiveMessage", userName, message, chatMessage.SentAt);
        }

        public async Task GetRecentMessages(int count = 50)
        {
            var messages = _context.ChatMessages
                .OrderByDescending(m => m.SentAt)
                .Take(count)
                .OrderBy(m => m.SentAt)
                .ToList();

            await Clients.Caller.SendAsync("ReceiveMessageHistory", messages);
        }
    }
}
