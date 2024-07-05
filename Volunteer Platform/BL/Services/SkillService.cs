using AutoMapper;
using BL.IServices;
using BL.Models;
using DAL.IRepositories;

namespace BL.Services;

public class SkillService : ISkillService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public SkillService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }
    public ICollection<SkillDto> GetAll()
    {
        var allSkills = _repositoryManager.Skill.GetAll();
        return _mapper.Map<ICollection<SkillDto>>(allSkills).ToList();
    }

    public SkillDto? GetById(int id)
    {
        var skillToGet = _repositoryManager.Skill.GetFirstOrDefault(t => t.Id == id);
        if (skillToGet == null) throw new InvalidOperationException("Skill does not exist");
        return _mapper.Map<SkillDto>(skillToGet);
    }

    public SkillDto Create(SkillDto skill)
    {
        var existingSkill = _repositoryManager.Skill.GetFirstOrDefault(t => t.Name == skill.Name);
        if (existingSkill != null) throw new InvalidOperationException($"Skill with Name {skill.Name} already exists");

        var newSkill = _mapper.Map<DAL.Models.Skill>(skill);
        _repositoryManager.Skill.Add(newSkill);
        _repositoryManager.Save();
        return _mapper.Map<SkillDto>(newSkill);
    }


    public SkillDto? Update(int id, SkillDto skill)
    {
        var skillToUpdate = _repositoryManager.Skill.GetFirstOrDefault(s => s.Id == id);
        if (skillToUpdate == null) throw new InvalidOperationException("Skill does not exist");
        {
            var existingSkillWithSameName =
                _repositoryManager.Skill.GetFirstOrDefault(s => s.Name == skill.Name && s.Id != id);
            if (existingSkillWithSameName != null)
                throw new InvalidOperationException($"Skill with Name {skill.Name} already exists");


            _mapper.Map(skill, skillToUpdate);
            _repositoryManager.Save();
            return _mapper.Map<SkillDto>(skillToUpdate);
        }


    }

    public SkillDto? Delete(int id)
    {
        var skillToDelete = _repositoryManager.Skill.GetFirstOrDefault(t => t.Id == id);
        if ( skillToDelete == null) throw new InvalidOperationException("Skill does not exist");



        _repositoryManager.Skill.Delete( skillToDelete);
        _repositoryManager.Save();
        return _mapper.Map<SkillDto>( skillToDelete);
    }

 

}