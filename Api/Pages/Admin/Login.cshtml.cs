using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Api.Pages.Admin;

// ZMIANA: Zmiana nazwy klasy z Login na LoginModel
public class Login : PageModel 
{
    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        // Pobieramy dane bezpośrednio z formularza HTTP, ignorując mechanizmy powiązań
        string cleanEmail = Request.Form["Email"].ToString().Trim();
        string cleanPassword = Request.Form["Password"].ToString().Trim();

        // Sprawdzenie poprawności
        bool isValidUser = (cleanEmail == "admin@test.pl" && cleanPassword == "admin123");

        if (!isValidUser)
        {
            ErrorMessage = $"Nieprawidłowy login lub hasło. Odebrano: Email='{cleanEmail}', Password='{cleanPassword}'";
            return Page();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Name, cleanEmail),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));

        return RedirectToPage("/Admin/Index");
    }

    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToPage("/Admin/Login");
    }
}