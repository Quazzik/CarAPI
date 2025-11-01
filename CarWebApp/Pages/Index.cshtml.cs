using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using CarWebApp.Models;

namespace CarWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _factory;
        public IndexModel(IHttpClientFactory factory) => _factory = factory;

        public List<CarDto> Cars { get; set; } = [];
        public List<EntityDto> Brands { get; set; } = [];
        public List<EntityDto> Trims { get; set; } = [];

        [BindProperty] public CreateCarDto NewCar { get; set; } = new();

        public async Task OnGetAsync()
        {
            var client = _factory.CreateClient("CarAPI");

            Cars = await client.GetFromJsonAsync<List<CarDto>>("Cars") ?? [];
            Brands = await client.GetFromJsonAsync<List<EntityDto>>("CarBrands") ?? [];
            Trims = await client.GetFromJsonAsync<List<EntityDto>>("TrimLevels") ?? [];
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            var client = _factory.CreateClient("CarAPI");
            var resp = await client.PostAsJsonAsync("cars", NewCar);
            if (resp.IsSuccessStatusCode)
                return RedirectToPage();

            ModelState.AddModelError("", "Ошибка при создании автомобиля");
            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _factory.CreateClient("CarAPI");
            await client.DeleteAsync($"cars/{id}");
            return RedirectToPage();
        }
    }
}