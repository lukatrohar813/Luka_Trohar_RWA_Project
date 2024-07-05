using AutoMapper;
using BL.IServices;
using BL.Models;
using DAL.IRepositories;
using Microsoft.Extensions.Configuration;

namespace BL.Services;

public class ProjectSkillService : IProjectSkillService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _projectSkillMapper;

    public ProjectSkillService(IRepositoryManager repositoryManager, IConfiguration configuration, IMapper projectSkillMapper)
    {
        _repositoryManager = repositoryManager;
        _projectSkillMapper = projectSkillMapper;
    }



    public ICollection<ProjectSkillDto> GetAll()
    {
        var allProjectSkills = _repositoryManager.ProjectSkills.GetAll().ToList();
        var projectSkillDtos = _projectSkillMapper.Map<ICollection<ProjectSkillDto>>(allProjectSkills); 
        return projectSkillDtos;
    }
}
