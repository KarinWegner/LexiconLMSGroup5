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
        CreateMap<CourseUpdateDTO, Course>().ReverseMap();

        CreateMap<Module, ModuleDTO>();
        CreateMap<ModuleCreateDTO, Module>();
        CreateMap<ModuleUpdateDTO, Module>().ReverseMap();

        CreateMap<Activity, ActivityDTO>();
        CreateMap<ActivityCreateDTO, Activity>();
        CreateMap<ActivityUpdateDTO, Activity>().ReverseMap();

        CreateMap<ActivityType, ActivityTypeDTO>().ReverseMap();
        CreateMap<ActivityTypeCreateDTO, ActivityType>();

        CreateMap<ApplicationUserDTO, ApplicationUser>();

        CreateMap<Course, EnrollmentListDTO>()
        .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Name));
        CreateMap<(ApplicationUser user, string role), EnrolledUserDTO>()
        .ForMember(d => d.Name, opt => opt.MapFrom(s => s.user.Name))
        .ForMember(d => d.Id, opt => opt.MapFrom(s => s.user.Id))
        .ForMember(d => d.Role, opt => opt.MapFrom(s => s.role));
    }
}
