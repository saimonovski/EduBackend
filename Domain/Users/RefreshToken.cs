using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Intrinsics.Arm;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Domain.Users;

[PrimaryKey("Id")]
public class RefreshToken
{
    
    public Guid Id { get; } = Guid.NewGuid();
    public required  User User { get; set; }
    
    public required string TokenHash { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(14);
 
     
    
}