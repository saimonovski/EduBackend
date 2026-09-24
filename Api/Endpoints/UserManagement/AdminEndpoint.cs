using FastEndpoints;

namespace Api.Pages.Admin;

public class AdminDashboardEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/ad"); // Ścieżka dla admina
       Roles("admin");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        string htmlContent = """
                             <!DOCTYPE html>
                             <html>
                             <head><title>Admin Panel</title></head>
                             <body>
                             <h1>Panel Administracyjny</h1>
                             <p>Witaj w panelu!</p>
                             </body>
                             </html>
                             """;

// Zwracamy su rowy HTML
          
        await Send.StringAsync(htmlContent,statusCode: 200, contentType: "text/html", cancellation: ct);
    }
}