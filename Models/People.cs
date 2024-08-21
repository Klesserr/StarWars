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
		public int Id { get; set; }
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
		[DisplayName("Nave")]
		[Required(ErrorMessage = "La nave es obligatoria")]
		public string StarshipName { get; set; }

	}
	public enum Laser
	{
		Rojo, Azul, Verde, Amarillo, Morado, Naranja, Cyan
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
