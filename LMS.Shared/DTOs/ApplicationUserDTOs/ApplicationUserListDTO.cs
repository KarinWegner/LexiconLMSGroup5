using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.ApplicationUserDTOs
{
    public record ApplicationUserListDTO
    {
        public string? Id { get; init; }
        public string? Name { get; init; }
        public string? Email { get; init; }
        public int CourseID { get; init; }
        public string? Role {  get; init; }
    }
}
