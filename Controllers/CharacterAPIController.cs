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
	public class CharacterAPIController : Controller
	{
		public const string urlPeople = "https://swapi.py4e.com/api/people/"; 
		private readonly HttpClient _client;
		private readonly GenericController _generic;
		public CharacterAPIController(HttpClient client, GenericController generic)
		{
			_client = client;
			_generic = generic;
		}
		public async Task<List<Character>> GetListAllCharactersAPI()
		{

			List<Character> listCharacters = new List<Character>();
			List<string> listUrlFilms = new List<string>();
			string url = $"https://swapi.py4e.com/api/people/";
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
						url = null;
					}
				}
				else
				{
					break;
				}

			} while (url != null);

			return (listCharacters);
		}
		public async Task<List<Character>> GetAllCharactersAppearFilm(List<Character> listCharacters, Film film)
		{
			List<Character> listAppearCharacter = new List<Character>();
			foreach (var character in listCharacters)
			{
				foreach (var url in film.Characters)
				{
					if (character.Url == url)
					{
						listAppearCharacter = await _generic.GetInformation<Character>(urlPeople, film.Characters);
					}
				}
			}
			return listAppearCharacter;
		}
	}
}
