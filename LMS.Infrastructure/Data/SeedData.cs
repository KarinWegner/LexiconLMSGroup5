using Bogus;
using Bogus.DataSets;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
                await GenerateUsersAsync(10, 4);
                await AssignRolesAsync(db.Users.ToList());

                List<Course> courses = await GenerateCoursesAsync(3);
                 await db.Courses.AddRangeAsync(courses);
                await db.SaveChangesAsync();

                List<Module> modules = await GenerateModulesInCourses(db.Courses.ToList());
                await db.Modules.AddRangeAsync(modules);
                await db.SaveChangesAsync();

                List<Activity> activities= await GenerateActivitiesInModules(db.Modules.ToList(), db.ActivityTypes.ToList());
                await db.Activities.AddRangeAsync(activities);
                await db.SaveChangesAsync();

                await EnrollUsersInCourses(await db.Users.Where(u=>u.Role == teacherRole).Include(t=>t.Enrollments).ToListAsync(), await db.Users.Where(u=>u.Role == studentRole).Include(t => t.Enrollments).ToListAsync(), await db.Courses.ToListAsync());
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    private static async Task EnrollUsersInCourses(List<ApplicationUser> teachers, List<ApplicationUser> students, List<Course> courses)
    {
        var rnd = new Random();
        int extraTeachers= teachers.Count-courses.Count;
        int i = 0;
        for (i = 0; i < courses.Count; i++)
        {
            teachers[i].Enrollments.Add(courses[i]);
        }
        //Adds teachers that are left after assigning one to each course to a random course
        for ( int j = 0; j < extraTeachers; j++) 
        {
            teachers[i++].Enrollments.Add(courses[rnd.Next(0,courses.Count-1)]);
        }

        foreach (var student in students)
        {
            student.Enrollments.Add(courses[rnd.Next(0, courses.Count - 1)]);
        }

    }

    private static async Task<List<Activity>> GenerateActivitiesInModules(List<Module> modules, List<ActivityType> activityTypes)
    {
        int activitiesPerModule = 5;
        List<List<Activity>> activityLists = new List<List<Activity>>();
        foreach (var module in modules) 
        {
            TimeSpan activitySpan = (module.StartDate - module.EndDate) / activitiesPerModule;
            
        int activityNumber = 1;
            
                var faker = new Faker<Activity>("sv").Rules((f, m) =>
                {
                    m.ModuleId = module.ModuleId;
                    m.StartDate = module.StartDate.Add(activitySpan * (activityNumber - 1));
                    m.EndDate = module.StartDate.Add(activitySpan * activityNumber++);
                    m.ActivityType = activityTypes[f.Random.Int(0, activityTypes.Count - 1)];
                    m.ActivityTypeId= m.ActivityType.ActivityTypeId;
                    m.Name = m.ActivityType.Name +": " +f.Hacker.Noun();
                    m.Description = "A(n) " + m.ActivityType.Name + " about " + module.Name;
                });
            activityLists.Add(faker.Generate(activitiesPerModule));
            
        }
        return activityLists.SelectMany(a=>a).ToList();
    }

    private static async Task<List<Module>> GenerateModulesInCourses(List<Course> courses)
    {
        var moduleWords = new string[] { "integration", "is built", "is applied", "functions", "", "works" };
        int moduleWordCount = moduleWords.Count();
        List<List<Module>> moduleLists = new();
        foreach (var course in courses) 
        {
            int numberOfModules = 3;
            TimeSpan moduleSpan = (course.StartDate - course.EndDate) / numberOfModules;
            int moduleNumber = 1;
            var faker = new Faker<Module>("sv").Rules((f, m) =>
            {
                m.CourseId = course.CourseId;
                m.StartDate= course.StartDate.Add(moduleSpan * (moduleNumber-1));
                m.EndDate = course.StartDate.Add(moduleSpan *  moduleNumber);
                m.Name = "Module " + moduleNumber++ +": "+f.Hacker.Noun();
                m.Description = m.Name + ". How " + f.Hacker.Adjective() + " " + f.Hacker.Verb() +" "+ moduleWords[f.Random.Int(0, moduleWordCount - 1)];
            });
            moduleLists.Add(faker.Generate(numberOfModules));
        }
        return moduleLists.SelectMany(m=>m).ToList();

    }

    private static async Task<List<Course>> GenerateCoursesAsync(int nrOfCourses)
    {
       
        DateTime refDate = DateTime.UtcNow;
        var faker = new Faker<Course>("sv").Rules((f, c) =>
        {
            c.Name = f.Hacker.IngVerb();
            c.StartDate = f.Date.Past(1, refDate);
            c.EndDate = c.StartDate.AddMonths(6);
            c.Description = "";            
        });
        return faker.Generate(nrOfCourses);

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



