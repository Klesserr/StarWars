using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarWars.Client;
using StarWars.Data;
using StarWars.ViewModel;
using System.Net.Http.Headers;

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
			do
			{
				_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var httpResponse = await _client.GetAsync(url);
				if (httpResponse.IsSuccessStatusCode)
				{
					var responseContent2 = await httpResponse.Content.ReadAsStringAsync();

					Swapi swapi2 = JsonConvert.DeserializeObject<Swapi>(responseContent2);
					foreach (Character c in swapi2.Results)
					{
						listCharacters.Add(c);

					}

					string urlNext = swapi2.Next;
					Console.WriteLine(urlNext);
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
			string url = $"https://swapi.dev/api/people/";
			do
			{
				_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var httpResponse = await _client.GetAsync(url);
				if (httpResponse.IsSuccessStatusCode)
				{
					var responseContent = await httpResponse.Content.ReadAsStringAsync();

					Swapi swapi = JsonConvert.DeserializeObject<Swapi>(responseContent);
					foreach (Character c in swapi.Results)
					{
						listCharacters.Add(c);

					}

					string urlNext = swapi.Next;
					Console.WriteLine(urlNext);
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
		public async Task<ActionResult> SearchCharacterByName(string name)
		{
			var getAllCharacters = GetListAllCharactersAPI();
			SwapiViewModel swapiVM = new SwapiViewModel();
			foreach (var character in getAllCharacters.Result)
			{
				if (character.Name == name)
				{
					var getPlanet = await GetPlanetAPI(character.HomeWorld);
					swapiVM.NamePeople = character.Name;
					swapiVM.HairColor = character.Hair_Color;
					swapiVM.NamePlanet = getPlanet.Name;
					swapiVM.TerrainPlanet = getPlanet.Terrain;
					return View(swapiVM);
				}
			}
			return View();
			//Buscar el nombre del personaje con LinQ.
			//var query = getAll.Result.Where(x => x.Name == name);
			//return View(query);
		}

		public async Task<PlanetAPI> GetPlanetAPI(string url_planet)
		{
			PlanetAPI planet = new PlanetAPI();
			var getAll = GetListAllCharactersAPI(); //Pondremos.Result para obtener un listado directamente.
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var hhtpResponse = await _client.GetAsync(url_planet);
			if (hhtpResponse.IsSuccessStatusCode)
			{
				var responseContent = await hhtpResponse.Content.ReadAsStringAsync();
				planet = JsonConvert.DeserializeObject<PlanetAPI>(responseContent);
				foreach (var character in getAll.Result)
				{
					if (url_planet == character.HomeWorld)
					{
						return planet;
					}
				}
			}

			return planet;
		}
	}
}
