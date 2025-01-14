using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs
{
    public record ApplicationUserDTO
    {
        public string? Id { get; init; }
        public string? UserName { get; init; }
        public string? Name { get; init; }
        public string? Email { get; init; }

        public ApplicationUserDTO() { }

        public ApplicationUserDTO(string? id, string? username, string? name, string? email)
        {
            Id = id;
            UserName = username;
            Name = name;
            Email = email;
        }

    }
}
