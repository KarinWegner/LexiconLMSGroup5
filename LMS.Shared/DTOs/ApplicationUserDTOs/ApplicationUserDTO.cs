using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.ApplicationUserDTOs
{
    public record ApplicationUserDTO
    {
        public string? Id { get; init; }
        public string? UserName { get; init; }
        public string? Name { get; init; }
        public string? Email { get; init; }

        public string? Role { get; init; }

        public ApplicationUserDTO() { }

        public ApplicationUserDTO(string? id, string? username, string? name, string? email, string? role)
        {
            Id = id;
            UserName = username;
            Name = name;
            Email = email;
            Role = role;
        }
        public ApplicationUserDTO(ApplicationUserDTO user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Name = user.Name;
            Email = user.Email;
            Role = user.Role;
        }
    }

    public record ApplicationUserResultDTO
    {
        public List<ApplicationUserDTO> Result { get; init; }
    }
}
