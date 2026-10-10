using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Application.Entity;
using Domain.Questions;
using Domain.Users;
using Domain.Util;

namespace Application.Mappings;



public record UserDto(
    string? Username,
    string? Email,
    bool emailVerified,
    UserData? UserData
);


public static class UserMapper
{
    public static UserDto CreateUserDto(User user)
    {
        return new UserDto(
            user.UserName,
            user.Email,
            user.EmailConfirmed,
            user.UserData
        );
    }
    
   
   


  
   
    
}