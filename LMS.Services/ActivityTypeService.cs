using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using LMS.Shared.DTOs.ActivityTypeDTOs;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services
{
    public class ActivityTypeService : IActivityTypeService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ActivityTypeService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ActivityTypeDTO>> GetActivityTypesAsync()
        {
            var activityTypes = await _uow.ActivityTypes.GetAllAsync();
            return _mapper.Map<IEnumerable<ActivityTypeDTO>>(activityTypes);
        }


        public async Task<ActivityTypeDTO> GetActivityTypeByIdAsync(int id)
        {
            var activityType = await _uow.ActivityTypes.GetByIdAsync(id);
            if (activityType == null)
                return null;

            return _mapper.Map<ActivityTypeDTO>(activityType);
        }

        public async Task<ActivityTypeDTO> CreateActivityTypeAsync(ActivityTypeCreateDTO activityTypeDto)
        {
            var activityType = _mapper.Map<ActivityType>(activityTypeDto);
            await _uow.ActivityTypes.AddAsync(activityType);
            await _uow.CompleteASync();

            return _mapper.Map<ActivityTypeDTO>(activityType);
        }


        public async Task<bool> UpdateActivityTypeAsync(int id, ActivityTypeDTO activityTypeDto)
        {
            var activityType = await _uow.ActivityTypes.GetByIdAsync(id);
            if (activityType == null) return false;

            _mapper.Map(activityTypeDto, activityType);
            await _uow.CompleteASync();

            return true;
        }


        public async Task<bool> DeleteActivityTypeAsync(int id)
        {
            var activityType = await _uow.ActivityTypes.GetByIdAsync(id);
            if (activityType == null) return false;

            await _uow.ActivityTypes.DeleteAsync(activityType);
            await _uow.CompleteASync();

            return true;
        }

    }
}
