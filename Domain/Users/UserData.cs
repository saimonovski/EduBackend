using Domain.Util;

namespace Domain.Users;

public class UserData
{
    public int Id { get; set; }
    public  Country UserCountry { get; set; } = Country.Poland;
    public Language UserLanguage { get; set; } = Language.Pl;
}