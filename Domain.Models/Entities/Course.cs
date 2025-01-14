using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<ApplicationUser> Enrollments { get; set; }
        =new List<ApplicationUser>();
        public ICollection<Module> Modules { get; set; }
        = new List<Module>();
    }
}
