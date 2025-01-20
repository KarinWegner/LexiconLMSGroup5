using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
