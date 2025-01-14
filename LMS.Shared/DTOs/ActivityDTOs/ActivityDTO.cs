using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.ActivityDTOs
{
    public record ActivityDTO
    {
        public int ActivityId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }

        //Foreign Keys
        public int ActivityTypeId { get; init; }
        public int ModuleId { get; init; }
    }
}
