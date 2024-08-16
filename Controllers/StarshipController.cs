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
    public class StarshipController : Controller
    {
        private readonly StarWarsContext _context;

        public StarshipController(StarWarsContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create([Bind("Name,Model,Longitude,MaxPassengers,Armament")] Starship starship)
        {
            if (ModelState.IsValid)
            {
                var stship = new Starship
                {
                    Name = starship.Name,
                    Model = starship.Model,
                    Longitude = starship.Longitude,
                    MaxPassengers = starship.MaxPassengers,
                    Armament = starship.Armament
                };
                _context.Starship.Add(stship);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(Starship starship)
        {
            if (ModelState.IsValid)
            {
                _context.Starship.Remove(starship);
                await _context.SaveChangesAsync();
				return RedirectToAction("Index");
			}
            return View();
		}
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> ViewDelete(string name)
        {
            return View(await _context.Starship.FirstOrDefaultAsync(p=>p.Name == name));
        }
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Starship starship)
		{
			if (ModelState.IsValid)
			{
				_context.Starship.Update(starship);
				await _context.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View();
		}
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewEdit(string name)
        {
            return View(await _context.Starship.FirstOrDefaultAsync(p => p.Name == name));
        }
        
        public async Task<IActionResult> Falcon()
        {
            var query = (await _context.Starship.OrderByDescending(p=>p.Model).ToListAsync());
            return View("Index",query);
        }

		public async Task<IActionResult> Index(List<Starship> list)
        {
            if (list.Count > 0)
            {
				return View(list);
			}
            return View(await _context.Starship.ToListAsync());
        }

    }
}
