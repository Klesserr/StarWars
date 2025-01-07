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
	public class VehicleAPIController : Controller
	{
		public const string urlVehicle = "https://swapi.py4e.com/api/vehicles/";
		private readonly HttpClient _client;
		private readonly GenericController _generic;
		public VehicleAPIController(HttpClient client,GenericController generic)
		{
			_client= client;
			_generic = generic;		
		}
		public async Task<List<Vehicle>> GetVehiclesCharacterAPI(List<string> url_vehicles)
		{
			List<Vehicle> ListVehicles = new List<Vehicle>();
			List<Vehicle> ListAllVehiclesAPI = new List<Vehicle>();
			string url = "https://swapi.py4e.com/api/vehicles/";

			ListAllVehiclesAPI = await GetAllVehiclesAPI(url);
			ListVehicles = AddVehicleHasCharacter(url_vehicles, ListAllVehiclesAPI);

			return ListVehicles;
		}
		public async Task<List<Vehicle>> GetAllVehiclesAPI(string url)
		{
			List<Vehicle> listAllVehicles = new List<Vehicle>();
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
		private List<Vehicle> AddVehicleHasCharacter(List<string> url_vehicles, List<Vehicle> listAllVehicles)
		{
			List<Vehicle> listVehicles  = new List<Vehicle>();
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
		public List<ResultadoVehicles> GetUrlImagesJsonVehicles()
		{
			List<ResultadoVehicles> characterImagesVehicles = new List<ResultadoVehicles>();
			SwapiImageJson swapiVehiclesImageJson = new SwapiImageJson();
			string urlJson = "wwwroot/archivosJSON/character-image.json";
			string fileJson = System.IO.File.ReadAllText(urlJson);
			swapiVehiclesImageJson = JsonConvert.DeserializeObject<SwapiImageJson>(fileJson);

			foreach (var v in swapiVehiclesImageJson.ResultadoVehicles)
			{
				characterImagesVehicles.Add(v);
			}
			return characterImagesVehicles;
		}
		public async Task<List<Vehicle>> GetAllVehiclesAppearFilm(List<Vehicle> listVehicle, Film film)
		{
			List<Vehicle> listAppearVehicles = new List<Vehicle>();
			foreach (var vehicle in listVehicle)
			{
				foreach (string url in film.Vehicles)
				{
					if (vehicle.Url == url)
					{
						listAppearVehicles = await _generic.GetInformation<Vehicle>(urlVehicle, film.Vehicles);
					}
				}
			}
			return listAppearVehicles;
		}
	}
}
