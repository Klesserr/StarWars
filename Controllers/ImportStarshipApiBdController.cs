using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarWars.Data;
using StarWars.Models;

namespace StarWars.Controllers
{
	public class ImportStarshipApiBdController : Controller
	{
		public const string urlStarship = "https://swapi.py4e.com/api/starships/";
		private readonly HttpClient _client;
		private readonly StarshipAPIController _starshipApiController;
		private readonly StarshipController _starship;
		private readonly StarWarsContext _context;

		public ImportStarshipApiBdController(HttpClient client, StarWarsContext context, 
			StarshipAPIController starshipAPIController, StarshipController starship)
		{
			_client = client;
			_context = context;
			_starshipApiController = starshipAPIController;
			_starship = starship;
		}
		public async Task<ActionResult> ImportStarship()
		{
			var getAllStarships = await _starshipApiController.GetListAllStarshipAPI(urlStarship);
			return View(getAllStarships);
		}
		public IActionResult Notfound()
		{
			return View();
		}
		public async Task<IActionResult> Index(List<Starship> list)
		{
			if (list.Count > 0)
			{
				return View(list);
			}
			return View(await _context.Starship.ToListAsync());
		}
		public async Task<ActionResult> SearchStarshipByName(string nameStarship, double longitudeStarship,
			double passangers)
		{
			var getAllStarships = await _starshipApiController.GetListAllStarshipAPI(urlStarship);
			var especificStarship = getAllStarships.FirstOrDefault(s => s.Name == nameStarship);

			
			if (especificStarship != null)
			{
				var existStarshipInDb = _context.Starship.FirstOrDefault(s => s.Name == especificStarship.Name);
				if (existStarshipInDb != null)
				{
					TempData["ExistingStarship"] = $"La nave {existStarshipInDb.Name} ya está existe.\n" +
						$"Longitud: {existStarshipInDb.Longitude},\n " +
						$"Pasajeros: {existStarshipInDb.MaxPassengers},\n" +
						$"Modelo: {existStarshipInDb.Model} , \n" +
						$"Clase: {existStarshipInDb.Class_Starship}.\n";

					return RedirectToAction("Index");
                }
				else
				{
					Starship starship = new Starship();
					starship.Name = especificStarship.Name;
                    starship.Model = especificStarship.Model;
                    starship.Class_Starship = especificStarship.Starship_Class;
                    starship.Longitude = longitudeStarship;
                    starship.MaxPassengers = passangers;
                    _context.Starship.Add(starship);
					await _context.SaveChangesAsync();
					return View(starship);
				}
			}
			else
			{
				return RedirectToAction("Notfound");
			}
		}
	}
}
