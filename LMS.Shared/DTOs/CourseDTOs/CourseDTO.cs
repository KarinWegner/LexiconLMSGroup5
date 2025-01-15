using Domain.Models.Entities;
using LMS.Shared.DTOs.ModuleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.CourseDTOs
{
    public record CourseDTO
    {
        public int CourseId { get; init; }
        public string? Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<ApplicationUser> Enrollments { get; set; }
        = new List<ApplicationUser>();
        public IEnumerable<ModuleDTO> Modules { get; set; }
        = new List<ModuleDTO>();
    }
}
