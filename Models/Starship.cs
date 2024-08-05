using System.ComponentModel.DataAnnotations;

namespace StarWars.Models
{
    public class Starship
    {
        [Key]
        public string Name { get; set; }
        public string Model { get; set; }
        public double Longitude { get; set; }
        public double MaxPassengers { get; set; }
        public string Armament { get; set; }
    }
}
