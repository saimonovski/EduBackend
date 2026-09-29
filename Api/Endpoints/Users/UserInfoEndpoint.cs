using System.Security.Claims;
using Application.Users;
using FastEndpoints;

namespace Api.Endpoints.Users;

public record UserInfoResponse(string Username, string Email);

    public class UserInfoEndpoint(IUserService userService) : EndpointWithoutRequest<UserInfoResponse>
    {
        public override void Configure()
        {
            Get("/api/users/profile");
            Roles("User");
        }

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
            
            
            await Send.OkAsync(new UserInfoResponse(user.UserName, user.Email), ct);
        }
    }
    