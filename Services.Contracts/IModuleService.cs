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
        Task<IEnumerable<ModuleDTO>> GetModulesAsync(int courseId, bool includeActivities);
        Task<ModuleDTO> GetModuleByIdAsync(int id, int courseId, bool includeActivities);
        Task<bool> UpdateModuleAsync(int id, ModuleUpdateDTO moduleDto);
        Task<ModuleDTO> CreateModuleAsync(ModuleCreateDTO moduleDto, int courseId);
        Task<bool> DeleteModuleAsync(int id);

        //Task<ModuleDTO> PatchModuleAsync(int id, int courseId, JsonPatchDocument<ModuleUpdateDTO> patchDocument);
    }

}
