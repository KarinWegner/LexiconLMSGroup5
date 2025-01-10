using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.ActivityTypeDTOs
{
    public record ActivityTypeDTO
    {
        [Required(ErrorMessage = "Activity type name is required.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 20 characters.")]
        public string Name { get; init; }
        public int ActivityTypeId { get; init; }
    }
}
