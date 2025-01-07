using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StarWars.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StarWars.Controllers;
using System.ComponentModel;


namespace StarWars.Models
{
	public class People
	{

		[Key]
		[DisplayName("Nombre")]
		[Required(ErrorMessage = "Campo obligatorio.")]
		[MinLength(4, ErrorMessage = "Debe introducir mínimo 4 caracteres")]
		public string Name { get; set; }
		[DisplayName("Color Sable")]
		[Column(TypeName = "nvarchar(24)")]
		[Required(ErrorMessage = "Elige un color")]
		public Laser LaserSword { get; set; }
		[DisplayName("Orden")]
		[Column(TypeName = "nvarchar(24)")]
		[Required(ErrorMessage = "Debes pertenecer a una orden")]
		public Order? Order { get; set; }
		[DisplayName("Raza")]
		[Column(TypeName = "nvarchar(24)")]
		[Required(ErrorMessage = "Debes pertenecer a una raza")]
		public Race Race { get; set; }
		[DisplayName("Planeta")]
		[Required(ErrorMessage = "El planeta es obligatorio")]
		public string PlanetName { get; set; }
	
		[DisplayName("Género")]
		public string Gender { get; set; }
		[DisplayName("Peso")]
		public string Height { get; set; }
		[DisplayName("Masa")]
		public string Mass { get; set; }
		[DisplayName("Color Pelo")]
		public string Hair_Color { get; set; }
		[DisplayName("Color Piel")]
		public string Skin_Color { get; set; }
		[DisplayName("Hogar")]
		public string HomeWorld { get; set; }
		[DisplayName("Naves usadas")]
		public List<string> StarshipsList { get; set; } = new List<string>();
	}
	public enum Laser
	{
		Rojo, Azul, Verde, Amarillo, Morado, Naranja, Cyan, SinLaser
	}
	public enum Race
	{
		Human, Ewok, Zabrak, Gungans, Wookie, GenDai, Hutt, Imperial, Voxyn, Kaleesh
	}
	public enum Order
	{
		Jedi, Sith
	}

}
