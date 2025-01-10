using Bogus;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LMS.Infrastructure.Data;

public class LmsContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public LmsContext(DbContextOptions<LmsContext> options) : base(options) { }
    
        public DbSet<Course> Courses { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Domain.Models.Entities.Activity> Activities { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Module> Modules { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ActivityType>()
            .HasData(
            new ActivityType()
            {
                Name="Lecture",
                ActivityTypeId = 1
            },
            new ActivityType()
            {
                Name ="Essay",
                ActivityTypeId = 2
            },
            new ActivityType()
            {
                Name ="Assignment",
                ActivityTypeId = 3
            },
            new ActivityType()
            {
                Name ="Discussion",
                ActivityTypeId = 4
            },
            new ActivityType()
            {
                Name ="Webinar",
                ActivityTypeId = 5
            },
            new ActivityType()
            {
                Name ="Other",
                ActivityTypeId = 6
            });
        base.OnModelCreating(builder);
    }
}

