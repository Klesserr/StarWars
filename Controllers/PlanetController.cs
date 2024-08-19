using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StarWars.Data;
using StarWars.Models;


namespace StarWars.Controllers
{
	public class PlanetController : Controller
	{
		private readonly StarWarsContext _context;

		public PlanetController(StarWarsContext context)
		{
			_context = context;
		}
		//GET: Planet/CrearPlaneta
		[HttpGet]
		[Route("CrearPlaneta")]
		public IActionResult CreateANewPlanet()
		{
			return View();
		}

		//POST: Planet/CrearPlaneta
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("CrearPlaneta")]
		public async Task<IActionResult> CreateANewPlanet([Bind("Name,Climate,Gravity")] Planet planet)
		/*Bind:Especifica que propiedades de un modelo(en este claso planeta) se deben incluir durante el
		  enlace de datos desde formularios.*/
		{
			if (ModelState.IsValid)
			{ 
				_context.Planet.Add(planet); //Añadimos el planeta a nuestra bd
				await _context.SaveChangesAsync(); //Guardamos los cambios.
				return RedirectToAction(nameof(Index));
			}
			return View();

		}

		//POST: Planet/Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteOnePlanet(Planet planet)
		{
			if (ModelState.IsValid)
			{
				_context.Planet.Remove(planet);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View("Index");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("Planet/Borrar",Name ="namePlanet")]
		//POST: /Borrar?namePlanet="nombreDelPlaneta"
		public async Task<IActionResult> ViewDelete(string namePlanet)
		{
			return View(await _context.Planet.FirstOrDefaultAsync(p => p.Name == namePlanet));
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		
		//POST: Planet/Actualizar
		public async Task<IActionResult> UpdateOnePlanet(Planet planet)
		{
			if (ModelState.IsValid)
			{
				_context.Planet.Update(planet);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Route("Planet/Actualizar")]
		public async Task<IActionResult> EditView(string namePlanet)
		{
            return View(await _context.Planet.FirstOrDefaultAsync(p => p.Name == namePlanet));
        }
		[Route("Planet/OrdenDescendente")]
		public async Task<IActionResult> OrderNameDescending()
		{
			var query = await _context.Planet.OrderByDescending(p => p.Name).ToListAsync();

            return View ("Index",query);
			
		}
		[Route("Planet/Clima")]
		public async Task<IActionResult> ClimateView(string nameClimate) 
		{
			return View(await _context.Planet.Where(p => p.Climate == nameClimate).ToListAsync());
		}

		public async Task<IActionResult> Index(List<Planet> listPlanets)
		{
			if (listPlanets.Count > 0)
			{
				return View(listPlanets);
			}
			return View(await _context.Planet.OrderBy(p=>p.Name).ToListAsync());
		}

		/*public async Task<Planet> Get()
		{
			return await _context.Planet.FirstOrDefaultAsync(p => p.Name == "Tatooine");
		}*/
	}
}
