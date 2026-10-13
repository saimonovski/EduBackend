using System.Security.Claims;
using Application.Users;
using Domain.Users;
using Domain.Util;
using FastEndpoints;

namespace Api.Endpoints.Users;

public record UpdateUserRequest(
    Country? UserCountry,
    Language? UserLanguage,
    Level? ClassLevel,
    Gender? Gender
)
{
   public UserData UpdateData(UserData userData)
    {
        userData.UserCountry = UserCountry ?? userData.UserCountry;
        userData.UserLanguage = UserLanguage ?? userData.UserLanguage;
        userData.ClassLevel = ClassLevel ?? userData.ClassLevel;
        userData.Gender = Gender ?? userData.Gender;
        
        return userData;
    }
}

public class UpdateUserEndpoint(IUserService userService) : Endpoint<UpdateUserRequest>
{
    
    public override async Task HandleAsync(UpdateUserRequest req, CancellationToken ct)
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

        var userData = user.UserData;
       userData =  req.UpdateData(userData);
       user.UserData = userData;
     await userService.UpdateUserAsync(user, ct);
    await Send.OkAsync("Everything okay!", ct);

    }

    public override void Configure()
    {
        Post("/api/users/update");
        Roles("User");
    }
}