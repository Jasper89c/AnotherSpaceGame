namespace AnotherSpaceGame.Models
{
    public class UserShipFleet()
    {
        public int TotalShips { get; set; }
        public int TotalPowerRating { get; set; }
        public int TotalUpkeep { get; set; }
        public string ShipName { get; set; }
        public int ShipId { get; set; }
        public int FleetId { get; set; } 
        public ShipType ShipType { get; set; }
        public int BuildRate { get; set; }

        public int PowerRatingPerShip { get; set; }
    }
}
