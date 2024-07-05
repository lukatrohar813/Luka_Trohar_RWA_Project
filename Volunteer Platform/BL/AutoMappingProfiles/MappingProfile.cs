
using AutoMapper;
using BL.Models;
using DAL.Models;



namespace BL.AutoMappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
	        CreateMap<DAL.Models.Project, BL.Models.ProjectIncludeDto>()
		        .ForMember(dest => dest.Skills,
			        opt => opt.MapFrom(src =>
				        src.ProjectSkills != null ? src.ProjectSkills.Select(ps => ps.Skill.Name) : new List<string>()))
		        .ForMember(dest => dest.Users,
			        opt => opt.MapFrom(src =>
				        src.Applications != null ? src.Applications.Select(a => a.User.Username) : new List<string>()));
            CreateMap<ProjectDto, Project>()
     .ForMember(dest => dest.Image, opt => opt.Ignore())                                                
     .ReverseMap();
            CreateMap<DAL.Models.Application, Models.ApplicationDto>().ReverseMap();
            CreateMap<ApplicationDto, Application>()
                .ForMember(dest => dest.Project, opt => opt.Ignore()) 
                .ForMember(dest => dest.User, opt => opt.Ignore());
            CreateMap<DAL.Models.ProjectSkill, Models.ProjectSkillDto>().ReverseMap();
            CreateMap<DAL.Models.Skill, Models.SkillDto>().ReverseMap();
            CreateMap<DAL.Models.Type, Models.TypeDto>().ReverseMap();
            CreateMap<DAL.Models.User, Models.UserDto>().ReverseMap();
            CreateMap<DAL.Models.Log, Models.LogDto>().ReverseMap();
            CreateMap<DAL.Models.Image, Models.ImageDto>().ReverseMap();
            CreateMap<TypeDto, DAL.Models.Type>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
           




        }
    }
}
