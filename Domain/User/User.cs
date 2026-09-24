
namespace Domain.User;

public class User( string username, string password, int id, string email)
{
    public string Username{get;set;} = username;
    public string Password{get;set;} = password;
    public int Id{get;} = id;
    public string Email{get;set;} = email;

    public UserData UserData{get;set;} = new  UserData();
}