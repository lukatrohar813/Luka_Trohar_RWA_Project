using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IRepositories;
using DAL.Models;

namespace DAL.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly DatabaseContext _context;

        public RepositoryManager(DatabaseContext context)
        {
            _context = context;
            Application = new ApplicationRepository(_context);
            Project = new ProjectRepository(_context);
            ProjectSkills = new ProjectSkillsRepository(_context);
            Skill = new SkillRepository(_context);
            Type = new TypeRepository(_context);
            User = new UserRepository(_context);
            Log = new LogRepository(_context);
            Image = new ImageRepository(_context);
        }
        public IApplicationRepository Application { get; private set; }
        public IProjectRepository Project { get; private set; }
        public IProjectSkillsRepository ProjectSkills { get; private set; }
        public ISkillRepository Skill { get; private set; }
        public ITypeRepository Type { get; private set; }
        public IUserRepository User { get; private set; }
        public ILogRepository Log { get; private set; }
        public IImageRepository Image { get; private set; }

        public void Save()
        {
            _context.SaveChanges(); 
        }
      

      
    }
}
