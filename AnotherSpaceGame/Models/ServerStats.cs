using System.ComponentModel.DataAnnotations;

namespace AnotherSpaceGame.Models
{
    public class ServerStats
    {
        [Key]
        public int Id { get; set; }
        public int TurnTimer { get; set; }
        public int TableTimer { get; set; }
        public int FederationTimer { get; set; }
        public bool UWEnabled { get; set; }
        public bool UWCompleted { get; set; }
        public string UWHolderName { get; set; }
        public string UWHolderId { get; set; }


    }
}
