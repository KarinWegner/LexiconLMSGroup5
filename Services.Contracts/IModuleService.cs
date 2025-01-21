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
        Task<(IEnumerable<ModuleDTO> Modules, int TotalCount)> GetModulesAsync(int courseId,
                bool includeActivities = false,
                bool includeDocuments = false,
                int? pageNr = null,
                int? pageSize = null,
                string? sortBy = null,
                bool isAscending = true,
                string? filteringValue = null);
        Task<ModuleDTO> GetModuleByIdAsync(int id, bool includeActivities = false, bool includeDocuments = false);
        Task<bool> UpdateModuleAsync(int id, int courseId, ModuleUpdateDTO moduleDto);
        Task<ModuleDTO> CreateModuleAsync(ModuleCreateDTO moduleDto, int courseId);
        Task<bool> DeleteModuleAsync(int id);

    }

}
