using BL.Models;


namespace BL.IServices;

public interface ILogService
{
    ICollection<LogDto> GetLastN(int n);
    int GetCount();
    void Log(string level, string message);
  

}