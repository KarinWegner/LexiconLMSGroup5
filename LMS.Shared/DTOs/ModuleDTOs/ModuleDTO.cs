using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.ModuleDTOs
{
    public record ModuleDTO
    {
        public int ModuleId { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public int CourseId { get; init; }
    }
}
