using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs
{
    public record EnrolledUserDTO
    {
        public string Id { get; set; }
        public string Name {  get; set; }
        public string Role {  get; set; }

    }
}
