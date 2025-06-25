using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class PrivateMessagesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PrivateMessagesModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<PrivateMessage> Messages { get; set; }

        [BindProperty]
        [Required]
        public string ReceiverUserName { get; set; }

        [BindProperty]
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }

        public string StatusMessage { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            Messages = await _context.PrivateMessages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => m.ReceiverId == user.Id || m.SenderId == user.Id)
                .OrderByDescending(m => m.SentAt)
                .Take(50)
                .ToListAsync();

            // Mark unread messages as read
            var unread = Messages.Where(m => m.ReceiverId == user.Id && !m.IsRead).ToList();
            if (unread.Any())
            {
                foreach (var msg in unread)
                    msg.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            var receiver = await _userManager.FindByNameAsync(ReceiverUserName);

            if (receiver == null)
            {
                StatusMessage = "User not found.";
                await OnGetAsync();
                return Page();
            }

            if (!ModelState.IsValid)
            {
                StatusMessage = "Please enter a message.";
                await OnGetAsync();
                return Page();
            }

            var message = new PrivateMessage
            {
                SenderId = user.Id,
                ReceiverId = receiver.Id,
                Content = Content,
                SentAt = DateTime.UtcNow
            };

            _context.PrivateMessages.Add(message);
            await _context.SaveChangesAsync();

            StatusMessage = "Message sent!";
            Content = "";
            await OnGetAsync();
            return Page();
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var message = await _context.PrivateMessages
                .FirstOrDefaultAsync(m => m.Id == id && (m.ReceiverId == user.Id || m.SenderId == user.Id));

            if (message == null)
            {
                StatusMessage = "Message not found or you do not have permission to delete it.";
                await OnGetAsync();
                return Page();
            }

            _context.PrivateMessages.Remove(message);
            await _context.SaveChangesAsync();

            StatusMessage = "Message deleted.";
            await OnGetAsync();
            return Page();
        }
    }
}
