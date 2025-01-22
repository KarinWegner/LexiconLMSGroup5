using LMS.Shared.DTOs;
using Domain.Models.Responses;
using LMS.Shared.DTOs.CourseDTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace Services.Contracts
{
    public interface ICourseService
    {

        Task<ApiBaseResponse> GetAllCoursesAsync(bool includeModules = false, bool includeEnrollments = false, int pageNr = 1, int pageSize = 10);
        Task<ApiBaseResponse> GetCourseByIdAsync(int id, bool includeModules = false, bool includeEnrollments = false);
        Task<ApiBaseResponse> CreateCourseAsync(CourseCreateDTO courseDto);
        Task<ApiBaseResponse> UpdateCourseAsync(int id, CourseUpdateDTO courseDto);
        Task<ApiBaseResponse> DeleteCourseAsync(int id);
    }
}
