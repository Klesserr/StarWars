using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarWars.Models;

namespace StarWars.Data
{
    public class StarWarsContext : DbContext
    {
        public StarWarsContext (DbContextOptions<StarWarsContext> options)
            : base(options)
        {
        }

        public DbSet<People> People { get; set; } = default!;
        public DbSet<Planet> Planet { get; set; } = default!;
        public DbSet<Starship> Starship { get; set; } = default!;
    }
}
