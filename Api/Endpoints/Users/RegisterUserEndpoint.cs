using Application.Mappings;
using Application.Users;
using Domain.User;
using Domain.Util;
using FastEndpoints;

namespace Api.Endpoints.Users;

public record RegisterUserRequest(string Email, string Password, string Username, Language Language, string Country);

public class RegisterUserEndpoint(IUserService userService) : Endpoint<RegisterUserRequest, UserDto>
{
    public override void Configure()
    {
        Post("api/users/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterUserRequest userRequest, CancellationToken ct)
    {
        var user = new User(userRequest.Username, userRequest.Password, -1, userRequest.Email)
        {
            UserData =
            {
                Language = userRequest.Language,
                Country = userRequest.Country
            }
        };
        
        var result = await userService.CreateUserAsync(user);
        if(!result.IsSuccess)
        {
            await Send.NotFoundAsync(ct);
            await Console.Error.WriteLineAsync(result.ErrorMessage);
            return;
        }
        var userDto = UserMapper.CreateUserDto(result.Value!);
        await Send.OkAsync(userDto, ct);
        
    }
    
}