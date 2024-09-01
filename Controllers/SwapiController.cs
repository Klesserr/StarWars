using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StarWars.Client;
using StarWars.Data;
using StarWars.ViewModel;
using System.Net.Http.Headers;
using System.Security.Policy;

namespace StarWars.Controllers
{
	public class SwapiController : Controller
	{
		private readonly HttpClient _client;
		public SwapiController(HttpClient client)
		{
			_client = client;
		}
		public async Task<ActionResult> GetAllCharactersAPI()
		//Se hace Task<List<Character>> Si queremos que devuelva un TaskList.Para que sea una lista cuando llamemos al método pondremos await delante.
		{
			List<Character> listCharacters = new List<Character>();
			string url = $"https://swapi.dev/api/people/";
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			do
			{

				var httpResponse = await _client.GetAsync(url);
				if (httpResponse.IsSuccessStatusCode)
				{
					var responseContent = await httpResponse.Content.ReadAsStringAsync();
					Swapi swapi = JsonConvert.DeserializeObject<Swapi>(responseContent);

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
					Swapi swapi = JsonConvert.DeserializeObject<Swapi>(responseContent);
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
		public async Task<List<Film>> GetFilmsAPI(List<string> url_films)
		{
			List<Film> listAllFilms = new List<Film>();
			List<Film> listFilms = new List<Film>();
			string url = $"https://swapi.dev/api/films/";

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

			foreach (var s in url_films)
			{
				var query = listAllFilms.FirstOrDefault(u => u.Url == s);
				if (query != null)
				{
					listFilms.Add(query);
				}
			}
			return (listFilms);
		}
		public async Task<List<StarshipAPI>> GetStarshipAPI(List<string> url_starships)
		{
			List<StarshipAPI> listStarship = new List<StarshipAPI>();
			List<StarshipAPI> getAllStarshipsAPI = new List<StarshipAPI>();
			if (url_starships.IsNullOrEmpty())
			{
				return listStarship;
			}
			string url = "https://swapi.dev/api/starships/";
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			do
			{
				var httpResponse = await _client.GetAsync(url);
				if (httpResponse.IsSuccessStatusCode)
				{
					var responseContent = await httpResponse.Content.ReadAsStringAsync();
					SwapiStarship swapiStarship = JsonConvert.DeserializeObject<SwapiStarship>(responseContent);

					//getAllStarshipsAPI = GetAllStarships(getAllStarshipsAPI, swapiStarship, url);
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
						break;
					}

				}

				//listStarship = AddStarshipList(url_starships, listStarship, getAllStarshipsAPI);
			} while (url != null);
			foreach (var pUrl in url_starships)
			{
				var query = getAllStarshipsAPI.FirstOrDefault(s => s.Url == pUrl);
				if (query != null)
				{
					listStarship.Add(query);
				}
			}
			return listStarship;
		}

		private List<StarshipAPI> GetAllStarships(List<StarshipAPI> getAllStarshipsAPI, SwapiStarship swapiStarship, string url)
		{
			do
			{
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
					break;
				}
			} while (url != null);

			return getAllStarshipsAPI;
		}

		private List<StarshipAPI> AddStarshipList(List<string> url_starships, List<StarshipAPI> listStarship, List<StarshipAPI> getAllStarshipsAPI)
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

		public async Task<ActionResult> SearchCharacterByName(string name)
		{
			Swapi swapi = new Swapi();
			swapi.Results = await GetListAllCharactersAPI(); //Todo el listado de personas que contiene la API
			var query = swapi.Results.Where(x => x.Name == name).ToList();
			SwapiFilm swapiFilm = new SwapiFilm();
			//swapiFilm.Results = await GetFilmsAPI();//Todo el listado de peliculas que contiene la API

			SwapiViewModel swapiVM = new SwapiViewModel(); //Creamos un ViewModel 

			
			foreach (Character character in query)
			{
				var getPlanet = await GetPlanetAPI(character.HomeWorld);

				swapiVM.NamePeople = character.Name;
				swapiVM.HairColor = character.Hair_Color;
				swapiVM.NamePlanet = getPlanet.Name;
				swapiVM.TerrainPlanet = getPlanet.Terrain;
				swapiVM.listFilms = await GetFilmsAPI(character.Films);
				swapiVM.ListStarship = await GetStarshipAPI(character.Starships);

				return View(swapiVM);
			}
			return View();
			//Buscar el nombre del personaje con LinQ.
			//var query = getAll.Result.Where(x => x.Name == name);
			//return View(query);
		}
	}
}
