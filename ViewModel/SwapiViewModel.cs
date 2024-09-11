using StarWars.Client;

namespace StarWars.ViewModel
{
	public class SwapiViewModel
	{
		public string NameCharacter { get; set; }
		public string GenderCharacter { get; set; }
		public string HeightCharacter {  get; set; }
		public string MassCharacter { get; set; }
		public string SkinColorCharacter {  get; set; }
		public string HairColorCharacter { get; set; }
		public string HomeWorld {  get; set; }
		
		//
		public string NamePlanet { get; set; }
		public string TerrainPlanet { get; set; }
		public string GravityPlanet {  get; set; }
		public string ClimatePlanet {  get; set; }
		//
		public string TitleFilm { get; set; }
		public string OpeningFilm {  get; set; }
		public string Director {  get; set; }
		public int EpisodeFilm {  get; set; }
		//
		public string NameStarship {  get; set; }
		public string ModelStarship { get; set; }
		public string StarshipClass {  get; set; }
		public string ManufacturerStarship { get; set; }
		//
		public string NameVehicle {  get; set; }
		public string ModelVehicle { get; set; }
		public string ManufacturerVehicle {  get; set; }
		public string VehicleClass {  get; set; }
		//
		public List<Character> ListCharacter {  get; set; }
		public List<StarshipAPI> ListStarship { get; set; }
		public List<Film> ListFilms { get; set; }
		public List<Vehicle> ListVehicle {  get; set; }
		public List<PlanetAPI> ListPlanetAPI {  get; set; }
		//
		public List<CharacterImageJson> ListCharacterImageJson { get; set; }
		public string NameJson { get; set; }
		public string ImageUrl { get; set; }
		
	}
}
