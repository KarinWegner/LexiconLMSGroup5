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
            new ActivityType("Lecture")
            {
                Id = 1
            },
            new ActivityType("Essay")
            {
                Id = 2
            },
            new ActivityType("Assignment")
            {
                Id = 3
            },
            new ActivityType("Discussion")
            {
                Id = 4
            },
            new ActivityType("Webinar")
            {
                Id = 5
            },
            new ActivityType("Other")
            {
                Id = 6
            });
        base.OnModelCreating(builder);
    }
}

