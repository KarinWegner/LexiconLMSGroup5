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
using Domain.Models.Responses;
using System.Reflection.Metadata.Ecma335;
using LMS.Shared.DTOs.CourseDTOs;
using Azure;

namespace LMS.Presemtation.Controllers
{
    [Route("api/courses/{courseId}/modules")]
    [ApiController]
    public class ModulesController : ApiControllerBase
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
            ApiBaseResponse response = await _serviceManager.ModuleService.GetModulesAsync(courseId, includeActivities);

            return response.Success ?
               Ok(response.GetOkResult<IEnumerable<ModuleDTO>>()) :
               ProcessError(response);
        }

        // GET: api/courses/{courseId}/modules/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ModuleDTO>> GetModule(int id, int courseId, bool includeActivities)
        {
            var response = await _serviceManager.ModuleService.GetModuleByIdAsync(id, courseId, includeActivities);
            
            return response.Success ?
               Ok(response.GetOkResult<ModuleDTO>()) :
               ProcessError(response);
        }

        // PUT: api/courses/{courseId}/modules/{id}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModule(int id, ModuleUpdateDTO moduleDto)
        {
            var response = await _serviceManager.ModuleService.UpdateModuleAsync(id, moduleDto);
            

            return response.Success ?
                NoContent():
                ProcessError(response);
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
            var response = await _serviceManager.ModuleService.CreateModuleAsync(moduleDto, courseId);

            var createdModule = response.GetCreatedAtResult<ModuleDTO>();
            
            if (createdModule == null)
            {
                return BadRequest("Invalid module data.");
            }

            return response.Success ?
                CreatedAtAction("GetModule", new { courseId = courseId, id = createdModule.ModuleId }, createdModule) :
                ProcessError(response);
        }

        // DELETE: api/courses/{courseId}/modules/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var result = await _serviceManager.ModuleService.DeleteModuleAsync(id);
            
            
            return result.Success? NoContent() :
                ProcessError(result);
        }
    }
}
