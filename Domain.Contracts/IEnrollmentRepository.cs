using Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IEnrollmentRepository
    {
        Task AddEnrollment(int courseId, ApplicationUser user);
        Task DeleteEnrollment(int courseId, string userId);
        Task EditEnrollment(int moveFromCourseId, string userId, int moveToCourseId);
        Task<ApplicationUser> FindUserByIdAsync(string userId);
        Task<IEnumerable<Course>> GetUserEnrollments(string userId);
        Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string? roleFilter);
        IQueryable<ApplicationUser> UserQuery();
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
        Task AssignRoleToUserAsync(string userId, string roleName);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
    }
}
