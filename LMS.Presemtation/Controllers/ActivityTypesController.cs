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
using Azure;

namespace LMS.Presemtation.Controllers
{
    [Route("api/Activitytypes")]
    [ApiController]
    public class ActivityTypesController : ApiControllerBase
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
            var response = await _serviceManager.ActivityTypeService.GetActivityTypesAsync();
            return response.Success ? Ok(response.GetOkResult<IEnumerable<ActivityTypeDTO>>()):
                ProcessError(response);
        }

        // GET: api/ActivityTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityTypeDTO>> GetActivityType(int id)
        {
            var response = await _serviceManager.ActivityTypeService.GetActivityTypeByIdAsync(id);


            return response.Success ? Ok(response.GetOkResult<ActivityTypeDTO>()) :
               ProcessError(response);
        }

        // PUT: api/ActivityTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivityType(int id, ActivityTypeUpdateDTO activityTypeDto)
        {        
            var response = await _serviceManager.ActivityTypeService.UpdateActivityTypeAsync(id, activityTypeDto);

            return response.Success ? NoContent() :
               ProcessError(response);
        }

        // POST: api/ActivityTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ActivityTypeDTO>> PostActivityType(ActivityTypeCreateDTO activityTypeDto)
        {
           
            var response = await _serviceManager.ActivityTypeService.CreateActivityTypeAsync(activityTypeDto);

            if (response.Success) {
                var createdActivityType = response.GetCreatedAtResult<ActivityTypeDTO>();
                return CreatedAtAction(nameof(GetActivityType), new { id = createdActivityType.ActivityTypeId }, createdActivityType);    
            }

            return ProcessError(response);           
        }

        // DELETE: api/ActivityTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivityType(int id)
        {
            var response = await _serviceManager.ActivityTypeService.DeleteActivityTypeAsync(id);

            return response.Success ? NoContent() :
                ProcessError(response);
        }
    }
}
