using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.EnrollmentDTOs
{
    public record EnrollmentCreateDTO
    {
        public int CourseId { get; set; }
        public string UserId { get; set; }
    }
}
