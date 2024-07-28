using System.ComponentModel.DataAnnotations;

namespace StarWars.Models
{
    public class Starship
    {
        [Key]
        public string Name { get; set; }
        public string Model { get; set; }
        public int Gravity { get; set; }
    }
}
