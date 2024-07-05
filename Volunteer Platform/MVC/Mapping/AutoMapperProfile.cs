using AutoMapper;
using BL.Models;
using DAL.Models;
using MVC.Models;


namespace MVC.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TypeDto, TypeVM>().ReverseMap();
            CreateMap<SkillDto, SkillVM>().ReverseMap();
            CreateMap<ProjectDeleteVM, ProjectIncludeDto>().ReverseMap();
            CreateMap<ProjectIncludeDto, ListAdminVM>().ReverseMap();
            CreateMap<ProjectVM, ProjectIncludeDto>().ReverseMap();
            CreateMap<ProjectDto, ProjectVM>().ReverseMap();
            CreateMap<ProjectVM, Project>().ReverseMap();
            CreateMap<ProjectCreateVM, ProjectDto>();
            CreateMap<ProjectDto, ProjectEditVM>().ReverseMap();
            CreateMap<ProjectDto, ProjectDetailsVM>();
            CreateMap<UserDto, UserEditVM>().ReverseMap();
            CreateMap<UserDto, ProfileEditVM>().ReverseMap();
            CreateMap<ProjectDto, ProjectListVM>().ReverseMap();
            CreateMap<ProjectIncludeDto, ProjectDetailsVM>().ReverseMap();
            CreateMap<ProjectDto, ProjectCreateVM>().ReverseMap();



        }
    }
}
