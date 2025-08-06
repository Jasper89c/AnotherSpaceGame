using System;
using System.ComponentModel.DataAnnotations;

namespace AnotherSpaceGame.Models
{
    public class ChatMessage
    {
        [Key]        
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}