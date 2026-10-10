using FastEndpoints;
using Infrastructure.Services.Tokens;

namespace Api.Endpoints.Auth;

public record LogOutRequest(string RefreshToken);

public class LogoutEndpoint(RefreshTokenService refreshTokenService) : Endpoint<LogOutRequest>
{
    public override async Task HandleAsync(LogOutRequest req, CancellationToken ct)
    {
         await refreshTokenService.DeactivateToken(req.RefreshToken, ct);
        await Send.OkAsync(200,ct);
    }
    public override void Configure()
    {
        Post("api/auth/logout");
    }
}