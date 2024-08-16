	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Rendering;
	using Microsoft.EntityFrameworkCore;
	using StarWars.Data;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using StarWars.Controllers;


	namespace StarWars.Models
	{
		public class People
		{
		
			[Key]
			public int Id { get; set; }
		
			public string Name { get; set; }
			[Column(TypeName = "nvarchar(24)")]
			public Laser LaserSword { get; set; }
			[Column(TypeName = "nvarchar(24)")]
			public Order? Order { get; set; }
			[Column(TypeName= "nvarchar(24)")]
			public Race Race { get; set; }
		
			public string PlanetName { get; set; }
			public string StarshipName { get; set; }
		
		}
		public enum Laser
		{
			Rojo,Azul,Verde,Amarillo,Morado,Naranja,Cyan
		}
		public enum Race
		{ 
			Human,Ewok, Zabrak, Gungans, Wookie, GenDai, Hutt, Imperial, Voxyn, Kaleesh
		}
		public enum Order
		{
			Jedi, Sith
		}
	
	}
