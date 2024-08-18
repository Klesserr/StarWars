using StarWars.Models;

namespace StarWars.ViewModel
{
	public class ThreeInOneViewModel
	{
		public string NamePeople { get; set; }
		public Laser ColorLaser { get; set; }
		public Race Race {  get; set; }
		public Order? Order { get; set; }
		public List<Planet> PlanetList { get; set; }
		public List<Starship> StarshipList { get; set; }

	}
}
