using Application.Entity;
using Application.Mappings;
using Application.Users;
using Domain.Users;
using FastEndpoints;
using Infrastructure.Services;
using Infrastructure.Services.Tokens;

namespace Api.Endpoints.Users;


public record LoginRequest(string Password, string Email);
public record LoginResponse(string Token, string RefreshToken, UserDto User);

public class LoginUserEndpoint(IUserService userService, JwtTokenService jwtTokenService, RefreshTokenService refreshTokenService) : Endpoint<LoginRequest, LoginResponse>
{
    public override void Configure()
    {
        Post("api/users/login/");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest loginUserRequest, CancellationToken ct)
    {
        var user = await userService.FindUserByEmailAsync(loginUserRequest.Email);
        if (user == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        var correct = await  userService.LoginUser(user, loginUserRequest.Password, ct);
        if (!correct)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        var roles = await userService.GetRolesAsync(user);
        
        var token = jwtTokenService.GenerateToken(user, roles);
        var refreshToken = await refreshTokenService.LoginRefreshToken(user, ct);
     
        await Send.OkAsync((new LoginResponse(token, refreshToken, UserMapper.CreateUserDto(user))), ct);
    }
}