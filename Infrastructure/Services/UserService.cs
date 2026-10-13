using Application.Entity;
using Application.Users;
using Domain.Categories;
using Domain.Questions;
using Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class UserService(UserManager<User> userManager, ApplicationDbContext dbContext) : IUserService
{
    public async Task<User?> FindUserByEmailAsync(string userEmail)
    {
       return  await userManager.FindByEmailAsync(userEmail);
       
    }

    public async Task<User?> FindUserByIdAsync(string userId)
    {
        return await userManager.FindByIdAsync(userId);
    }

    public async Task<IEnumerable<string>> GetRolesAsync(User user)
    {
        return await userManager.GetRolesAsync(user);
    }

    public async Task<(User?, List<IdentityError>)> CreateUserAsync(User user, string password)
    {
        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, User.UserRole);
            return (user, []);
        }


        return (null, result.Errors.ToList());
    }

    public async Task UpdateUserAsync(User user, CancellationToken ct)
    {
        dbContext.Update(user);
      await  dbContext.SaveChangesAsync(ct);
    }

    public Task<bool> DeleteUserAsync(string userId)
    {
        throw new NotImplementedException();
    }
    
    
    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
     var correct = await userManager.CheckPasswordAsync(user, password);
    
     
     return correct;
    }

    public async Task<bool> LoginUser(User user, string password, CancellationToken ct =default)
    {
       var correctPassword = await CheckPasswordAsync(user, password);
       if (!correctPassword)
       {
           return false;
       }
       user.UserData.LastLoggedInAt =  DateTime.UtcNow;
       return true;
    }

}
