using ApiDataCollection.Helpers;
using ApiDataCollection.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace ApiDataCollection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyApiController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private StringBuilder _filename;
        private CountriesResponseHandlers _handlers;
        public MyApiController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _filename = new StringBuilder("countries");
            _handlers = new CountriesResponseHandlers();
        }

        [HttpGet("params")]
        public async Task<IActionResult> GetParamsAsync([FromQuery] string name = null, [FromQuery] int? limit = null, [FromQuery] string nameSorting = null, [FromQuery] int? first = null)
        {
            try
            {
                IEnumerable<Country> countries;
                // Send a GET request to the public API
                var client = _clientFactory.CreateClient();

                var response = await client.GetAsync($"https://restcountries.com/v3.1/all");

                if (response.IsSuccessStatusCode)
                {
                    // Get the response content
                    var content = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON response to a list of Country objects
                    countries = JsonConvert.DeserializeObject<IEnumerable<Country>>(content);

                    if (countries != null)
                    {
                        countries = _handlers.GetCountryFilteredByName(countries, name);
                        _filename.Append(name is not null ? $"_name_{name}" : "_all");

                        countries = _handlers.GetCountriesWithPopulationLessThanLimit(countries, limit);
                        _filename.Append(limit is not null ? $"_popLimit_{limit}" : string.Empty);

                        countries = _handlers.GetOrderedCountriesList(countries, nameSorting);

                        countries = _handlers.GetFirstCountriesFromList(countries, first);
                        _filename.Append(first is not null ? $"_first_{first}_countries" : string.Empty);
                    }

                    if (countries.Count() > 0)
                    {
                        // Serialize the list of Country objects back to a JSON string
                        var json = JsonConvert.SerializeObject(countries, Formatting.Indented);

                        // Write the JSON string to a file
                        await System.IO.File.WriteAllTextAsync($"{_filename}.json", json);

                        return Ok("countries stored in file");
                    }

                    return Ok("countries are not found");
                }
            }
            catch { }

            return StatusCode((int)HttpStatusCode.InternalServerError, "Something went wrong");
        }

        
    }
}