using CarWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace CarWebApp.Pages
{
    public class RegisterModel : BasePageModel
    {
        private readonly IHttpClientFactory _factory;

        public RegisterModel(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        [BindProperty]
        public string Login { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Пароли не совпадают.";
                return Page();
            }

            var client = _factory.CreateClient("CarAPI");
            var dto = new UserDataDto
            {
                Login = Login,
                Password = Password
            };

            var response = await client.PostAsJsonAsync("Auth/Register", dto);

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Ошибка регистрации.";
                return Page();
            }

            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            HttpContext.Session.SetString("JWT", authResponse.Token);

            return RedirectToPage("/Index");
        }
    }
}