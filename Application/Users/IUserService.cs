using Application.Entity;
using Domain.Categories;
using Domain.Questions;
using Domain.Users;

namespace Application.Users;

public interface IUserService
{

    Task<IEnumerable<string>> GetRolesAsync(User user);
     Task<User?> FindUserByEmailAsync(string userEmail);
     Task<User?> FindUserByIdAsync(string userId);

     Task<Result<User>> CreateUserAsync(User user, string password);
    
    Task<bool> DeleteUserAsync(string userId);
    
    Task<bool> CheckPasswordAsync(User user, string password);
    
 
    
    
    //todo user data, user profile - odnośnie modelu uzytkownika dane o jego postepach w nauce itp.


    Task<bool> LoginUser(User user, string password, CancellationToken ct = default);
}