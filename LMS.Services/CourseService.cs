using AutoMapper;
using Azure;
using Domain.Contracts;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using Domain.Models.Responses;
using LMS.Shared.DTOs.CourseDTOs;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Microsoft.AspNetCore.JsonPatch;

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

        public async Task<ApiBaseResponse> GetAllCoursesAsync(
            bool includeModules = false,
            bool includeEnrollments = false,
            int pageNr = 1,
            int pageSize = 10)
        {
            IQueryable<Course> query = _uow.Courses.Query();

            if (includeModules) query = query.Include(m => m.Modules);

            if (includeEnrollments) query = query.Include(e => e.Enrollments);

            //pagination
            var courses = await query
                .Skip((pageNr - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var courseDtos = _mapper.Map<IEnumerable<CourseDTO>>(courses);
            return new ApiOkResponse<IEnumerable<CourseDTO>>(courseDtos);
        }

        public async Task<ApiBaseResponse> GetCourseByIdAsync(int id, bool includeModules = false, bool includeEnrollments = false)
        {
            IQueryable<Course> query = _uow.Courses.Query().Where(c => c.CourseId == id);

            query = IncludeRelatedEntities(query, includeModules, includeEnrollments);

            var course = await query.FirstOrDefaultAsync();

            if (course == null) return new CourseNotFoundResponse(id);

            var courseDto = _mapper.Map<CourseDTO>(course);

            return new ApiOkResponse<CourseDTO>(courseDto);
        }

        public async Task<ApiBaseResponse> CreateCourseAsync(CourseCreateDTO courseDto)
        {
            try
            {
                if(!ValidateCourseDates(courseDto)) return new BadDateSequenceResponse(courseDto.StartDate, courseDto.EndDate);
                var courseToAdd = _mapper.Map<Course>(courseDto);
                await _uow.Courses.AddAsync(courseToAdd);

                await _uow.CompleteASync();

                var createdCourseToReturn =_mapper.Map<CourseDTO>(courseToAdd);

                return new ApiCreatedAtResponse<CourseDTO>(createdCourseToReturn);
            }
            
            catch(Exception ex) 
            {
                throw;
            }

        }

        public async Task<ApiBaseResponse> DeleteCourseAsync(int id)
        {
            try
            {
                var course = await _uow.Courses.GetByIdAsync(id);
                if (course == null) return new CourseNotFoundResponse(id);
                await _uow.Courses.DeleteAsync(course);
                await _uow.CompleteASync();
                return new ApiNoContentResponse();
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"An unhandled error occurred: {ex}");
            }
            
            
        }

        public async Task<ApiBaseResponse> UpdateCourseAsync(int id, CourseUpdateDTO courseDto)
        {

            try
            {
                if (!ValidateCourseDates(courseDto)) return new BadDateSequenceResponse(courseDto.StartDate, courseDto.EndDate);
                var existingCourse = await _uow.Courses.GetByIdAsync(id);
                if (existingCourse == null) return new CourseNotFoundResponse(id);
                _mapper.Map(courseDto, existingCourse);
                await _uow.CompleteASync();
                return new ApiNoContentResponse();
            }        
            catch(Exception ex)
            {
                throw new ArgumentException($"An unhandled error occurred: {ex}");
            }

        }

      
        private IQueryable<Course> IncludeRelatedEntities(
        IQueryable<Course> query, 
        bool includeModules, 
        bool includeEnrollments)
        {
            if (includeModules) query = query.Include(m => m.Modules);

            if (includeEnrollments) query = query.Include(e => e.Enrollments);

            return query;
        }

        private bool ValidateCourseDates(dynamic courseDto)
        {
            if (courseDto.EndDate < courseDto.StartDate)
            {
                return false;
            }
            return true;
        }

        


    }
}
