using DAL.IRepositories;
using DAL.Models;

namespace DAL.Repositories
{
    public class ApplicationRepository : Repository<Application>, IApplicationRepository
    {
        public ApplicationRepository(DatabaseContext dbContext) : base(dbContext)
        {
        }
    }
}