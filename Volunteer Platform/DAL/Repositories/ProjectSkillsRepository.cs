using DAL.IRepositories;
using DAL.Models;

namespace DAL.Repositories
{
    public class ProjectSkillsRepository : Repository<ProjectSkill>, IProjectSkillsRepository
    {
        public ProjectSkillsRepository(DatabaseContext dbContext) : base(dbContext)
        {
        }
    }
}