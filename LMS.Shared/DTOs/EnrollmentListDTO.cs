using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs
{
    public record EnrollmentListDTO
    {
        public string CourseName {  get; init; }
        public string User { get; init; }
        public DateTime CourseStart { get; init; }
        public DateTime CourseEnd { get; init; }
        public List<string> TeacherNames { get; init; }
        //ToDo: Add list of teachers enrolled
    }
}
