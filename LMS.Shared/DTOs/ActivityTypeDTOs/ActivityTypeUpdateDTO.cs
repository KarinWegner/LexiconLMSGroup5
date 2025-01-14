using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.ActivityTypeDTOs
{
    public class ActivityTypeUpdateDTO
    {
        public int ActivityTypeId { get; }
        public string Name { get; set; }
    }
}
