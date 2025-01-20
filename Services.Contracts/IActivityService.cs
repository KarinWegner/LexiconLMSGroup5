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
        Task<(IEnumerable<ActivityDTO> Activities, int TotalCount)> GetActivitiesAsync(int moduleId,
            bool includeDocuments = false,
            int? pageNr = null,
            int? pageSize = null,
            string? sortBy = null,
            bool isAscending = true,
            string? filteringValue = null);
        Task<ActivityDTO> GetActivityByIdAsync(int id, bool includeDocuments = false);
        Task<ActivityDTO> CreateActivityAsync(ActivityCreateDTO activityDto, int moduleId);
        Task<bool> UpdateActivityAsync(int id, ActivityUpdateDTO activityDto);
        Task<bool> DeleteActivityAsync(int id);

       // Task<bool> IsOverlappingActivityAsync(int moduleId, int? activityId, DateTime startDate, DateTime endDate);
    }
}

