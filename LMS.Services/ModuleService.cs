using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
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

        public async Task<(IEnumerable<ModuleDTO> Modules, int TotalCount)> GetModulesAsync(
                int courseId,
                bool includeActivities = false,
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

            var query = _uow.Modules.Query();

            if (includeActivities)
            {
                query = query.Include(m => m.Activities);
            }

            var (modules, totalCount) = await _uow.Modules.GetFilteredAndSortedEntitiesAsync(
                filter: filter,
                sortBy: sortBy,
                isAscending: isAscending,
                pageNr: pageNr,
                pageSize: pageSize
            );

            var moduleDTOs = _mapper.Map<IEnumerable<ModuleDTO>>(modules);

            return (moduleDTOs, totalCount);
        }

        public async Task<ModuleDTO> GetModuleByIdAsync(int id, bool includeActivities = false)
        {
            IQueryable<Domain.Models.Entities.Module> query = _uow.Modules.Query().Where(m => m.ModuleId == id);

            if (includeActivities) query = query.Include(m => m.Activities);

            var module = await query.FirstOrDefaultAsync();

            if (module == null) return null;

            return _mapper.Map<ModuleDTO>(module);
        }


        public async Task<ModuleDTO> CreateModuleAsync(ModuleCreateDTO moduleDto, int courseId)
        {
            var course = await _uow.Courses.GetByIdAsync(courseId, c => c.Modules); 
            if (course == null) return null;

            //Check dates - does it end before it starts, does it fit into the course timeline, does it overlap with other modules
            ValidateModuleDates(moduleDto);
            if (!ValidateModuleFitsCourseDate(moduleDto, course)) throw new ArgumentException("The module date must fit into the course timeline.");
            if (!ValidateModulesDoNotOverlapOnCreate(moduleDto, course)) throw new ArgumentException("The module dates can't overlap.");

            var moduleToAdd = _mapper.Map<Domain.Models.Entities.Module>(moduleDto);
            moduleToAdd.CourseId = courseId;
            await _uow.Modules.AddAsync(moduleToAdd);

            await _uow.CompleteASync();

            return _mapper.Map<ModuleDTO>(moduleToAdd);
        }

        public async Task<bool> UpdateModuleAsync(int id, int courseId, ModuleUpdateDTO moduleDto)
        {
            var existingModule = await GetModuleIfExists(id);
            if (existingModule == null) return false;

            var course = await _uow.Courses.GetByIdAsync(courseId, c => c.Modules);
            if (course == null) return false;

            //Validate dates
            ValidateModuleDates(moduleDto);
            if (!ValidateModuleFitsCourseDate(moduleDto, course)) throw new ArgumentException("The module date must fit into the course timeline.");
            if (!ValidateModulesDoNotOverlapOnUpdate(moduleDto, course)) throw new ArgumentException("The module dates can't overlap.");

            _mapper.Map(moduleDto, existingModule);

            await _uow.CompleteASync();
            return true;
        }


        public async Task<bool> DeleteModuleAsync(int id)
        {
            var module = await GetModuleIfExists(id);

            await _uow.Modules.DeleteAsync(module);
            await _uow.CompleteASync();
            return true;
        }


        private async Task<Domain.Models.Entities.Module> GetModuleIfExists(int id)
        {
            var existingModule = await _uow.Modules.GetByIdAsync(id);
            if (existingModule == null)
            {
                throw new KeyNotFoundException($"Module with ID {id} not found.");
            }
            return existingModule;
        }


        private void ValidateModuleDates(dynamic moduleDto)
        {
            if (moduleDto.EndDate < moduleDto.StartDate)
            {
                throw new ArgumentException("The course cannot end before the start date.");
            }
        }

        private bool ValidateModuleFitsCourseDate(dynamic moduleDto, Course course)
        {
            DateTime moduleStartDate = moduleDto.StartDate;
            DateTime moduleEndDate = moduleDto.EndDate;
            DateTime courseStartDate = course.StartDate;
            DateTime courseEndDate = course.EndDate;

            return moduleStartDate >= courseStartDate && moduleEndDate <= courseEndDate;
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
                if (moduleDto.StartDate.Date == existingModule.StartDate.Date ||
                    moduleDto.EndDate.Date == existingModule.EndDate.Date ||
                    moduleDto.StartDate.Date == existingModule.EndDate.Date ||
                    moduleDto.EndDate.Date == existingModule.StartDate.Date) return false;
                if (moduleDto.StartDate.Date > existingModule.StartDate.Date && moduleDto.EndDate.Date < existingModule.EndDate.Date) return false;
                if (moduleDto.StartDate.Date < existingModule.StartDate.Date && moduleDto.EndDate.Date > existingModule.EndDate.Date) return false;
                if (moduleDto.StartDate.Date < existingModule.EndDate.Date && moduleDto.EndDate.Date > existingModule.StartDate.Date) return false;
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

                if (moduleDto.StartDate.Date == existingModule.StartDate.Date ||
                    moduleDto.EndDate.Date == existingModule.EndDate.Date ||
                    moduleDto.StartDate.Date == existingModule.EndDate.Date ||
                    moduleDto.EndDate.Date == existingModule.StartDate.Date) return false;
                if (moduleDto.StartDate.Date > existingModule.StartDate.Date && moduleDto.EndDate.Date < existingModule.EndDate.Date) return false;
                if (moduleDto.StartDate.Date < existingModule.StartDate.Date && moduleDto.EndDate.Date > existingModule.EndDate.Date) return false;
                if (moduleDto.StartDate.Date < existingModule.EndDate.Date && moduleDto.EndDate.Date > existingModule.StartDate.Date) return false;
            }

            return true;
        }

    }

}
