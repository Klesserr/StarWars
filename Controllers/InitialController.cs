using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarWars.Client;
using StarWars.ViewModel;

namespace StarWars.Controllers
{
	public class InitialController : Controller
	{
		public ActionResult InitialPage()
		{
			SwapiViewModel swapiVM = new SwapiViewModel();
			swapiVM.ListCharacterImageJson = GetUrlImagesJsonCharacters();
			return View(swapiVM);
		}
		public List<CharacterImageJson> GetUrlImagesJsonCharacters()
		{
			List<CharacterImageJson> charactersImageJson = new List<CharacterImageJson>();

			SwapiImageJson swapiCharacterImageJson = new SwapiImageJson();
			string urlJson = "wwwroot/archivosJSON/character-image.json";
			string fileJson = System.IO.File.ReadAllText(urlJson);
			swapiCharacterImageJson = JsonConvert.DeserializeObject<SwapiImageJson>(fileJson);

			foreach (var i in swapiCharacterImageJson.Resultado)
			{
				charactersImageJson.Add(i);
			}
			return charactersImageJson;
		}
	}
}
