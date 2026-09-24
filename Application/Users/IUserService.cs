using Application.Entity;
using Domain.Categories;
using Domain.Questions;
using Domain.User;

namespace Application.Users;

public interface IUserService
{
    Task<Result<User>> FindUserByIdAsync(int userId);
    Task<Result<List<User>>> GetAllUsersAsync();
    
    Task<Result<User>> CreateUserAsync(User user);
    Task GetQuestionForUserAsync(int userId, Question question, Category category);
    
    Task<Result<bool>> DeleteUserAsync(int userId);
    Task<Result<bool>> CheckPassword(int userId, string password);
    
    Task<Result<User>> Login(string login, string password);
    Task<Result<Boolean>> ChangePassword(int userId, string oldPassword, string newPassword);
    Task<Result<Boolean>> ResetPassword(int userId, string password);
    Task<Result<Boolean>> ChangeEmail(int userId, string email);
    Task<Result<Boolean>> ChangeName(int userId, string name);
    
    
    //todo user data, user profile - odnośnie modelu uzytkownika dane o jego postepach w nauce itp.
}