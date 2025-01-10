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

namespace LMS.Presemtation.Controllers
{
    [Route("api/courses/{courseId}/modules/{moduleId}/activities")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly LmsContext _context;
        private readonly IMapper _mapper;

        public ActivitiesController(LmsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Activities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityDTO>>> GetActivities()
        {
            IEnumerable<Activity> activities = await _context.Activities.ToListAsync();
            var activitiesDto= _mapper.Map<IEnumerable<Activity>>(activities);
            return Ok(activitiesDto);
        }

        // GET: api/Activities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDTO>> GetActivity(int id)
        {
            var activity = await _context.Activities.FindAsync(id);

            if (activity == null)
            {
                return NotFound();
            }
            var activityDTO =_mapper.Map<ActivityDTO>(activity);

            return Ok(activityDTO);
        }

        // PUT: api/Activities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivity(int id, Activity activity)
        {
            if (id != activity.Id)
            {
                return BadRequest();
            }

            _context.Entry(activity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(id))
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

        // POST: api/Activities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ActivityDTO>> PostActivity(ActivityCreateDTO activityDto, int courseId, int moduleId)
        {
            if(!_context.Courses.Any(c=> c.CourseId == courseId)) return NotFound("Course not found");
            if (!_context.ActivityTypes.Any(a => a.Id == activityDto.ActivityTypeId)) return NotFound("ActivityType could not be found");

            var module = _context.Modules.FirstOrDefault(m=>m.ModuleId == moduleId);

            if(module == null) return NotFound("Module not found");
            if (module.CourseId != courseId) return BadRequest("Module is not part of selected course");

            if (activityDto.StartDate < module.StartDate || activityDto.EndDate > module.EndDate) return BadRequest("Activity start/end date is not within the course timeframe.");
            if (activityDto.EndDate < activityDto.StartDate) return BadRequest("The module cannot end before it starts.");

            Activity activityToAdd = _mapper.Map<Activity>(activityDto);

            module.Activities.Add(activityToAdd);
            await _context.SaveChangesAsync();

            ActivityDTO createdActivityToReturn = _mapper.Map<ActivityDTO>(activityToAdd);

            return CreatedAtAction("GetActivity", new {courseId = courseId, moduleId = moduleId, id = createdActivityToReturn.Id}, createdActivityToReturn);
        }

        // DELETE: api/Activities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool ActivityExists(int id)
        {
            return _context.Activities.Any(e => e.Id == id);
        }
    }
}
