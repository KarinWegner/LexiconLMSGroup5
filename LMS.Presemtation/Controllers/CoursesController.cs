using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using AutoMapper;
using LMS.Shared.DTOs.CourseDTOs;
using LMS.Shared.DTOs.ModuleDTOs;
using LMS.Shared.DTOs.ActivityDTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace LMS.Presemtation.Controllers
{
    [Route("api/Courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly LmsContext _context;
        private readonly IMapper _mapper;

        public CoursesController(LmsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDTO>>> GetCourses()
        {
            var courses =  await _context.Courses.ToListAsync();
            var courseDTOs = _mapper.Map<IEnumerable<CourseDTO>>(courses);
            return Ok(courseDTOs);
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDTO>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }
            var courseDTO = _mapper.Map<CourseDTO>(course);
            return Ok(courseDTO);
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, CourseUpdateDTO courseDto)
        {
            if (id != courseDto.CourseId)
            {
                return BadRequest();
            }

            

            var existingCourse = await _context.Courses.FirstOrDefaultAsync(a => a.CourseId == id);

            if (existingCourse == null) return NotFound("Course not found");




            _mapper.Map(courseDto, existingCourse);

            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPatch("{id}")]
        public async Task<ActionResult<CourseDTO>> PatchCourse(int id, JsonPatchDocument<CourseUpdateDTO> patchDocument)
        {
           

            var courseEntity = await _context.Courses.FirstOrDefaultAsync(a => a.CourseId == id);
            if (courseEntity == null) return NotFound("Activity not found");

            var courseToPatch = _mapper.Map<CourseUpdateDTO>(courseEntity);

            patchDocument.ApplyTo(courseToPatch, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!TryValidateModel(courseToPatch)) return BadRequest(ModelState);

            _mapper.Map(courseToPatch, courseEntity);

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<CourseDTO>(courseEntity));
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(CourseCreateDTO courseDto)
        {
            if (courseDto == null) return NotFound("No course to add could be found.");
            if(courseDto.EndDate<courseDto.StartDate) return BadRequest("The course cannot end before the start date.");

            var courseToAdd = _mapper.Map<Course>(courseDto);
            _context.Courses.Add(courseToAdd);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = courseToAdd.CourseId }, courseToAdd);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
