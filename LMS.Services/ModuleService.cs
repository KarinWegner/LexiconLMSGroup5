using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using Domain.Models.Responses;
using LMS.Infrastructure.Data;
using LMS.Shared.DTOs.CourseDTOs;
using LMS.Shared.DTOs.ModuleDTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ModuleService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> GetModulesAsync(
                int courseId,
                bool includeActivities = false,
                bool includeDocuments = false,
                int? pageNr = null,
                int? pageSize = null,
                string? sortBy = null,
                bool isAscending = true,
                string? filteringValue = null
                )
        {
            Expression<Func<Domain.Models.Entities.Module, bool>> filter = m =>
                m.CourseId == courseId &&
                (string.IsNullOrEmpty(filteringValue) || m.Name.Contains(filteringValue));

            var includes = new List<Expression<Func<Domain.Models.Entities.Module, object>>>();

            if (includeActivities) includes.Add(m => m.Activities);

            if (includeDocuments) includes.Add(m => m.Documents);

            var (modules, totalCount) = await _uow.Modules.GetFilteredAndSortedEntitiesAsync(
                filter: filter,
                sortBy: sortBy,
                isAscending: isAscending,
                pageNr: pageNr,
                pageSize: pageSize,
                includes: includes.ToArray()
            );

            var moduleDTOs = _mapper.Map<IEnumerable<ModuleDTO>>(modules);

            return new ApiOkResponse<(IEnumerable<ModuleDTO> module, int totalCount)> ((moduleDTOs, totalCount));
        }

        public async Task<ApiBaseResponse> GetModuleByIdAsync(int id, bool includeActivities = false, bool includeDocuments = false)
        {
            IQueryable<Domain.Models.Entities.Module> query = _uow.Modules.Query().Where(m => m.ModuleId == id);

            if (includeActivities) 
            {
                query = query.Include(m => m.Activities).ThenInclude(a => a.ActivityType);
            }
            if (includeDocuments) query = query.Include(m => m.Documents);

            var module = await query.FirstOrDefaultAsync();

            if (module == null) return new ModuleNotFoundResponse(id);

            var moduleDto =  _mapper.Map<ModuleDTO>(module);
            return new ApiOkResponse<ModuleDTO>(moduleDto);
        }


        public async Task<ApiBaseResponse> CreateModuleAsync(ModuleCreateDTO moduleDto)
        {

            if (moduleDto == null) return new BadModuleRequestResponse("Module info is required.");

            var course = await _uow.Courses.GetByIdAsync(moduleDto.CourseId, c => c.Modules); 

            if (course == null) return new CourseNotFoundResponse(moduleDto.CourseId);

            //Check dates - does it end before it starts, does it fit into the course timeline, does it overlap with other modules
            ValidateModuleDates(moduleDto);
            if (!ValidateModuleFitsCourseDate(moduleDto, course)) return new BadDateTimeFrameBreakResponse();
            if (!ValidateModulesDoNotOverlapOnCreate(moduleDto, course)) return new BadDateOverlapResponse();

            var moduleToAdd = _mapper.Map<Domain.Models.Entities.Module>(moduleDto);
           // moduleToAdd.CourseId = courseId;
            await _uow.Modules.AddAsync(moduleToAdd);

            await _uow.CompleteASync();

            var moduleToReturn = _mapper.Map<ModuleDTO>(moduleToAdd);

            return new ApiCreatedAtResponse<ModuleDTO>(moduleToReturn);
        }

        public async Task<ApiBaseResponse> UpdateModuleAsync(int id, int courseId, ModuleUpdateDTO moduleDto)
        {
            var existingModule = await _uow.Modules.GetByIdAsync(id);
            if (existingModule == null) return new ModuleNotFoundResponse(id);

            var course = await _uow.Courses.GetByIdAsync(courseId);
            if (course == null) return new CourseNotFoundResponse(courseId);

            //Validate dates
            ValidateModuleDates(moduleDto);
            if (!ValidateModuleFitsCourseDate(moduleDto, course)) return new BadDateTimeFrameBreakResponse();
            if (!ValidateModulesDoNotOverlapOnUpdate(moduleDto, course)) return new BadDateOverlapResponse();

            _mapper.Map(moduleDto, existingModule);

            await _uow.CompleteASync();
            return new ApiNoContentResponse();
        }


        public async Task<ApiBaseResponse> DeleteModuleAsync(int id)
        {
            var module = await _uow.Modules.GetByIdAsync(id);
            if(module == null) return new ModuleNotFoundResponse(id);

            await _uow.Modules.DeleteAsync(module);
            await _uow.CompleteASync();

            if (await _uow.Modules.GetByIdAsync(id) != null)
            {
                throw new ArgumentException("Module was not deleted properly");
            }
            return new ApiNoContentResponse();
        }

      
        private bool ValidateModuleDates(dynamic moduleDto)
        {
            if (moduleDto.EndDate < moduleDto.StartDate)
            {
                return false;
            }
            return true;
        }

        private bool ValidateModuleFitsCourseDate(dynamic moduleDto, Course course)
        {
            DateTime moduleStartDate = moduleDto.StartDate;
            DateTime moduleEndDate = moduleDto.EndDate;
            DateTime courseStartDate = course.StartDate;
            DateTime courseEndDate = course.EndDate;

            return moduleStartDate.Date >= courseStartDate.Date && moduleEndDate.Date <= courseEndDate.Date;
        }

        private bool ValidateModulesDoNotOverlapOnCreate(ModuleCreateDTO moduleDto, Course course)
        {
            DateTime moduleStartDate = moduleDto.StartDate;
            DateTime moduleEndDate = moduleDto.EndDate;
            var existingModules = course.Modules;

            if (existingModules == null || !existingModules.Any()) return true;

            var existingModulesList = existingModules.ToList();

            foreach (var existingModule in existingModulesList)
            {
                if (moduleStartDate.Date == existingModule.StartDate.Date ||
                    moduleEndDate.Date == existingModule.EndDate.Date ||
                    moduleStartDate.Date == existingModule.EndDate.Date ||
                    moduleEndDate.Date == existingModule.StartDate.Date) return false;
                if (moduleStartDate.Date > existingModule.StartDate.Date && moduleEndDate.Date < existingModule.EndDate.Date) return false;
                if (moduleStartDate.Date < existingModule.StartDate.Date && moduleEndDate.Date > existingModule.EndDate.Date) return false;
                if (moduleStartDate.Date < existingModule.EndDate.Date && moduleEndDate.Date > existingModule.StartDate.Date) return false;
            }

            return true;
        }


        private bool ValidateModulesDoNotOverlapOnUpdate(ModuleUpdateDTO moduleDto, Course course)
        {
            DateTime moduleStartDate = moduleDto.StartDate;
            DateTime moduleEndDate = moduleDto.EndDate;
            var existingModules = course.Modules;

            if (existingModules == null || !existingModules.Any()) return true;

            var existingModulesList = existingModules.ToList();

            foreach (var existingModule in existingModulesList)
            {
                // Skip comparison on itself
                if (moduleDto.ModuleId == existingModule.ModuleId) continue;

                if (moduleStartDate.Date == existingModule.StartDate.Date ||
                     moduleEndDate.Date == existingModule.EndDate.Date ||
                     moduleStartDate.Date == existingModule.EndDate.Date ||
                     moduleEndDate.Date == existingModule.StartDate.Date) return false;
                if (moduleStartDate.Date > existingModule.StartDate.Date && moduleEndDate.Date < existingModule.EndDate.Date) return false;
                if (moduleStartDate.Date < existingModule.StartDate.Date && moduleEndDate.Date > existingModule.EndDate.Date) return false;
                if (moduleStartDate.Date < existingModule.EndDate.Date && moduleEndDate.Date > existingModule.StartDate.Date) return false;
            }

            return true;
        }

    }

}
