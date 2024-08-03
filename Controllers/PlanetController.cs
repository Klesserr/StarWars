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
		//GET: Planet/Create
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		//POST: Planet/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Name,Climate,Gravity")] Planet planet)
		/*Bind:Especifica que propiedades de un modelo(en este claso planeta) se deben incluir durante el
		  enlace de datos desde formularios.*/
		{
			if (ModelState.IsValid)
			{
				var p = new Planet //Creamos un nuevo planeta con los datos que nos proporciona el usuario mediante el formulario.
				{
					Name = planet.Name,
					Climate = planet.Climate,
					Gravity = planet.Gravity
				};
				_context.Planet.Add(p); //Añadimos el planeta a nuestra bd
				await _context.SaveChangesAsync(); //Guardamos los cambios.
				return RedirectToAction(nameof(Index));
			}
			return View();

		}

		//POST: Planet/Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(Planet planet)
		{
			if (ModelState.IsValid)
			{
				_context.Planet.Remove(planet);
				await _context.SaveChangesAsync();
				return View();
			}
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		//POST: Planet/Delete/namePlanet?=nombreDelPlaneta
		public async Task<IActionResult> ViewDelete(string namePlanet)
		{
			return View(await _context.Planet.FirstOrDefaultAsync(p => p.Name == namePlanet));
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		//POST: Planet/Edit
		public async Task<IActionResult> Edit(Planet planet)
		{
			if (ModelState.IsValid)
			{
				_context.Planet.Update(planet);
				await _context.SaveChangesAsync();
				return View();
			}
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditView(string namePlanet)
		{
            return View(await _context.Planet.FirstOrDefaultAsync(p => p.Name == namePlanet));
        }
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("Gravedad/MayorDe4",Name ="prueba")]
		public async Task<IActionResult> SeeGravity()
		{
			if (ModelState.IsValid)
			{
				return View(await _context.Planet.Where(p=>p.Gravity >=4).ToListAsync());
			}
			return View();
		}
		
		public async Task<IActionResult> OrdenName()
		{
			var query = await _context.Planet.OrderByDescending(p => p.Name).ToListAsync();

            return View ("Index",query);
			
		}

		public async Task<IActionResult> Index(List<Planet> list)
		{
            if(list.Count > 0)
			{
				return View(await _context.Planet.OrderByDescending(p => p.Name).ToListAsync());
			}
			return View(await _context.Planet.ToListAsync());
		}
		/*public async Task<Planet> Get()
		{
			return await _context.Planet.FirstOrDefaultAsync(p => p.Name == "Tatooine");
		}*/
	}
}
