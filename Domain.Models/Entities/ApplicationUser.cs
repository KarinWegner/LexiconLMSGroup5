using Microsoft.AspNetCore.Identity;

namespace Domain.Models.Entities;

//ApplicationUser is shared between Blazor and API
public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpireTime { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public ICollection<Course> Enrollments { get; set; }
}