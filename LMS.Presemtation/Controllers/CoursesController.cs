using Microsoft.AspNetCore.Mvc;
using LMS.Shared.DTOs.CourseDTOs;
using Microsoft.AspNetCore.JsonPatch;
using Services.Contracts;
using AutoMapper;
using Azure;
using Domain.Models.Responses;
using Domain.Models.Exceptions;
using LMS.Shared.DTOs.ModuleDTOs;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

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
        public async Task<ActionResult> GetCourses(
            bool includeModules = false, 
            bool includeEnrollments = false, 
            bool includeDocuments = false,
            int pageNr = 1,
            int pageSize = 10
            )
        {
            ApiBaseResponse response = await _serviceManager.CourseService.GetAllCoursesAsync(includeModules, includeEnrollments, includeDocuments, pageNr, pageSize);

            var (courseDtos, totalCount) = response.GetOkResult<(IEnumerable<CourseDTO> courseDtos, int totalCount)>();

            var metadata = new
            {
                TotalItems = totalCount,
                PageSize = pageSize,
                CurrentPage = pageNr,
                TotalPages = (totalCount / pageSize)
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            return response.Success ?
               Ok(response.GetOkResult<(IEnumerable<CourseDTO> courseDtos, int totalCount)>()) :
               ProcessError(response);
        }


        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCourse(int id, bool includeModules = false, bool includeEnrollments = false, bool includeDocuments = false)
        {
            ApiBaseResponse response = await _serviceManager.CourseService.GetCourseByIdAsync(id, includeModules, includeEnrollments, includeDocuments);

            return response.Success ?
               Ok(response.GetOkResult<CourseDTO>()) :
               ProcessError(response);
        }

        // POST: api/Courses
        [HttpPost]
        public async Task<ActionResult> CreateCourse([FromBody] CourseCreateDTO courseDto)
        {
            if (courseDto == null) return BadRequest("Course info is required.");
            var response = await _serviceManager.CourseService.CreateCourseAsync(courseDto);

            if (response.Success) 
            {
            CourseDTO createdCourse = response.GetCreatedAtResult<CourseDTO>();
            return CreatedAtAction(nameof(GetCourse), new { id = createdCourse.CourseId }, createdCourse);
            }

            return ProcessError (response);
        }

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseUpdateDTO courseDto)
        {
            if (id != courseDto.CourseId) return BadRequest("Mismatched Course ID.");

            var result =  await _serviceManager.CourseService.UpdateCourseAsync(id, courseDto);
            if (result.Success)   return NoContent();
            return ProcessError(result);
        }


        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var deleted = await _serviceManager.CourseService.DeleteCourseAsync(id);
            if (deleted.Success) return NoContent();
            return ProcessError(deleted);
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchCourse(int id, [FromBody] JsonPatchDocument<CourseUpdateDTO> patchDocument)
        {
            if (patchDocument == null) return BadRequest("Invalid patch document.");
            
            var courseToPatch = await _serviceManager.CourseService.GetCourseByIdAsync(id); 
            if (courseToPatch == null) return NotFound($"Course with ID {id} not found.");

            var courseUpdateDto = _mapper.Map<CourseUpdateDTO>(courseToPatch);
            patchDocument.ApplyTo(courseUpdateDto, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _serviceManager.CourseService.UpdateCourseAsync(id, courseUpdateDto);

            return NoContent();
        }

    }
}
