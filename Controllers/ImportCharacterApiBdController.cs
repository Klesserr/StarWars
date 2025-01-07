using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using StarWars.Data;
using StarWars.Models;

namespace StarWars.Controllers
{
	public class ImportCharacterApiBdController : Controller
	{
		public const string urlCharacters = "";
		public const string urlStarship = "https://swapi.py4e.com/api/starships/1"; 
		private readonly HttpClient _client;
		private readonly PeopleController _peopleController;
		private readonly StarWarsContext _context;
		private readonly CharacterAPIController _characterAPIController;
		private readonly StarshipAPIController _starshipAPIController;
		private readonly PlanetAPIController _planetAPIController;
		private readonly PlanetController _planetController;
		private readonly StarshipController _starshipController;
		public ImportCharacterApiBdController(HttpClient client, PeopleController peopleController,
			StarWarsContext context, CharacterAPIController characterAPIController, StarshipAPIController starshipAPIController,
			PlanetAPIController planetAPIController)
		{
			_client = client;
			_peopleController = peopleController;
			_context = context;
			_characterAPIController = characterAPIController;
			_starshipAPIController = starshipAPIController;
			_planetAPIController = planetAPIController;
		}
		public IActionResult Notfound()
		{
			return View();
		}
		public async Task<IActionResult> MainPage()
		{
			var getAllPlanetName = await _context.Planet.Select(p => p.Name).ToListAsync();
			ViewBag.PlanetName = getAllPlanetName;

			var getAllStarshipName = await _context.Starship.Select(p => p.Name).ToListAsync();
			ViewBag.StarshipName = getAllStarshipName;

			return View(await _context.People.ToListAsync());
		}
		public async Task<IActionResult> ImportCharacter()
		{
			var getAllCharactersApi = await _characterAPIController.GetListAllCharactersAPI();
			ViewBag.ListAllCharactersApi = getAllCharactersApi;
			return View();

		}
		public async Task<IActionResult> SearchCharacterByName(string nameCharacter, Laser laserSword, Race race, Order order)
		{
			var getAllCharactersApi = await _characterAPIController.GetListAllCharactersAPI();
			var especificCharacter = getAllCharactersApi.FirstOrDefault(x => x.Name == nameCharacter);
			ViewBag.ListAllCharactersApi = getAllCharactersApi;

			if (especificCharacter != null)
			{
				var existCharacterInDb = _context.People.FirstOrDefault(x => x.Name == especificCharacter.Name);
				if (existCharacterInDb != null)
				{
					TempData["existCharacterInDb"] = $"El personaje {existCharacterInDb.Name} ya existe en la bd.";
					return RedirectToAction("MainPage");
				}
				else
				{
					People people = new People();
					people.Name = especificCharacter.Name;
					people.Order = order;
					people.Race = race;
					people.LaserSword = laserSword;
					people.Hair_Color = especificCharacter.Hair_Color;
					people.Skin_Color = especificCharacter.Skin_Color;
					people.Mass = especificCharacter.Mass;
					people.Height = especificCharacter.Height;
					people.Gender = especificCharacter.Gender;
					people.PlanetName = await GetNamePlanet(especificCharacter.HomeWorld);
					people.StarshipsList = await GetStarshipsCharacter(especificCharacter.Starships);
					_context.People.Add(people);
					await _context.SaveChangesAsync();
					return View(people);
				}
			}
			else
			{
				return RedirectToAction("Notfound");
			}
		}

		public async Task<string> GetNamePlanet(string url)
		{
			var namePlanet = await _planetAPIController.GetPlanetAPI(url);
			return namePlanet.Name;
		}
		public async Task<List<string>> GetStarshipsCharacter(List<string> urlStarshipCharacter)
		{
			List<string> getListStarship = new List<string>();
			var getAllStarshipApi = await _starshipAPIController.GetStarshipsCharacterAPI(urlStarshipCharacter);	
			foreach(var starship in getAllStarshipApi)
			{
				getListStarship.Add(starship.Name);
			}
			return getListStarship;
		}
	}
}
