using AutoMapper;
using BL.IServices;
using BL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MVC.Models;

namespace MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SkillController : Controller
    {
        private readonly ISkillService _skillService;
        private readonly IMapper _mapper;
        private readonly ILogger<SkillController> _logger;

        public SkillController(ISkillService skillService, IMapper mapper, ILogger<SkillController> logger)
        {
            _skillService = skillService;
            _mapper = mapper;
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            return RedirectToAction("List");
        }

        public IActionResult ListPartial()
        {
            try
            {

                var allSkills = _skillService.GetAll();
                var allSkillsVm = _mapper.Map<ICollection<SkillVM>>(allSkills);
                return PartialView("_ListPartial", allSkillsVm);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = "An error occurred while processing your request";
                TempData["ToastType"] = "error";
                _logger.LogError(ex, "An error occurred while retrieving list of skills");
                return RedirectToAction("List", "Skill");
            }

        }
        public IActionResult List()
        {
            try
            {

                var allSkills = _skillService.GetAll();
                var allSkillsVm = _mapper.Map<ICollection<SkillVM>>(allSkills); 
                return View(allSkillsVm);
            }
            catch (Exception ex)
            {

                TempData["ToastMessage"] = "An error occurred while processing your request";
                TempData["ToastType"] = "error";
                _logger.LogError(ex, "An error occurred while retrieving list of skills");
                return RedirectToAction("List", "Project");
            }

        }



        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("_CreateModalPartial");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SkillVM skill)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { success = false, message = "Validation failed", errors = errorMessages, type = "error" });
                }

                _skillService.Create(_mapper.Map<SkillDto>(skill));
                return Json(new { success = true, message = $"Skill {skill.Name} created successfully!", type = "success" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the skill.");
                return Json(new { success = false, message = ex.Message, type = "error" });
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
	        try
	        {
		       
		        var skillVm = _mapper.Map<SkillVM>(_skillService.GetById(id));
		        return PartialView("_EditModalPartial", skillVm);
			}
			catch (Exception ex)
			{
		        TempData["ToastMessage"] = "An error occurred while processing your request";
		        TempData["ToastType"] = "error";
		        _logger.LogError(ex, "An error occurred while fetching the skill with ID {SkillId}.", id);
				return RedirectToAction("List");
	        }
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SkillVM skill)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { success = false, message = "Validation failed", errors = errorMessages, type = "error" });
                }

                _skillService.Update(id, _mapper.Map<SkillDto>(skill));
                return Json(new { success = true, message = $"Skill {skill.Name} edited successfully!", type = "success" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the skill with ID {SkillId}.", id);
                return  Json(new { success = false, message = ex.Message, type = "error" });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                var skill = _skillService.GetById(id);
                _skillService.Delete(id);
                return Json(new { success = true, message = $"Skill: {skill.Name} deleted successfully!", type = "success" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the skill with ID {SkillId}.", id);
                return Json(new { success = false, message = ex.Message, type = "error" });
            }
        }
    }
}
