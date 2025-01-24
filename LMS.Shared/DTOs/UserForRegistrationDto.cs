using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.DTOs;

#nullable disable
public record UserForRegistrationDto
{
    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; init; }
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; init; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; init; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; init; }
}
