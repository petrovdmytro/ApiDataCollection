using ApiDataCollection.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public async Task<IActionResult> GetParamsAsync([FromQuery] string param1 = null, [FromQuery] string param2 = null, [FromQuery] string param3 = null, [FromQuery] string param4 = null)
        {
            var paramsList = new List<string> { param1, param2, param3, param4 };
            paramsList.RemoveAll(p => p == null);  // remove null params

            // Do something with paramsList...

            // Send a GET request to the public API
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://restcountries.com/v3.1/all");

            if (response.IsSuccessStatusCode)
            {
                // Get the response content
                var content = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response to a list of Country objects
                var countries = JsonConvert.DeserializeObject<List<Country>>(content);

                // Serialize the list of Country objects back to a JSON string
                var json = JsonConvert.SerializeObject(countries, Formatting.Indented);

                // Write the JSON string to a file
                await System.IO.File.WriteAllTextAsync("countries.json", json);
            }
            else
            {
                //ViewData["Result"] = "Error: " + response.StatusCode;
            }



            return Ok(paramsList);
        }
    }
}
