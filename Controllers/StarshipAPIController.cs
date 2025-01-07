using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StarWars.Client;
using System.Net.Http.Headers;
using StarWars.Data;
using StarWars.ViewModel;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;

namespace StarWars.Controllers
{
	public class StarshipAPIController : Controller
	{
		public const string urlStarship = "https://swapi.py4e.com/api/starships/";
		private readonly HttpClient _client;
		private readonly GenericController _generic;
		public StarshipAPIController(HttpClient client,GenericController generic)
		{
			_client = client;
			_generic = generic;
		}
		public async Task<List<StarshipAPI>> GetStarshipsCharacterAPI(List<string> url_starships)
		{
			List<StarshipAPI> listStarship = new List<StarshipAPI>();
			List<StarshipAPI> getAllStarshipsAPI = new List<StarshipAPI>();
			if (url_starships.IsNullOrEmpty())
			{
				return listStarship;
			}
			string url = "https://swapi.py4e.com/api/starships/";

			getAllStarshipsAPI = await GetListAllStarshipAPI(url);
			listStarship = AddStarshipHasCharacter(url_starships, getAllStarshipsAPI);

			return listStarship;
		}

		public async Task<List<StarshipAPI>> GetListAllStarshipAPI(string url)
		{
			List<StarshipAPI> getAllStarshipsAPI = new List<StarshipAPI>();
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

		private List<StarshipAPI> AddStarshipHasCharacter(List<string> url_starships, List<StarshipAPI> getAllStarshipsAPI)
		{
			List<StarshipAPI> listStarship = new List<StarshipAPI>();
			foreach (var pUrl in url_starships)
			{
				var starshipAPI = getAllStarshipsAPI.FirstOrDefault(s => s.Url == pUrl);
				if (starshipAPI != null)
				{
					listStarship.Add(starshipAPI);
				}
				else
				{
					continue;
				}
			}
			return listStarship;
		}
		public List<ResultadoStarships> GetUrlImagesJsonStarships()
		{
			List<ResultadoStarships> characterImagesStarships = new List<ResultadoStarships>();
			SwapiImageJson swapiCharacterImageJson = new SwapiImageJson();
			string urlJson = "wwwroot/archivosJSON/character-image.json";
			string fileJson = System.IO.File.ReadAllText(urlJson);
			swapiCharacterImageJson = JsonConvert.DeserializeObject<SwapiImageJson>(fileJson);
			foreach (var s in swapiCharacterImageJson.ResultadoStarships)
			{
				characterImagesStarships.Add(s);
			}
			return characterImagesStarships;
		}
		public async Task<List<StarshipAPI>> GetAllStarshipsAppearFilm(List<StarshipAPI> starships, Film film)
		{
			List<StarshipAPI> listStarshipAPI = new List<StarshipAPI>();
			/*Creamos un objeto de tipo StarshipAPI y recorremos la lista que nos llega
			 * (que es de tipo SwapiStarship(contiene un listado con todas las naves de la api))
			 * Después recorremos filmStarships-> es un listado de string que contiene la url de las naves en el listado de peliculas.
			 * En caso de que las url coincidan, llamamos al método que nos devuelve la información sobre esa nave y retornamos su listado..*/
			foreach (var starshipAPI in starships)
			{
				foreach (string url in film.Starships) //filmStarships->Listado de string que contiene la url de las starships en el listado de peliculas.
				{
					if (starshipAPI.Url == url)
					{
						listStarshipAPI = await _generic.GetInformation<StarshipAPI>(urlStarship, film.Starships);
					}
				}
			}
			return listStarshipAPI;
		}
	}
}
