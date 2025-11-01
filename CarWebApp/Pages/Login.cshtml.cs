using CarWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace CarWebApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _factory;

        public LoginModel(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        [BindProperty]
        public string Login { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _factory.CreateClient("CarAPI");

            try
            {
                var response = await client.PostAsJsonAsync("Auth/Login", new UserDataDto
                {
                    Login = Login,
                    Password = Password
                });

                if (response.IsSuccessStatusCode)
                {
                    var auth = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
                    if (auth != null)
                    {
                        HttpContext.Session.SetString("JWT", auth.Token);
                        HttpContext.Session.SetString("UserLogin", auth.User.Login);
                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        ErrorMessage = "Сервер вернул пустой ответ";
                        return Page();
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    // Берём текст ошибки прямо из ответа
                    var serverMessage = await response.Content.ReadAsStringAsync();
                    ErrorMessage = serverMessage;
                    return Page();
                }
                else
                {
                    ErrorMessage = $"Ошибка авторизации: {response.StatusCode}";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при запросе: {ex.Message}";
                return Page();
            }
        }
    }
}
