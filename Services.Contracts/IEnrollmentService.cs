using Domain.Models.Entities;
using Domain.Models.Responses;
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
        Task<ApiBaseResponse> GetEnrollments();

        Task<ApiBaseResponse> GetEnrollmentsForCourse(int courseId, bool excludeTeachers = false);
        Task<ApiBaseResponse> EditEnrollment(int courseId, EnrollmentUpdateDTO updateDto);
        Task<ApiBaseResponse> AddEnrollment(int courseId, EnrollmentCreateDTO createDto);
        Task<ApiBaseResponse> RemoveEnrollment(int courseId, string userId);
        Task<ApiBaseResponse> GetUserEnrollments(string userId);
    }
}
