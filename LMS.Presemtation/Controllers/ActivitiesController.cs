using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using LMS.Shared.DTOs.ActivityDTOs;
using AutoMapper;
using LMS.Shared.DTOs.ModuleDTOs;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Services.Contracts;
using System.Reflection;
using System.Text.Json;

namespace LMS.Presemtation.Controllers
{
    [Route("api/activities")]
    [ApiController]
    public class ActivitiesController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IServiceManager _serviceManager;


        public ActivitiesController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper;
        }

        // GET: api/Activities/module/{moduleId}
        [HttpGet("module/{moduleId}")]
        public async Task<ActionResult> GetActivities(
            int moduleId,
            bool includeDocuments = false,
            int pageNr = 1,
            int pageSize = 10,
            string? sortBy = null,
            bool isAscending = true,
            string? filteringValue = null
            )
        {

               var response = await _serviceManager.ActivityService.GetActivitiesAsync(moduleId: moduleId,
                includeDocuments: includeDocuments,
                pageNr: pageNr,
                pageSize: pageSize,
                sortBy: sortBy,
                isAscending: isAscending,
                filteringValue: filteringValue
                );

            var (activities, totalCount) = response.GetOkResult<(IEnumerable<ActivityDTO>, int)>();
                

            var metadata = new
            {
                TotalItems = totalCount,
                PageSize = pageSize,
                CurrentPage = pageNr,
                TotalPages = (totalCount / pageSize)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            return response.Success ? Ok(response.GetOkResult<(IEnumerable<ActivityDTO>, int)>()) :
                ProcessError(response);
        }


        // GET: api/Activities/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetActivity(int id, bool includeDocuments = false)
        {
            var response = await _serviceManager.ActivityService.GetActivityByIdAsync(id, includeDocuments);

            return response.Success ? Ok(response.GetOkResult<ActivityDTO>()):
            ProcessError(response);
        }

        // PUT: api/Activities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActivity(int id, [FromBody] ActivityUpdateDTO activityDto)
        {
            if (id != activityDto.ActivityId) return BadRequest("Mismatched activity ID.");


            var response = await _serviceManager.ActivityService.UpdateActivityAsync(id, activityDto);
            return response.Success ? NoContent() : ProcessError(response);
        }

        // POST: api/Activities
        [HttpPost]
        public async Task<ActionResult> CreateActivity([FromBody] ActivityCreateDTO activityDto)
        {
            if(activityDto == null) return BadRequest("Activity info is required");
            var response = await _serviceManager.ActivityService.CreateActivityAsync(activityDto, activityDto.ModuleId);

            if (response.Success)
            {
            var moduleRes = await _serviceManager.ModuleService.GetModuleByIdAsync(activityDto.ModuleId);
            var module = moduleRes.GetOkResult<ModuleDTO>();
            var createdActivity = response.GetCreatedAtResult<ActivityDTO>();
                 
                return CreatedAtAction(nameof(GetActivity), new { module.ModuleId, id = createdActivity.ActivityId }, createdActivity);
            }

            return ProcessError(response);
        }

        //PATCH: api/Activities/5
        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchActivity(int id, int moduleId, [FromBody] JsonPatchDocument<ActivityUpdateDTO> patchDocument)
        {
            if (patchDocument == null) return BadRequest("Invalid patch document");

            var activityToPatch = await _serviceManager.ActivityService.GetActivityByIdAsync(id);
            if (activityToPatch == null) return NotFound($"Activity with {id} not found");

            var activityUpdateDto = _mapper.Map<ActivityUpdateDTO>(activityToPatch);

            patchDocument.ApplyTo(activityUpdateDto, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _serviceManager.ActivityService.UpdateActivityAsync(id, activityUpdateDto);

            return NoContent();
        }

        // DELETE: api/Activities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            var response = await _serviceManager.ActivityService.DeleteActivityAsync(id);
            return response.Success ? NoContent() : ProcessError(response);
        }

    }
}
