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
using Microsoft.AspNetCore.Http.HttpResults;
using Services.Contracts;

namespace LMS.Presemtation.Controllers
{
    [Route("api/enrollment")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public EnrollmentsController(IServiceManager serviceManager, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        ///
        // GET: api/Enrollments
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EnrollmentListDTO>>> GetEnrollments()
        {

            var enrollments = await _serviceManager.EnrollmentService.GetEnrollments();
                          
            return Ok(enrollments);
        }

        /// <summary>
        /// Returns list of all students and teachers enrolled in a course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Enrollments/5
        [HttpGet("{courseId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EnrolledUserDTO>>> GetEnrollmentsForCourse(int courseId, bool excludeTeachers = false)
        {
            //var course = await _context.Courses.Include(c => c.Enrollments).Where(c => c.CourseId == courseId).FirstOrDefaultAsync();

            var enrolledUsers = _serviceManager.EnrollmentService.GetEnrollmentsForCourse(courseId, excludeTeachers);
            

            return Ok(enrolledUsers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseId">The id of the course a user is currently in (and want to change to another course)</param>
        /// <param name="userId">The id of the user to move</param>
        /// <param name="newCourseId">The id of the course to move the user to</param>
        /// <returns></returns>
        /// <response code="200"></responsecode>
        // PUT: api/Enrollments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{courseid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EditEnrollment(int courseId, string userId, int newCourseId)
        {

            if (courseId == newCourseId) return BadRequest("Current and new course cannot have the same course Id");
            await _serviceManager.EnrollmentService.EditEnrollment(courseId, userId, newCourseId);


            return Ok();
        }

        //    user.Enrollments.Remove(course);
        //    user.Enrollments.Add(newCourse);


        //    await _context.SaveChangesAsync();

        //    return Ok();
        //}

        ///// <summary>
        ///// Adds a user to a course Enrollment
        ///// </summary>
        ///// <param name="course"></param>
        ///// <returns></returns>

        //// POST: api/Enrollments
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost("{courseId}")]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //public async Task<ActionResult<Course>> AddEnrollment(int courseId, string userId)
        //{
        //    var course = await _context.Courses.Where(c=>c.CourseId == courseId).Include(c => c.Enrollments).FirstOrDefaultAsync();

        //    if (course == null)
        //    {
        //        return NotFound("Course not found");
        //    }

        //    var user = await _userManager.FindByIdAsync(userId);

        //    if (user == null)
        //    {
        //        return NotFound("Student not found");
        //    }



        //    var assignedRoles = await _userManager.GetRolesAsync(user);
        //    if (assignedRoles.Count == 0) { return BadRequest("User has not been assigned a role"); }

        //    if (await _userManager.IsInRoleAsync(user, "student"))
        //    {
        //        var userEnrollments = await _context.Users.Where(u => u.Id == userId).Include(u => u.Enrollments).Select(u=>u.Enrollments).FirstOrDefaultAsync();
        //        if (userEnrollments.Count >0) 
        //        {                
        //            return BadRequest("Student can only be enrolled in one course at a time.");
        //        }
        //    }

        //    if (course.Enrollments.Any(u => u.Id == userId))
        //    {
        //        return BadRequest("User is already enrolled in course");
        //    }

        //    course.Enrollments.Add(user);

        //    await _context.SaveChangesAsync();

        //    return Created();
        //    //return CreatedAtAction("GetEnrollmentsForCourse", new { id = course.CourseId }, course);
        //}

        ///// <summary>
        ///// Removes a user from a courses Enrollment
        ///// </summary>
        ///// <param name="courseId"></param>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //// DELETE: api/Enrollments/5
        //[HttpDelete("{courseId}")]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //public async Task<IActionResult> RemoveEnrollment(int courseId, string userId)
        //{
        //    var course = await _context.Courses.FindAsync(courseId);
        //    if (course == null)
        //    {
        //        return NotFound("Course not found");
        //    }

        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //    {
        //        return NotFound("User not found");
        //    }

        //    if (!course.Enrollments.Any(u => u.Id == userId))
        //        return BadRequest("User is not enrolled in course.");

        //    course.Enrollments.Remove(user);

        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool CourseExists(int id)
        //{
        //    return _context.Courses.Any(e => e.CourseId == id);
        //}


        ///// <summary>
        ///// Gets list of all courses a user is enrolled in
        ///// Does not function properly in swagger but works in Postman
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //[HttpGet("user/{userId}")]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult<IEnumerable<EnrollmentListDTO>>> GetUserEnrollments(string userId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //    {
        //        return NotFound("User not found");
        //    }

        //    var enrollmentList = await _context.Courses.Where(c => c.Enrollments.Contains(user)).ToListAsync();

        //    if (enrollmentList.Count == 0) return Ok("User has no enrollments");
        //    var enrollmentlist = enrollmentList.Select(c=>c.Name);

        //    return Ok(enrollmentList.ToList());
        //}
    }
}
