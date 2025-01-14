using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using LMS.Shared.DTOs.ActivityDTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ActivityService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ActivityDTO>> GetActivitiesAsync(int moduleId)
        {
            var activities = await _uow.Activities
                .Query()
                .Where(a => a.ModuleId == moduleId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ActivityDTO>>(activities);
        }

        
        public async Task<ActivityDTO> GetActivityByIdAsync(int id, int moduleId)
        {
            var activity = await _uow.Activities
                .Query()
                .Where(a => a.ActivityId == id && a.ModuleId == moduleId)
                .FirstOrDefaultAsync();

            if (activity == null) return null;

            return _mapper.Map<ActivityDTO>(activity);
        }

        public async Task<ActivityDTO> CreateActivityAsync(ActivityCreateDTO activityDto, int moduleId)
        {
            var module = await _uow.Modules.GetByIdAsync(moduleId);
            if (module == null) return null;

            var activity = _mapper.Map<Activity>(activityDto);
            activity.ModuleId = moduleId; // Ensure that the moduleId is set

            await _uow.Activities.AddAsync(activity);
            await _uow.CompleteASync();

            return _mapper.Map<ActivityDTO>(activity);
        }

        // Update an existing activity
        public async Task<bool> UpdateActivityAsync(int id, ActivityUpdateDTO activityDto)
        {
            var activity = await _uow.Activities.GetByIdAsync(id);
            if (activity == null) return false;

            _mapper.Map(activityDto, activity);

            await _uow.Activities.UpdateAsync(activity);
            await _uow.CompleteASync();

            return true;
        }

        // Delete an activity
        public async Task<bool> DeleteActivityAsync(int id)
        {
            var activity = await _uow.Activities.GetByIdAsync(id);
            if (activity == null) return false;

            await _uow.Activities.DeleteAsync(activity);
            await _uow.CompleteASync();

            return true;
        }


        //public async Task<ActivityDTO> PatchActivityAsync(int id, JsonPatchDocument<ActivityUpdateDTO> patchDocument)
        //{
        //    var activity = await _uow.Activities.GetByIdAsync(id);
        //    if (activity == null) return null;

        //    var activityToPatch = _mapper.Map<ActivityUpdateDTO>(activity);
        //    patchDocument.ApplyTo(activityToPatch);

        //    _mapper.Map(activityToPatch, activity);
        //    await _uow.CompleteASync();

        //    return _mapper.Map<ActivityDTO>(activity);
        //}
    
}
}
