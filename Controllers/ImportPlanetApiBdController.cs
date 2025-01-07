using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StarWars.Client;
using StarWars.Data;
using StarWars.Models;

namespace StarWars.Controllers
{
	public class ImportPlanetApiBdController : Controller
	{
		public const string urlPlanet = "https://swapi.py4e.com/api/planets/";
		
		private readonly HttpClient _client;
		private readonly StarWarsContext _context;
		private readonly PlanetAPIController _planetAPIController;
		private readonly PlanetController _planetController;
		private readonly GenericController _generic;

		public ImportPlanetApiBdController(HttpClient client, StarWarsContext context, PlanetAPIController planetAPIController,
			GenericController generic, PlanetController planetController)
		{
			_client = client;
			_context = context;
			_planetAPIController = planetAPIController;
			_generic = generic;
			_planetController = planetController;
		}
		public async Task<IActionResult> Index(List<Planet> listPlanets)
		{
			if (listPlanets.Count > 0)
			{
				return View(listPlanets);
			}
			return View(await _context.Planet.OrderBy(p => p.Name).ToListAsync());
		}
		public IActionResult Notfound()
		{
			return View();
		}
		public async Task<IActionResult> ImportPlanet()
		{
			var getAllPlanetsAPI = await _planetAPIController.GetAllPlanetsAPI(urlPlanet);
			return View(getAllPlanetsAPI);
		}

		public async Task<IActionResult> SearchPlanetByName(string namePlanet)
		{
			var allPlanetsAPI = await _planetAPIController.GetAllPlanetsAPI(urlPlanet);
			var searchEspecificPlanet = allPlanetsAPI.FirstOrDefault(x => x.Name == namePlanet);

			if (searchEspecificPlanet != null)
			{
				var existPlanetInDb = _context.Planet.FirstOrDefault(x=>x.Name == searchEspecificPlanet.Name);
				
				if (existPlanetInDb != null)
				{
					TempData["existPlanetInDb"] = $"El planeta {existPlanetInDb.Name} ya existe en la base de datos.\n" +
						$"Clima : {existPlanetInDb.Climate},\n" +
						$"Gravedad : {existPlanetInDb.Gravity},\n" +
						$"Terreno : {existPlanetInDb.Terrain}";
					return RedirectToAction("Index");
				}
				else
				{
					Planet planet = new Planet();
					planet.Name = searchEspecificPlanet.Name;
					planet.Climate = searchEspecificPlanet.Climate;
					planet.Gravity = searchEspecificPlanet.Gravity;
					planet.Terrain = searchEspecificPlanet.Terrain;
					_context.Planet.Add(planet);
					await _context.SaveChangesAsync();
					return View(planet);
				}
			}
			else
			{
				return RedirectToAction("Notfound");
			}
		}
	}

}
