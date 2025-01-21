using LMS.Shared.DTOs;
using LMS.Shared.DTOs.CourseDTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace Services.Contracts
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDTO>> GetAllCoursesAsync(bool includeModules = false, bool includeEnrollments = false, int pageNr = 1, int pageSize = 10);
        Task<CourseDTO> GetCourseByIdAsync(int id, bool includeModules = false, bool includeEnrollments = false);
        Task<CourseDTO> CreateCourseAsync(CourseCreateDTO courseDto);
        Task<bool> UpdateCourseAsync(int id, CourseUpdateDTO courseDto);
        Task<bool> DeleteCourseAsync(int id);
    }
}
