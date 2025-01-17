using Domain.Models.Entities;
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
        Task<IEnumerable<ModuleDTO>> GetModulesAsync(int courseId, bool includeActivities = false, int pageNr = 1, int pageSize = 10);
        Task<ModuleDTO> GetModuleByIdAsync(int id, bool includeActivities = false);
        Task<bool> UpdateModuleAsync(int id, int courseId, ModuleUpdateDTO moduleDto);
        Task<ModuleDTO> CreateModuleAsync(ModuleCreateDTO moduleDto, int courseId);
        Task<bool> DeleteModuleAsync(int id);
    }

}
