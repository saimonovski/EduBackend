using Application.Entity;
using Domain.Categories;
using Domain.Questions;
using Domain.Users;

namespace Application.Users;

public interface IUserService
{

    Task<IEnumerable<string>> GetRolesAsync(User user);
     Task<User?> FindUserByEmailAsync(string userEmail);
     Task<User?> FindUserByIdlAsync(string userId);
    Task<List<User>> GetAllUsersAsync();
    
    Task<Result<User>> CreateUserAsync(User user, string password);
    
    Task<bool> DeleteUserAsync(string userId);
    
    Task<bool> CheckPassword(User user, string password);
    
    Task<Result<Boolean>> ChangePassword(string userId, string oldPassword, string newPassword);
    Task<Result<Boolean>> ResetPassword(string userId, string password);
    Task<Result<Boolean>> ChangeEmail(string userId, string email);
    Task<Result<Boolean>> ChangeName(string userId, string name);
    
    
    //todo user data, user profile - odnośnie modelu uzytkownika dane o jego postepach w nauce itp.
   
    
}