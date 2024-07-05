using AutoMapper;
using BL.IServices;
using BL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MVC.Models;

namespace MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TypeController : Controller
    {
        private readonly ITypeService _typeService;
        private readonly IMapper _mapper;
        private readonly ILogger<TypeController> _logger;

        public TypeController(ITypeService typeService, IMapper mapper, ILogger<TypeController> logger)
        {
            _typeService = typeService;
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
                var allTypes = _typeService.GetAll();
                var allTypesVm = _mapper.Map<ICollection<TypeVM>>(allTypes);
                return PartialView("_ListPartial", allTypesVm);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = "An error occurred while processing your request";
                TempData["ToastType"] = "error";
                _logger.LogError(ex, "An error occurred while retrieving list of types");
                return RedirectToAction("List", "Type");
            }
        }
        public IActionResult List()
        {
            try
            {
                var allTypes = _typeService.GetAll();
                var allTypesVm = _mapper.Map<ICollection<TypeVM>>(allTypes);
                return View(allTypesVm);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = "An error occurred while processing your request";
                TempData["ToastType"] = "error";
                _logger.LogError(ex, "An error occurred while retrieving list of types");
                return RedirectToAction("List", "Type");
            }
        }



        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("_CreateModalPartial");

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TypeVM type)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { success = false, message = "Validation failed", errors = errorMessages, type = "error" });
                }

                _typeService.Create(_mapper.Map<TypeDto>(type));
                return Json(new { success = true, message = $"Type {type.Name} created successfully!", type = "success" });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Json(new { success = false, message = ex.Message, type = "error" });
            }
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                var type = _typeService.GetById(id);
                if (type == null)
                {
                    throw new InvalidOperationException("Type does not exist");
                }

                var TypeVm = _mapper.Map<TypeVM>(type);
                return PartialView("_EditModalPartial", TypeVm);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                _logger.LogError(ex, "An error occurred while fetching the type with ID {TypeId}", id);
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TypeVM type)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { success = false, message = "Validation failed", errors = errorMessages, type = "error" });
                }

                _typeService.Update(id, _mapper.Map<TypeDto>(type));
                return Json(new { success = true, message = $"Type {type.Name} edited successfully!", type = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, type = "error" });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                var type = _typeService.GetById(id);
                
                _typeService.Delete(id);
                return Json(new { success = true, message = $"Type: {type.Name} deleted successfully!", type = "success" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the type with ID {TypeId}.", id);
                return Json(new { success = false, message = ex.Message, type = "error" });
            }
        }
    }
}
