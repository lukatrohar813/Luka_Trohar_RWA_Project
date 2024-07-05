namespace DAL.IRepositories;

public interface IRepositoryManager
{
    IApplicationRepository Application { get; }
    IProjectRepository Project { get; }
    IProjectSkillsRepository ProjectSkills { get; }
    ISkillRepository Skill { get; }
    ITypeRepository Type { get; }
    IUserRepository User { get; }
    ILogRepository Log { get; }
    IImageRepository Image { get; }

    void Save();
 

  
}