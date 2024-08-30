using RestSharp;
using System.Text;
using Newtonsoft;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Reflection.PortableExecutable;

namespace StarWars.Client
{
	public class Swapi
	{
		public PlanetAPI Planet { get; set; }
		public List<Character> Results { get; set; }
		public string Next { get; set; } //Contenido de people que nos dice en que página nos encontramos
	}

	public class PlanetAPI
	{
		public string Next { get; set; }
		public string Name { get; set; }
		public string Gravity { get; set; }
		public string Climate { get; set; }
		public string Terrain { get; set; }
		
	}
	public class Character
	{
		public string Name { get; set; }
		public string Gender { get; set; }
		public string Height { get; set; }
		public string Mass { get; set; }
		public string Hair_Color { get; set; }
		public string Skin_Color { get; set; }
		public string HomeWorld { get; set; }

	}
}
