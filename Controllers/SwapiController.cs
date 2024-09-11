using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StarWars.Client;
using StarWars.Data;
using StarWars.ViewModel;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Security.Policy;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StarWars.Controllers
{
	public class SwapiController : Controller
	{
		private readonly HttpClient _client;
		public SwapiController(HttpClient client)
		{
			_client = client;
		}
		public const string urlPeople = "https://swapi.dev/api/people";
		public const string urlVehicle = "https://swapi.dev/api/vehicles";
		public const string urlStarship = "https://swapi.dev/api/starships";
		public const string urlPlanet = "https://swapi.dev/api/planets";
		public const string urlFilm = "https://swapi.dev/api/films";
		public ActionResult InitialPage()
		{
			SwapiViewModel swapiVM = new SwapiViewModel();
			swapiVM.ListCharacterImageJson = GetUrlImagesJsonCharacters();
			return View(swapiVM);
		}
		public async Task<ActionResult> GetAllCharactersAPI()
		//Se hace Task<List<Character>> Si queremos que devuelva un TaskList.Para que sea una lista cuando llamemos al método pondremos await delante.
		{
			List<Character> listCharacters = new List<Character>();
			string url = $"https://swapi.dev/api/people/";
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


			var httpResponse = await _client.GetAsync(url);
			if (httpResponse.IsSuccessStatusCode)
			{
				var responseContent = await httpResponse.Content.ReadAsStringAsync();
				SwapiCharacter swapi = JsonConvert.DeserializeObject<SwapiCharacter>(responseContent);

				foreach (var c in swapi.Results)
				{
					listCharacters.Add(c);
				}

				string urlNext = swapi.Next;
				if (urlNext != null)
				{
					url = urlNext;
				}

			}
			//List<CharacterImageJson> images = new List<CharacterImageJson>();
			return View(listCharacters);
		}
		//Realizar algo con ambos métodos (más adelante) ya que son iguales,solo cambian el valor que retornan.
		public async Task<List<Character>> GetListAllCharactersAPI()
		{

			List<Character> listCharacters = new List<Character>();
			List<string> listUrlFilms = new List<string>();
			string url = $"https://swapi.dev/api/people/";
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			do
			{

				var httpResponse = await _client.GetAsync(url);
				if (httpResponse.IsSuccessStatusCode)
				{
					var responseContent = await httpResponse.Content.ReadAsStringAsync();
					SwapiCharacter swapi = JsonConvert.DeserializeObject<SwapiCharacter>(responseContent);
					foreach (var c in swapi.Results)
					{
						listCharacters.Add(c);
					}

					string urlNext = swapi.Next;
					if (urlNext != null)
					{
						url = urlNext;
					}
					else
					{
						break;
					}
				}
				else
				{
					break;
				}

			} while (url != null);


			return (listCharacters);
		}


		public async Task<List<Film>> GetFilmsCharacterAPI(List<string> url_films)
		{
			List<Film> listAllFilms = new List<Film>();
			List<Film> listFilmsCharacter = new List<Film>();
			string url = $"https://swapi.dev/api/films/";

			listAllFilms = await GetAllFilmsAPI(listAllFilms, url);
			listFilmsCharacter = AddListFilmsCharacterAPI(listAllFilms, listFilmsCharacter, url_films);

			return listFilmsCharacter;
		}

		private async Task<List<Film>> GetAllFilmsAPI(List<Film> listAllFilms, string url)
		{
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			var httpResponse = await _client.GetAsync(url);

			if (httpResponse.IsSuccessStatusCode)
			{
				var contestResponse = await httpResponse.Content.ReadAsStringAsync();
				SwapiFilm swapi = JsonConvert.DeserializeObject<SwapiFilm>(contestResponse);

				foreach (Film f in swapi.Results)
				{
					listAllFilms.Add(f);
				}
			}
			return listAllFilms;
		}
		private List<Film> AddListFilmsCharacterAPI(List<Film> listAllFilms, List<Film> listFilmsCharacter, List<string> url_films)
		{
			foreach (var s in url_films)
			{
				var query = listAllFilms.FirstOrDefault(u => u.Url == s);
				if (query != null)
				{
					listFilmsCharacter.Add(query);
				}
			}
			return listFilmsCharacter;
		}

		public async Task<ActionResult> GetFilmsAPI(List<string> url_films)
		{
			var query = await GetFilmsCharacterAPI(url_films);
			return View(query);
		}
		public async Task<List<PlanetAPI>> GetAllPlanetsAPI(List<PlanetAPI> listPlanetAPI,string url)
		{
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			do
			{
				var httpResponse = await _client.GetAsync(url);
				if (httpResponse.IsSuccessStatusCode)
				{
					var contestResponse = await httpResponse.Content.ReadAsStringAsync();
					SwapiPlanetAPI swapiPlanetAPI = new SwapiPlanetAPI();
					swapiPlanetAPI = JsonConvert.DeserializeObject<SwapiPlanetAPI>(contestResponse);
					foreach(var p in swapiPlanetAPI.Results)
					{
						listPlanetAPI.Add(p);
					}
					string urlNext = swapiPlanetAPI.Next;
					if (urlNext != null)
					{
						url = urlNext;
					}
					else
					{
						url = null;
					}
				}
			} while (url != null);
			return listPlanetAPI;
		}
		
		public async Task<List<StarshipAPI>> GetStarshipsCharacterAPI(List<string> url_starships)
		{
			List<StarshipAPI> listStarship = new List<StarshipAPI>();
			List<StarshipAPI> getAllStarshipsAPI = new List<StarshipAPI>();
			if (url_starships.IsNullOrEmpty())
			{
				return listStarship;
			}
			string url = "https://swapi.dev/api/starships/";

			getAllStarshipsAPI = await GetListAllStarshipAPI(getAllStarshipsAPI, url);
			listStarship = AddStarshipHasCharacter(url_starships, listStarship, getAllStarshipsAPI);

			return listStarship;
		}

		private async Task<List<StarshipAPI>> GetListAllStarshipAPI(List<StarshipAPI> getAllStarshipsAPI, string url)
		{
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			do
			{
				var httpResponse = await _client.GetAsync(url);
				if (httpResponse.IsSuccessStatusCode)
				{
					var responseContent = await httpResponse.Content.ReadAsStringAsync();
					SwapiStarship swapiStarship = JsonConvert.DeserializeObject<SwapiStarship>(responseContent);

					foreach (var starshipAPI in swapiStarship.Results)
					{
						getAllStarshipsAPI.Add(starshipAPI);
					}
					string urlNext = swapiStarship.Next;
					if (urlNext != null)
					{
						url = urlNext;
					}
					else
					{
						url = null;
					}
				}

			} while (url != null);

			return getAllStarshipsAPI;
		}

		private List<StarshipAPI> AddStarshipHasCharacter(List<string> url_starships, List<StarshipAPI> listStarship, List<StarshipAPI> getAllStarshipsAPI)
		{
			foreach (var pUrl in url_starships)
			{
				var query = getAllStarshipsAPI.FirstOrDefault(s => s.Url == pUrl);
				if (query != null)
				{
					listStarship.Add(query);
				}
				else
				{
					continue;
				}
			}
			return listStarship;
		}


		public async Task<List<Vehicle>> GetVehiclesCharacterAPI(List<string> url_vehicles)
		{
			List<Vehicle> ListVehicles = new List<Vehicle>();
			List<Vehicle> ListAllVehiclesAPI = new List<Vehicle>();
			string url = "https://swapi.dev/api/vehicles/";

			ListAllVehiclesAPI = await GetAllVehiclesAPI(ListAllVehiclesAPI, url);
			ListVehicles = AddVehicleHasCharacter(url_vehicles, ListAllVehiclesAPI, ListVehicles);

			return ListVehicles;
		}
		private async Task<List<Vehicle>> GetAllVehiclesAPI(List<Vehicle> listAllVehicles, string url)
		{
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			do
			{
				var httpResponse = await _client.GetAsync(url);
				if (httpResponse.IsSuccessStatusCode)
				{
					var responseContent = await httpResponse.Content.ReadAsStringAsync();
					SwapiVehicle swapiVehicle = JsonConvert.DeserializeObject<SwapiVehicle>(responseContent);
					foreach (var v in swapiVehicle.Results)
					{
						listAllVehicles.Add(v);
					}
					string urlNext = swapiVehicle.Next;
					if (urlNext != null)
					{
						url = urlNext;
					}
					else
					{
						url = null;
					}
				}
			} while (url != null);
			return listAllVehicles;
		}
		private List<Vehicle> AddVehicleHasCharacter(List<string> url_vehicles, List<Vehicle> listAllVehicles, List<Vehicle> listVehicles)
		{
			foreach (var s in url_vehicles)
			{
				var query = listAllVehicles.FirstOrDefault(p => p.Url == s);
				if (query != null)
				{
					listVehicles.Add(query);
				}
				else
				{
					continue;
				}
			}
			return listVehicles;
		}

		public async Task<PlanetAPI> GetPlanetAPI(string url_planet)
		{
			PlanetAPI planet = new PlanetAPI();
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var hhtpResponse = await _client.GetAsync(url_planet);
			if (hhtpResponse.IsSuccessStatusCode)
			{
				var responseContent = await hhtpResponse.Content.ReadAsStringAsync();
				planet = JsonConvert.DeserializeObject<PlanetAPI>(responseContent);

				return planet;
			}

			return planet;
		}

		public async Task<ActionResult> SearchCharacterByName(string name)
		{
			SwapiCharacter swapi = new SwapiCharacter();
			swapi.Results = await GetListAllCharactersAPI(); //Todo el listado de personas que contiene la API
			var query = swapi.Results.Where(x => x.Name == name).ToList();

			SwapiViewModel swapiVM = new SwapiViewModel();

			foreach (var item in query)
			{
				var getPlanet = await GetPlanetAPI(item.HomeWorld);
				//Propiedades de la clase Character:
				swapiVM.NameCharacter = item.Name;
				swapiVM.GenderCharacter = item.Gender;
				swapiVM.MassCharacter = item.Mass;
				swapiVM.HairColorCharacter = item.Hair_Color;
				swapiVM.SkinColorCharacter = item.Skin_Color;
				swapiVM.HeightCharacter = item.Height;
				//Propiedades de la clase Planet:
				swapiVM.NamePlanet = getPlanet.Name;
				swapiVM.TerrainPlanet = getPlanet.Terrain;
				swapiVM.GravityPlanet = getPlanet.Gravity;
				swapiVM.ClimatePlanet = getPlanet.Climate;
				//Propiedades de la clase Starship:
				swapiVM.ListStarship = await GetStarshipsCharacterAPI(item.Starships);
				//Propiedades de la clase Film:
				swapiVM.ListFilms = await GetFilmsCharacterAPI(item.Films);
				foreach (var p in swapiVM.ListFilms)
				{
					swapiVM.TitleFilm = p.Title;
				}
				//Propiedades de la clase Vehicle:
				swapiVM.ListVehicle = await GetVehiclesCharacterAPI(item.Vehicles);
			}
			return View(swapiVM);
			/*
			 foreach (Character character in swapi.Results)
			{
				character.ListFilms = new List<Film>();
					foreach (var url_film in character.Films)
				{
						foreach (Film f in swapiFilm.Results) 
				{
							if (f.Url == url_film)
							{
								character.ListFilms.Add(f);
								break;
							}
				}
			}*/
		}
		public async Task<List<T>> GetInformation<T>(string url, List<string> urlList) where T : ISwapiHasUrl //(DONDE el generico implemente la interfaz ISwapiHasUrl)
			/* where T:ISwapiHasUrl -> nos dice que el tipo T(Genérico) debe implementar esa interfaz.
				Además ahora sabemos que cualquier tipo T tiene la propiedad Url que es la que esta definida en dicha interfaz, Ya que 
				cualquier objeto de tipo T tendrá dicha propiedad.*/
		{
			List<T> result = new List<T>();
			List<T> getAllInformationAPI = new List<T>();
			if (urlList.IsNullOrEmpty())
			{
				return result;
			}
			getAllInformationAPI = await GenericInfo(getAllInformationAPI, url);

			result = AddEspecificInfo(urlList, getAllInformationAPI, result);


			return result;
		}
		public async Task<List<T>> GenericInfo<T>(List<T> listGeneric, string url)
		{
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			do
			{
				var httpResponse = await _client.GetAsync(url);
				if (httpResponse.IsSuccessStatusCode)
				{
					var contestRespone = await httpResponse.Content.ReadAsStringAsync();
					SwapiGeneric<T> swapi = JsonConvert.DeserializeObject<SwapiGeneric<T>>(contestRespone);
					foreach (var item in swapi.Results)
					{
						listGeneric.Add(item);
					}
					string urlNext = swapi.Next;
					if (urlNext != null)
					{
						url = urlNext;
					}
					else
					{
						url = null;
					}
				}
			} while (url != null);
			return listGeneric;
		}
		public List<T> AddEspecificInfo<T>(List<string> urlList, List<T> genericList, List<T> result) where T : ISwapiHasUrl
		{
			foreach (var url in urlList)
			{
				var query = genericList.FirstOrDefault(p => p.Url == url);
				if (query != null)
				{
					result.Add(query);
				}
				else
				{
					continue;
				}
			}
			return result;
		}
		public async Task<ActionResult> GetGenericView(string name, string image)
		{

			SwapiCharacter swapi = new SwapiCharacter();
			swapi.Results = await GetListAllCharactersAPI();
			var query = swapi.Results.Where(x => x.Name == name).ToList();

			List<CharacterImageJson> characterImageJsons = new List<CharacterImageJson>();
			characterImageJsons = GetUrlImagesJsonCharacters();

			SwapiViewModel swapiVM = new SwapiViewModel();
			swapiVM.ListCharacterImageJson = characterImageJsons;

			foreach (var c in query)
			{
				var getPlanet = await GetPlanetAPI(c.HomeWorld);
				swapiVM.NameCharacter = c.Name;
				swapiVM.GenderCharacter = c.Gender;
				swapiVM.HeightCharacter = c.Height;
				swapiVM.MassCharacter = c.Mass;
				swapiVM.SkinColorCharacter = c.Skin_Color;
				swapiVM.HairColorCharacter = c.Hair_Color;
				//
				swapiVM.NamePlanet = getPlanet.Name;
				swapiVM.TerrainPlanet = getPlanet.Terrain;
				swapiVM.ClimatePlanet = getPlanet.Climate;
				swapiVM.GravityPlanet = getPlanet.Gravity;
				//
				swapiVM.ListStarship = await GetInformation<StarshipAPI>(urlStarship, c.Starships);
				swapiVM.ListFilms = await GetInformation<Film>(urlFilm, c.Films);
				swapiVM.ListVehicle = await GetInformation<Vehicle>(urlVehicle, c.Vehicles);
				//
				swapiVM.NameJson = name;
				swapiVM.ImageUrl = image;
				//
			}
			return View(swapiVM);
		}
		public List<CharacterImageJson> GetUrlImagesJsonCharacters()
		{
			List<CharacterImageJson> charactersImageJson = new List<CharacterImageJson>();

			SwapiImageJson swapiCharacterImageJson = new SwapiImageJson();
			string urlJson = "wwwroot/archivosJSON/character-image.json";
			string fileJson = System.IO.File.ReadAllText(urlJson);
			swapiCharacterImageJson = JsonConvert.DeserializeObject<SwapiImageJson>(fileJson);

			foreach (var i in swapiCharacterImageJson.Resultado)
			{
				charactersImageJson.Add(i);
			}
			return charactersImageJson;
		}
		public async Task<ActionResult> GetTheFilm(string title, string name)
		{
			SwapiCharacter swapi = new SwapiCharacter();
			swapi.Results = await GetListAllCharactersAPI();

			SwapiViewModel swapiVM = new SwapiViewModel();
			var query = swapi.Results.Where(x => x.Name == name).ToList();

			SwapiStarship swapiStarship = new SwapiStarship();
			List<StarshipAPI> starshipAPIs = new List<StarshipAPI>();
			swapiStarship.Results = await GetListAllStarshipAPI(starshipAPIs, urlStarship);

			SwapiVehicle swapiVehicle = new SwapiVehicle();
			List<Vehicle> vehicles = new List<Vehicle>();
			swapiVehicle.Results = await GetAllVehiclesAPI(vehicles, urlVehicle);

			SwapiPlanetAPI swapiPlanetAPI = new SwapiPlanetAPI();
			List<PlanetAPI> planetAPI = new List<PlanetAPI>();
			swapiPlanetAPI.Results = await GetAllPlanetsAPI(planetAPI, urlPlanet);

			foreach (var c in query)
			{
				swapiVM.ListFilms = await GetInformation<Film>(urlFilm, c.Films);
				foreach (var film in swapiVM.ListFilms)
				{
					if (film.Title == title && c.Name == name)
					{
						swapiVM.TitleFilm = film.Title;
						swapiVM.EpisodeFilm = film.Episode_Id;
						swapiVM.OpeningFilm = film.Opening_Crawl;
						swapiVM.Director = film.Director;
						//Obtenemos el listado de naves que aparecen en la pelicula:(objeto SwapiStarship,lista<StarashipAPI>,objeto Film)
						swapiVM.ListStarship = await GetAllStarshipsAppearFilm(swapiStarship, swapiVM.ListStarship,film);
						//Obtenemos el listado de vehiculos que aparecen en la pelicula:(objeto SwapiVehicle,lista<Vehicle>,objeto Film)
						swapiVM.ListVehicle = await GetAllVehiclesAppearFilm(swapiVehicle,swapiVM.ListVehicle,film);
						//Obtenemos el listado de planetas que aparecen en la pelicula:(objeto SwapiPlanetAPI,lista<PlanetAPI>,objeto Film)
						swapiVM.ListPlanetAPI = await GetAllPlanetsAppearFilm(swapiPlanetAPI,swapiVM.ListPlanetAPI,film);
						//Obtenemos el listado de personajes que aparecen en la pelicula:(objeto SwapiCharacter,lista<Character>,objeto Film)
						swapiVM.ListCharacter = await GetAllCharactersAppearFilm(swapi,swapiVM.ListCharacter,film);
					}
				}
			}
			return View(swapiVM);

		}
		
		public async Task<List<StarshipAPI>> GetAllStarshipsAppearFilm(SwapiStarship swapiStarship,List<StarshipAPI>listStarshipAPI,Film film)
		{
			/*Creamos un objeto de tipo StarshipAPI y recorremos la lista que nos llega
			 * (que es de tipo SwapiStarship(contiene un listado con todas las naves de la api))
			 * Después recorremos filmStarships-> es un listado de string que contiene la url de las naves en el listado de peliculas.
			 * En caso de que las url coincidan, llamamos al método que nos devuelve la información sobre esa nave y retornamos su listado..*/
			foreach (var starshipAPI in swapiStarship.Results)
			{
				foreach (string url in film.Starships) //filmStarships->Listado de string que contiene la url de las starships en el listado de peliculas.
				{
					if (starshipAPI.Url == url)
					{
						listStarshipAPI = await GetInformation<StarshipAPI>(urlStarship, film.Starships);
					}
				}
			}
			return listStarshipAPI;
		}
		public async Task<List<Vehicle>> GetAllVehiclesAppearFilm(SwapiVehicle swapiVehicle,List<Vehicle>listVehicle,Film film)
		{
			foreach(var vehicle in swapiVehicle.Results)
			{
				foreach(string url in film.Vehicles)
				{
					if(vehicle.Url == url)
					{
						listVehicle = await GetInformation<Vehicle>(urlVehicle, film.Vehicles);
					}
				}
			}
			return listVehicle;
		}
		public async Task<List<PlanetAPI>> GetAllPlanetsAppearFilm(SwapiPlanetAPI swapiPlanetAPI,List<PlanetAPI>listPlanetAPI,Film film)
		{
			foreach(var planet in swapiPlanetAPI.Results)
			{
				foreach(string url in film.Planets)
				{
					if(planet.Url == url)
					{
						listPlanetAPI = await GetInformation<PlanetAPI>(urlPlanet, film.Planets);
					}
				}
			}
			return listPlanetAPI;
		}
		public async Task<List<Character>> GetAllCharactersAppearFilm(SwapiCharacter swapiCharacter,List<Character>listCharacters,Film film)
		{
			foreach(var character in swapiCharacter.Results)
			{
				foreach(var url in film.Characters)
				{
					if(character.Url == url)
					{
						listCharacters = await GetInformation<Character>(urlPeople,film.Characters);
					}
				}
			}
			return listCharacters;
		}
	}
}
