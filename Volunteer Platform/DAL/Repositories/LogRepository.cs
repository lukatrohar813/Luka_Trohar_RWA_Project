using DAL.IRepositories;
using DAL.Models;

namespace DAL.Repositories;

public class LogRepository : Repository<Log>, ILogRepository
{
    public LogRepository(DatabaseContext dbContext) : base(dbContext)
    {

    }
}