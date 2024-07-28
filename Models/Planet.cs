using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace StarWars.Models
{
    public class Planet
    {
        [Key]
        public string Name { get; set; }
        public string Climate { get; set; }
        public int Gravity {  get; set; }
    }
}
