using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.ModuleDTOs
{
    public record ModuleUpdateDTO
    {
        [Required(ErrorMessage = "A module name is required.")]
        [MaxLength(50, ErrorMessage = "Module name cannot exceed 50 characters.")]
        public string? Name { get; init; }
        [MaxLength(500, ErrorMessage = "Module description cannot exceed 500 characters.")]
        public string? Description { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public int ModuleId { get; init; }
    }
}
