using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using StarWars.Data;
using StarWars.Models;

namespace StarWars.Controllers
{
	public class PeopleController : Controller
	{
		private readonly StarWarsContext _context;

		public PeopleController(StarWarsContext context)
		{
			_context = context;
		}
		[HttpGet]
		public IActionResult Create()
		{
			var getAllPlanetName = _context.Planet.Select(p => p.Name).ToList();
			ViewBag.PlanetName = getAllPlanetName;

			var getAllStarshipName = _context.Starship.Select(p => p.Name).ToList();
			ViewBag.StarshipName = getAllStarshipName;
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Name,LaserSword,Order,Race,PlanetName,StarshipName")] People people)
		{
			if (ModelState.IsValid)
			{
				_context.People.Add(people);
				await _context.SaveChangesAsync();
				return View("Index");
			}
			return View(people);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(People people)
		{
			if (ModelState.IsValid)
			{
				_context.People.Remove(people);
				await _context.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(People people)
		{

			if (ModelState.IsValid)
			{

				_context.People.Update(people);
				await _context.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ViewDelete(int id)
		{
			return View(await _context.People.FirstOrDefaultAsync(p => p.Id == id));
		}
		public async Task<ActionResult> Index()
		{

			var getAllPlanetName = await _context.Planet.Select(p => p.Name).ToListAsync();
			ViewBag.PlanetName = getAllPlanetName;

			var getAllStarshipName = _context.Starship.Select(p => p.Name).ToList();
			ViewBag.StarshipName = getAllStarshipName;
			return View(await _context.People.ToListAsync());

		}
		public IActionResult Details(People people,Planet planet)
		{
            var prueba = from p in _context.Planet 
						 where people.PlanetName == planet.Name  
						 select new {p.Name,p.Gravity,p.Climate}; 
            ViewBag.InfoPlanet = prueba;
			return View(prueba);
		}
		
	}
}
