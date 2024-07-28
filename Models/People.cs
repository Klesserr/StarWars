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
        public Planet Planet { get; set; }
        public Starship Starship { get; set; }  
        public ICollection<Planet> planet { get; set; }
        public ICollection<Starship> starship { get; set; }

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
