using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StarWars.Client;
using System.Net.Http.Headers;

namespace StarWars.Controllers
{
	public class GenericController : Controller
	{
		private readonly HttpClient _client;
		public GenericController (HttpClient client)
		{
			_client = client;
		}

		public async Task<List<T>> GetInformation<T>(string url, List<string> urlList) where T : ISwapiHasUrl //(DONDE el generico implemente la interfaz ISwapiHasUrl)
			/* where T:ISwapiHasUrl -> nos dice que el tipo T(Genérico) debe implementar esa interfaz.
				Además ahora sabemos que cualquier tipo T tiene la propiedad Url que es la que esta definida en dicha interfaz, Ya que 
				cualquier objeto de tipo T tendrá dicha propiedad.*/
		{
			List<T> result = new List<T>();
			List<T> getAllInformationAPI = new List<T>();
			if (urlList.IsNullOrEmpty())
			{
				return result;
			}
			getAllInformationAPI = await GenericInfo<T>(url);
			result = AddEspecificInfo<T>(urlList, getAllInformationAPI);

			return result;
		}
		public async Task<List<T>> GenericInfo<T>(string url)
		{
			List<T> listGeneric = new List<T>();
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			do
			{
				var httpResponse = await _client.GetAsync(url);
				if (httpResponse.IsSuccessStatusCode)
				{
					var contestRespone = await httpResponse.Content.ReadAsStringAsync();
					SwapiGeneric<T> swapi = JsonConvert.DeserializeObject<SwapiGeneric<T>>(contestRespone);
					foreach (var item in swapi.Results)
					{
						listGeneric.Add(item);
					}
					string urlNext = swapi.Next;
					if (urlNext != null)
					{
						url = urlNext;
					}
					else
					{
						url = null;
					}
				}
			} while (url != null);
			return listGeneric;
		}
		public List<T> AddEspecificInfo<T>(List<string> urlList, List<T> genericList) where T : ISwapiHasUrl
		{
			List<T> listEspecificsUrl = new List<T>();
			foreach (var url in urlList)
			{
				var especificUrl = genericList.FirstOrDefault(p => p.Url == url);
				if (especificUrl != null)
				{
					listEspecificsUrl.Add(especificUrl);
				}
				else
				{
					continue;
				}
			}
			return listEspecificsUrl;
		}
		
	}
}
