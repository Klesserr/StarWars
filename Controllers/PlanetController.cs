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

    }
}
