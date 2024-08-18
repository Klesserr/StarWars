using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using Microsoft.EntityFrameworkCore;
using StarWars.Data;
using StarWars.Models;
using StarWars.ViewModel;

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
				return RedirectToAction("Index");
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
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Details(string namePlanet, string nameStarship,int id)
		{
			if(ModelState.IsValid)
			{
				List<Planet> getPlanetInfo = GetAllInformationPlanet(namePlanet);
				List<Starship> getStarshipInfo = GetAllInformationStarship(nameStarship);
				People p = GetAllInformation(id);

				ThreeInOneViewModel three = new ThreeInOneViewModel();
				three.PlanetList = getPlanetInfo;
				three.StarshipList = getStarshipInfo;

				three.NamePeople = p.Name;
				three.ColorLaser = p.LaserSword;
				three.Race = p.Race;
				three.Order = p.Order;
				return View(three);
			}
			return View();
           
		}

		public List<Starship> GetAllInformationStarship(string nameStarship)
		{
			var queryStarship = from star in _context.Starship
								where star.Name == nameStarship
								select star;
			return queryStarship.ToList();
		}

		public List<Planet> GetAllInformationPlanet(string namePlanet)
		{
			var queryPlanet = from pl in _context.Planet
							  where pl.Name == namePlanet
							  select pl;


			return queryPlanet.ToList();
		}
		public People GetAllInformation(int id)
		{
			return _context.People.FirstOrDefault(p=>p.Id == id);
		}
	}
}
