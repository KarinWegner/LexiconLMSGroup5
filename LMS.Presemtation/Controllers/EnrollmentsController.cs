using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using LMS.Shared.DTOs;
using AutoMapper;

namespace LMS.Presemtation.Controllers
{
    [Route("api/enrollment")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly LmsContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public EnrollmentsController(LmsContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        ///
        // GET: api/Enrollments
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        //{
        //    return await _context.Courses.ToListAsync();
        //}

        /// <summary>
        /// Returns list of all students and teachers enrolled in a course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Enrollments/5
        [HttpGet("{courseId}")]
        public async Task<ActionResult<IEnumerable<EnrolledUserDTO>>> GetEnrollmentsForCourse(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);

            if (course == null)
            {
                return NotFound("Course not found");
            }

            var enrollments = course.Enrollments.ToList();
            if (enrollments.Count == 0) return Ok("Course has no enrollments");

            var enrolledUserDto = enrollments
            .Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
            .Join(_context.Roles, ur => ur.ur.RoleId, r => r.Id, (ur, r) => new { ur, r })
            .Select(c => new EnrolledUserDTO()
            {
                Name = c.ur.u.Name,
                Role = c.r.Name
            }).ToList();

            return Ok(enrolledUserDto);
        }

        // PUT: api/Enrollments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{courseid}")]
        public async Task<IActionResult> EditEnrollment(int courseId, string userId, int newCourseId)
        {
            var course = await _context.Courses.FindAsync(courseId);

            if (course == null)
            {
                return NotFound("Course not found");
            }
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("Student not found");
            }

            var newCourse = await _context.Courses.FindAsync(newCourseId);
            if (course == null)
            {
                return NotFound("New course not found");
            }

            user.Enrollments.Remove(course);
            user.Enrollments.Add(newCourse);


            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Adds a user to a course Enrollment
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>

        // POST: api/Enrollments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{courseId}")]
        public async Task<ActionResult<Course>> AddEnrollment(int courseId, string userId)
        {
            var course = await _context.Courses.FindAsync(courseId);

            if (course == null)
            {
                return NotFound("Course not found");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("Student not found");
            }



            var assignedRoles = await _userManager.GetRolesAsync(user);
            if (assignedRoles.Count == 0) { return BadRequest("User has not been assigned a role"); }

            if (await _userManager.IsInRoleAsync(user, "student"))
            {

                if (user.Enrollments.Count > 0)
                {
                    return BadRequest("Student can only be enrolled in one course at a time.");
                }
            }

            if (course.Enrollments.Any(u => u.Id == userId))
            {
                return BadRequest("User is already enrolled in course");
            }

            course.Enrollments.Add(user);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEnrollmentsForCourse", new { id = course.CourseId }, course);
        }

        /// <summary>
        /// Removes a user from a courses Enrollment
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        // DELETE: api/Enrollments/5
        [HttpDelete("{courseId}")]
        public async Task<IActionResult> RemoveEnrollment(int courseId, string userId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return NotFound("Course not found");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            if (!course.Enrollments.Any(u => u.Id == userId))
                return BadRequest("User is not enrolled in course.");

            course.Enrollments.Remove(user);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }


        /// <summary>
        /// Gets list of all courses a user is enrolled in
        /// Does not function properly in swagger but works in Postman
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<EnrollmentListDTO>>> GetUserEnrollments(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var enrollmentList = await _context.Courses.Where(c => c.Enrollments.Contains(user)).ToListAsync();

            if (enrollmentList.Count == 0) return Ok("User has no enrollments");
            var enrollmentDto = _mapper.Map<IEnumerable<EnrollmentListDTO>>(enrollmentList);

            return Ok(enrollmentDto.ToList());
        }
    }
}
