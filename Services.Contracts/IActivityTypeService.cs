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
        Task<IEnumerable<ActivityTypeDTO>> GetActivityTypesAsync();
        Task<ActivityTypeDTO> GetActivityTypeByIdAsync(int id);
        Task<ActivityTypeDTO> CreateActivityTypeAsync(ActivityTypeCreateDTO activityTypeDto);
        Task<bool> UpdateActivityTypeAsync(int id, ActivityTypeUpdateDTO activityTypeUpdateDto);
        Task<bool> DeleteActivityTypeAsync(int id);
    }
}
