using AutoMapper;
using LMS.Shared.DTOs;
using Domain.Models.Entities;
using LMS.Shared.DTOs.CourseDTOs;
using LMS.Shared.DTOs.ModuleDTOs;
using LMS.Shared.DTOs.ActivityDTOs;
using LMS.Shared.DTOs.ActivityTypeDTOs;
namespace LMS.Infrastructure.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserForRegistrationDto, ApplicationUser>();

        CreateMap<Course, CourseDTO>();
        CreateMap<CourseCreateDTO, Course>();

        CreateMap<Module, ModuleDTO>();
        CreateMap<ModuleCreateDTO, Module>();

        CreateMap<Activity, ActivityDTO>();
        CreateMap<ActivityCreateDTO, Activity>();

        CreateMap<ActivityType, ActivityTypeDTO>();
        CreateMap<ActivityTypeCreateDTO, ActivityType>();

        CreateMap<ApplicationUserDTO, ApplicationUser>();


    }
}
