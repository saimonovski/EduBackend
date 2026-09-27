using Microsoft.AspNetCore.Identity;

namespace Domain.Users;

public class User() : IdentityUser
{
    public static readonly string UserRole = "User";
    public static readonly string AdminRole = "Admin";
    public UserData UserData{get;set;} = new  UserData();
}