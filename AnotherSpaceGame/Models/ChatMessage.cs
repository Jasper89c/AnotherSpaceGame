using System;

namespace AnotherSpaceGame.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}