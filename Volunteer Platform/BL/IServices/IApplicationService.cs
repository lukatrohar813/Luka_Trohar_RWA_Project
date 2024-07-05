using BL.Models;


namespace BL.IServices;

public interface IApplicationService
{
    ICollection<ApplicationDto> GetAll();

    ApplicationDto? Create(ApplicationDto application);
    ApplicationDto? GetByUserAndProject(ApplicationDto application);
    ApplicationDto? UpdateStatus(int id, string status);
    ApplicationDto? Delete(int id);

}