using BL.Models;

namespace BL.IServices;

public interface ISkillService
{
    ICollection<SkillDto> GetAll();
    SkillDto? GetById(int id);
    SkillDto Create(SkillDto type);
    SkillDto? Update(int id, SkillDto type);
    SkillDto? Delete(int id);
}