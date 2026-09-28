using System.Security.Cryptography;
using System.Text.Json.Serialization;
using Application.Entity;
using Application.Mappings;
using Application.Users;
using Domain.Users;
using Domain.Util;
using FastEndpoints;
using Infrastructure.Services;
using Infrastructure.Services.Tokens;
using Microsoft.AspNetCore.Identity;

namespace Api.Endpoints.Users;

public record RegisterRequest(string Email, string Password, string Username,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    Language Language, 
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    Country Country);
public record RegisterResponse(string Token, string RefreshToken, UserDto User, List<string> Roles);

public class RegisterUserEndpoint(IUserService userService, JwtTokenService tokenService, RefreshTokenService refreshTokenService) : Endpoint<RegisterRequest, Result<RegisterResponse>>
{
    public override void Configure()
    {
        Post("api/users/register");
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
        
        var result = await userService.CreateUserAsync(user, userRequest.Password);
        
        if(!result.IsSuccess)
        {
            await Send.ResponseAsync(Result<RegisterResponse>.Failure(result.ErrorMessage), 400, ct);
            return;
        }

        var createdUser = result.Value!;
        var roles = await  userService.GetRolesAsync(user);
        var enumerable = roles.ToList();
        var token = tokenService.GenerateToken(createdUser, enumerable);
        var refreshToken = await refreshTokenService.LoginRefreshToken(user);
        await Send.OkAsync(Result<RegisterResponse>.Success(new RegisterResponse(token, refreshToken,  UserMapper.CreateUserDto(createdUser), enumerable)), ct);
        
    }
    
}