using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StarWars.Models
{
    public class Planet
    {
        [Key]
        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El campo es obligatorio")]
        public string Name { get; set; }
		[DisplayName("Clima")]
		public string Climate { get; set; }
		[DisplayName("Gravedad")]
		public string Gravity {  get; set; }
		[DisplayName("Terreno")]
		public string Terrain {  get; set; }
    }
}
