using AutoMapper;
using BL.IServices;
using BL.Models;
using DAL.IRepositories;


namespace BL.Services;

public class ApplicationService : IApplicationService

{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _applicationMapper;

    public ApplicationService(IRepositoryManager repositoryManager, IMapper applicationMapper)
    {
        _repositoryManager = repositoryManager;
        _applicationMapper = applicationMapper;
    }

    public ICollection<ApplicationDto> GetAll()
    {

        var allApplications = _repositoryManager.Application.GetAll(null, "Project,User");
        var applicationDtos = _applicationMapper.Map<ICollection<ApplicationDto>>(allApplications);
        

        return applicationDtos.ToList();
    }


    public ApplicationDto? UpdateStatus(int id, string status)
    {
        var applicationToUpdate = _repositoryManager.Application.GetFirstOrDefault(a => a.Id == id);
        if (applicationToUpdate == null) throw new InvalidOperationException("Application not found");

        applicationToUpdate.Status = status;
        _repositoryManager.Save();
        return _applicationMapper.Map<ApplicationDto>(applicationToUpdate);
    }

    public ApplicationDto? Create(ApplicationDto application)
    {
      
            var existingApplication = _repositoryManager.Application.GetFirstOrDefault(a =>
                a.UserId == application.UserId && a.ProjectId == application.ProjectId);

            if (existingApplication != null)
            {
                throw new InvalidOperationException("Application already exists");
            }

            var newApplication = _applicationMapper.Map<DAL.Models.Application>(application);

            _repositoryManager.Application.Add(newApplication);
            _repositoryManager.Save();

            return _applicationMapper.Map<ApplicationDto>(newApplication);

    }

    public ApplicationDto? GetByUserAndProject(ApplicationDto application)
    {
        var applicationExists = _repositoryManager.Application.GetFirstOrDefault(a =>
            a.UserId == application.UserId && a.ProjectId == application.ProjectId);

        return applicationExists == null ? null : _applicationMapper.Map<ApplicationDto>(applicationExists);
    }
    public ApplicationDto? Delete(int id) { 
        var applicationToDelete = _repositoryManager.Application.GetFirstOrDefault(a => a.Id == id);
        if (applicationToDelete == null) return null;

        _repositoryManager.Application.Delete(applicationToDelete);
        _repositoryManager.Save();
        return _applicationMapper.Map<ApplicationDto>(applicationToDelete);}

}