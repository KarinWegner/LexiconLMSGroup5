using Microsoft.AspNetCore.Mvc;
using LMS.Shared.DTOs.CourseDTOs;
using Microsoft.AspNetCore.JsonPatch;
using Services.Contracts;
using AutoMapper;
using Azure;
using Domain.Models.Responses;

namespace LMS.Presemtation.Controllers
{
    [Route("api/Courses")]
    [ApiController]
    public class CoursesController : ApiControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IMapper _mapper;

        public CoursesController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult> GetCourses(bool includeModules = false, bool includeEnrollments = false)
        {
            ApiBaseResponse response = await _serviceManager.CourseService.GetAllCoursesAsync(includeModules, includeEnrollments);
           
            return response.Success ?
               Ok(response.GetOkResult<IEnumerable<CourseDTO>>()) :
               ProcessError(response);
        }


        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCourse(int id, bool includeModules = false, bool includeEnrollments = false)
        {
            ApiBaseResponse response = await _serviceManager.CourseService.GetCourseByIdAsync(id, includeModules, includeEnrollments);

            return response.Success ?
               Ok(response.GetOkResult<CourseDTO>()) :
               ProcessError(response);
        }

        // POST: api/Courses
        [HttpPost]
        public async Task<ActionResult> CreateCourse([FromBody] CourseCreateDTO courseDto)
        {
            if (courseDto == null) return BadRequest("Course info is required.");
            var createdCourse = await _serviceManager.CourseService.CreateCourseAsync(courseDto);
            return CreatedAtAction(nameof(GetCourse), new { id = createdCourse.CourseId }, createdCourse);
        }

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseUpdateDTO courseDto)
        {
            if (id != courseDto.CourseId) return BadRequest("Mismatched Course ID.");

            try
            {
                await _serviceManager.CourseService.UpdateCourseAsync(id, courseDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Course with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                var deleted = await _serviceManager.CourseService.DeleteCourseAsync(id);
                return deleted ? NoContent() : NotFound($"Course with ID {id} was not found.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound($"Course with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchCourse(int id, [FromBody] JsonPatchDocument<CourseUpdateDTO> patchDocument)
        {
            if (patchDocument == null) return BadRequest("Invalid patch document.");
            
            var courseToPatch = await _serviceManager.CourseService.GetCourseByIdAsync(id); //Already CourseDTO
            if (courseToPatch == null) return NotFound($"Course with ID {id} not found.");

            var courseUpdateDto = _mapper.Map<CourseUpdateDTO>(courseToPatch); //Mapp it to UpdateCourseDto
            patchDocument.ApplyTo(courseUpdateDto, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            //_mapper.Map(courseUpdateDto, courseToPatch);
            await _serviceManager.CourseService.UpdateCourseAsync(id, courseUpdateDto);

            return NoContent();
        }

    }
}
