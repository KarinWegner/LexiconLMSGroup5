using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using LMS.Shared.DTOs.ActivityTypeDTOs;
using AutoMapper;
using LMS.Shared.DTOs.ActivityDTOs;
using Services.Contracts;

namespace LMS.Presemtation.Controllers
{
    [Route("api/Activitytypes")]
    [ApiController]
    public class ActivityTypesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IMapper _mapper;
        public ActivityTypesController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper;
        }

        // GET: api/ActivityTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityTypeDTO>>> GetActivityTypes()
        {
            var activityTypes = await _serviceManager.ActivityTypeService.GetActivityTypesAsync();
            return Ok(activityTypes);
        }

        // GET: api/ActivityTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityTypeDTO>> GetActivityType(int id)
        {
            var activityType = await _serviceManager.ActivityTypeService.GetActivityTypeByIdAsync(id);

            if (activityType == null)
            {
                return NotFound("Activity type not found.");
            }

            return Ok(activityType);
        }

        // PUT: api/ActivityTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivityType(int id, ActivityTypeUpdateDTO activityTypeDto)
        {
            //if (id != activityTypeDto.ActivityTypeId)
            //{
            //    return BadRequest("Activity type ID mismatch.");
            //}

            var isUpdated = await _serviceManager.ActivityTypeService.UpdateActivityTypeAsync(id, activityTypeDto);

            if (!isUpdated)
            {
                return NotFound("Activity type not found.");
            }

            return NoContent();
        }

        // POST: api/ActivityTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ActivityTypeDTO>> PostActivityType(ActivityTypeCreateDTO activityTypeDto)
        {
            try
            {
                var createdActivityType = await _serviceManager.ActivityTypeService.CreateActivityTypeAsync(activityTypeDto);
                return CreatedAtAction(nameof(GetActivityType), new { id = createdActivityType.ActivityTypeId }, createdActivityType);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/ActivityTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivityType(int id)
        {
            var isDeleted = await _serviceManager.ActivityTypeService.DeleteActivityTypeAsync(id);

            if (!isDeleted)
            {
                return NotFound("Activity type not found.");
            }

            return NoContent();
        }
    }
}
