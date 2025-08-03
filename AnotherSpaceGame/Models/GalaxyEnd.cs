using System.ComponentModel.DataAnnotations;

namespace AnotherSpaceGame.Models
{
    public class GalaxyEnd
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PowerRating { get; set; }
        public string ApplicationUserId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
