using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using LMS.Shared.DTOs.CourseDTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;

namespace LMS.Services
{
    public class CourseService : ICourseService
    {

        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CourseService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseDTO>> GetAllCoursesAsync(bool includeModules = false, bool includeEnrollments = false)
        {
            IQueryable<Course> query = _uow.Courses.Query();

            if (includeModules)
            {
                query = query.Include(m => m.Modules);
            }

            if (includeEnrollments)
            {
                query = query.Include(e => e.Enrollments);
            }

            var courses = await query.ToListAsync();
            return _mapper.Map<IEnumerable<CourseDTO>>(courses);
        }

        public async Task<CourseDTO> GetCourseByIdAsync(int id, bool includeModules = false, bool includeEnrollments = false)
        {
            IQueryable<Course> query = _uow.Courses.Query().Where(c => c.CourseId == id);

            if (includeModules)
            {
                query = query.Include(m => m.Modules);
            }

            if (includeEnrollments)
            {
                query = query.Include(e => e.Enrollments);
            }

            var course = await query.FirstOrDefaultAsync();

            if (course == null)
            {
                return null; 
            }

            return _mapper.Map<CourseDTO>(course);
        }

        public async Task<CourseDTO> CreateCourseAsync(CourseCreateDTO courseDto)
        {
            if (courseDto.EndDate < courseDto.StartDate)
            {
                throw new ArgumentException("The course cannot end before the start date.");
            }

            var courseToAdd = _mapper.Map<Course>(courseDto);
            await _uow.Courses.AddAsync(courseToAdd);

            await _uow.CompleteASync();

            return _mapper.Map<CourseDTO>(courseToAdd);
        }
       
        public async Task<bool> DeleteCourseAsync(int id)
        {
            var course = await _uow.Courses.GetByIdAsync(id);
            if (course == null) return false;

            await _uow.Courses.DeleteAsync(course);
            await _uow.CompleteASync();
            return true;
        }

        public async Task<bool> UpdateCourseAsync(int id, CourseUpdateDTO courseDto)
        {
            var existingCourse = await _uow.Courses.GetByIdAsync(id);
            if (existingCourse == null) return false;

            _mapper.Map(courseDto, existingCourse);

            await _uow.Courses.UpdateAsync(existingCourse);
            await _uow.CompleteASync();
            return true;
        }

    }
}
