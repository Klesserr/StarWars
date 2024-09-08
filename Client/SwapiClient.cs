using RestSharp;
using System.Text;
using Newtonsoft;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Reflection.PortableExecutable;

namespace StarWars.Client
{
	

	public class SwapiCharacter : ISwapiHasUrl
	{
		public List<Character> Results { get; set; }
		public string Next { get; set; } //Contenido de people que nos dice en que página nos encontramos
		public string Url { get; set; }
	}

	public class Character : ISwapiHasUrl
	{
		public string Name { get; set; }
		public string Url {  set; get; }
		public string Gender { get; set; }
		public string Height { get; set; }
		public string Mass { get; set; }
		public string Hair_Color { get; set; }
		public string Skin_Color { get; set; }
		public string HomeWorld { get; set; }
		public List<string> Films { get; set; }
		public List<string> Starships { get; set; }
		public List<string> Vehicles {  get; set; }

	}


	public class SwapiFilm : ISwapiHasUrl
	{
		public string Url { get; set; }
		public List<Film> Results { get; set; }
	}
	public class Film : ISwapiHasUrl
	{
		public int Episode_Id { get; set; }
		public string Title { get; set; }
		public string Opening_Crawl { get; set; }
		public string Url { get; set; } 
	}

	public class SwapiStarship : ISwapiHasUrl
	{
		public string Url { get; set; }
		public List<StarshipAPI> Results { get; set; }
		public string Next { get; set; }
	}
	public class StarshipAPI : ISwapiHasUrl
	{
		public string Url { get; set; }
		public string Name { get; set; }
		public string Model {  get; set; }
		public string Starship_Class {  get; set; }
		public string Manufacturer {  get; set; }

	}

	public class PlanetAPI : ISwapiHasUrl
	{
		public string Url { get; set; }
		public string Name { get; set; }
		public string Gravity { get; set; }
		public string Climate { get; set; }
		public string Terrain { get; set; }

	}

	public class SwapiVehicle : ISwapiHasUrl
	{
		public List<Vehicle> Results;
		public string Next { get; set; }
		public string Url { get; set; }
	}
	public class Vehicle : ISwapiHasUrl
	{
		public string Url { get; set; }
		public string Name { get; set; }
		public string Model { get; set; }
		public string Vehicle_Class { get; set; }
		public string Manufacturer { get; set; }
	}

	public class SwapiGeneric<T> : ISwapiHasUrl
	{
		public string Next { set; get; }
		public string Url { set; get; }
		public List<T> Results { get; set; }
	}
	public class AllSwapi<T>
	{
		public SwapiCharacter SwapiCharacter { get; set; }
		public SwapiFilm SwapiFilm {  get; set; }
		public SwapiStarship SwapiStarship { get; set; }
		public SwapiVehicle SwapiVehicle { get; set; }

	}

	public class CharacterImageJson
	{
        public string NameJson { get; set; }
        public string ImageUrl { get; set; }
		public List<ResultadoStarships> ResultadoStarships { get; set; }
		public List<ResultadoVehicles> ResultadoVehicles { get; set; }
		public List<ResultadoPlanets> ResultadoPlanets { get; set; }

	}
	public class ResultadoStarships
	{
		public string NameJsonStarship { get; set; }
		public string ImageUrlStarship { get; set; }
	}
	public class ResultadoVehicles
	{
		public string NameJsonVehicle { get; set; }
		public string ImageUrlVehicle { get; set; }
	}
	public class ResultadoPlanets
	{
		public string NameJsonPlanet { get; set; }
		public string ImageUrlPlanet { get; set; }
	}
	public class ResultadoFilms
	{
		public string NameJsonFilm { get; set; }
		public string ImageUrlFilm { get; set; }
	}
	public class SwapiImageJson
	{
		public List<CharacterImageJson> Resultado { get; set; }
		public List<ResultadoStarships> ResultadoStarships { get; set; }
		public List<ResultadoVehicles> ResultadoVehicles { get; set; }
		public List<ResultadoPlanets> ResultadoPlanets { get; set; }
		public List<ResultadoFilms> ResultadoFilms { get; set; }
	}
}
