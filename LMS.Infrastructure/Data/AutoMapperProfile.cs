using AutoMapper;
using LMS.Shared.DTOs;
using Domain.Models.Entities;
namespace LMS.Infrastructure.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserForRegistrationDto, ApplicationUser>();

        CreateMap<Course, CourseDTO>();
        CreateMap<CourseCreateDTO, Course>().ReverseMap();

        CreateMap<ApplicationUserDTO, ApplicationUser>();


    }
}
