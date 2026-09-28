using Application.Users;
using FastEndpoints;
using Infrastructure.Services;
using Infrastructure.Services.Tokens;

namespace Api.Endpoints.Users;

public record RefreshRequest(string RefreshToken);
public record RefreshResponse(string Token, string RefreshToken);
public class RefreshEndpoint(RefreshTokenService refreshTokenService, JwtTokenService tokenService, IUserService userService) : Endpoint<RefreshRequest, RefreshResponse>
{
    public override async Task HandleAsync(RefreshRequest req, CancellationToken ct)
    {
        var refresh = req.RefreshToken;
       var (tokenFound, token) = await refreshTokenService.CheckRefreshTokenAsync(refresh);
       if (!tokenFound || token == null)
       {
           await Send.UnauthorizedAsync(ct);
           return;
       }

      
       var rawToken = await refreshTokenService.RotateRefreshTokenAsync(token);
       var roles = await userService.GetRolesAsync(token.User);
       var newToken = tokenService.GenerateToken(token.User, roles);
       
       var response = new RefreshResponse(newToken, rawToken);
       await Send.OkAsync(response, ct);
    }

    public override void Configure()
    {
        Post("api/users/refresh");
        AllowAnonymous();
    }
}