using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.ActivityDTOs
{
    public record ActivityUpdateDTO
    {
        [Required(ErrorMessage = "Activity name is required.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Activity requires a description")]
        [MaxLength(500, ErrorMessage = "Description can not exceed 500 characters")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "The Activity needs a start date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "The Activity needs an end date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "An activity type needs to be selected.")]
        public int ActivityTypeId { get; set; }
        //public int ActivityId { get; set; }

        public int ModuleId { get; set; }
    }
}
