using LMS.Shared.DTOs.CourseDTOs;

namespace Services.Contracts
{
    public interface ICourseService
    {
        Task<List<CourseDTO>> GetAllCoursesAsync(bool includeModules = false, bool includeEnrollments = false);
        Task<CourseDTO> GetCourseByIdAsync(int id, bool includeModules = false, bool includeEnrollments = false);
        Task<int> CreateCourseAsync(CourseCreateDTO courseDto);
        Task<CourseDTO> UpdateCourseAsync(int id, CourseUpdateDTO courseDto);
        Task<bool> DeleteCourseAsync(int id);

    }
}
