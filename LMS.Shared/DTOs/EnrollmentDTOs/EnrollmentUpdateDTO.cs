using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.EnrollmentDTOs
{
    public record EnrollmentUpdateDTO
    {
        public int MoveFromCourseId { get; set; }
        public string UserId { get; set; }
        public int MoveToCourseId { get; set; }
    }
}
