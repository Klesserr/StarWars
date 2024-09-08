﻿using Microsoft.AspNetCore.Mvc;
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
		//public List<VehiclesImageJson> GetUrlImagesVehiclesJsonCharacters()
		//{
		//	List<VehiclesImageJson> vehiclesImagesJson = new List<VehiclesImageJson>();
		//	string urlJson = "wwwroot/archivosJSON/character-image.json";
		//	string fileJson = System.IO.File.ReadAllText(urlJson);
		//	SwapiImageJson swapiImage = new SwapiImageJson();
		//	swapiImage = JsonConvert.DeserializeObject<SwapiImageJson>(fileJson);
		//	foreach(var i in swapiImage.ResultadoVehicles)
		//	{
		//		vehiclesImagesJson.Add(i);
		//	}
		//	return vehiclesImagesJson;
		//}
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
			foreach (var s in urlList)
			{
				var query = genericList.FirstOrDefault(p => p.Url == s);
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
			SwapiViewModel swapiVM = new SwapiViewModel();
			characterImageJsons = GetUrlImagesJsonCharacters();
			swapiVM.ListCharacterImageJson = characterImageJsons;

			foreach (var v in query)
			{
				var getPlanet = await GetPlanetAPI(v.HomeWorld);
				swapiVM.NameCharacter = v.Name;
				swapiVM.GenderCharacter = v.Gender;
				swapiVM.HeightCharacter = v.Height;
				swapiVM.MassCharacter = v.Mass;
				swapiVM.SkinColorCharacter = v.Skin_Color;
				swapiVM.HairColorCharacter = v.Hair_Color;
				//
				swapiVM.NamePlanet = getPlanet.Name;
				swapiVM.TerrainPlanet = getPlanet.Terrain;
				swapiVM.ClimatePlanet = getPlanet.Climate;
				swapiVM.GravityPlanet = getPlanet.Gravity;
				//
				swapiVM.ListStarship = await GetInformation<StarshipAPI>(urlStarship, v.Starships);
				swapiVM.ListFilms = await GetInformation<Film>(urlFilm, v.Films);
				swapiVM.ListVehicle = await GetInformation<Vehicle>(urlVehicle, v.Vehicles);
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
	}
}
