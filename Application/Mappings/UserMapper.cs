using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Application.Entity;
using Application.Interfaces;
using Domain.Questions;
using Domain.Users;
using Domain.Util;

namespace Application.Mappings;



public record UserDto(
    string? Username,
    string? Email,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    Language UserLanguage,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    Country UserCountry
);


public static class UserMapper
{
    public static UserDto CreateUserDto(User user)
    {
        return new UserDto(
            user.UserName,
            user.Email,
            user.UserData.UserLanguage,
            user.UserData.UserCountry
        );
    }
    
   
   


  
   
    
}