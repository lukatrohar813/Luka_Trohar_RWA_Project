using BL.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Web_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }
        
        [HttpGet("get/{n?}")]
        public IActionResult GetLastNLogs(int n = 10)
        {
            try
            {
                var logs = _logService.GetLastN(n);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }
        
        [HttpGet("count")]
        public IActionResult GetLogCount()
        {
            try
            {
                var count = _logService.GetCount();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
            
        }
   
    
}
