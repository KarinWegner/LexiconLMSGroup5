using Bogus;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Infrastructure.Data;

public static class SeedData
{
    private static UserManager<ApplicationUser> userManager = null!;
    private static RoleManager<IdentityRole> roleManager = null!;
    //private const string adminRole = "Admin";
    private const string teacherRole = "Teacher";
    private const string studentRole = "Student";

    public static async Task SeedDataAsync(this IApplicationBuilder builder)
    {
        using (var scope = builder.ApplicationServices.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            var db = serviceProvider.GetRequiredService<LmsContext>();

            if (await db.Users.AnyAsync()) return;

            userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>() ?? throw new ArgumentNullException(nameof(userManager));
            roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>() ?? throw new ArgumentNullException(nameof(roleManager));

            try
            {
                await CreateRolesAsync([teacherRole, studentRole]);
                await GenerateUsersAsync(5, 2);
                await AssignRolesAsync(db.Users.ToList());
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    //Assigns set number of teachers and remaining users to students
    private static async Task AssignRolesAsync(List<ApplicationUser> users)
    {
           foreach (var user in users) {


            if (!await userManager.IsInRoleAsync(user, user.Role))
            {
                var result = await userManager.AddToRoleAsync(user, user.Role);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

            }
        }
    }
    

    private static async Task CreateRolesAsync(string[] roleNames)
    {
        foreach (var roleName in roleNames)
        {
            if (await roleManager.RoleExistsAsync(roleName)) continue;
            var role = new IdentityRole { Name = roleName };
            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
        }
    }

    private static async Task GenerateUsersAsync(int nrOfUsers, int nrOfTeachers)
    {
        int i = 0;
        var faker = new Faker<ApplicationUser>("sv").Rules((f, e) =>
        {
            e.Email = f.Person.Email;
            e.UserName = f.Person.Email;
            e.Name = f.Person.FullName;
            e.Role = i++ < nrOfTeachers ? "Teacher" : "Student";
        });

        var users = faker.Generate(nrOfUsers);

        //ToDo: Add to user.secrets
        var passWord = "BytMig123!";
        if (string.IsNullOrEmpty(passWord))
            throw new Exception("password nor found");

        foreach (var user in users)
        {
            var result = await userManager.CreateAsync(user, passWord);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

        }
    }
}



