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

        public async Task<IEnumerable<ModuleDTO>> GetModulesAsync(
                int courseId,
                bool includeActivities = false,
                int pageNr = 1,
                int pageSize = 10,
                string? sortBy = null,
                bool isAscending = true,
                string? filterByName = null
                )
        {
            Expression<Func<Module, bool>> filter = m =>
                m.CourseId == courseId &&
                (string.IsNullOrEmpty(filterByName) || m.Name.Contains(filterByName));

            // Query for modules with optional sorting/pagination/filtering
            var query = _uow.Modules.Query();

            if (includeActivities)
            {
                query = query.Include(m => m.Activities);
            }

            var modules = await _uow.GetFilteredAndSortedEntitiesAsync(
                filter: filter,
                sortBy: sortBy,
                isAscending: isAscending,
                pageNr: pageNr,
                pageSize: pageSize
            );

            return _mapper.Map<IEnumerable<ModuleDTO>>(modules);

            //if (includeActivities) query = query.Include(m => m.Activities);

            //var modules = await query.Where(m => m.CourseId == courseId)
            //    .Skip((pageNr - 1) * pageSize)
            //    .Take(pageSize)
            //    .ToListAsync();
            //return _mapper.Map<IEnumerable<ModuleDTO>>(modules);
        }

        public async Task<ModuleDTO> GetModuleByIdAsync(int id, bool includeActivities = false)
        {
            IQueryable<Module> query = _uow.Modules.Query().Where(m => m.ModuleId == id);

            if (includeActivities) query = query.Include(m => m.Activities);

            var module = await query.FirstOrDefaultAsync();

            if (module == null) return null;

            return _mapper.Map<ModuleDTO>(module);
        }


        public async Task<ModuleDTO> CreateModuleAsync(ModuleCreateDTO moduleDto, int courseId)
        {
            var course = await _uow.Courses.GetByIdAsync(courseId, c => c.Modules); // does NOT return modules
            if (course == null) return null;

            //Check dates - does it end before it starts, does it fit into the course timeline, does it overlap with other modules
            ValidateModuleDates(moduleDto);
            if (!ValidateModuleFitsCourseDate(moduleDto, course)) throw new ArgumentException("The module date must fit into the course timeline.");
            if (!ValidateModulesDoNotOverlap(moduleDto, course)) throw new ArgumentException("The module dates can't overlap.");

            var moduleToAdd = _mapper.Map<Module>(moduleDto);
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
            if (!ValidateModulesDoNotOverlap(moduleDto, course)) throw new ArgumentException("The module dates can't overlap.");

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

        private async Task<Module> GetModuleIfExists(int id)
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

        private bool ValidateModulesDoNotOverlap(dynamic moduleDto, Course course)
        {
            DateTime moduleStartDate = moduleDto.StartDate;
            DateTime moduleEndDate = moduleDto.EndDate;
            var existingModules = course.Modules;

            Console.WriteLine($"moduleDto: {moduleDto.StartDate} - {moduleDto.EndDate}");
            Console.WriteLine(course.Modules);

            if (existingModules == null || !existingModules.Any()) return true;

            var existingModulesList = existingModules.ToList();

            //existingModules.Any(module => module.StartDate == moduleDto.StartDate || module.EndDate == moduleDto.EndDate);

            foreach (var existingModule in existingModulesList)
            {
                // Skip comparison if the existingModule is the same object as moduleDto - used reference instead of ID as id doesn't exist on CreateModuleDTO
              


                if (moduleDto.StartDate.Date == existingModule.StartDate.Date || moduleDto.EndDate.Date == existingModule.EndDate.Date) return false;
                if (moduleDto.StartDate.Date == existingModule.EndDate.Date || moduleDto.EndDate.Date == existingModule.StartDate.Date) return false;
                if (moduleDto.StartDate.Date > existingModule.StartDate.Date && moduleDto.EndDate.Date < existingModule.EndDate.Date) return false;
                if (moduleDto.StartDate.Date < existingModule.StartDate.Date && moduleDto.EndDate.Date > existingModule.EndDate.Date) return false;
                if (moduleDto.StartDate.Date < existingModule.EndDate.Date && moduleDto.EndDate.Date > existingModule.StartDate.Date) return false;
            }

            return true;
        }

    }

}
