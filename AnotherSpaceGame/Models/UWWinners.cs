using System.ComponentModel.DataAnnotations;

namespace AnotherSpaceGame.Models
{
    public class UWWinners
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string DaysTaken { get; set; }
        public DateTime Date { get; set; }
        public string PowerRating { get; set; }
        public string TotalPlanets { get; set; }
    }
}
