using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace StarWars.Models
{
	public class Vehicle
	{
		[Key]
		[DisplayName("Nombre")]
		public string Name { get; set; }
		[DisplayName("Modelo")]
		public string Model { get; set; }
		[DisplayName("Clase")]
		public string VehicleClass { get; set; }
		[DisplayName("Fabricacion")]
		public string Manufacturer { get; set; }
    }
}
