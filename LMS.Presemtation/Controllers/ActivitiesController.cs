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

namespace LMS.Presemtation.Controllers
{
    [Route("api/activities")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IServiceManager _serviceManager;


        public ActivitiesController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper;
        }

        // GET: api/Activities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityDTO>>> GetActivities(int moduleId)
        {
            var activities = await _serviceManager.ActivityService.GetActivitiesAsync(moduleId);
            return Ok(activities);
        }


        // GET: api/Activities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDTO>> GetActivity(int id)
        {
            var activity = await _serviceManager.ActivityService.GetActivityByIdAsync(id);
            if (activity == null) return NotFound("Activity not found");
            return Ok(activity);
        }

        // PUT: api/Activities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivity(int id, ActivityUpdateDTO activityDto)
        {
            //if (id != activityDto.ActivityId)
            //{
            //    return BadRequest("Activity ID mismatch.");
            //}

            var isUpdated = await _serviceManager.ActivityService.UpdateActivityAsync(id, activityDto);

            if (!isUpdated)
            {
                return NotFound("Activity not found");
            }

            return NoContent();
        }

        // POST: api/Activities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ActivityDTO>> PostActivity(ActivityCreateDTO activityDto, int courseId, int moduleId)
        {
            var createdActivity = await _serviceManager.ActivityService.CreateActivityAsync(activityDto, moduleId);

            if (createdActivity == null) return BadRequest("Invalid module or course details");

            return CreatedAtAction(nameof(GetActivity), new { courseId, moduleId, id = createdActivity.ActivityId }, createdActivity);
        }

        // DELETE: api/Activities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            var isDeleted = await _serviceManager.ActivityService.DeleteActivityAsync(id);
            if (!isDeleted) return NotFound("Activity not found");
            return NoContent();
        }

        //[HttpPatch("{id}")]
        //public async Task<ActionResult<ActivityDTO>> PatchActivity(int id, int moduleId, JsonPatchDocument<ActivityUpdateDTO> patchDocument)
        //{
        //    if (patchDocument == null) return BadRequest("Invalid patch document");

        //    var updatedActivity = await _serviceManager.ActivityService.PatchActivityAsync(id, patchDocument);
        //    if (updatedActivity == null) return NotFound("Activity not found");

        //    return Ok(updatedActivity);
        //}

    }
}
