using Microsoft.AspNetCore.Mvc;
using LMS.Shared.DTOs.CourseDTOs;
using Microsoft.AspNetCore.JsonPatch;
using Services.Contracts;
using AutoMapper;

namespace LMS.Presemtation.Controllers
{
    [Route("api/Courses")]
    [ApiController]
    public class CoursesController : ControllerBase
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
        public async Task<ActionResult<IEnumerable<CourseDTO>>> GetCourses(bool includeModules = false, bool includeEnrollments = false)
        {
            var courses = await _serviceManager.CourseService.GetAllCoursesAsync(includeModules, includeEnrollments);
            return Ok(courses);
        }


        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDTO>> GetCourse(int id, bool includeModules = false, bool includeEnrollments = false)
        {
            var course = await _serviceManager.CourseService.GetCourseByIdAsync(id, includeModules, includeEnrollments);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }


        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, CourseUpdateDTO courseDto)
        {

            if (id != courseDto.CourseId)
            {
                return BadRequest("Mismatched Course ID.");
            }

            try
            {
                await _serviceManager.CourseService.UpdateCourseAsync(id, courseDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CourseDTO>> PostCourse(CourseCreateDTO courseDto)
        {
            if (courseDto == null)
            {
                return BadRequest("Course info is required.");
            }
            try
            {
                var createdCourse = await _serviceManager.CourseService.CreateCourseAsync(courseDto);
                return CreatedAtAction("GetCourse", new { id = createdCourse.CourseId }, createdCourse);
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
            var deleted = await _serviceManager.CourseService.DeleteCourseAsync(id);

            if (!deleted)
            {
                return NotFound($"Course with ID {id} was not found.");
            }

            return NoContent();
        }


        //[HttpPatch("{id}")]
        //public async Task<ActionResult> PatchCourse(int id, JsonPatchDocument<CourseUpdateDTO> patchDocument)
        //{
        //    if (patchDocument == null) return BadRequest("Invalid patch document.");

        //    try
        //    {
        //        var courseToPatch = await _serviceManager.CourseService.GetCourseByIdAsync(id);
        //        if (courseToPatch == null) return NotFound($"Course with ID {id} not found.");

        //        var dto = _mapper.Map<CourseUpdateDTO>(courseToPatch);
        //        patchDocument.ApplyTo(dto, ModelState);

        //        if (!ModelState.IsValid || !TryValidateModel(dto))
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        _mapper.Map(dto, courseToPatch);
        //        await _serviceManager.CourseService.UpdateCourseAsync(id, dto);

        //        return NoContent();
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred: {ex.Message}");
        //    }

        //}
    }
}
