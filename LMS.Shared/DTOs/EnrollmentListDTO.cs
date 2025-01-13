using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs
{
    public record EnrollmentListDTO
    {
        public string CourseName {  get; set; }
        //ToDo: Add list of teachers enrolled
    }
}
