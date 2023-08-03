using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApiDataCollection.Pages
{
    public class FormModel : PageModel
    {
        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Limit { get; set; }

        [BindProperty]
        public string Sorting { get; set; }

        [BindProperty]
        public string Count { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostSubmitAsync()
        {
            using var client = new HttpClient();
            var queryParams = new List<string>();

            //if (!string.IsNullOrEmpty(CountryNameContains))
                queryParams.Add($"name={Name}");
            //if (!string.IsNullOrEmpty(Param2))
                queryParams.Add($"limit={Limit}");
            //if (!string.IsNullOrEmpty(Param3))
                queryParams.Add($"nameSorting={Sorting}");
            //if (!string.IsNullOrEmpty(Param4))
                queryParams.Add($"first={Count}");

            var response = await client.GetAsync($"https://localhost:7171/api/MyApi/params?{string.Join("&", queryParams)}");

            return Page();
        }
    }

}
