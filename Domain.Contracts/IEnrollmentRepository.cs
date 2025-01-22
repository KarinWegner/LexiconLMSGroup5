using Domain.Models.Entities;
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
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(string? roleFilter);
    }
}
