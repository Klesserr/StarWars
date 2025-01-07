using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarWars.Data;
using StarWars.Models;
namespace StarWars.Controllers
{
	public class ImportVehicleApiBdController : Controller
	{
		private readonly HttpClient _client;
		private readonly VehicleAPIController _vehicleAPIController;
		private readonly GenericController _genericController;
		private readonly StarWarsContext _context;
		public const string urlVehicle = "https://swapi.py4e.com/api/vehicles/";
		public ImportVehicleApiBdController(HttpClient client, VehicleAPIController vehicleAPIController, GenericController genericController,
			StarWarsContext context)
		{
			_client = client;
			_vehicleAPIController = vehicleAPIController;
			_genericController = genericController;
			_context = context;
		}
		public IActionResult Notfound()
		{
			return View();
		}
		
		public async Task<IActionResult> MainPage()
		{
			return View(await _context.Vehicle.ToListAsync());
		}
		public async Task<IActionResult> ImportVehicle()
		{
			var getAllVehiclesApi = await _vehicleAPIController.GetAllVehiclesAPI(urlVehicle);
			return View(getAllVehiclesApi);
		}

		public IActionResult SeeUpdateVehicle(string nameVehicle)
		{
			var getVehicle = _context.Vehicle.FirstOrDefault(x => x.Name == nameVehicle);
			return View(getVehicle);
		}

		public async Task<IActionResult> UpdateVehicle(Vehicle vehicle)
		{
			if (ModelState.IsValid)
			{
				_context.Vehicle.Update(vehicle);
				await _context.SaveChangesAsync();
				return RedirectToAction("MainPage");
			}
			else
			{
				return RedirectToAction("MainPage");
			}
		}
		public IActionResult SeeDeleteVehicle(string nameVehicle)
		{
			var getVehicle = _context.Vehicle.FirstOrDefault(x => x.Name == nameVehicle);
			return View(getVehicle);
		}

		public async Task<IActionResult> DeleteOneVehicle(string nameVehicle)
		{
			var getVehicle = _context.Vehicle.FirstOrDefault(x => x.Name == nameVehicle);
			if (ModelState.IsValid)
			{
				_context.Vehicle.Remove(getVehicle);
				await _context.SaveChangesAsync();
				return RedirectToAction("MainPage");
			}
			else
			{
				return RedirectToAction("MainPage");
			}
		}

		public async Task<IActionResult> SearchVehicleByName(string nameVehicle)
		{
			var getAllVehiclesApi = await _vehicleAPIController.GetAllVehiclesAPI(urlVehicle);
			var getEspecificVehicle = getAllVehiclesApi.FirstOrDefault(x => x.Name == nameVehicle);

			if (getEspecificVehicle != null)
			{
				var existVehicleInDb = _context.Vehicle.FirstOrDefault(x => x.Name == getEspecificVehicle.Name);
				if (existVehicleInDb != null) //Si existVehicle es null, creamos un nuevo objeto y le agregamos las propiedades.
				{
					TempData["existVehicleInDb"] = $"El vehículo {existVehicleInDb.Name} ya existe en la base de datos.\n" +
						$"Modelo: {existVehicleInDb.Model}\n." +
						$"Clase: {existVehicleInDb.VehicleClass}\n." +
						$"Fabricación: {existVehicleInDb.Manufacturer}\n.";
					return RedirectToAction("MainPage");
				}
				else
				{
					Vehicle vehicle = new Vehicle();
					vehicle.Name = getEspecificVehicle.Name;
					vehicle.Model = getEspecificVehicle.Model;
					vehicle.Manufacturer = getEspecificVehicle.Manufacturer;
					vehicle.VehicleClass = getEspecificVehicle.Vehicle_Class;

					_context.Vehicle.Add(vehicle);
					await _context.SaveChangesAsync();
					return View(vehicle);
				}
			}
			else
			{
				return RedirectToAction("Notfound");
			}
		}
	}
}
