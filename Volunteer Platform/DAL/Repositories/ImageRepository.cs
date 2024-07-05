using DAL.IRepositories;
using DAL.Models;

namespace DAL.Repositories
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        public ImageRepository(DatabaseContext dbContext) : base(dbContext)
        {
        }
    }
}