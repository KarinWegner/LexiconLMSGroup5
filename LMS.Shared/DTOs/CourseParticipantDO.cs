using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs
{
    public record CourseParticipantDO : ApplicationUserDTO
    {
        public string? ProfilePictureLink { get; set; }
        public string? Role { get; set; }

        public CourseParticipantDO(string? ProfilePictureLink, string? role, string? id, string? username, string? name, string? email) : base(id, username, name, email)
        {

            this.ProfilePictureLink = ProfilePictureLink ?? $"https://picsum.photos/{new Random().Next(10,1000)}";
            Role = role;
        }

        public CourseParticipantDO() { }
    }
}
