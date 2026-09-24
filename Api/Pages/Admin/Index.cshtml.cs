using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Api.Pages.Admin;

[Authorize(Roles = "Admin")] // Blokuje dostęp osobom niezalogowanym lub bez roli Admin
public class Index : PageModel
{
    public string Username { get; set; } = string.Empty;

    public void OnGet()
    {
        // Odczytujemy nazwę użytkownika z zalogowanej sesji
        Username = User.Identity?.Name ?? "Administrator";
    }
}