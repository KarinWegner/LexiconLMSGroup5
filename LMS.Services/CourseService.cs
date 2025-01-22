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
            bool includeDocuments = false,
            int pageNr = 1,
            int pageSize = 10)
        {
            IQueryable<Course> query = _uow.Courses.Query();
            int totalCount = query.Count();

            query = IncludeRelatedEntities(query, includeModules, includeEnrollments, includeDocuments);

            //pagination
            var courses = await query
                .Skip((pageNr - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var courseDtos = _mapper.Map<IEnumerable<CourseDTO>>(courses);
            return new ApiOkResponse<(IEnumerable<CourseDTO> courseDtos, int totalCount)>((courseDtos, totalCount));
        }

        public async Task<ApiBaseResponse> GetCourseByIdAsync(int id, bool includeModules = false, bool includeEnrollments = false, bool includeDocuments = false)
        {
            IQueryable<Course> query = _uow.Courses.Query().Where(c => c.CourseId == id);

            query = IncludeRelatedEntities(query, includeModules, includeEnrollments, includeDocuments);

            var course = await query.FirstOrDefaultAsync();

            if (course == null) return new CourseNotFoundResponse(id);

            var courseDto = _mapper.Map<CourseDTO>(course);

            return new ApiOkResponse<CourseDTO>(courseDto);
        }

        public async Task<ApiBaseResponse> CreateCourseAsync(CourseCreateDTO courseDto)
        {
            try
            {
                ValidateCourseDates(courseDto);
            }
            catch (BadDateSequenceException ex)
            {
                return new BadDateSequenceRequestResponse(ex.StartDate, ex.EndDate);
                throw;
            }
            catch(Exception ex) 
            {
                throw;
            }

            var courseToAdd = _mapper.Map<Course>(courseDto);
            await _uow.Courses.AddAsync(courseToAdd);

            await _uow.CompleteASync();

            var createdCourseToReturn =_mapper.Map<CourseDTO>(courseToAdd);

            return new ApiCreatedAtResponse<CourseDTO>(createdCourseToReturn);
        }

        public async Task<ApiBaseResponse> DeleteCourseAsync(int id)
        {
            try
            {
            var course = await GetCourseIfExists(id);
            await _uow.Courses.DeleteAsync(course);
            await _uow.CompleteASync();

            }
            catch (CourseNotFoundException)
            {
                return new CourseNotFoundResponse(id);
            }
            catch(Exception ex)
            {
                throw;
            }
            
                return new ApiNoContentResponse();
            
        }

        public async Task<ApiBaseResponse> UpdateCourseAsync(int id, CourseUpdateDTO courseDto)
        {

            try
            {
                ValidateCourseDates(courseDto);
                var existingCourse = await GetCourseIfExists(id);
                _mapper.Map(courseDto, existingCourse);

            }
            catch (BadDateSequenceException ex)
            {
                return new BadDateSequenceRequestResponse(ex.StartDate, ex.EndDate);
            }
            catch (CourseNotFoundException )
            {
                return new CourseNotFoundResponse(id);
            }           
            catch(Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }




            await _uow.CompleteASync();
            return new ApiNoContentResponse();
        }

      
        private IQueryable<Course> IncludeRelatedEntities(
        IQueryable<Course> query, 
        bool includeModules, 
        bool includeEnrollments,
        bool includeDocuments)
        {
            if (includeModules) query = query.Include(m => m.Modules);

            if (includeEnrollments) query = query.Include(e => e.Enrollments);

            if (includeDocuments) query = query.Include(d => d.Documents);

            return query;
        }

        private void ValidateCourseDates(dynamic courseDto)
        {
            if (courseDto.EndDate < courseDto.StartDate)
            {
                throw new BadDateSequenceException(courseDto.StartDate, courseDto.EndDate);
            }
        }

        private async Task<Course> GetCourseIfExists(int id)
        {
            var existingCourse = await _uow.Courses.GetByIdAsync(id);
            if (existingCourse == null)
            {
                throw new CourseNotFoundException(id);
            }
            return existingCourse;
        }


    }
}
