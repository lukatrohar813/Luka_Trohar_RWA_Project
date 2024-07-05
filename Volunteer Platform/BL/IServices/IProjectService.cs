using BL.Models;

namespace BL.IServices;

public interface IProjectService
{
    ICollection<ProjectDto> GetAll();
    ProjectDto? GetById(int id);
    ProjectIncludeDto? GetByIdFullInclude(int id);
   
    ProjectDto? Create(ProjectDto project);
    ProjectDto? Update(int id, ProjectDto project);
    ProjectDto? Delete(int id);
    ICollection<ProjectDto> GetPaged(int page, int size);
    int GetCount();
    ICollection<ProjectDto> GetFiltered(ICollection<ProjectDto> projects, string? filterBy, string? filter);
    PaginatedResponse<ProjectDto> Search(int size, int page, string? filterNames);
    ICollection<ProjectIncludeDto> GetPagedIncludeAll(int page, int size);
    ICollection<ProjectIncludeDto> GetFilteredIncludeAll(ICollection<ProjectIncludeDto> projects, string? filterBy,
	    string? filter);


}

