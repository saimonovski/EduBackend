using System.Security.Claims;
using Application.Mappings;
using Application.Users;
using FastEndpoints;

namespace Api.Endpoints;


    public class UserInfoEndpoint(IUserService userService) : EndpointWithoutRequest
    {
        public override void Configure()
        {
            Get("/api/users/me");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null)
            {
                await Send.UnauthorizedAsync(ct);
                return;
            }

            var user = await userService.FindUserByIdlAsync(id);
            if (user == null)
            {
                await Send.UnauthorizedAsync(ct);
                return;
            }
            
            
            
            await Send.OkAsync(UserMapper.CreateUserDto(user), ct);
        }
    }
    