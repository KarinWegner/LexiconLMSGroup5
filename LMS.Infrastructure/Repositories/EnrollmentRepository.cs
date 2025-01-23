using Domain.Contracts;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
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
            if (course == null) throw new CourseNotFoundException(courseId);

            var user = course.Enrollments.Where(u => u.Id == userId).FirstOrDefault();
            if (user == null) throw new UserNotFoundException(userId);

            var newCourse = await _context.Courses.Where(c => c.CourseId == newCourseId).Include(c => c.Enrollments).FirstOrDefaultAsync();
            if (newCourse == null) { throw new CourseNotFoundException(newCourseId); }

            try
            {
            course.Enrollments.Remove(user);
            newCourse.Enrollments.Add(user);
            }
            catch (Exception ex)
            {
                throw new EnrollmentEditFailedException(userId, courseId, newCourseId, ex.Message);
            }

        }
        public async Task<ApplicationUser> FindUserByIdAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user;
        }

        public async Task<IEnumerable<Course>> GetUserEnrollments(string userId)
        {
            var enrollments = await UserQuery().Where(u=>u.Id == userId).Include(u=>u.Enrollments).SelectMany(u=>u.Enrollments).ToListAsync();

            return enrollments;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string? roleFilter)
        {
            IEnumerable<ApplicationUser> users = UserQuery().Where(u => u.Role == roleFilter);
                     
            return users;
        }

        public IQueryable<ApplicationUser> UserQuery()
        {
            return _context.Users.AsQueryable();
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
