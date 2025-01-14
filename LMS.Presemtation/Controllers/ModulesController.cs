using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using AutoMapper;
using LMS.Shared.DTOs.ModuleDTOs;
using Bogus;
using LMS.Shared.DTOs.ActivityDTOs;
using Microsoft.AspNetCore.JsonPatch;
using Services.Contracts;

namespace LMS.Presemtation.Controllers
{
    [Route("api/courses/{courseId}/modules")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IMapper _mapper;

        public ModulesController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper;
        }

        // GET: api/courses/{courseId}/modules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleDTO>>> GetModules(int courseId, bool includeActivities)
        {
            var modules = await _serviceManager.ModuleService.GetModulesAsync(courseId, includeActivities);
            var modulesDTO = _mapper.Map<IEnumerable<ModuleDTO>>(modules);
            return Ok(modulesDTO);
        }

        // GET: api/courses/{courseId}/modules/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ModuleDTO>> GetModule(int id, int courseId, bool includeActivities)
        {
            var module = await _serviceManager.ModuleService.GetModuleByIdAsync(id, courseId, includeActivities);
            if (module == null)
            {
                return NotFound();
            }

            var moduleDTO = _mapper.Map<ModuleDTO>(module);
            return Ok(moduleDTO);
        }

        // PUT: api/courses/{courseId}/modules/{id}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModule(int id, ModuleUpdateDTO moduleDto)
        {
            var result = await _serviceManager.ModuleService.UpdateModuleAsync(id, moduleDto);
            if (!result)
            {
                return NotFound("Module not found");
            }

            return NoContent();
        }


        //// PATCH: api/courses/{courseId}/modules/{id}
        //[HttpPatch("{id}")]
        //public async Task<ActionResult<ModuleDTO>> PatchModule(int id, int courseId, JsonPatchDocument<ModuleUpdateDTO> patchDocument)
        //{
        //    if (_serviceManager.CourseService.GetCourseByIdAsync(courseId) == null) return NotFound("Course not found.");

        //    var moduleEntity = await _context.Modules.FirstOrDefaultAsync(a => a.ModuleId == id);
        //    if (moduleEntity == null) return NotFound("Activity not found");

        //    var ModuleToPatch = _mapper.Map<ModuleUpdateDTO>(moduleEntity);

        //    patchDocument.ApplyTo(ModuleToPatch, ModelState);

        //    if (!ModelState.IsValid) return BadRequest(ModelState);
        //    if (!TryValidateModel(ModuleToPatch)) return BadRequest(ModelState);

        //    _mapper.Map(ModuleToPatch, moduleEntity);

        //    await _context.SaveChangesAsync();

        //    return Ok(_mapper.Map<ModuleDTO>(moduleEntity));
        //}

        // POST: api/courses/{courseId}/modules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Module>> PostModule(ModuleCreateDTO moduleDto, int courseId)
        {
            var createdModule = await _serviceManager.ModuleService.CreateModuleAsync(moduleDto, courseId);
            if (createdModule == null)
            {
                return BadRequest("Invalid module data.");
            }

            return CreatedAtAction("GetModule", new { courseId = courseId, id = createdModule.ModuleId }, createdModule);
        }

        // DELETE: api/courses/{courseId}/modules/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var result = await _serviceManager.ModuleService.DeleteModuleAsync(id);
            if (!result)
            {
                return NotFound("Module not found");
            }

            return NoContent();
        }
    }
}
