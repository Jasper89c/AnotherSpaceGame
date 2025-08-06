using System.ComponentModel.DataAnnotations;

namespace AnotherSpaceGame.Models
{
    public class UWShips
    {
        [Key]
        public int Id { get; set; }
        public bool UWship1 { get; set; }
        public bool UWship2 { get; set; }
        public bool UWship3 { get; set; }
        public bool UWship4 { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public UWShips()
        {
            // Default constructor
            UWship1 = false;
            UWship2 = false;
            UWship3 = false;
            UWship4 = false;
        }
    }
}
