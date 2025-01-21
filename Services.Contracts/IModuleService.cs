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
        Task<ApiBaseResponse> GetModulesAsync(int courseId, bool includeActivities);
        Task<ApiBaseResponse> GetModuleByIdAsync(int id, int courseId, bool includeActivities);
        Task<ApiBaseResponse> UpdateModuleAsync(int id, ModuleUpdateDTO moduleDto);
        Task<ApiBaseResponse> CreateModuleAsync(ModuleCreateDTO moduleDto, int courseId);
        Task<ApiBaseResponse> DeleteModuleAsync(int id);

        //Task<ModuleDTO> PatchModuleAsync(int id, int courseId, JsonPatchDocument<ModuleUpdateDTO> patchDocument);
    }

}
