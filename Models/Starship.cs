using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StarWars.Models
{
    public class Starship
    {
        [Key]
        [DisplayName("Nombre")]
        [Required(ErrorMessage ="Campo primario obligatorio")]
        public string Name { get; set; }
		[DisplayName("Modelo")]
		[MinLength(4)]
        [Required(ErrorMessage ="El modelo de la nave debe introducirse")]
        public string Model { get; set; }
		[DisplayName("Longitud")]
		[Required(ErrorMessage ="No puede dejarse vacío")]
        public double Longitude { get; set; }
		[DisplayName("Pasajeros a bordo")]
		[Required(ErrorMessage = "No puede dejarse vacío")]
		public double MaxPassengers { get; set; }
		[DisplayName("Armamento")]
		[MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
		[Required]
		public string Armament { get; set; }
    }
}
