using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using CarWebApp.Models;

namespace CarWebApp.Pages
{
    public class GuideModel : PageModel
    {
        private readonly IHttpClientFactory _factory;
        public GuideModel(IHttpClientFactory factory) => _factory = factory;

        public List<EntityDto> Brands { get; set; } = [];
        public List<EntityDto> Trims { get; set; } = [];

        [BindProperty] public CreateEntityDto NewBrand { get; set; } = new();
        [BindProperty] public CreateEntityDto NewTrim { get; set; } = new();

        public async Task OnGetAsync()
        {
            var client = _factory.CreateClient("CarAPI");
            Brands = await client.GetFromJsonAsync<List<EntityDto>>("carbrands") ?? [];
            Trims = await client.GetFromJsonAsync<List<EntityDto>>("trimlevels") ?? [];
        }

        public async Task<IActionResult> OnPostAddBrandAsync()
        {
            var client = _factory.CreateClient("CarAPI");
            await client.PostAsJsonAsync("carbrands", NewBrand);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddTrimAsync()
        {
            var client = _factory.CreateClient("CarAPI");
            await client.PostAsJsonAsync("trimlevels", NewTrim);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, string type)
        {
            var client = _factory.CreateClient("CarAPI");
            if (type == "brand") await client.DeleteAsync($"carbrands/{id}");
            else if (type == "trim") await client.DeleteAsync($"trimlevels/{id}");
            return RedirectToPage();
        }
    }
}