using StarWars.Models;
using System.ComponentModel;

namespace StarWars.ViewModel
{
	public class ThreeInOneViewModel
	{
		public string NamePeople { get; set; }
		public Laser ColorLaser { get; set; }
		public Race Race {  get; set; }
		public Order? Order { get; set; }

		public string Gender { get; set; }
		public string Height { get; set; }
		public string Mass { get; set; }
		public string Hair_Color { get; set; }
		public string Skin_Color { get; set; }
		public string HomeWorld { get; set; }
		
		public List<string> Starships { get; set; }
		public List<Planet> PlanetList { get; set; }
		public List<Starship> StarshipList { get; set; }

	}
}
