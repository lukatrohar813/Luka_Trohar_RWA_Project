using BL.IServices;
using BL.Models;
using BL.Services;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ILogService _logService;
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService, ILogService logService)
        {
            _projectService = projectService;
            _logService = logService;
        }

        // GET: api/<ProjectController>
        [HttpGet("getall")]
        public IActionResult List()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var projects = _projectService.GetAll();
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] int size = 5, [FromQuery] int page = 0, [FromQuery] string? filterNames = null)
        {
           
            try
	        {
		        var result = _projectService.Search(size, page, filterNames);
		        return Ok(result);
	        }
	        catch (Exception ex)
	        {
                return BadRequest( new { message = ex.Message });
            }
        }

		[HttpPost("create")]
        public IActionResult CreateProject([FromBody] ProjectDto project)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var createdProject = _projectService.Create(project);
                return StatusCode(201,createdProject);
            }
            catch (Exception ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateProject(int id, [FromBody] ProjectDto project)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var updatedProject = _projectService.Update(id, project);

                return Ok(updatedProject);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message== "Project name already exists.") return Conflict(new { message = ex.Message });
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteProject(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var deletedProject = _projectService.Delete(id);
                if (deletedProject == null) return NotFound("Project does not exist");
                return Ok(deletedProject);
            }
        
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
