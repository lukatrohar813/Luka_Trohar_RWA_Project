using AutoMapper;
using BL.IServices;
using BL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MVC.Controllers
{
    
    public class ApplicationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IApplicationService _applicationService;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        private readonly ILogger<ApplicationController> _logger;

        public ApplicationController(IApplicationService applicationService, IMapper mapper, IProjectService projectService, IUserService userService, ILogger<ApplicationController> logger)
        {
            
            _applicationService = applicationService;
            _mapper = mapper;
            _projectService = projectService;
            _userService = userService;
			_logger = logger;
		}


        [Authorize(Roles = "User,Admin")]
        public ActionResult Index()
        {
            try
            {
              

                    var user = _userService.GetByName(HttpContext.User.Identity.Name);
                    if (user.Role == 2)
                    {
                        return RedirectToAction("List", "Application");
                    }

                    var allApplicationsByUser = _applicationService.GetAll().Where(a => a.UserId == user.Id).ToList();
                    var allApplicationsByUserDto = _mapper.Map<ICollection<ApplicationDto>>(allApplicationsByUser);

                    return View(allApplicationsByUserDto);

                
            }
            catch (Exception ex)
            {

				TempData["ToastMessage"] =ex.Message;
				TempData["ToastType"] = "error";
                _logger.LogError(ex, "An error occurred while retrieving list of user applications");
				return View();

            }

        }
        
        [Authorize(Roles = "Admin")]

        public ActionResult List()
        {
            try
            {
                var allApplications = _applicationService.GetAll();
                var allApplicationsDto = _mapper.Map<ICollection<ApplicationDto>>(allApplications);
                return View(allApplicationsDto);
            }
            catch (Exception ex)
            {
	            TempData["ToastMessage"] = "An error occurred while processing your request " + ex.Message;
	            TempData["ToastType"] = "error";
	            _logger.LogError(ex, "An error occurred while retrieving the list of all applications");
				return View();
            }
        }


        [Authorize(Roles = "Admin")]
        public ActionResult ListPartial()
        {
            try
            {
                var allApplications = _applicationService.GetAll();
                return PartialView("_ListPartial", _mapper.Map<ICollection<ApplicationDto>>(allApplications));
            }
            catch (Exception ex)
            {
	            _logger.LogError(ex, "An error occurred while retrieving the list of all applications.");
	            return Json(new { success = false, message = "An error occurred while processing your request: " + ex.Message });
			}
        }
        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public ActionResult UpdateStatus(int id, string status)
        {
            try
            {
                var AppDto = new ApplicationDto
                {
                    
                    
                    Status = status
                };
                var updatedApplication = _applicationService.UpdateStatus(id, status);
              

                return Json(new { success = true, status = updatedApplication.Status });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public IActionResult SignUp(int projectId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            try
            {
              
                var project = _projectService.GetById(projectId);

                var newApplication = new ApplicationDto
                {
                    ProjectId = projectId,
                    UserId = userId
                };

               
                _applicationService.Create(newApplication);

				TempData["ToastMessage"] = $"You have successfully signed up for a project {project.Name}.";
				TempData["ToastType"] = "success";
				return RedirectToAction("Details", "Project", new { id = projectId });
			}
            catch (Exception ex)
            {
				TempData["ToastMessage"] = "An error occurred while processing your request";
				TempData["ToastType"] = "error"; 
				_logger.LogError(ex, "An error occurred while signing up to a project.");
				return RedirectToAction("Details", "Project", new { id = projectId });
			}
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public IActionResult SignOff(int projectId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            try
            {
                var project = _projectService.GetById(projectId);
                if (project == null)
                {
                    return NotFound("Project not found.");
                }

                var application = _applicationService.GetByUserAndProject(new ApplicationDto
                {
                    ProjectId = projectId,
                    UserId = userId
                });


                _applicationService.Delete((int)application.Id);

				TempData["ToastMessage"] = $"You have signed off a project {project.Name}";
				TempData["ToastType"] = "error";
				return RedirectToAction("Details", "Project", new { id = projectId });
            }
            catch (Exception ex)
            {
				TempData["ToastMessage"] = "An error occurred while processing your request ";
				TempData["ToastType"] = "error";
				_logger.LogError(ex, "An error occurred while signing off a project");
				return RedirectToAction("Details", "Project", new { id = projectId });
            }
        }

    }

}
