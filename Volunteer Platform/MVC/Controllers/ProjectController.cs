using AutoMapper;
using BL.IServices;
using BL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;


namespace MVC.Controllers
{

	public class ProjectController : Controller
	{
		private readonly IProjectService _projectService;
		private readonly IImageService _imageService;
		private readonly ITypeService _typeService;
		private readonly ISkillService _skillService;
		private readonly IProjectSkillService _projectSkillService;
		private readonly IMapper _mapper;
		private readonly IApplicationService _applicationService;
		private readonly IUserService _userService;
        private readonly ILogger<ProjectController> _logger;

		public ProjectController(IProjectService projectService, IImageService imageService, ITypeService typeService,
			IMapper mapper, ISkillService skillService, IProjectSkillService projectSkillService,
			IApplicationService applicationService, IUserService userService, ILogger<ProjectController> logger)
		{
			_projectService = projectService;
			_imageService = imageService;
			_typeService = typeService;
			_skillService = skillService;
			_mapper = mapper;
			_projectSkillService = projectSkillService;
			_applicationService = applicationService;
			_userService = userService;
            _logger = logger;

		}


		[Authorize(Roles = "NonUser,User,Admin")]
		public IActionResult Index()
		{
			
				return RedirectToAction("List", "Project");

			


		}
        [Authorize(Roles = "NonUser,User,Admin")]
        public IActionResult List(int page = 1, int size = 10, string filterBy = null, string filter = null)
        {
            try
            {
                var pagedProjects = _projectService.GetPaged(page, size);
                var totalProjects = _projectService.GetCount();

                if (!string.IsNullOrEmpty(filter))
                {
                    pagedProjects = _projectService.GetFiltered(pagedProjects, filterBy, filter).ToList();
                  

                }

                ViewData["page"] = page;
                ViewData["size"] = size;
                ViewData["pages"] = (int)Math.Ceiling((double)totalProjects / size);
                var projectsVm = _mapper.Map<List<ProjectListVM>>(pagedProjects);
                return View(projectsVm);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = "An error occurred while processing your request";
                TempData["ToastType"] = "error";
                _logger.LogError(ex, "An error occurred while retrieving list of projects for admin.");
                return BadRequest("Error loading projects.");
            }
        }

  

        [Authorize(Roles = "NonUser,User,Admin")]
        public IActionResult ListPartial(int page = 1, int size = 10, string filterBy = null, string filter = null)
        {
            try
            {
                var pagedProjects = _projectService.GetPaged(page, size);
                var totalProjects = _projectService.GetCount();

                if (!string.IsNullOrEmpty(filter))
                {
                    pagedProjects = _projectService.GetFiltered(pagedProjects, filterBy, filter).ToList();
                }
                

                ViewData["page"] = page;
                ViewData["size"] = size;
                ViewData["pages"] = (int)Math.Ceiling((double)totalProjects / size);
                var projectsVm = _mapper.Map<List<ProjectListVM>>(pagedProjects);
                return PartialView("_ListPartial", projectsVm);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = "An error occurred while processing your request";
                TempData["ToastType"] = "error";
                _logger.LogError(ex, "An error occurred while retrieving list of projects for user.");
                return BadRequest("Error loading projects.");
            }
        }


        [Authorize (Roles ="Admin")]
        public IActionResult ListAdmin(int page = 1, int size = 10, string filterBy = null, string filter = null)
        {
            try
            {
                var pagedProjects = _projectService.GetPagedIncludeAll(page, size);
                var totalProjects = _projectService.GetCount();

                if (!string.IsNullOrEmpty(filter))
                {
                    pagedProjects = _projectService.GetFilteredIncludeAll(pagedProjects, filterBy, filter);
                }

                ViewData["page"] = page;
                ViewData["size"] = size;
                ViewData["pages"] = (int)Math.Ceiling((double)totalProjects / size);
                var projectsVm = _mapper.Map<List<ListAdminVM>>(pagedProjects); 
                return View(projectsVm);

            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = "An error occurred while processing your request";
                TempData["ToastType"] = "error";
                _logger.LogError(ex, "An error occurred while retrieving list of projects for admin.");
                return BadRequest("Error loading projects.");
            }
        }

