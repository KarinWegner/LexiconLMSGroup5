using Domain.Models.Entities;
using LMS.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentListDTO>> GetEnrollments();

        Task<IEnumerable<EnrolledUserDTO>> GetEnrollmentsForCourse(int courseId, bool excludeTeachers = false);
        Task EditEnrollment(int courseId, string userId, int newCourseId);
        Task<Course> AddEnrollment(int courseId, string userId);
        Task RemoveEnrollment(int courseId, string userId);
        Task<IEnumerable<EnrollmentListDTO>> GetUserEnrollments(string userId);
    }
}
