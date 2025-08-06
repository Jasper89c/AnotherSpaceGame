using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatHub(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SendMessage(string message)
        {
            var userName = Context.User.Identity?.Name ?? "Unknown";
            var chatMessage = new ChatMessage
            {
                UserName = userName,
                Message = message,
                SentAt = DateTime.UtcNow
            };
            _dbContext.ChatMessages.Add(chatMessage);
            await _dbContext.SaveChangesAsync();

            await Clients.All.SendAsync("ReceiveMessage", userName, message, chatMessage.SentAt);
        }

        public async Task GetRecentMessages(int count)
        {
            var messages = await _dbContext.ChatMessages
                .OrderByDescending(m => m.SentAt)
                .Take(count)
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            await Clients.Caller.SendAsync("ReceiveMessageHistory", messages.Select(m => new {
                userName = m.UserName,
                message = m.Message,
                sentAt = m.SentAt
            }));
        }
    }
}
    

