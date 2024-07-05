using AutoMapper;
using BL.IServices;
using BL.Models;
using DAL.IRepositories;
using DAL.Models;
using Microsoft.Extensions.Configuration;


namespace BL.Services;

public class ProjectService : IProjectService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _projectMapper;
    private readonly ILogService _logService;
    private readonly ITypeService _typeService;

    public ProjectService(IRepositoryManager repositoryManager, IConfiguration configuration, IMapper projectMapper, ILogService logService, ITypeService typeService)
    {
        _repositoryManager = repositoryManager;
        _projectMapper = projectMapper;
        _logService = logService;
        _typeService = typeService;
    }
    public ICollection<ProjectDto> GetAll()
    {
       var allProjects = _repositoryManager.Project.GetAll(includeProperties: "Type,Image");
        _logService.Log("info", "All projects were Fetched");
        return _projectMapper.Map<ICollection<ProjectDto>>(allProjects).ToList();
    }


    public ProjectDto? GetById(int id)
    {
        var requestedProject = _repositoryManager.Project.GetFirstOrDefault(p => p.Id == id);
        if (requestedProject == null) throw new InvalidOperationException("Requested project does not exist");

        _logService.Log("info", "Project with id " + id + " was Fetched");
        return _projectMapper.Map<ProjectDto>(requestedProject);

    }

    public ProjectIncludeDto? GetByIdFullInclude(int id)
    {
        var requestedProject = _repositoryManager.Project.GetFirstOrDefault(p => p.Id == id, "Type,Image,ProjectSkills.Skill,Applications.User");
        if (requestedProject == null) throw new InvalidOperationException("Requested project does not exist");

        _logService.Log("info", "Project with id " + id + " was Fetched");
        return  _projectMapper.Map<ProjectIncludeDto>(requestedProject);
    }

    public ProjectDto? Create(ProjectDto project)
    {
        var existingProject = _repositoryManager.Project.GetFirstOrDefault(u => u.Name == project.Name);
        if (existingProject != null)
        {
            _logService.Log("error", "Project with name " + project.Name + " already exists and could not be created");
            throw new InvalidOperationException("Project with that name already exists");
        }
       

        var newProject = _projectMapper.Map<Project>(project);
        newProject.ImageId = project.ImageId;
        var skills = _repositoryManager.Skill.GetAll(s => project.Skills.Contains(s.Name)).ToList();
        if (!skills.Any())
        {
            
            throw new Exception("No skills found");
        }
        var users = _repositoryManager.User.GetAll(u => project.Users.Contains(u.Username));
        newProject.ProjectSkills = skills.Select(s => new ProjectSkill
        {
            Project = newProject,
            Skill = s
        }).ToList();
        newProject.Applications = users.Select(u => new Application
        {
            Project = newProject,
            User = u
        }).ToList();



        _repositoryManager.Project.Add(newProject);
        _repositoryManager.Save();
        _logService.Log("info", "Project with name " + project.Name + " was Created");
        return _projectMapper.Map<ProjectDto>(newProject);
        
    }
    public ProjectDto? Update(int id, ProjectDto project)
    {
        var projectForUpdate = _repositoryManager.Project.GetFirstOrDefault(p => p.Id == id, "ProjectSkills.Skill");
        if (projectForUpdate == null) throw new InvalidOperationException("Project does not exist.");
        var existingProject = _repositoryManager.Project.GetFirstOrDefault(p => p.Name == project.Name && p.Id != id);
        if (existingProject != null) throw new InvalidOperationException("Project name already exists.");
        if (_typeService.GetById(project.TypeId)==null) throw new InvalidOperationException("Type does not exist.");
       
        projectForUpdate.Name = project.Name;
        projectForUpdate.Description = project.Description;
        projectForUpdate.TypeId = project.TypeId;
        projectForUpdate.ImageId = project.ImageId;
        projectForUpdate.StartDate = project.StartDate;
        projectForUpdate.EndDate = project.EndDate;

        var existingSkills = projectForUpdate.ProjectSkills.ToList();

        var projectSkillsToRemove = existingSkills
            .Where(ps => !project.Skills.Contains(ps.Skill.Name))
            .ToList();

        foreach (var projectSkill in projectSkillsToRemove)
        {
            _repositoryManager.ProjectSkills.Delete(projectSkill);
        }

        var newSkillNames = project.Skills
            .Except(existingSkills.Select(ps => ps.Skill.Name))
            .ToList();

        foreach (var newSkillName in newSkillNames)
        {
            var skillFromDb = _repositoryManager.Skill.GetFirstOrDefault(s => s.Name == newSkillName);
            if (skillFromDb == null) continue;

            projectForUpdate.ProjectSkills.Add(new ProjectSkill
            {
                Project = projectForUpdate,
                Skill = skillFromDb
            });
        }

        _repositoryManager.Save();

        _logService.Log("info", "Project with name " + project.Name + " was Updated");
        return _projectMapper.Map<ProjectDto>(projectForUpdate);
    }


    public ProjectDto? Delete(int id)
    {
        var toDelete = _repositoryManager.Project.GetFirstOrDefault(p => p.Id == id);
        if (toDelete == null) return null;

        var projectSkills = _repositoryManager.ProjectSkills.GetAll(ps => ps.ProjectId == id);
        foreach (var projectSkill in projectSkills)
        {
            _repositoryManager.ProjectSkills.Delete(projectSkill);
        }

        var applications = _repositoryManager.Application.GetAll(a => a.ProjectId == id);
        foreach (var application in applications)
        {
            _repositoryManager.Application.Delete(application);
        }

        _repositoryManager.Project.Delete(toDelete);
        _repositoryManager.Save();
        _logService.Log("info", "Project with name " + toDelete.Name + " was Deleted");
        return _projectMapper.Map<ProjectDto>(toDelete);
    }

    public ICollection<ProjectDto> GetPaged(int page, int size)
    {
      
            var allProjects = _repositoryManager.Project.GetAll(null,"Type,Image");
			var pagedProjects = allProjects.Skip((page - 1) * size).Take(size);
            var projectDtos = _projectMapper.Map<IEnumerable<ProjectDto>>(pagedProjects).ToList();
            _logService.Log("info", "All projects were Fetched");
            return projectDtos;
            
        
    }

    public int GetCount() => _repositoryManager.Project.GetAll().Count();


    public ICollection<ProjectDto> GetFiltered(ICollection<ProjectDto> projects, string? filterBy, string? filter)
    {
        filter = filter?.ToLower();

        if (string.IsNullOrWhiteSpace(filter) || string.IsNullOrWhiteSpace(filterBy))
        {
            foreach (var project in projects)
            {
                project.TypeName = _typeService.GetById(project.TypeId).ToString();
            }
            return projects;
        }

        var filterMap = new Dictionary<string, Func<ProjectDto, string>>()
        {
            
            { "name", u => u.Name },
            { "description", u => u.Description.ToString() },
            { "type", u => u.TypeName.ToString() }
         


    };

        if (filterMap.TryGetValue(filterBy.ToLower(), out var filterFunc))
        {
            projects = projects.Where(u => filterFunc(u).ToLower().Contains(filter)).ToList();
        }

        return projects;
    }

    public PaginatedResponse<ProjectDto> Search(int size, int page, string? filterNames)
    {
		if (size <= 0) size = 10;
		if (page < 0) page = 0;

		var projects = _repositoryManager.Project.GetAll();

		if (!string.IsNullOrEmpty(filterNames))
		{
			projects = projects.Where(p => p.Name.Contains(filterNames));
		}

		var totalItems = projects.Count();
		var pagedProjects = projects.Skip(size * page).Take(size).ToList();

		var projectDtos = _projectMapper.Map<IEnumerable<ProjectDto>>(pagedProjects);

		return new PaginatedResponse<ProjectDto>
		{
			Items = projectDtos,
			TotalCount = totalItems,
			Page = page,
			PageSize = size
		};
    }

    public ICollection<ProjectIncludeDto> GetPagedIncludeAll(int page, int size)
    {
		var allProjects =
			_repositoryManager.Project.GetAll(includeProperties: "Type,Image,ProjectSkills.Skill,Applications.User");
		var pagedProjects = allProjects.Skip((page - 1) * size).Take(size);
		var projectDtos = _projectMapper.Map<ICollection<ProjectIncludeDto>>(pagedProjects).ToList();

		_logService.Log("info", "All projects were Fetched");
		return projectDtos;
	}

    public ICollection<ProjectIncludeDto> GetFilteredIncludeAll(ICollection<ProjectIncludeDto> projects, string? filterBy, string? filter)
    {
		filter = filter?.ToLower();

		if (string.IsNullOrWhiteSpace(filter) || string.IsNullOrWhiteSpace(filterBy))
		{


			return projects.ToList();
		}

		var filterMap = new Dictionary<string, Func<ProjectIncludeDto, string>>()
		{

			{ "name", u => u.Name },
			{ "description", u => u.Description.ToString() },
			{ "type", u => u.TypeName.ToString() }



		};

		if (filterMap.TryGetValue(filterBy.ToLower(), out var filterFunc))
		{
            projects = projects.Where(u => filterFunc(u).ToLower().Contains(filter)).ToList();
        }

		return projects;
    }
}
