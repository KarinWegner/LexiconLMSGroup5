using AutoMapper;
using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using LMS.Shared.DTOs.CourseDTOs;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services
{
    public class CourseService : ICourseService
    {

        private readonly LmsContext _context;
        private readonly IMapper _mapper;

        public CourseService(LmsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> CreateCourseAsync(CourseCreateDTO courseDto)
        {
            if (courseDto.EndDate < courseDto.StartDate)
            {
                throw new ArgumentException("The course cannot end before the start date.");
            }

            var courseToAdd = _mapper.Map<Course>(courseDto);
            _context.Courses.Add(courseToAdd);
            await _context.SaveChangesAsync();
            // Return the ID to use in the controller as it's not a part of the DTO
            return courseToAdd.CourseId;
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return false;
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CourseDTO>> GetAllCoursesAsync(bool includeModules = false, bool includeEnrollments = false)
        {
            IQueryable<Course> query = _context.Courses;

            if (includeModules)
            {
                query = query.Include(m => m.Modules);
            }

            if (includeEnrollments)
            {
                query = query.Include(e => e.Enrollments);
            }

            var courses = await query.ToListAsync();
            return _mapper.Map<List<CourseDTO>>(courses);
        }

        public async Task<CourseDTO> GetCourseByIdAsync(int id, bool includeModules = false, bool includeEnrollments = false)
        {
            var query = _context.Courses.AsQueryable();

            if (includeModules)
            {
                query = query.Include(m => m.Modules);
            }

            if (includeEnrollments)
            {
                query = query.Include(e => e.Enrollments);
            }

            var course = await query.FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
            {
                return null; // or throw an exception
            }

            return _mapper.Map<CourseDTO>(course);
        }

        public async Task<CourseDTO> UpdateCourseAsync(int id, CourseUpdateDTO courseDto)
        {
            var existingCourse = await _context.Courses.FindAsync(id);
            if (existingCourse == null)
            {
                throw new KeyNotFoundException("Course not found");
            }

            _mapper.Map(courseDto, existingCourse);
            await _context.SaveChangesAsync();
            return _mapper.Map<CourseDTO>(existingCourse);
        }


    }
}
