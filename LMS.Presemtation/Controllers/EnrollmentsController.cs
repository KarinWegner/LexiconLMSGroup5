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
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Services.Contracts;
using LMS.Shared.DTOs.EnrollmentDTOs;
using LMS.Shared.DTOs.ApplicationUserDTOs;
using LMS.Shared.DTOs;

namespace LMS.Presemtation.Controllers
{
    [Route("api/enrollment")]
    [ApiController]
    public class EnrollmentsController : ApiControllerBase
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
            if (enrolledUsers == null) { return NotFound("course not found"); }

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
        [HttpPut("{courseId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EditEnrollment(int courseId, EnrollmentUpdateDTO updateDTO)
        {
            if (updateDTO == null) { return NotFound("Update information not found."); }

            if (courseId != updateDTO.MoveFromCourseId) return BadRequest("Mismatched Course Id");
            if (courseId == updateDTO.MoveToCourseId) return BadRequest("Current and new course cannot have the same course Id");

            var response = await _serviceManager.EnrollmentService.EditEnrollment(courseId, updateDTO);


            return response.Success ? NoContent() :
                ProcessError(response);
        }

        /// <summary>
        /// Adds a user to a course Enrollment
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>

        // POST: api/Enrollments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{courseId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Course>> AddEnrollment(int courseId, EnrollmentCreateDTO enrollmentCreateDTO)
        {
            var result = await _serviceManager.EnrollmentService.AddEnrollment(courseId, enrollmentCreateDTO);

            return result.Success ? NoContent() :
                ProcessError(result);
;            //return CreatedAtAction("GetEnrollmentsForCourse", new { id = course.CourseId }, course);
        }

        /// <summary>
        /// Removes a user from a courses Enrollment
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        // DELETE: api/Enrollments/5
        [HttpDelete("{courseId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveEnrollment(int courseId, string userId)
        {
            var response = await _serviceManager.EnrollmentService.RemoveEnrollment(courseId, userId);
           

            return response.Success ?  NoContent():
                ProcessError(response);
        }




        /// <summary>
        /// Gets list of all courses a user is enrolled in
        /// Does not function properly in swagger but works in Postman
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EnrollmentUserCourseListDTO>>> GetUserEnrollments(string userId)
        {
            var result = await _serviceManager.EnrollmentService.GetUserEnrollments(userId);

         

            return result.Success ? Ok(result.GetOkResult <IEnumerable<EnrollmentUserCourseListDTO>>()):
                ProcessError(result);
        }

        [HttpGet("user/")]
        public async Task<ActionResult<IEnumerable<ApplicationUserDTO>>> GetUsers(string? roleFilter)
        {
            var response = await _serviceManager.EnrollmentService.GetUsers(roleFilter);

            return response.Success ? Ok(response.GetOkResult<IEnumerable<ApplicationUserListDTO>>()):
                ProcessError(response);
        }

        [HttpGet("roles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetAllRoles()
        {
            var response = await _serviceManager.EnrollmentService.GetAllRolesAsync();
            return response.Success ? Ok(response.GetOkResult<IEnumerable<RoleDTO>>()) : ProcessError(response);
        }

        [HttpGet("users/all")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var response = await _serviceManager.EnrollmentService.GetAllUsersAsync();
            return response.Success ? Ok(response.GetOkResult<IEnumerable<UserDTO>>()) : ProcessError(response);
        }
    }
}
