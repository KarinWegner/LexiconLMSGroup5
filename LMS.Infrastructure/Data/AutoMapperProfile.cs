using AutoMapper;
using LMS.Shared.DTOs;
using Domain.Models.Entities;
using LMS.Shared.DTOs.CourseDTOs;
using LMS.Shared.DTOs.ModuleDTOs;
using LMS.Shared.DTOs.ActivityDTOs;
using LMS.Shared.DTOs.ActivityTypeDTOs;
using LMS.Shared.DTOs.EnrollmentDTOs;
using LMS.Shared.DTOs.ApplicationUserDTOs;
using LMS.Shared.DTOs.DocumentDTOs;
using Microsoft.AspNetCore.Identity;

namespace LMS.Infrastructure.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserForRegistrationDto, ApplicationUser>();

       // CreateMap<Course, CourseDTO>();
        CreateMap<Course, CourseDTO>()
            .ForMember(dest => dest.Modules, opt => opt.MapFrom(src => src.Modules)) 
            .ForMember(dest => dest.Documents, opt => opt.MapFrom(src => src.Documents)) 
            .ForMember(dest => dest.Enrollments, opt => opt.MapFrom(src => src.Enrollments)); 
        CreateMap<CourseCreateDTO, Course>();
        CreateMap<CourseUpdateDTO, Course>().ReverseMap();
        CreateMap<CourseDTO, CourseUpdateDTO>();
        CreateMap<CourseCreateDTO, Course>();

        CreateMap<Module, ModuleDTO>()
            .ForMember(dest => dest.Activities, opt => opt.MapFrom(src => src.Activities))
            .ForMember(dest => dest.Documents, opt => opt.MapFrom(src => src.Documents));
        CreateMap<ModuleCreateDTO, Module>();
        CreateMap<ModuleUpdateDTO, Module>().ReverseMap();
        CreateMap<ModuleDTO, ModuleUpdateDTO>();


        CreateMap<Activity, ActivityDTO>()
          .ForMember(dest => dest.Documents, opt => opt.MapFrom(src => src.Documents))
          .ForMember(dest => dest.ActivityTypeName, opt => opt.MapFrom(src => src.ActivityType.Name));
        CreateMap<ActivityCreateDTO, Activity>();
        CreateMap<ActivityUpdateDTO, Activity>().ReverseMap();
        CreateMap<ActivityDTO, ActivityUpdateDTO>();

        CreateMap<ActivityType, ActivityTypeDTO>().ReverseMap();
        CreateMap<ActivityTypeCreateDTO, ActivityType>();
        CreateMap<ActivityTypeUpdateDTO, ActivityType>();

        CreateMap<Document, DocumentDTO>();
        CreateMap<DocumentCreateDTO, Document>();
        CreateMap<DocumentUploadDTO, Document>().ReverseMap();
        CreateMap<DocumentDTO, DocumentUploadDTO>();


        CreateMap<ApplicationUserDTO, ApplicationUser>().ReverseMap();
        CreateMap<ApplicationUser, ApplicationUserListDTO>();

        CreateMap<ApplicationUser , EnrolledUserDTO > ();
        CreateMap<IdentityRole, RoleDTO>();
        CreateMap<ApplicationUser, UserDTO>();
    }
}
