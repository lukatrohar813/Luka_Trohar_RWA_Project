using AutoMapper;
using BL.IServices;
using BL.Models;
using DAL.IRepositories;
using DAL.Models;
using Microsoft.Extensions.Configuration;

namespace BL.Services;

public class LogService : ILogService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _logMapper;
    private readonly IConfiguration _configuration;

    public LogService(IRepositoryManager repositoryManager, IMapper logMapper, IConfiguration configuration)
    {
        _repositoryManager = repositoryManager;
        _logMapper = logMapper;
        _configuration = configuration;
    }

    public ICollection<LogDto> GetLastN(int n)
    {
        var logs = _repositoryManager.Log.GetAll()
            .OrderByDescending(log => log.Timestamp)
            .Take(n)
            .Select(log => _logMapper.Map<LogDto>(log))
            .ToList();
        return logs;
    }

   
    public int GetCount() => _repositoryManager.Log.GetAll().Count();


    public void Log(string level, string message)
    {
        var log = new Log
        {
            Timestamp = DateTime.Now,
            Level = level,
            Message = message,
            
        };
        _repositoryManager.Log.Add(log);
        _repositoryManager.Save();
        
    }


}
