using CarWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace CarWebApp.Pages
{
    public class IndexModel : BasePageModel
    {
        private readonly IHttpClientFactory _factory;

        public IndexModel(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public List<CarDto> Cars { get; set; } = new();
        public List<EntityDto> Brands { get; set; } = new();
        public List<EntityDto> Trims { get; set; } = new();

        [BindProperty] public CarDto NewCar { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // 🔹 Проверка авторизации
            var redirect = CheckAuthorization();
            if (redirect != null) return redirect;

            var client = _factory.CreateClient("CarAPI");
            AddJwtHeader(client);

            Cars = await client.GetFromJsonAsync<List<CarDto>>("Cars") ?? new();
            Brands = await client.GetFromJsonAsync<List<EntityDto>>("CarBrands") ?? new();
            Trims = await client.GetFromJsonAsync<List<EntityDto>>("TrimLevels") ?? new();

            return Page();
        }

        // Создание автомобиля
        public async Task<IActionResult> OnPostCreateAsync()
        {
            var redirect = CheckAuthorization();
            if (redirect != null) return redirect;

            var client = _factory.CreateClient("CarAPI");
            AddJwtHeader(client);

            await client.PostAsJsonAsync("Cars", NewCar);
            return RedirectToPage();
        }

        // Удаление автомобиля
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var redirect = CheckAuthorization();
            if (redirect != null) return redirect;

            var client = _factory.CreateClient("CarAPI");
            AddJwtHeader(client);

            await client.DeleteAsync($"Cars/{id}");
            return RedirectToPage();
        }

        // Редактирование автомобиля
        public async Task<IActionResult> OnPostEditAsync(CarDto car)
        {
            var redirect = CheckAuthorization();
            if (redirect != null) return redirect;

            var client = _factory.CreateClient("CarAPI");
            AddJwtHeader(client);

            await client.PutAsJsonAsync($"Cars/{car.Id}", car);
            return RedirectToPage();
        }
    }
}
