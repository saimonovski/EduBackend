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

    public async Task<User?> FindUserByIdlAsync(string userId)
    {
        return await userManager.FindByIdAsync(userId);
    }

    public async Task<IEnumerable<string>> GetRolesAsync(User user)
    {
        return await userManager.GetRolesAsync(user);
    }

    public Task<List<User>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }

    public async  Task<Result<User>> CreateUserAsync(User user, string password)
    {
     var result = await userManager.CreateAsync(user, password);
     await userManager.AddToRoleAsync(user, User.UserRole);
     
     return result.Succeeded ? Result<User>.Success(user)  : Result<User>.Failure(result.Errors.Select(e => e.Description).ToArray());
    }



    public Task<bool> DeleteUserAsync(string userId)
    {
        throw new NotImplementedException();
    }
    
    
    public async Task<Boolean> CheckPassword(User user, string password)
    {
     var correct = await userManager.CheckPasswordAsync(user, password);
     if (!await userManager.IsInRoleAsync(user, User.UserRole))
     {
         await userManager.AddToRoleAsync(user, User.UserRole);
     }
     
     return correct;
    }
    
    public Task<Result<bool>> ChangePassword(string userId, string oldPassword, string newPassword)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> ResetPassword(string userId, string password)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> ChangeEmail(string userId, string email)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> ChangeName(string userId, string name)
    {
        throw new NotImplementedException();
    }
}
