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
        [MinLength(4 ,ErrorMessage ="Debe tener un mínimo de 4 caracteres")]
		public string Climate { get; set; }
		[DisplayName("Gravedad")]
        
		public int Gravity {  get; set; }
    }
}
