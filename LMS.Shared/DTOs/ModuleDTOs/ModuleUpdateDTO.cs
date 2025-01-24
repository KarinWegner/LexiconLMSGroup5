using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.ModuleDTOs
{
    public record ModuleUpdateDTO : ITimeDTO
    {
        [Required(ErrorMessage = "A module name is required.")]
        [MaxLength(50, ErrorMessage = "Module name cannot exceed 50 characters.")]
        public string? Name { get; set; }
        [MaxLength(500, ErrorMessage = "Module description cannot exceed 500 characters.")]
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ModuleId { get; set; }


        public ModuleUpdateDTO (ModuleUpdateDTO module)
        {
            Name = module.Name;
            Description = module.Description;
            StartDate = module.StartDate;
            EndDate = module.EndDate;
            ModuleId = module.ModuleId;
        }
        public ModuleUpdateDTO(ModuleUpdateDTO module, ModuleUpdateDTO thisMod)
        {
            Name = module.Name;
            Description = module.Description;
            StartDate = module.StartDate;
            EndDate = module.EndDate;
            ModuleId = thisMod.ModuleId;
        }
        public ModuleUpdateDTO (ModuleDTO module)
        {
            Name = module.Name;
            Description = module.Description;
            StartDate = module.StartDate;
            EndDate = module.EndDate;
            ModuleId = module.ModuleId;
        }
        public ModuleUpdateDTO ()
        {

        }
    }
}
