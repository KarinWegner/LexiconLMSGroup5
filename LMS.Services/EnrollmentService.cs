using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using LMS.Shared.DTOs;
using LMS.Shared.DTOs.EnrollmentDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;

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
        public async Task AddEnrollment(int courseId, EnrollmentCreateDTO createDto)
        {

            var course = await _uow.Courses.Query().Where(c => c.CourseId == courseId).Include(c => c.Enrollments).FirstOrDefaultAsync();

            if (course == null)
            {
                return;
                //ToDo: Add error
                // return NotFound("Course not found");
            }

            var user = await _uow.Enrollments.FindUserByIdAsync(createDto.UserId);

            if (user == null)
            {
                return;
                // return NotFound("Student not found");
            }
                       
            if (string.IsNullOrEmpty(user.Role))
            {
                return;
                // return BadRequest("User has not been assigned a role");
            }

            if (course.Enrollments.Any(u => u.Id == createDto.UserId))
            {
                return;
               // return BadRequest("User is already enrolled in course");
            }

            if (user.Role == "Student")
            {
                
                if (await _uow.Courses.Query().Include(c => c.Enrollments).SelectMany(c => c.Enrollments).Where(c => c.Id == createDto.UserId).AnyAsync())
                {
                    return;
                    //return BadRequest("Student can only be enrolled in one course at a time.");
                }
            }                    

            await _uow.Enrollments.AddEnrollment(courseId, user);

            await _uow.CompleteASync();
           
        }

        public async Task EditEnrollment(int courseId, EnrollmentUpdateDTO updateDto)
        {
            await _uow.Enrollments.EditEnrollment(updateDto.MoveFromCourseId, updateDto.UserId, updateDto.MoveToCourseId);
             await _uow.CompleteASync();


            //Checks that user has been removed from old course
            if( await _uow.Courses.Query().Where(c => c.CourseId == updateDto.MoveFromCourseId).Include(c => c.Enrollments).SelectMany(c => c.Enrollments).Where(c => c.Id == updateDto.UserId).AnyAsync())
            {
                return;
                //ToDo: Add error
            }

            //Checks that user has been added to new course
            if (await _uow.Courses.Query().Where(c => c.CourseId == updateDto.MoveToCourseId).Include(c => c.Enrollments).SelectMany(c => c.Enrollments).Where(c => c.Id == updateDto.UserId).AnyAsync())
            {
                //ToDo: add error await
                return;
            }
        }

        public async Task<IEnumerable<EnrollmentListDTO>> GetEnrollments()
        {
            //Fetch list of all courses
            IQueryable<Course> query = _uow.Courses.Query();

           //Include all enrollments
                query = query.Include(e => e.Enrollments);
            

            var courses = await query.ToListAsync();

            var EnrollmentListDTOs = new List<EnrollmentListDTO>();

            foreach (var course in courses)
            {
                var teacherNames = await _uow.Courses.Query().Where(c => c.CourseId == course.CourseId).SelectMany(c => c.Enrollments).Where(u => u.Role == "Teacher").Select(u => u.Name).ToListAsync();
                foreach (var enrollment in course.Enrollments)
                {
                    EnrollmentListDTOs.Add(new EnrollmentListDTO
                    {
                        CourseName = course.Name,
                        CourseStart = course.StartDate,
                        CourseEnd = course.EndDate,
                        User = enrollment.Name,
                        TeacherNames = teacherNames
                    });
                }
            }
            return EnrollmentListDTOs;
        }

        public async Task<IEnumerable<EnrolledUserDTO>> GetEnrollmentsForCourse(int courseId, bool excludeTeachers = false)
        {
            IQueryable<ApplicationUser> enrollments =  _uow.Courses.Query().Where(c => c.CourseId == courseId).Include(c=>c.Enrollments).SelectMany(c=>c.Enrollments);

            if (enrollments == null)
            {
                return null;
                //ToDo: Add error handling
                // return NotFound("Course not found");
            }
            var enrolledUsers = excludeTeachers ?
                 enrollments!.Where(u => u.Role == "Student")
                 .ToList() :
                enrollments!.ToList();


            return _mapper.Map<IEnumerable<EnrolledUserDTO>>(enrolledUsers);
           
        }

        public async Task<IEnumerable<EnrollmentUserCourseListDTO>> GetUserEnrollments(string userId)
        {
            var user = await _uow.Enrollments.FindUserByIdAsync(userId);
            if (user == null)
            {
                return null; // NotFound("User not found");
            }

            var enrollmentList = await _uow.Enrollments.GetUserEnrollments(userId);

            if (enrollmentList.Count() == 0) return null; //ToDo: Add response for empty reply Ok("User has no enrollments");

            var enrollmentListDTO = new List<EnrollmentUserCourseListDTO>();
            foreach (var course in enrollmentList)
            {
                var teacherNames = await _uow.Courses.Query().Where(c => c.CourseId == course.CourseId).SelectMany(c => c.Enrollments).Where(u => u.Role == "Teacher").Select(u => u.Name).ToListAsync();
               
                    enrollmentListDTO.Add(new EnrollmentUserCourseListDTO
                    {
                        CourseName = course.Name,
                        CourseStart = course.StartDate,
                        CourseEnd = course.EndDate,
                        TeacherNames = teacherNames
                    });
                
            }


            return enrollmentListDTO;
        }

        public async Task<IEnumerable<ApplicationUserDTO>> GetUsers(bool onlyTeachers, bool onlyStudents)
        {
           var userList = await _uow.Enrollments.GetAllUsersAsync(onlyTeachers, onlyStudents);

            var userListDto = _mapper.Map<IEnumerable<ApplicationUserDTO>>(userList);

            return userListDto;
        }

        public async Task RemoveEnrollment(int courseId, string userId)
        {
            var course = await _uow.Courses.GetByIdAsync(courseId);
            if (course == null)
            {
                return; //NotFound("Course not found");
            }

            var user = await _uow.Enrollments.FindUserByIdAsync(userId);
            if (user == null)
            {
                return; //NotFound("User not found");
            }

            if (!course.Enrollments.Any(u => u.Id == userId))
                return; //BadRequest("User is not enrolled in course.");
            await _uow.Enrollments.DeleteEnrollment(courseId, userId);
            

            await _uow.CompleteASync();
        }
    }
}
