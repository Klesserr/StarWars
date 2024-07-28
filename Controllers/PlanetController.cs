using System;
using System.Collections.Generic;
using System.Linq;
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
		public IActionResult Create([Bind("Name,Climate,Gravity")] Planet planet)
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
			    _context.SaveChanges(); //Guardamos los cambios.
				return RedirectToAction(nameof(Index));
			}
			return View();

		}
		public IActionResult Index()
		{
			return View(_context.Planet.ToList());
		}
		public async Task<Planet> Get()
		{
			return await _context.Planet.FirstOrDefaultAsync(p => p.Name == "Tatooine");
		}
	}
}
