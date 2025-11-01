using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace CarWebApp.Pages
{
    public class BasePageModel : PageModel
    {
        
        public string? CurrentUser => HttpContext.Session.GetString("UserLogin");
        /// <summary>
        /// Проверяет, авторизован ли пользователь. Если JWT отсутствует, редирект на Login.
        /// </summary>
        /// <returns>RedirectToPage("/Login") или null</returns>
        protected IActionResult? CheckAuthorization()
        {
            var token = HttpContext.Session.GetString("JWT");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Login");
            }
            return null;
        }

        /// <summary>
        /// Универсальный Logout для всех страниц
        /// </summary>
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Remove("JWT");
            HttpContext.Session.Remove("Login");
            return RedirectToPage("/Login");
        }

        /// <summary>
        /// Получение JWT текущего пользователя
        /// </summary>
        protected string? GetToken()
        {
            return HttpContext.Session.GetString("JWT");
        }

        /// <summary>
        /// Добавляет JWT в заголовок Authorization для HttpClient
        /// </summary>
        protected void AddJwtHeader(System.Net.Http.HttpClient client)
        {
            var token = GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}