using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApiDataCollection.Pages
{
    public class FormModel : PageModel
    {
        [BindProperty]
        public string Param1 { get; set; }

        [BindProperty]
        public string Param2 { get; set; }

        [BindProperty]
        public string Param3 { get; set; }

        [BindProperty]
        public string Param4 { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetSubmitAsync()
        {
            using var client = new HttpClient();
            var queryParams = new List<string>();

            if (!string.IsNullOrEmpty(Param1))
                queryParams.Add($"param1={Param1}");
            if (!string.IsNullOrEmpty(Param2))
                queryParams.Add($"param2={Param2}");
            if (!string.IsNullOrEmpty(Param3))
                queryParams.Add($"param3={Param3}");
            if (!string.IsNullOrEmpty(Param4))
                queryParams.Add($"param4={Param4}");

            var response = await client.GetAsync($"http://localhost:7171/api/myapi/params?{string.Join("&", queryParams)}");

            // handle response...

            return Page();
        }
    }

}
