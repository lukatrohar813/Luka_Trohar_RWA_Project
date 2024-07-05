using DAL.IRepositories;
using DAL.Models;

namespace DAL.Repositories
{
    public class SkillRepository : Repository<Skill>, ISkillRepository
    {
        public SkillRepository(DatabaseContext dbContext) : base(dbContext)
        {
        }
    }
}