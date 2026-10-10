using System.Text.Json.Serialization;
using Application.Entity;
using Application.Users;
using Domain.Users;
using Domain.Util;
using FastEndpoints;
using Infrastructure.Services;
using Infrastructure.Services.Tokens;
using Microsoft.AspNetCore.Identity;

namespace Api.Endpoints.Auth;

public record RegisterRequest(string Email, string Password, string Username,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    Language Language, 
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    Country Country);
public record RegisterResponse(string Token, string RefreshToken);

public class RegisterUserEndpoint(UserManager<User> userManager,IUserService userService, JwtTokenService tokenService, RefreshTokenService refreshTokenService, EmailService emailService) : Endpoint<RegisterRequest, Result<RegisterResponse>>
{
    public override void Configure()
    {
        Post("api/auth/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterRequest userRequest, CancellationToken ct)
    {
        var user = new User()
        {
            UserName = userRequest.Username,
            Email = userRequest.Email,
            
            UserData =
            {
                UserLanguage = userRequest.Language,
                UserCountry = userRequest.Country
            }
        };
        
        var (resultUser, errors) = await userService.CreateUserAsync(user, userRequest.Password);
        //409 - dla istniejacego konta
        if(resultUser is null)
        {
           await Send.ErrorsAsync(409, ct);
            //todo send error
            return;
        }
        //todo generate and send code to user
        var roles = await  userService.GetRolesAsync(user);
        var enumerable = roles.ToList();
        var token = tokenService.GenerateToken(resultUser, enumerable);
        var refreshToken = await refreshTokenService.LoginRefreshToken(user, ct);
        
        var code = await userManager.GenerateTwoFactorTokenAsync(
            user,
            TokenOptions.DefaultEmailProvider
        );

        await emailService.SendEmail(userRequest.Email, code);
        
        await Send.OkAsync(Result<RegisterResponse>.Success(new RegisterResponse(token, refreshToken)), ct);
        
    }
    
}