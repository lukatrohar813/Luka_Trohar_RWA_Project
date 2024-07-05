using BL.Models;

namespace BL.IServices;

public interface IProjectSkillService
{



    ICollection<ProjectSkillDto> GetAll();


}