        [Authorize(Roles = "Admin")]

        public IActionResult ListAdminPartial(int page = 1, int size = 10, string filterBy = null, string filter = null)
        {
            try
            {
                var pagedProjects = _projectService.GetPagedIncludeAll(page, size);
                

                if (!string.IsNullOrEmpty(filter))
                {
                    pagedProjects = _projectService.GetFilteredIncludeAll(pagedProjects, filterBy, filter);
                }

                ViewData["page"] = page;
                ViewData["size"] = size;
                ViewData["pages"] = (int)Math.Ceiling((double)_projectService.GetCount() / size);
                var projectsVm = _mapper.Map<List<ListAdminVM>>(pagedProjects);
                return PartialView("_ListAdminPartial", projectsVm);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = "An error occurred while processing your request";
                TempData["ToastType"] = "error";
                _logger.LogError(ex, "An error occurred while retrieving list of projects for admin.");
                return BadRequest("Error loading projects.");
            }
        }
    






    [Authorize(Roles = "User,Admin")]
        public ActionResult Details(int id)
        {
            try
            {
                var projectToDisplay = _projectService.GetByIdFullInclude(id);
                var applications = _applicationService.GetAll();
                var projectVm = _mapper.Map<ProjectDetailsVM>(projectToDisplay);

                
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
                {
                    projectVm.IsUserSignedToProject = applications.Any(a => a.UserId == userId && a.ProjectId == projectVm.Id);
                }
                return View(projectVm);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = "An error occurred while processing your request";
                TempData["ToastType"] = "error";
                _logger.LogError(ex, "An error occurred while opening project Details");
                return RedirectToAction("List");
            }
        }




        [Authorize(Roles = "NonUser, User,Admin")]
        [HttpGet]
        [AllowAnonymous]
        private List<SelectListItem> GetTypeListItems()
        {
            var typeListItems = _typeService.GetAll().Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.Id.ToString()
            }).ToList();


