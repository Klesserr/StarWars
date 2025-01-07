using Microsoft.AspNetCore.Mvc;
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
	public class PlanetAPIController : Controller
	{
		public const string urlPlanet = "https://swapi.py4e.com/api/planets/";
		private readonly HttpClient _client;
		private readonly GenericController _generic;
		public PlanetAPIController(HttpClient client, GenericController generic)
		{
			_client = client;
			_generic = generic;
		}

		public async Task<List<PlanetAPI>> GetAllPlanetsAPI(string urlPlanet)
		{
			List<PlanetAPI> listPlanetAPI = new List<PlanetAPI>();
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			do
			{
				var httpResponse = await _client.GetAsync(urlPlanet);
				if (httpResponse.IsSuccessStatusCode)
				{
					var contestResponse = await httpResponse.Content.ReadAsStringAsync();
					SwapiPlanetAPI swapiPlanetAPI = new SwapiPlanetAPI();
					swapiPlanetAPI = JsonConvert.DeserializeObject<SwapiPlanetAPI>(contestResponse);
					foreach (var p in swapiPlanetAPI.Results)
					{
						listPlanetAPI.Add(p);
					}
					string urlNext = swapiPlanetAPI.Next;
					if (urlNext != null)
					{
						urlPlanet = urlNext;
					}
					else
					{
						urlPlanet = null;
					}
				}
			} while (urlPlanet != null);
			return listPlanetAPI;
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
		public List<ResultadoPlanets> GetUrlImagesJsonPlanets()
		{
			List<ResultadoPlanets> characterImagesPlanets = new List<ResultadoPlanets>();
			SwapiImageJson swapiCharacterImageJson = new SwapiImageJson();
			string urlJson = "wwwroot/archivosJSON/character-image.json";
			string fileJson = System.IO.File.ReadAllText(urlJson);
			swapiCharacterImageJson = JsonConvert.DeserializeObject<SwapiImageJson>(fileJson);
			foreach (var p in swapiCharacterImageJson.ResultadoPlanets)
			{
				characterImagesPlanets.Add(p);
			}
			return characterImagesPlanets;
		}
		public async Task<List<PlanetAPI>> GetAllPlanetsAppearFilm(List<PlanetAPI> listPlanetAPI, Film film)
		{
			List<PlanetAPI> listAppearPlanets = new List<PlanetAPI>();
			foreach (var planet in listPlanetAPI)
			{
				foreach (string url in film.Planets)
				{
					if (planet.Url == url)
					{
						listAppearPlanets = await _generic.GetInformation<PlanetAPI>(urlPlanet, film.Planets);
					}
				}
			}
			return listAppearPlanets;
		}
	}
}
