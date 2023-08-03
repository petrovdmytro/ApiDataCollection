using ApiDataCollection.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Text;

namespace ApiDataCollection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyApiController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        public MyApiController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet("params")]
        public async Task<IActionResult> GetParamsAsync([FromQuery] string name = null, [FromQuery] int? limit = null, [FromQuery] string nameSorting = null, [FromQuery] string param4 = null)
        {

            // Send a GET request to the public API
            var client = _clientFactory.CreateClient();

            string filter = "all";
            var filename = new StringBuilder("countries");

            if (!string.IsNullOrEmpty(name))
            {
                filter = $"name/{name}";
            }
            filename.Append($"_{filter}".Replace("/", "_").Replace("\\", "_"));

            var response = await client.GetAsync($"https://restcountries.com/v3.1/{filter}");

            if (response.IsSuccessStatusCode)
            {
                // Get the response content
                var content = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response to a list of Country objects
                var countries = JsonConvert.DeserializeObject<List<Country>>(content);

                if (countries != null && limit is not null)
                {
                    countries = countries.Where(c => c.Population < limit).ToList();
                    filename.Append($"_popLimit_{limit}");
                }

                CountryNameSorting sorting = CountryNameSorting.None;
                if (!string.IsNullOrEmpty(nameSorting) && Enum.TryParse($"{nameSorting.ToUpper()[0]}{nameSorting.Substring(1).ToLower()}", out sorting))
                {
                    if (sorting == CountryNameSorting.Asc)
                    {
                        countries = countries.OrderBy(c => c.Name).ToList();
                        filename.Append($"_sortedByName_A-Z");
                    }
                    else if (sorting == CountryNameSorting.Desc) {
                        countries = countries.OrderByDescending(c => c.Name).ToList();
                        filename.Append($"_sortedByName_Z-A");
                    }
                }

                // Serialize the list of Country objects back to a JSON string
                var json = JsonConvert.SerializeObject(countries, Formatting.Indented);

                // Write the JSON string to a file
                await System.IO.File.WriteAllTextAsync($"{filename}.json", json);
            }
            else
            {
                //ViewData["Result"] = "Error: " + response.StatusCode;
            }

            return Ok();
        }
    }
}