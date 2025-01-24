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
    public class ModulesController : ApiControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IMapper _mapper;

        public ModulesController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper;
        }

        // GET: api/Modules/course/{courseId}
        [HttpGet("course/{courseId}")]
        public async Task<ActionResult> GetModules(
            int courseId, 
            bool includeActivities = false,
            bool includeDocuments = false,
            int pageNr = 1,
            int pageSize = 10,
            string? sortBy = null,
            bool isAscending = true,
            string? filteringValue = null
            )
        {
            var response = await _serviceManager.ModuleService.GetModulesAsync(
                courseId: courseId,
                includeActivities: includeActivities,
                includeDocuments: includeDocuments,
                pageNr: pageNr,
                pageSize: pageSize,
                sortBy: sortBy,
                isAscending: isAscending,
                filteringValue: filteringValue
                );
            
                var (modules, totalCount) = response.GetOkResult<(IEnumerable<ModuleDTO> modules, int totalCount)>();

            var metadata = new
            {
                TotalItems = totalCount,
                PageSize = pageSize,
                CurrentPage = pageNr,
                TotalPages = (totalCount / pageSize)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            return response.Success ? Ok(response.GetOkResult<(IEnumerable<ModuleDTO> modules, int totalCount)>()) :
                ProcessError(response);

        }


        // GET: api/modules/22
        [HttpGet("{id}")]
        public async Task<ActionResult> GetModule(int id, bool includeActivities = false, bool includeDocuments = false)
        {
                var response = await _serviceManager.ModuleService.GetModuleByIdAsync(id, includeActivities, includeDocuments);

            return response.Success ? Ok(response.GetOkResult<ModuleDTO>()) :
                ProcessError(response);
        }


        // POST: api/modules
        [HttpPost("course/{courseId}/module")]
        public async Task<ActionResult> CreateModule([FromBody] ModuleCreateDTO moduleDto)
        {
                var response = await _serviceManager.ModuleService.CreateModuleAsync(moduleDto);

            if (response.Success)
            {
                var createdModule = response.GetCreatedAtResult<ModuleDTO>();


                return CreatedAtAction(nameof(GetModule), new { moduleDto.CourseId, id = createdModule.ModuleId }, createdModule);
            }
            return ProcessError(response);
        }


        // PUT: api/modules/22
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateModule(int id, int courseId, [FromBody] ModuleUpdateDTO moduleDto)
        {
            if (id != moduleDto.ModuleId) return BadRequest("Mismatched module ID.");

                var result = await _serviceManager.ModuleService.UpdateModuleAsync(id, courseId, moduleDto);
             
            return result.Success ? NoContent() : ProcessError(result);
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

            var response = await _serviceManager.ModuleService.UpdateModuleAsync(id, courseId, moduleUpdateDto);

            return response.Success ? NoContent() : ProcessError(response);
        }



        // DELETE: api/modules/22
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var response = await _serviceManager.ModuleService.DeleteModuleAsync(id);
            return response.Success ? NoContent() : ProcessError(response);
        }


    }
}

