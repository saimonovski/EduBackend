using System.Security.Claims;
using Application.Users;
using Domain.Users;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace Api.Endpoints.Auth;

public record VerifyEmailRequest(string code);

public class VerifyEmailEndpoint(UserManager<User> userManager, IUserService userService) :  Endpoint<VerifyEmailRequest, bool>
{
    public override async Task HandleAsync(VerifyEmailRequest req, CancellationToken ct)
    {   
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (id == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        var user = await userService.FindUserByIdAsync(id);
        if (user == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var code = req.code;
        
        var valid = await userManager.VerifyTwoFactorTokenAsync(
            user,
            TokenOptions.DefaultEmailProvider,
            code
        );

        if (!valid)
        {
            await Send.ErrorsAsync(400, ct);
            return;
        }

        user.EmailConfirmed = true;

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            await Send.ErrorsAsync(400, ct);
            return;
        }

        await Send.OkAsync(ct);
    }

    public override void Configure()
    {
        Post("api/auth/verify-code");
        Roles("User");
    }
    
    
}