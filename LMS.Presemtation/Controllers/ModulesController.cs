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
using LMS.Shared.DTOs.ModuleDTOs;
using Bogus;

namespace LMS.Presemtation.Controllers
{
    [Route("api/courses/{courseId}/modules")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly LmsContext _context;
        private readonly IMapper _mapper;

        public ModulesController(LmsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Modules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Module>>> GetModules()
        {
            return await _context.Modules.ToListAsync();
        }

        // GET: api/Modules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Module>> GetModule(int id)
        {
            var @module = await _context.Modules.FindAsync(id);

            if (@module == null)
            {
                return NotFound();
            }

            return @module;
        }

        // PUT: api/Modules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModule(int id, Module @module)
        {
            if (id != @module.ModuleId)
            {
                return BadRequest();
            }

            _context.Entry(@module).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Modules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Module>> PostModule(ModuleCreateDTO moduleDto, int courseId)
        {
            if ((moduleDto == null)) return NotFound("No module to add was found.");
            if (!CourseExists(courseId)) return NotFound("Course could not be found.");

            Course course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == courseId);

            if (moduleDto.StartDate < course.StartDate || moduleDto.EndDate > course.EndDate) return BadRequest("Module startdate is not within the course timeframe.");
            if (moduleDto.EndDate < moduleDto.StartDate) return BadRequest("The module cannot end before it starts.");
           
            //ToDo: Add check for overlapping start/end dates

            Module module = _mapper.Map<Module>(moduleDto);
            course.Modules.Add(module);
            await _context.SaveChangesAsync();

            var createdModuleToReturn = _mapper.Map<ModuleDTO>(module);

            return CreatedAtAction("GetModule", new { courseId = courseId,id = module.ModuleId }, createdModuleToReturn);
        }

        // DELETE: api/Modules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var @module = await _context.Modules.FindAsync(id);
            if (@module == null)
            {
                return NotFound();
            }

            _context.Modules.Remove(@module);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModuleExists(int id)
        {
            return _context.Modules.Any(e => e.ModuleId == id);
        }
        private bool CourseExists(int id) 
        {
            return _context.Courses.Any(c => c.CourseId == id);
        }
    }
}
