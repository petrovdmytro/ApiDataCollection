using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApiDataCollection.Pages
{
    public class FormModel : PageModel
    {
        [BindProperty]
        public string CountryNameContains { get; set; }

        [BindProperty]
        public string Param2 { get; set; }

        [BindProperty]
        public string Param3 { get; set; }

        [BindProperty]
        public string Param4 { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostSubmitAsync()
        {
            using var client = new HttpClient();
            var queryParams = new List<string>();

            //if (!string.IsNullOrEmpty(CountryNameContains))
                queryParams.Add($"CountryNameContains={CountryNameContains}");
            //if (!string.IsNullOrEmpty(Param2))
                queryParams.Add($"param2={Param2}");
            //if (!string.IsNullOrEmpty(Param3))
                queryParams.Add($"param3={Param3}");
            //if (!string.IsNullOrEmpty(Param4))
                queryParams.Add($"param4={Param4}");

            var response = await client.GetAsync($"https://localhost:7171/api/MyApi/params?{string.Join("&", queryParams)}");

            return Page();
        }
    }

}
