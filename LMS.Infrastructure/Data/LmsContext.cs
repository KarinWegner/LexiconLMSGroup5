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
}

