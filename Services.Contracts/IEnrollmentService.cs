using Domain.Models.Entities;
using LMS.Shared.DTOs.ApplicationUserDTOs;
using LMS.Shared.DTOs.EnrollmentDTOs;
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
        Task EditEnrollment(int courseId, EnrollmentUpdateDTO updateDto);
        Task AddEnrollment(int courseId, EnrollmentCreateDTO createDto);
        Task RemoveEnrollment(int courseId, string userId);
        Task<IEnumerable<EnrollmentUserCourseListDTO>> GetUserEnrollments(string userId);
        Task<IEnumerable<ApplicationUserListDTO>> GetUsers(string? roleFilter);
    }
}
