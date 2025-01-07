using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
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
	public class FilmAPIController : Controller
	{
		private readonly HttpClient _client;
		public const string urlFilm = "https://swapi.py4e.com/api/films/"; 
		public FilmAPIController(HttpClient client)
		{
			_client = client;
		}
		public async Task<List<Film>> GetFilmsCharacterAPI(List<string> url_films)
		{
			string url = $"https://swapi.py4e.com/api/films/";

			var listAllFilms = await GetAllFilmsAPI(url);
			var listFilmsCharacter = AddListFilmsCharacterAPI(listAllFilms, url_films);

			return listFilmsCharacter;
		}

		private async Task<List<Film>> GetAllFilmsAPI(string url)
		{
			List<Film> listAllFilms = new List<Film>();
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

		private List<Film> AddListFilmsCharacterAPI(List<Film> listAllFilms, List<string> url_films)
		{
			List<Film> listFilmsCharacter = new List<Film>();
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

		public List<ResultadoFilms> GetUrlImagesJsonFilms()
		{
			List<ResultadoFilms> charactersImageFilms = new List<ResultadoFilms>();
			SwapiImageJson swapiCharacterImageJson = new SwapiImageJson();
			string urlJson = "wwwroot/archivosJSON/character-image.json";
			string fileJson = System.IO.File.ReadAllText(urlJson);
			swapiCharacterImageJson = JsonConvert.DeserializeObject<SwapiImageJson>(fileJson);

			foreach (var i in swapiCharacterImageJson.ResultadoFilms)
			{
				charactersImageFilms.Add(i);
			}
			return charactersImageFilms;
		}
	}
}
