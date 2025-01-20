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
using System.Text.Json;

namespace LMS.Presemtation.Controllers
{
    [Route("api/Modules")]
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

        // GET: api/Modules
        [HttpGet]
        public async Task<ActionResult> GetModules(
            int courseId, 
            bool includeActivities = false,
            int pageNr = 1,
            int pageSize = 10,
            string? sortBy = null,
            bool isAscending = true,
            string? filteringValue = null
            )
        {
            var (modules, totalCount )= await _serviceManager.ModuleService.GetModulesAsync(
                courseId: courseId,
                includeActivities: includeActivities,
                pageNr: pageNr,
                pageSize: pageSize,
                sortBy: sortBy,
                isAscending: isAscending,
                filteringValue: filteringValue
                );

            var metadata = new
            {
                TotalItems = totalCount,
                PageSize = pageSize,
                CurrentPage = pageNr,
                TotalPages = (totalCount / pageSize)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            return Ok(modules);

        }


        // GET: api/modules/22
        [HttpGet("{id}")]
        public async Task<ActionResult> GetModule(int id, bool includeActivities = false)
        {
            ModuleDTO module = await _serviceManager.ModuleService.GetModuleByIdAsync(id, includeActivities);
            if (module == null) return NotFound($"Module with ID {id} not found.");
            return Ok(module);
        }


        // POST: api/modules
        [HttpPost]
        public async Task<ActionResult> CreateModule([FromBody] ModuleCreateDTO moduleDto, int courseId)
        {
            if (moduleDto == null) return BadRequest("Module info is required.");
            var createdModule = await _serviceManager.ModuleService.CreateModuleAsync(moduleDto, courseId);
            if (createdModule == null)
            {
                return BadRequest("Invalid module data.");
            }

            return CreatedAtAction(nameof(GetModule), new { courseId, id = createdModule.ModuleId }, createdModule);
        }


        // PUT: api/modules/22
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateModule(int id, int courseId, [FromBody] ModuleUpdateDTO moduleDto)
        {
            if (id != moduleDto.ModuleId) return BadRequest("Mismatched module ID.");

            var updated = await _serviceManager.ModuleService.UpdateModuleAsync(id, courseId, moduleDto);
            return updated ? NoContent() : NotFound($"Module with ID {id} was not found.");
        }


        // PATCH: api/modules/22
        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchModule(int id, int courseId, [FromBody] JsonPatchDocument<ModuleUpdateDTO> patchDocument)
        {
            if (patchDocument == null) return BadRequest("Invalid patch document.");

            var moduleToPatch = await _serviceManager.ModuleService.GetModuleByIdAsync(id, false);
            if (moduleToPatch == null) return NotFound($"Module with ID {id} not found.");

            var moduleUpdateDto = _mapper.Map<ModuleUpdateDTO>(moduleToPatch);

            patchDocument.ApplyTo(moduleUpdateDto, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _serviceManager.ModuleService.UpdateModuleAsync(id, courseId, moduleUpdateDto);

            return NoContent();
        }



        // DELETE: api/modules/22
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var deleted = await _serviceManager.ModuleService.DeleteModuleAsync(id);
            return deleted ? NoContent() : NotFound($"Module with ID {id} was not found.");
        }


    }
}

