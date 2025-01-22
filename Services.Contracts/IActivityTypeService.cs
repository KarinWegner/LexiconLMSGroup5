using Domain.Models.Responses;
using LMS.Shared.DTOs.ActivityTypeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IActivityTypeService
    {
        Task<ApiBaseResponse> GetActivityTypesAsync();
        Task<ApiBaseResponse> GetActivityTypeByIdAsync(int id);
        Task<ApiBaseResponse> CreateActivityTypeAsync(ActivityTypeCreateDTO activityTypeDto);
        Task<ApiBaseResponse> UpdateActivityTypeAsync(int id, ActivityTypeUpdateDTO activityTypeUpdateDto);
        Task<ApiBaseResponse> DeleteActivityTypeAsync(int id);
    }
}
