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

namespace LMS.Presemtation.Controllers
{
    [Route("api/Activitytypes")]
    [ApiController]
    public class ActivityTypesController : ControllerBase
    {
        private readonly LmsContext _context;
        private readonly IMapper _mapper;
        public ActivityTypesController(LmsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/ActivityTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityTypeDTO>>> GetActivityTypes()
        {
            var activityTypes = await _context.ActivityTypes.ToListAsync();
            var activityTypeDtos = _mapper.Map<IEnumerable<ActivityTypeDTO>>(activityTypes);
            return Ok(activityTypeDtos);
        }

        // GET: api/ActivityTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityTypeDTO>> GetActivityType(int id)
        {
            var activityType = await _context.ActivityTypes.FindAsync(id);

            if (activityType == null)
            {
                return NotFound();
            }
            var activityTypeDTO = _mapper.Map<ActivityTypeDTO>(activityType);

            return Ok(activityTypeDTO);
        }

        // PUT: api/ActivityTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivityType(int id, ActivityTypeDTO activityType)
        {
            if (id != activityType.ActivityTypeId)
            {
                return BadRequest();
            }

            if (activityType.ActivityTypeId != id) return BadRequest("Not allowed to change Activity type Id.");

            var existingActivityType = await _context.ActivityTypes.FirstOrDefaultAsync(a => a.ActivityTypeId == id);


            if (existingActivityType == null) return NotFound("Activity not found");




            _mapper.Map(activityType, existingActivityType);
            await _context.SaveChangesAsync();
           

            return NoContent();
        }

        // POST: api/ActivityTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ActivityTypeDTO>> PostActivityType(ActivityTypeCreateDTO activityTypeDto)
        {
            if(!_context.ActivityTypes.Any(a=>a.Name == activityTypeDto.Name)) return BadRequest("Activity already exists");

            ActivityType activityToAdd = _mapper.Map<ActivityType>(activityTypeDto);
            _context.ActivityTypes.Add(activityToAdd);
            await _context.SaveChangesAsync();

            var dtoToReturn = _mapper.Map<ActivityTypeDTO>(activityToAdd);
            return CreatedAtAction("GetActivityType", new { id = dtoToReturn.ActivityTypeId }, dtoToReturn);
        }

        // DELETE: api/ActivityTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivityType(int id)
        {
            var activityType = await _context.ActivityTypes.FindAsync(id);
            if (activityType == null)
            {
                return NotFound();
            }

            _context.ActivityTypes.Remove(activityType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActivityTypeExists(int id)
        {
            return _context.ActivityTypes.Any(e => e.ActivityTypeId == id);
        }
    }
}
