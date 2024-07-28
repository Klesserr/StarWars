namespace StarWars.Models
{
    public class People
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? LaserSword { get; set; }
        public Order? Order {  get; set; }
        public Race? Race { get; set; }
     
        public string? Starshsips;

        public ICollection<Planet> Planet { get; set; }
        public ICollection<Starship> Starship { get; set; }

    }

    public enum Race
    {
        Human,Ewok,Zabrak,Gungans,Wookie,GenDai,Hutt,Imperial,Voxyn,Kaleesh
    }
    public enum Order
    {
        Jedi,Sith
    }
}
