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
        Task EditEnrollment(int courseId, string userId, int newCourseId);
        Task<ApplicationUser> FindUserByIdAsync(string userId);
    }
}
