using Domain.Contracts;
using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Infrastructure.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly LmsContext _context;
 
        public EnrollmentRepository(LmsContext context)
        {
            _context = context;
        }
        public async Task AddEnrollment(int courseId, ApplicationUser user)
        {
            var course = await _context.Courses.FindAsync(courseId);
            course.Enrollments.Add(user);

        }

        public async Task DeleteEnrollment(int courseId, string userId)
        {
            var course = await _context.Courses.Where(c=>c.CourseId == courseId).Include(c=>c.Enrollments).FirstOrDefaultAsync();
            var user =  course.Enrollments.Where(u => u.Id == userId).FirstOrDefault();

            course.Enrollments.Remove(user);
        }

        public async Task EditEnrollment(int courseId, string userId, int newCourseId)
        {
            var course = await _context.Courses.Where(c => c.CourseId == courseId).Include(c => c.Enrollments).FirstOrDefaultAsync();
            if (course == null) {/*ToDo: Add error*/ }

            var user = course.Enrollments.Where(u => u.Id == userId).FirstOrDefault();
            if (user == null) {/*ToDo: Add error*/ }

            var newCourse = await _context.Courses.Where(c => c.CourseId == newCourseId).Include(c => c.Enrollments).FirstOrDefaultAsync();
            if (course == null) {/*ToDo: Add error*/ }

            course.Enrollments.Remove(user);
            newCourse.Enrollments.Add(user);

        }
        public async Task<ApplicationUser> FindUserByIdAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user;
        }

        public async Task<IEnumerable<Course>> GetUserEnrollments(string userId)
        {
            var enrollments = await _context.Users.Where(u=>u.Id == userId).Include(u=>u.Enrollments).SelectMany(u=>u.Enrollments).ToListAsync();

            return enrollments;
        }
    }
}
