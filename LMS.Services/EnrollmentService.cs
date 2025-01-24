using AutoMapper;
using Azure;
using Domain.Contracts;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using Domain.Models.Responses;
using LMS.Shared.DTOs;
using LMS.Shared.DTOs.ApplicationUserDTOs;
using LMS.Shared.DTOs.EnrollmentDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.IO;
using static Domain.Models.Responses.UserDublicateEnrollmentResponse;

namespace LMS.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public EnrollmentService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<ApiBaseResponse> AddEnrollment(int courseId, EnrollmentCreateDTO createDto)
        {

            var course = await _uow.Courses.Query().Where(c => c.CourseId == courseId).Include(c => c.Enrollments).FirstOrDefaultAsync();

            if (course == null) 
                return new CourseNotFoundResponse(courseId);
                            
            var user = await _uow.Enrollments.FindUserByIdAsync(createDto.UserId);

            if (user == null) 
                return new UserNotFoundResponse(createDto.UserId);
                                      
            if (string.IsNullOrEmpty(user.Role)) 
                return new UserMissingRoleResponse();

            if (course.Enrollments.Any(u => u.Id == user.Id))
                return new UserDublicateEnrollmentResponse(user.Id, courseId);


            if (user.Role == "Student")
            {
                 if (course.Enrollments.Any(u => u.Id == createDto.UserId))
                return new UserEnrollmetLimitResponse(createDto.UserId);                
            }                    

            await _uow.Enrollments.AddEnrollment(courseId, user);

            await _uow.CompleteASync();

            return new ApiNoContentResponse();
        }

        public async Task<ApiBaseResponse> EditEnrollment(int courseId, EnrollmentUpdateDTO updateDto)
        {
            if (await _uow.Courses.GetByIdAsync(updateDto.MoveFromCourseId) == null)
                return new CourseNotFoundResponse(updateDto.MoveFromCourseId);
            if (await _uow.Courses.GetByIdAsync(updateDto.MoveToCourseId) == null)
                return new CourseNotFoundResponse(updateDto.MoveToCourseId);

            var user = await _uow.Enrollments.FindUserByIdAsync(updateDto.UserId);
            if (user == null) return new UserNotFoundResponse(updateDto.UserId);

            var userEnrollments = await _uow.Enrollments.GetUserEnrollments(updateDto.UserId);
            if (!userEnrollments.Any(c=>c.CourseId == courseId))
                return new UserNotEnrolledResponse();

            try
            {
            await _uow.Enrollments.EditEnrollment(updateDto.MoveFromCourseId, updateDto.UserId, updateDto.MoveToCourseId);
             await _uow.CompleteASync();

            }
            catch(CourseNotFoundException ex)
            {
                return new CourseNotFoundResponse(ex.Id);
            }
            catch(UserNotFoundException ex)
            {
                return new UserNotFoundResponse(ex.UserId);
            }
            catch (EnrollmentEditFailedException ex)
            {
                return new EnrollmentEditErrorResponse(ex.Message);
            }

            return new ApiNoContentResponse();
                        
        }

       

        public async Task<ApiBaseResponse> GetEnrollments(int pageNr, int pageSize)
        {
            //Fetch list of all courses
            IQueryable<ApplicationUser> query = _uow.Enrollments.UserQuery().OrderBy(u=>u.Name);
            int totalCount = query.Count();

           //Include all enrollments
                query = query.Include(e => e.Enrollments);

            var enrollments = await query
                .Skip((pageNr - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var EnrollmentListDTOs = new List<EnrollmentListDTO>();

            foreach (var user in enrollments)
            {
                if (EnrollmentListDTOs.Count>pageSize-1)
                {
                    break;
                }
                foreach (var course in user.Enrollments)
                {
                var teacherNames = await _uow.Courses.Query()
                    .Where(c => c.CourseId == course.CourseId)
                    .SelectMany(c => c.Enrollments)
                    .Where(u => u.Role == "Teacher")
                    .Select(u => u.Name)
                    .ToListAsync();

                    EnrollmentListDTOs.Add(new EnrollmentListDTO
                    {
                        CourseName = course.Name,
                        CourseStart = course.StartDate,
                        CourseEnd = course.EndDate,
                        User = user.Name,
                        TeacherNames = teacherNames
                        
                    });
                }
            }
            return new ApiOkResponse<(IEnumerable<EnrollmentListDTO> enrollments, int totalCount)>((EnrollmentListDTOs, totalCount));
        }

        public async Task<ApiBaseResponse> GetEnrollmentsForCourse(int courseId, bool excludeTeachers, int pageNr, int pageSize)
        {
            IQueryable<ApplicationUser> query =  _uow.Courses.Query()
                                                             .Where(c => c.CourseId == courseId)
                                                             .Include(c=>c.Enrollments)
                                                             .SelectMany(c=>c.Enrollments)
                                                             .OrderBy(u=>u.Name);

            if (query == null)
            {
                return new CourseNotFoundResponse(courseId);
            }

            int totalCount = query.Count();

            var filteredUsers = excludeTeachers ? query!.Where(u => u.Role == "Student"):
                                                  query!;

            var enrolledUsers = await query
               .Skip((pageNr - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();

            var enrollmentsDto = _mapper.Map<IEnumerable<EnrolledUserDTO>>(enrolledUsers);

            return new ApiOkResponse<(IEnumerable<EnrolledUserDTO> enrolledUsers, int totalCount)>((enrollmentsDto, totalCount));
           
        }

        public async Task<ApiBaseResponse> GetUserEnrollments(string userId)
        {
            var user = await _uow.Enrollments.FindUserByIdAsync(userId);
            if (user == null)
            {
                return new UserNotFoundResponse(userId);
            }

            var enrollmentList = await _uow.Enrollments.GetUserEnrollments(userId);

           

            var enrollmentListDTO = new List<EnrollmentUserCourseListDTO>();
            foreach (var course in enrollmentList)
            {
                var teacherNames = await _uow.Courses.Query()
                    .Where(c => c.CourseId == course.CourseId)
                    .SelectMany(c => c.Enrollments)
                    .Where(u => u.Role == "Teacher")
                    .Select(u => u.Name)
                    .ToListAsync();
               
                    enrollmentListDTO.Add(new EnrollmentUserCourseListDTO
                    {
                        CourseName = course.Name,
                        CourseId = course.CourseId,
                        CourseStart = course.StartDate,
                        CourseEnd = course.EndDate,
                        TeacherNames = teacherNames
                    });
                
            }
            if (enrollmentList.Count() == 0) return new ApiOkResponse<IEnumerable<EnrollmentUserCourseListDTO>>(enrollmentListDTO);

            return new ApiOkResponse<IEnumerable<EnrollmentUserCourseListDTO>>(enrollmentListDTO);
        }


        public async Task<ApiBaseResponse> GetUsers(string? roleFilter, int pageNr = 1, int pageSize=1)
        {
            if (roleFilter == null) roleFilter = "";
            IQueryable<ApplicationUser> query = _uow.Enrollments.UserQuery()
                                                              .Where(u => u.Role == roleFilter);

            int totalCount = query.Count();

            var userList = await query
                .Skip((pageNr - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();


            var userListDto = _mapper.Map<IEnumerable<ApplicationUserListDTO>>(userList);

            return new ApiOkResponse<(IEnumerable<ApplicationUserListDTO> userList, int totalCount)>((userListDto, totalCount));
        }

        public async Task<ApiBaseResponse> RemoveEnrollment(int courseId, string userId)
        {
            var course = await _uow.Courses.GetByIdAsync(courseId);
            if (course == null)
            {
                return new CourseNotFoundResponse(courseId);
            }

            var user = await _uow.Enrollments.FindUserByIdAsync(userId);
            if (user == null)
            {
                return new UserNotFoundResponse(userId);
            }

            if (!course.Enrollments.Any(u => u.Id == userId))
                return new UserNotEnrolledResponse();
            await _uow.Enrollments.DeleteEnrollment(courseId, userId);
            

            await _uow.CompleteASync();

            return new ApiNoContentResponse();
        }

        public async Task<ApiBaseResponse> GetAllRolesAsync()
        {
            var roles = await _uow.Enrollments.GetAllRolesAsync();
            var roleDTOs = _mapper.Map<IEnumerable<RoleDTO>>(roles);
            return new ApiOkResponse<IEnumerable<RoleDTO>>(roleDTOs);
        }

        public async Task<ApiBaseResponse> GetAllUsersAsync()
        {
            var users = await _uow.Enrollments.GetAllUsersAsync();
            var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);
            return new ApiOkResponse<IEnumerable<UserDTO>>(userDTOs);
        }

        public async Task<ApiBaseResponse> AssignRoleToUserAsync(AssignRoleDTO assignRoleDto)
        {
            

            try
            {
                await _uow.Enrollments.AssignRoleToUserAsync(assignRoleDto.Id, assignRoleDto.Role);
                await _uow.CompleteASync();
               
                return new ApiNoContentResponse();
            }
            catch (UserNotFoundException ex)
            {
              
                return new UserNotFoundResponse(ex.UserId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to assign role to user.", ex);
            }
        }
           
        
    }
}
