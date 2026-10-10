using System.Security.Claims;
using Application.Users;
using Domain.Users;
using FastEndpoints;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;

namespace Api.Endpoints.Auth;

public class SendVerificationCodeEndpoint(IUserService userService, UserManager<User> userManager, EmailService emailService) : EndpointWithoutRequest
{
    public override async Task HandleAsync(CancellationToken ct)
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
        var code = await userManager.GenerateTwoFactorTokenAsync(
            user,
            TokenOptions.DefaultEmailProvider
        );

        await emailService.SendEmail(User.FindFirstValue(ClaimTypes.Email!), code);
        await Send.OkAsync("Email sent", ct);
    }

    public override void Configure()
    {
        Post("/api/auth/send-verification-code-email");
        Roles("User");
    }
}