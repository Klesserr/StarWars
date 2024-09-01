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
		public List<Character> Results { get; set; }
		public string Next { get; set; } //Contenido de people que nos dice en que página nos encontramos
	}
	

	public class PlanetAPI
	{
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
		public List<string> Films { get; set; }
		public List<string> Starships { get; set; }
		public List<Film> ListFilms {  get; set; }

	}
	public class SwapiFilm
	{
		public List<Film> Results { get; set; }
	}
	public class Film
	{
		public int Episode_Id { get; set; }
		public string Title { get; set; }
		public string Opening_Crawl { get; set; }
		public string Url { get; set; } 
	}
	public class StarshipAPI
	{
		public string Url { get; set; }
		public string Name { get; set; }
		public string Model {  get; set; }
		public string Starship_Class {  get; set; }
		public string Manufacturer {  get; set; }

	}

	public class SwapiStarship
	{
		public List<StarshipAPI> Results { get; set; }
		public string Next { get; set; }
	}
}
