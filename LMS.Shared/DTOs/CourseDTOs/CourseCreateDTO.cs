using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.CourseDTOs
{
    public record CourseCreateDTO
    {
        [Required(ErrorMessage = "Course name is required.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Course requires a description")]
        [MaxLength(500, ErrorMessage = "Description can not exceed 500 characters")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "The Course needs a start date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "The Course needs an end date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }


    }
}
