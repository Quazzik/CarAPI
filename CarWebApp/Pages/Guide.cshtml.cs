using CarWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace CarWebApp.Pages
{
    public class GuideModel : BasePageModel
    {
        private readonly IHttpClientFactory _factory;

        public GuideModel(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public List<EntityDto> Brands { get; set; } = new();
        public List<EntityDto> Trims { get; set; } = new();

        [BindProperty] public CreateEntityDto NewBrand { get; set; } = new();
        [BindProperty] public CreateEntityDto NewTrim { get; set; } = new();

        [BindProperty] public EntityDto EditBrand { get; set; } = new();
        [BindProperty] public EntityDto EditTrim { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var redirect = CheckAuthorization();
            if (redirect != null) return redirect;

            var client = _factory.CreateClient("CarAPI");
            AddJwtHeader(client);

            Brands = await client.GetFromJsonAsync<List<EntityDto>>("CarBrands") ?? new();
            Trims = await client.GetFromJsonAsync<List<EntityDto>>("TrimLevels") ?? new();

            return Page();
        }

        public async Task<IActionResult> OnPostAddBrandAsync()
        {
            var redirect = CheckAuthorization();
            if (redirect != null) return redirect;

            var client = _factory.CreateClient("CarAPI");
            AddJwtHeader(client);

            await client.PostAsJsonAsync("CarBrands", NewBrand);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddTrimAsync()
        {
            var redirect = CheckAuthorization();
            if (redirect != null) return redirect;

            var client = _factory.CreateClient("CarAPI");
            AddJwtHeader(client);

            await client.PostAsJsonAsync("TrimLevels", NewTrim);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteBrandAsync(int id)
        {
            var redirect = CheckAuthorization();
            if (redirect != null) return redirect;

            var client = _factory.CreateClient("CarAPI");
            AddJwtHeader(client);

            await client.DeleteAsync($"CarBrands/{id}");
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteTrimAsync(int id)
        {
            var redirect = CheckAuthorization();
            if (redirect != null) return redirect;

            var client = _factory.CreateClient("CarAPI");
            AddJwtHeader(client);

            await client.DeleteAsync($"TrimLevels/{id}");
            return RedirectToPage();
        }

        // Новый обработчик редактирования бренда
        public async Task<IActionResult> OnPostEditBrandAsync()
        {
            var redirect = CheckAuthorization();
            if (redirect != null) return redirect;

            var client = _factory.CreateClient("CarAPI");
            AddJwtHeader(client);

            await client.PutAsJsonAsync($"CarBrands/{EditBrand.Id}", new CreateEntityDto { Name = EditBrand.Name });
            return RedirectToPage();
        }

        // Новый обработчик редактирования комплектации
        public async Task<IActionResult> OnPostEditTrimAsync()
        {
            var redirect = CheckAuthorization();
            if (redirect != null) return redirect;

            var client = _factory.CreateClient("CarAPI");
            AddJwtHeader(client);

            await client.PutAsJsonAsync($"TrimLevels/{EditTrim.Id}", new CreateEntityDto { Name = EditTrim.Name });
            return RedirectToPage();
        }
    }
}
