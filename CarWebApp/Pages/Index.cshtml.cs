using CarWebApp.Models; // Пути к CarDto, CreateCarDto
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace CarWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _factory;

        public IndexModel(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public List<CarDto> Cars { get; set; } = new();
        public List<EntityDto> Brands { get; set; } = new();
        public List<EntityDto> Trims { get; set; } = new();

        [BindProperty] public CreateCarDto NewCar { get; set; } = new();

        public async Task OnGetAsync()
        {
            var client = _factory.CreateClient("CarAPI");
            Cars = await client.GetFromJsonAsync<List<CarDto>>("Cars") ?? new();
            Brands = await client.GetFromJsonAsync<List<EntityDto>>("CarBrands") ?? new();
            Trims = await client.GetFromJsonAsync<List<EntityDto>>("TrimLevels") ?? new();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            var client = _factory.CreateClient("CarAPI");
            await client.PostAsJsonAsync("Cars", NewCar);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync(int Id, string Name, int CarBrandId, int TrimLevelId, int Amount)
        {
            var client = _factory.CreateClient("CarAPI");
            var car = new CreateCarDto
            {
                Name = Name,
                CarBrandId = CarBrandId,
                TrimLevelId = TrimLevelId,
                Amount = Amount
            };
            await client.PutAsJsonAsync($"Cars/{Id}", car);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _factory.CreateClient("CarAPI");
            await client.DeleteAsync($"Cars/{id}");
            return RedirectToPage();
        }
    }
}