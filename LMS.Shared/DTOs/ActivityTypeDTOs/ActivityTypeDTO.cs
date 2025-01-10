using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.ActivityTypeDTOs
{
    public record ActivityTypeDTO
    {
        public string Name { get; init; }
        public string ActivityTypeId { get; init; }
    }
}
