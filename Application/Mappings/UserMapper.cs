using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Application.Dto;
using Application.Entity;
using Application.Interfaces;
using Domain.Questions;
using Domain.User;
using Domain.Util;

namespace Application.Mappings;

public record UserDao(
    int UserId,
    string Email,
    string Username,
    string Password,
    UserData UserData
);

public record UserDto(
    int Id,
    string Username,
    string Email,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    Language Language,
    string Country
);


public static class UserMapper
{
    public static UserDto CreateUserDto(User user)
    {
        return new UserDto(
            user.Id,
            user.Username,
            user.Email,
            user.UserData.Language,
            user.UserData.Country
        );
    }
    
   
    public static UserDao ToDao(User user)
    {
        return new UserDao(
            user.Id,
            user.Email,
            user.Username,
            user.Password,
            user.UserData
        );
    }

    public static User ToDomain(UserDao dao)
    {
        return new User(
           dao.Username,
           dao.Password,
           dao.UserId,
           dao.Email
        )
        {
            UserData =  dao.UserData
        }
            ;
    }


  
   
    
}