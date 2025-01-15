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

        public async Task<IEnumerable<ModuleDTO>> GetModulesAsync(int courseId, bool includeActivities)
        {
            IQueryable<Module> query = _uow.Modules.Query();

            if (includeActivities)
            {
                query = query.Include(m => m.Activities);
            }
            var modules = await query.Where(m => m.CourseId == courseId).ToListAsync();
            return _mapper.Map<IEnumerable<ModuleDTO>>(modules);
        }

        public async Task<ModuleDTO> GetModuleByIdAsync(int id, int courseId, bool includeActivities)
        {
            IQueryable<Module> query = _uow.Modules.Query().Where(m => m.CourseId == courseId);

            if (includeActivities)
            {
                query = query.Include(m => m.Activities);
            }

            var module = await query.FirstOrDefaultAsync();

            if (module == null)
            {
                return null;
            }

            return _mapper.Map<ModuleDTO>(module);
        }


        //public async Task<ModuleDTO> PatchModuleAsync(int id, int courseId, JsonPatchDocument<ModuleUpdateDTO> patchDocument)
        //{
        //    var moduleEntity = await _context.Modules.FirstOrDefaultAsync(m => m.ModuleId == id && m.CourseId == courseId);
        //    if (moduleEntity == null) return null;

        //    var moduleToPatch = _mapper.Map<ModuleUpdateDTO>(moduleEntity);
        //    //patchDocument.ApplyTo(moduleToPatch);

        //    //if (!TryValidateModel(moduleToPatch)) return null;

        //    //_mapper.Map(moduleToPatch, moduleEntity);
        //    //await _context.SaveChangesAsync();

        //    return _mapper.Map<ModuleDTO>(moduleEntity);
        //}

        public async Task<bool> UpdateModuleAsync(int id, ModuleUpdateDTO moduleDto)
        {
            var existingModule = await _uow.Modules.GetByIdAsync(id);
            if (existingModule == null) return false;

            _mapper.Map(moduleDto, existingModule);

            await _uow.Modules.UpdateAsync(existingModule);
            await _uow.CompleteASync();
            return true;
            
        }

        public async Task<ModuleDTO> CreateModuleAsync(ModuleCreateDTO moduleDto, int courseId)
        {
            var course = await _uow.Courses.GetByIdAsync(courseId);
            if (course == null) return null;

            var moduleToAdd = _mapper.Map<Module>(moduleDto);
            moduleToAdd.CourseId = courseId;
            await _uow.Modules.AddAsync(moduleToAdd);

            await _uow.CompleteASync();

            return _mapper.Map<ModuleDTO>(moduleToAdd);
        }

        public async Task<bool> DeleteModuleAsync(int id)
        {
            var module = await _uow.Modules.GetByIdAsync(id);
            if (module == null) return false;

            await _uow.Modules.DeleteAsync(module);
            await _uow.CompleteASync();
            return true;
        }

    }

}
