using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using LMS.Shared.DTOs;
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
        public async Task AddEnrollment(int courseId, string userId)
        {

            var course = await _uow.Courses.Query().Where(c => c.CourseId == courseId).Include(c => c.Enrollments).FirstOrDefaultAsync();

            if (course == null)
            {
                //ToDo: Add error
               // return NotFound("Course not found");
            }

            var user = await _uow.Enrollments.FindUserByIdAsync(userId);

            if (user == null)
            {
               // return NotFound("Student not found");
            }

           

            if (string.IsNullOrEmpty(user.Role)) 
            {
               // return BadRequest("User has not been assigned a role");
            }


            if (course.Enrollments.Any(u => u.Id == userId))
            {
               // return BadRequest("User is already enrolled in course");
            }


           

            if (user.Role == "Student")
            {
                
                if (await _uow.Courses.Query().Include(c => c.Enrollments).SelectMany(c => c.Enrollments).Where(c => c.Id == userId).AnyAsync())
                {
                    //return BadRequest("Student can only be enrolled in one course at a time.");
                }
            }

            

            await _uow.Enrollments.AddEnrollment(courseId, user);

            await _uow.CompleteASync();
           
        }

        public async Task EditEnrollment(int courseId, string userId, int newCourseId)
        {
            await _uow.Enrollments.EditEnrollment(courseId, userId, newCourseId);


            //Checks that user has been removed from old course
            if( await _uow.Courses.Query().Where(c => c.CourseId == courseId).Include(c => c.Enrollments).SelectMany(c => c.Enrollments).Where(c => c.Id == userId).AnyAsync())
            {
                //ToDo: Add error
        }

            //Checks that user has been added to new course
            if (await _uow.Courses.Query().Where(c => c.CourseId == newCourseId).Include(c => c.Enrollments).SelectMany(c => c.Enrollments).Where(c => c.Id == userId).AnyAsync())
        {
            await _uow.CompleteASync();
                return;
            }
            //ToDo: add error
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
            IQueryable<ApplicationUser> enrollments = _uow.Courses.Query().Where(c => c.CourseId == courseId).Include(c=>c.Enrollments).SelectMany(c=>c.Enrollments);

            if (enrollments == null)
            {
                //ToDo: Add error handling
                // return NotFound("Course not found");
            }
            var enrolledUsers = excludeTeachers ?
                 enrollments!.Where(u => u.Role == "Student")
                 .ToList() :
                enrollments!.ToList();


            return _mapper.Map<IEnumerable<EnrolledUserDTO>>(enrolledUsers);
           
        }

        public Task<IEnumerable<EnrollmentListDTO>> GetUserEnrollments(string userId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveEnrollment(int courseId, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
