using DAL.IRepositories;
using DAL.Models;
using Type = DAL.Models.Type;
namespace DAL.Repositories
{
    public class TypeRepository : Repository<Type>, ITypeRepository
    {
        public TypeRepository(DatabaseContext dbContext) : base(dbContext)
        {
        }
    }
}