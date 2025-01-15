using LMS.Shared.DTOs.ActivityDTOs;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IActivityService
    {
        Task<IEnumerable<ActivityDTO>> GetActivitiesAsync(int moduleId);
        Task<ActivityDTO> GetActivityByIdAsync(int id);
        Task<ActivityDTO> CreateActivityAsync(ActivityCreateDTO activityDto, int moduleId);
        Task<bool> UpdateActivityAsync(int id, ActivityUpdateDTO activityDto);
        Task<bool> DeleteActivityAsync(int id);
        Task<ActivityDTO> PatchActivityAsync(int id, JsonPatchDocument<ActivityUpdateDTO> patchDocument);

       // Task<bool> IsOverlappingActivityAsync(int moduleId, int? activityId, DateTime startDate, DateTime endDate);
    }
}

