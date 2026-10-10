using System.Security.Claims;
using FastEndpoints;
using Infrastructure.Services.Tokens;

namespace Api.Endpoints.Auth;


public class LogoutAllEndpoint(RefreshTokenService refreshTokenService) : EndpointWithoutRequest
{
    public override async Task HandleAsync( CancellationToken ct)
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        await refreshTokenService.DeactivateAllTokens(id, ct);
        await Send.OkAsync(200,ct);
    }
    public override void Configure()
    {
        Post("api/auth/logout/all");
        Roles("User");
    }
}