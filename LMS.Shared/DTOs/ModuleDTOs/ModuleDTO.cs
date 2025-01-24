using Domain.Models.Entities;
using LMS.Shared.DTOs.ActivityDTOs;
using LMS.Shared.DTOs.DocumentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.ModuleDTOs
{
    public record ModuleDTO : ITimeDTO
    {
        public int ModuleId { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CourseId { get; init; }

        public ICollection<ActivityDTO> Activities { get; init; }
       = new List<ActivityDTO>();

        public ICollection<DocumentDTO> Documents { get; init; }
      = new List<DocumentDTO>();
    }
}