            return typeListItems;
        }
        [Authorize(Roles = "NonUser, User,Admin")]
        private IEnumerable<SelectListItem> GetSkillListItems()
        {
            var skills = _skillService.GetAll();
            return skills.Select(skill => new SelectListItem
            {
                Value = skill.Name,
                Text = skill.Name
            });
            }

    private List<ApplicationDto> GetApplicationItems()
        {
           var applicationListItems = _applicationService.GetAll().ToList();

            return applicationListItems;
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
		{
			try
			{
				ViewBag.TypeDdlItems = GetTypeListItems();
				ViewBag.SkillDdlItems = GetSkillListItems();
				ViewBag.ApplicationItems = GetApplicationItems();

				var project = new ProjectCreateVM();
				return View(project);
			}
			catch (Exception)
			{
				return RedirectToAction("Error", "Home");
			}
		}

        [Authorize(Roles = "Admin")]
        [HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ProjectCreateVM project)
        {
            
            try
			{
                project.Name = project.Name.Trim();
                if (!ModelState.IsValid)
				{
					ViewBag.TypeDdlItems = GetTypeListItems();
					ViewBag.SkillDdlItems = GetSkillListItems();
					ViewBag.ApplicationItems = GetApplicationItems();
                    TempData["ToastMessage"] = "An error occurred while processing your request";
                    TempData["ToastType"] = "error";
                    _logger.LogError("Model state not valid.");
                    return View(project);
				}

				if (project.Image != null && project.Image.Length > 0)
				{
					var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
					var uniqueFileName = Guid.NewGuid().ToString() + "_" + project.Image.FileName;
					var filePath = Path.Combine(uploadsFolder, uniqueFileName);

					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						project.Image.CopyTo(fileStream);
					}

					var newImage = new ImageDto
					{
						FileName = project.Image.FileName,
						FilePath = "/images/" + uniqueFileName,
						ContentType = project.Image.ContentType
					};

					var createdImage = _imageService.Create(newImage);
					project.ImageId = createdImage.Id;
				}
				else
				{
                    var defaultImageId = _imageService.GetDefaultImage().Id;
                    project.ImageId = defaultImageId;

                }

				var newProject = _mapper.Map<ProjectDto>(project);
				_projectService.Create(newProject);
                TempData["ToastMessage"] = $"Project: {project.Name} successfully created";
                TempData["ToastType"] = "success";
                return RedirectToAction(nameof(ListAdmin));
			}
			catch (Exception ex)
			{
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                _logger.LogError(ex, "An error occurred while creating a project.");
                ViewBag.TypeDdlItems = GetTypeListItems();
				ViewBag.SkillDdlItems = GetSkillListItems();
				ViewBag.ApplicationItems = GetApplicationItems();
				return View(project);
			}
		}

        [Authorize(Roles = "Admin")]
        [HttpGet]
		public IActionResult Edit(int id)
		{
			try
			{
				
				ViewBag.TypeDdlItems = GetTypeListItems();
				ViewBag.SkillDdlItems = GetSkillListItems();

				var project = _projectService.GetById(id);
			

				var model = _mapper.Map<ProjectEditVM>(project);
				var image = _imageService.GetById((int)project.ImageId);
				model.ImageFilePath = image != null ? Url.Content(image.FilePath) : null;

				return View(model);
			}
			catch (Exception ex)
			{
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                _logger.LogError(ex, "An error occurred while retrieving project to edit");
                return RedirectToAction("Index","Home");

               
			}
		}

        [Authorize(Roles = "Admin")]
        [HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(int id, ProjectEditVM model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					ViewBag.TypeDdlItems = GetTypeListItems();
					ViewBag.SkillDdlItems = GetSkillListItems();
					ViewBag.ApplicationItems = GetApplicationItems();
                    TempData["ToastMessage"] = "An error occurred while processing your request";
                    TempData["ToastType"] = "error";
                    _logger.LogError( "Model state is not valid.");
                    return View(model);
				}

				if (model.Image != null && model.Image.Length > 0)
				{
					var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
					var uniqueFileName = Guid.NewGuid() + "_" + model.Image.FileName;
					var filePath = Path.Combine(uploadsFolder, uniqueFileName);

					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						model.Image.CopyTo(fileStream);
					}

					var newImage = new ImageDto
					{
						FileName = model.Image.FileName,
						FilePath = "/images/" + uniqueFileName,
						ContentType = model.Image.ContentType
					};

					var createdImage = _imageService.Create(newImage);
					model.ImageId = createdImage.Id;
				}
				else
				{
					var existingProject = _projectService.GetById(id);
					if (existingProject != null)
					{
						model.ImageId = existingProject.ImageId ?? _imageService.GetDefaultImage().Id;
					}
					else
					{
						model.ImageId = _imageService.GetDefaultImage().Id;
					}
				}

				var updatedProject = _mapper.Map<ProjectDto>(model);
				_projectService.Update(id, updatedProject);

				TempData["ToastMessage"] = $"Project: {model.Name} Successfully Updated!";
				TempData["ToastType"] = "success";
				return RedirectToAction(nameof(ListAdmin));
			}
			catch (Exception ex)
			{
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                _logger.LogError(ex,"Error occurred while editing {ProjectId}",id);

                ViewBag.TypeDdlItems = GetTypeListItems();
				ViewBag.SkillDdlItems = GetSkillListItems();
				ViewBag.ApplicationItems = GetApplicationItems();
				return View(model);
			}
		}



        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                var projectToDelete = _projectService.GetByIdFullInclude(id);
                var projectVm =_mapper.Map<ProjectDeleteVM>(projectToDelete);

                return View(projectVm);
            }
            catch
            {
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ProjectDeleteVM project)
        {
            try
            {
             
                _projectService.Delete(project.Id);
                TempData["ToastMessage"] = $"Project: {project.Name} Successfully Deleted!";
                TempData["ToastType"] = "success";
                return RedirectToAction("ListAdmin", "Project");
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                _logger.LogError(ex, "Error occurred while deleting {ProjectId}", project.Id);
                return View(project);
            }
        }
    }
}
