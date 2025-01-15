using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using LMS.Shared.DTOs;
using LMS.Shared.DTOs.CourseDTOs;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public Task<Course> AddEnrollment(int courseId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task EditEnrollment(int courseId, string userId, int newCourseId)
        {
            throw new NotImplementedException();
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
