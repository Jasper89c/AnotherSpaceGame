using System.ComponentModel.DataAnnotations;

namespace AnotherSpaceGame.Models
{
    public class ViralReversedShips
    {
        [Key]
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int ViralReversedShipsCount { get; set; }
        public int TerranReversedShipsCount { get; set; }
        public int AMinerReversedShipsCount { get; set; }
        public int MarauderReversedShipsCount { get; set; }
        public int TerranShip1Id { get; set; }
        public int TerranShip2Id { get; set; }
        public int TerranShip3Id { get; set; }
        public int AminerShip1Id { get; set; }
        public int AminerShip2Id { get; set; }
        public int AminerShip3Id { get; set; }
        public int MarauderShip1Id { get; set; }
        public int MarauderShip2Id { get; set; }
        public int MarauderShip3Id { get; set; }

        public ViralReversedShips()
        {
            ViralReversedShipsCount = 0;
            TerranReversedShipsCount = 0;
            AMinerReversedShipsCount = 0;
            MarauderReversedShipsCount = 0;
            TerranShip1Id = 0;
            TerranShip2Id = 0;
            TerranShip3Id = 0;
            AminerShip1Id = 0;
            AminerShip2Id = 0;
            AminerShip3Id = 0;
            MarauderShip1Id = 0;
            MarauderShip2Id = 0;
            MarauderShip3Id = 0;
        }

    }
}
