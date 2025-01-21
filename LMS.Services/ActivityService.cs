using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using LMS.Shared.DTOs.ActivityDTOs;
using LMS.Shared.DTOs.ModuleDTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<(IEnumerable<ActivityDTO> Activities, int TotalCount)> GetActivitiesAsync(
                int moduleId,
                bool includeDocuments = false,
                int? pageNr = null,
                int? pageSize = null,
                string? sortBy = null,
                bool isAscending = true,
                string? filteringValue = null
                )
        {
            Expression<Func<Activity, bool>> filter = m =>
                m.ModuleId == moduleId &&
                (string.IsNullOrEmpty(filteringValue) || m.Name.Contains(filteringValue)); //expand to cover all properties

            var includes = new List<Expression<Func<Activity, object>>>();

            if (includeDocuments) includes.Add(m => m.Documents);

            var (activities, totalCount) = await _uow.Activities.GetFilteredAndSortedEntitiesAsync(
                filter: filter,
                sortBy: sortBy,
                isAscending: isAscending,
                pageNr: pageNr,
                pageSize: pageSize,
                includes: includes.ToArray()
            );

            var activityDTOs = _mapper.Map<IEnumerable<ActivityDTO>>(activities);

            return (activityDTOs, totalCount);
        }


        
        public async Task<ActivityDTO> GetActivityByIdAsync(int id, bool includeDocuments = false)
        {
            IQueryable<Activity> query = _uow.Activities.Query().Where(m => m.ActivityId == id);

            if (includeDocuments) query = query.Include(m => m.Documents);

            var activity = await query.FirstOrDefaultAsync();

            if (activity == null) return null;

            return _mapper.Map<ActivityDTO>(activity);
        }

        public async Task<ActivityDTO> CreateActivityAsync(ActivityCreateDTO activityDto, int moduleId)
        {
            var module = await _uow.Modules.GetByIdAsync(moduleId);
            if (module == null) return null;


            ValidateActivityDates(activityDto);
            if (!ValidateActivityFitsModuleDate(activityDto, module)) throw new ArgumentException("The activity date must fit into the module timeline.");
            if (!ValidateActivitiesDoNotOverlapOnCreate(activityDto, module)) throw new ArgumentException("The activity dates can't overlap.");

            var activity = _mapper.Map<Activity>(activityDto);
            activity.ModuleId = moduleId; // Ensure that the moduleId is set

            await _uow.Activities.AddAsync(activity);
            await _uow.CompleteASync();

            return _mapper.Map<ActivityDTO>(activity);
        }


        public async Task<bool> UpdateActivityAsync(int id, ActivityUpdateDTO activityDto)
        {
            var activity = await _uow.Activities.GetByIdAsync(id);
            if (activity == null) return false;

            var module = await _uow.Modules.GetByIdAsync(activity.ModuleId);
            if (module == null) return false;

            // Validate activity timeframe
            ValidateActivityDates(activityDto);
            if (!ValidateActivityFitsModuleDate(activityDto, module)) throw new ArgumentException("The activity date must fit into the module timeline.");
            if(!ValidateActivitiesDoNotOverlapOnUpdate(activityDto, module)) throw new ArgumentException("The activity dates can't overlap.");

            _mapper.Map(activityDto, activity);

            await _uow.CompleteASync();

            return true;
        }


        public async Task<bool> DeleteActivityAsync(int id)
        {
            var activity = await GetActivityIfExists(id);
            if (activity == null) return false;

            await _uow.Activities.DeleteAsync(activity);
            await _uow.CompleteASync();
            return true;
        }

        private async Task<Activity> GetActivityIfExists(int id)
        {
            var activity = await _uow.Activities.GetByIdAsync(id);
            if (activity == null)
            {
                throw new KeyNotFoundException($"Activity with ID {id} not found.");
            }
            return activity;
        }


        private void ValidateActivityDates(dynamic activityDto)
        {
            if (activityDto.EndDate < activityDto.StartDate)
            {
                throw new ArgumentException("The activity cannot end before the start date.");
            }
        }

        private bool ValidateActivityFitsModuleDate(dynamic activityDto, Module module)
        {
            DateTime activityStartDate = activityDto.StartDate;
            DateTime activityEndDate = activityDto.EndDate;
            DateTime moduleStartDate = module.StartDate;
            DateTime moduleEndDate = module.EndDate;

            return activityStartDate.Date >= moduleStartDate.Date && activityEndDate.Date <= moduleEndDate.Date;
        }

        private bool ValidateActivitiesDoNotOverlapOnCreate(ActivityCreateDTO activityDto, Module module)
        {
            DateTime activityStartDate = activityDto.StartDate;
            DateTime activityEndDate = activityDto.EndDate;
            var existingActivities = module.Activities;

            if (existingActivities == null || !existingActivities.Any()) return true;

            var existingActivitiesList = existingActivities.ToList();

            foreach (var existingActivity in existingActivitiesList)
            {
                if (activityStartDate.Date == existingActivity.StartDate.Date ||
                    activityEndDate.Date == existingActivity.EndDate.Date ||
                    activityStartDate.Date == existingActivity.EndDate.Date ||
                    activityEndDate.Date == existingActivity.StartDate.Date) return false;
                if (activityStartDate.Date > existingActivity.StartDate.Date && activityEndDate.Date < existingActivity.EndDate.Date) return false;
                if (activityStartDate.Date < existingActivity.StartDate.Date && activityEndDate.Date > existingActivity.EndDate.Date) return false;
                if (activityStartDate.Date < existingActivity.EndDate.Date && activityEndDate.Date > existingActivity.StartDate.Date) return false;
            }

            return true;
        }

        private bool ValidateActivitiesDoNotOverlapOnUpdate(ActivityUpdateDTO activityDto, Module module)
        {
            DateTime activityStartDate = activityDto.StartDate;
            DateTime activityEndDate = activityDto.EndDate;
            var existingActivities = module.Activities;

            if (existingActivities == null || !existingActivities.Any()) return true;

            var existingActivitiesList = existingActivities.ToList();

            foreach (var existingActivity in existingActivitiesList)
            {
                // Skip comparison on itself
                if (activityDto.ActivityId == existingActivity.ActivityId) continue;
                if (activityStartDate.Date == existingActivity.StartDate.Date ||
                    activityEndDate.Date == existingActivity.EndDate.Date ||
                    activityStartDate.Date == existingActivity.EndDate.Date ||
                    activityEndDate.Date == existingActivity.StartDate.Date) return false;
                if (activityStartDate.Date > existingActivity.StartDate.Date && activityEndDate.Date < existingActivity.EndDate.Date) return false;
                if (activityStartDate.Date < existingActivity.StartDate.Date && activityEndDate.Date > existingActivity.EndDate.Date) return false;
                if (activityStartDate.Date < existingActivity.EndDate.Date && activityEndDate.Date > existingActivity.StartDate.Date) return false;
            }

            return true;
        }

    }
}
