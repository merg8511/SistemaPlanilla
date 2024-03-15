using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using SistemaOrbita.ActionFilters;
using SistemaOrbita.DAL.Repository.IRepository;
using SistemaOrbita.Model.Models;
using SistemaOrbita.Model.ViewModels;
using SistemaOrbita.Utilities;
using SistemaOrbita.Web.Areas.Identity.Data;

namespace SistemaOrbita.Areas.BusinessManagement.Controllers
{
    [Area("BusinessManagement")]
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [OrbitaAuthorize(Permissions.Project.View)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [OrbitaAuthorize(Permissions.Project.Upsert)]
        public async Task<IActionResult> Upsert(string? id)
        {
            ProjectVM projectVM = new ProjectVM()
            {
                Project = new Model.Models.Project(),
                Managers = _unitOfWork.Project.GetAllDropDownList("Employee")
            };

            if (string.IsNullOrEmpty(id))
            {
                projectVM.Project.IsActive = 1;
                return View(projectVM);
            }
            else
            {
                projectVM.Project = await _unitOfWork.Project.GetFirst(p => p.Id == id, includeProperties: "Manager");

                if (projectVM.Project == null)
                {
                    return NotFound();
                }

                return View(projectVM);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OrbitaAuthorize(Permissions.Project.Upsert)]
        public async Task<IActionResult> Upsert(ProjectVM projectVM)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(projectVM.Project.Id))
                {
                    projectVM.Project.Id = NUlid.Ulid.NewUlid().ToString();
                    projectVM.Project.CreateAt = DateTime.Now;
                    projectVM.Project.IsActive = 1;

                    await _unitOfWork.Project.Create(projectVM.Project);
                    TempData[DS.Success] = "Successfully created project";
                }
                else
                {
                    _unitOfWork.Project.Update(projectVM.Project);
                    TempData[DS.Success] = "Successfully updated project";
                }

                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            TempData[DS.Error] = "Error when creating the project";
            projectVM.Managers = _unitOfWork.Project.GetAllDropDownList("Employee");
            return View(projectVM);
        }

        #region API

        public async Task<IActionResult> GetAll()
        {
            var all = await _unitOfWork.Project.GetAll(includeProperties: "Manager");
            return Json(new { data = all });
        }

        [HttpGet]
        [OrbitaAuthorize(Permissions.Project.View)]
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _unitOfWork.Project.GetFirst(p => p.Id == id, includeProperties: "Manager");

            if (project == null)
            {
                return NotFound();
            }

            return Json(new { data = project });
        }


        [HttpPost]
        [OrbitaAuthorize(Permissions.Project.Delete)]
        public async Task<IActionResult> Delete(string? id)
        {
            var model = await _unitOfWork.Project.Get(id);

            if(model == null)
            {
                return Json(new { success = false, message = "There was an error deleting the event" });
            }

            model.IsActive = 0;
            _unitOfWork.Project.Update(model);
            await _unitOfWork.Save();

            return Json(new { success = true, message = "Project successfully eliminated" });
        }
        #endregion
    }
}
