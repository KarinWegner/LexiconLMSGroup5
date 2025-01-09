using AutoMapper;
using LMS.Shared.DTOs;
using Domain.Models.Entities;
using LMS.Shared.DTOs.CourseDTOs;
using LMS.Shared.DTOs.ModuleDTOs;
namespace LMS.Infrastructure.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserForRegistrationDto, ApplicationUser>();

        CreateMap<Course, CourseDTO>();
        CreateMap<CourseCreateDTO, Course>().ReverseMap();

        CreateMap<Module, ModuleDTO>();
        CreateMap<ModuleCreateDTO, Module>();

        CreateMap<ApplicationUserDTO, ApplicationUser>();


    }
}
