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
		private readonly InitialController _initialController;
		private readonly FilmAPIController _filmAPIController;
		private readonly StarshipAPIController _starshipAPIController;
		private readonly VehicleAPIController _vehicleAPIController;
		private readonly PlanetAPIController _planetAPIController;
		private readonly CharacterAPIController _characterAPIController;
		private readonly GenericController _genericController;

		public SwapiController(HttpClient client, InitialController initialController, FilmAPIController filmAPIController, StarshipAPIController starshipAPIController,
			VehicleAPIController vehicleAPIController, PlanetAPIController planetAPIController, CharacterAPIController characterAPIController, GenericController genericController)
		{
			_client = client;
			_initialController = initialController;
			_filmAPIController = filmAPIController;
			_starshipAPIController = starshipAPIController;
			_vehicleAPIController = vehicleAPIController;
			_planetAPIController = planetAPIController;
			_characterAPIController = characterAPIController;
			_genericController = genericController;
		}
		public const string urlPeople = "https://swapi.py4e.com/api/people/";
		public const string urlVehicle = "https://swapi.py4e.com/api/vehicles/";
		public const string urlStarship = "https://swapi.py4e.com/api/starships/";
		public const string urlPlanet = "https://swapi.py4e.com/api/planets/";
		public const string urlFilm = "https://swapi.py4e.com/api/films/";

		public ActionResult InitialPage()
		{
			return _initialController.InitialPage();
		}

		public async Task<ActionResult> GetGenericView(string name, string image)
		{

			SwapiCharacter swapi = new SwapiCharacter();
			SwapiViewModel swapiVM = new SwapiViewModel();
			swapi.Results = await _characterAPIController.GetListAllCharactersAPI();
			var uniqueCharacter = swapi.Results.FirstOrDefault(x => x.Name == name);

			swapiVM.ListCharacterImageJson = _initialController.GetUrlImagesJsonCharacters();
			swapiVM.ListImageFilms = _filmAPIController.GetUrlImagesJsonFilms();
			swapiVM.ListImageStarships = _starshipAPIController.GetUrlImagesJsonStarships();
			swapiVM.ListImagePlanets = _planetAPIController.GetUrlImagesJsonPlanets();
			swapiVM.ListImageVehicles = _vehicleAPIController.GetUrlImagesJsonVehicles();
			swapiVM.ListCharacter = swapi.Results;

			var getPlanet = await _planetAPIController.GetPlanetAPI(uniqueCharacter.HomeWorld);
			

			swapiVM.NameCharacter = uniqueCharacter.Name;
			swapiVM.GenderCharacter = uniqueCharacter.Gender;
			swapiVM.HeightCharacter = uniqueCharacter.Height;
			swapiVM.MassCharacter = uniqueCharacter.Mass;
			swapiVM.SkinColorCharacter = uniqueCharacter.Skin_Color;
			swapiVM.HairColorCharacter = uniqueCharacter.Hair_Color;
			//
			swapiVM.NamePlanet = getPlanet.Name;
			swapiVM.TerrainPlanet = getPlanet.Terrain;
			swapiVM.ClimatePlanet = getPlanet.Climate;
			swapiVM.GravityPlanet = getPlanet.Gravity;
			//
			swapiVM.ListStarship = await _genericController.GetInformation<StarshipAPI>(urlStarship, uniqueCharacter.Starships);
			swapiVM.ListFilms = await _genericController.GetInformation<Film>(urlFilm, uniqueCharacter.Films);
			swapiVM.ListVehicle = await _genericController.GetInformation<Vehicle>(urlVehicle, uniqueCharacter.Vehicles);
			//
			swapiVM.NameJson = name;
			swapiVM.ImageUrl = image;
			//
			return View(swapiVM);
		}


		public async Task<ActionResult> GetTheFilm(string title, string name)
		{
			SwapiCharacter swapiCharacter = new SwapiCharacter();
			swapiCharacter.Results = await _characterAPIController.GetListAllCharactersAPI();
			var character = swapiCharacter.Results.FirstOrDefault(x => x.Name == name);
			SwapiViewModel swapiVM = new SwapiViewModel();

			SwapiStarship swapiStarship = new SwapiStarship();
			swapiStarship.Results = await _starshipAPIController.GetListAllStarshipAPI(urlStarship);

			SwapiVehicle swapiVehicle = new SwapiVehicle();
			swapiVehicle.Results = await _vehicleAPIController.GetAllVehiclesAPI(urlVehicle);

			SwapiPlanetAPI swapiPlanetAPI = new SwapiPlanetAPI();
			swapiPlanetAPI.Results = await _planetAPIController.GetAllPlanetsAPI(urlPlanet);

			swapiVM.ListFilms = await _genericController.GetInformation<Film>(urlFilm, character.Films);

			foreach (var film in swapiVM.ListFilms)
			{
				if (film.Title == title && character.Name == name)
				{
					swapiVM.TitleFilm = film.Title;
					swapiVM.EpisodeFilm = film.Episode_Id;
					swapiVM.OpeningFilm = film.Opening_Crawl;
					swapiVM.Director = film.Director;
					//Obtenemos el listado de naves que aparecen en la pelicula:(objeto SwapiStarship,lista<StarashipAPI>,objeto Film)
					swapiVM.ListStarship = await _starshipAPIController.GetAllStarshipsAppearFilm(swapiStarship.Results, film);
					//Obtenemos el listado de vehiculos que aparecen en la pelicula:(objeto SwapiVehicle,lista<Vehicle>,objeto Film)
					swapiVM.ListVehicle = await _vehicleAPIController.GetAllVehiclesAppearFilm(swapiVehicle.Results,film);
					//Obtenemos el listado de planetas que aparecen en la pelicula:(objeto SwapiPlanetAPI,lista<PlanetAPI>,objeto Film)
					swapiVM.ListPlanetAPI = await _planetAPIController.GetAllPlanetsAppearFilm(swapiPlanetAPI.Results, film);
					//Obtenemos el listado de personajes que aparecen en la pelicula:(objeto SwapiCharacter,lista<Character>,objeto Film)
					swapiVM.ListCharacter = await _characterAPIController.GetAllCharactersAppearFilm(swapiCharacter.Results, film);
				}
			}
			return View(swapiVM);
		}
	}
}
