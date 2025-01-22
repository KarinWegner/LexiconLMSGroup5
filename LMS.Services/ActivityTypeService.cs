using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using Domain.Models.Responses;
using LMS.Shared.DTOs.ActivityTypeDTOs;
using Microsoft.EntityFrameworkCore;
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

        public async Task<ApiBaseResponse> GetActivityTypesAsync()
        {
            var activityTypes = await _uow.ActivityTypes.GetAllAsync();
            var activityTypesDto = _mapper.Map<IEnumerable<ActivityTypeDTO>>(activityTypes);
            return new ApiOkResponse<IEnumerable<ActivityTypeDTO>>(activityTypesDto);
        }


        public async Task<ApiBaseResponse> GetActivityTypeByIdAsync(int id)
        {
            var activityType = await _uow.ActivityTypes.GetByIdAsync(id);
            if (activityType == null) return new ActivityTypeNotFoundResponse(id);
            var activityTypeDto = _mapper.Map<ActivityTypeDTO>(activityType);
            return new ApiOkResponse<ActivityTypeDTO>(activityTypeDto);
        }

        public async Task<ApiBaseResponse> CreateActivityTypeAsync(ActivityTypeCreateDTO activityTypeDto)
        {
            if (await ActivityTypeExistsAsync(activityTypeDto.Name)) throw new ArgumentException("ActivityType with the same name already exists.");

            var activityType = _mapper.Map<ActivityType>(activityTypeDto);
            await _uow.ActivityTypes.AddAsync(activityType);
            await _uow.CompleteASync();
            var createdActivityType = _mapper.Map<ActivityTypeDTO>(activityType);

            return new ApiCreatedAtResponse<ActivityTypeDTO>(createdActivityType);
        }


        public async Task<ApiBaseResponse> UpdateActivityTypeAsync(int id, ActivityTypeUpdateDTO activityTypeUpdateDto)
        {
            var activityType = await _uow.ActivityTypes.GetByIdAsync(id);
            if (activityType == null) return new ActivityTypeNotFoundResponse(id);

            if (await ActivityTypeExistsAsync(activityTypeUpdateDto.Name)) throw new ArgumentException("ActivityType with the same name already exists.");

            _mapper.Map(activityTypeUpdateDto, activityType);
            await _uow.ActivityTypes.UpdateAsync(activityType);
            await _uow.CompleteASync();

            return new ApiNoContentResponse();
        }


        public async Task<ApiBaseResponse> DeleteActivityTypeAsync(int id)
        {
            var activityType = await _uow.ActivityTypes.GetByIdAsync(id);
            if (activityType == null) return new ActivityTypeNotFoundResponse(id);

            await _uow.ActivityTypes.DeleteAsync(activityType);
            await _uow.CompleteASync();

            return new ApiNoContentResponse();
        }

        private async Task<bool> ActivityTypeExistsAsync(string name)
        {
            var query = _uow.ActivityTypes.Query().Where(at => at.Name == name);

            return await query.AnyAsync();
        }

    }
}
