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
        public MyApiController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _filename = new StringBuilder("countries");
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
                        countries = GetCountryFilteredByName(countries, name);

                        countries = GetCountriesWithPopulationLessThanLimit(countries, limit);

                        countries = GetOrderedCountriesList(countries, nameSorting);

                        countries = GetFirstCountriesFromList(countries, first);
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

        private IEnumerable<Country> GetCountryFilteredByName(IEnumerable<Country> countries, string name)
        {
            _filename.Append(name is not null ? $"_name_{name}" : "_all");
            return countries.Where(c => string.IsNullOrWhiteSpace(name) || c.Name.Contains(name));
        }

        private IEnumerable<Country> GetCountriesWithPopulationLessThanLimit(IEnumerable<Country> countries, int? limit)
        {
            _filename.Append(limit is not null ? $"_popLimit_{limit}" : string.Empty);
            return countries.Where(c => limit == null || c.Population < limit);
        }

        private IEnumerable<Country> GetOrderedCountriesList(IEnumerable<Country> countries, string nameSorting)
        {
            CountryNameSorting sorting = CountryNameSorting.None;
            if (!string.IsNullOrEmpty(nameSorting) && Enum.TryParse($"{nameSorting.ToUpper()[0]}{nameSorting.Substring(1).ToLower()}", out sorting))
            {
                if (sorting == CountryNameSorting.Asc)
                {
                    countries = countries.OrderBy(c => c.Name).ToList();
                    _filename.Append($"_sortedByName_A-Z");
                }
                else if (sorting == CountryNameSorting.Desc)
                {
                    countries = countries.OrderByDescending(c => c.Name).ToList();
                    _filename.Append($"_sortedByName_Z-A");
                }
            }
            return countries;
        }
        private IEnumerable<Country> GetFirstCountriesFromList(IEnumerable<Country> countries, int? count)
        {
            _filename.Append(count is not null ? $"_first_{count}_countries" : string.Empty);
            return count.HasValue ? countries.Take(count.Value) : countries;
        }
    }
}