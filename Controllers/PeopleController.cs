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
		[Route("CrearPersonaje")]
		public IActionResult CreateOnePeople()
		{
			var getAllPlanetName = _context.Planet.Select(p => p.Name).ToList();
			ViewBag.PlanetName = getAllPlanetName;

			var getAllStarshipName = _context.Starship.Select(p => p.Name).ToList();
			ViewBag.StarshipName = getAllStarshipName;
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("CrearPersonaje")]
		public async Task<IActionResult> CreateOnePeople([Bind("Name,LaserSword,Order,Race,PlanetName,StarshipName")] People people)
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
		
		public async Task<IActionResult> DeleteOnePeople(People people)
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
		[Route("Editar")]
		public async Task<IActionResult> UpdateOnePeople(People people)
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
		[Route("BorrarPersonaje")]

		public async Task<IActionResult> ViewDelete(int id)
		{
			return View(await _context.People.FirstOrDefaultAsync(p => p.Id == id));
		}
		public async Task<ActionResult> Index()
		{
			var getAllPlanetName = await _context.Planet.Select(p => p.Name).ToListAsync();
			ViewBag.PlanetName = getAllPlanetName;

			var getAllStarshipName = await _context.Starship.Select(p => p.Name).ToListAsync();
			ViewBag.StarshipName = getAllStarshipName;

			return View(await _context.People.ToListAsync());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("Información")]
		public IActionResult AllInformationAboutCharacter(string namePlanet, string nameStarship,int id)
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
		public IActionResult SeeAllJedi()
		{
			List<People> getJedi = GetAllJedi();
			return View(getJedi);
		}
		public List<People> GetAllJedi()
		{
			var queryGetJedi = from p in _context.People
						where p.Order == Order.Jedi
						select p;

			return queryGetJedi.ToList();
		}

		public IActionResult SeeAllSith()
		{
			List<People> getJedi = GetAllSith();
			return View(getJedi);
		}
		public List<People> GetAllSith()
		{
			var queryGetSith = from p in _context.People
							   where p.Order == Order.Sith
							   select p;

			return queryGetSith.ToList();
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
