using Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace LMS.Blazor;

//Only used with Usermanager
//No navigationproperties or FK here!!!
public class ApplicationUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;
}