using Domain.Models.Entities;
using Domain.Models.Responses;
using LMS.Shared.DTOs.ModuleDTOs;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IModuleService
    {
        Task<ApiBaseResponse> GetModulesAsync(int courseId,
                bool includeActivities = false,
                bool includeDocuments = false,
                int? pageNr = null,
                int? pageSize = null,
                string? sortBy = null,
                bool isAscending = true,
                string? filteringValue = null);
        Task<ApiBaseResponse> GetModuleByIdAsync(int id, bool includeActivities = false, bool includeDocuments = false);
        Task<ApiBaseResponse> UpdateModuleAsync(int id, int courseId, ModuleUpdateDTO moduleDto);
        Task<ApiBaseResponse> CreateModuleAsync(ModuleCreateDTO moduleDto, int courseId);
        Task<ApiBaseResponse> DeleteModuleAsync(int id);

    }

}
