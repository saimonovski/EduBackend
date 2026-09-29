using FastEndpoints;

namespace Api.Endpoints.UserManagement;

public class AdminDashboardEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/admin"); // Ścieżka dla admina
        Roles("Admin");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {


       var admin = User.IsInRole("Admin");
       var user = User.IsInRole("User");
       await Send.OkAsync("Witaj w panelu administratora czy zalogowano: " + admin + " Czy user: "+user, ct);
    }
}