using Microsoft.AspNetCore.Mvc;
using SistemaOrbita.DAL.Repository.IRepository;
using SistemaOrbita.Model.Models;
using SistemaOrbita.Utilities;
using System.Security.Claims;

namespace SistemaOrbita.Areas.BusinessManagement.Controllers
{
    [Area("BusinessManagement")]
    public class OrganizationDepartmentController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public OrganizationDepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(string? id)
        {
            OrganizationDepartment model = new OrganizationDepartment();

            if (string.IsNullOrEmpty(id))
            {
                model.IsActive = 1;
                return View(model);
            }

            model = await _unitOfWork.OrganizationDepartment.Get(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(OrganizationDepartment model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    model.Id = NUlid.Ulid.NewUlid().ToString();
                    model.CreatedBy = User.FindFirstValue(ClaimTypes.Name);
                    model.CreatedAt = DateTime.Now;
                    model.IsActive = 1;
                    await _unitOfWork.OrganizationDepartment.Create(model);
                    TempData[DS.Success] = "Successfully created organization department";
                }
                else
                {
                    _unitOfWork.OrganizationDepartment.Update(model);
                    TempData[DS.Success] = "Successfully updated event";
                }
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error when creating the event";
            return View(model);
        }

        #region API 

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var all = await _unitOfWork.OrganizationDepartment.GetAll();
            return Json(new { data = all });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await _unitOfWork.OrganizationDepartment.Get(id);

            if (model == null)
            {
                return Json(new { success = false, message = "There was an error deleting the Organization Department" });
            }
            model.IsActive = 0;
            _unitOfWork.OrganizationDepartment.Update(model);
            await _unitOfWork.Save();
            return Json(new { success = true, message = "Organization Department successfully eliminated" });
        }

        #endregion
    }
}
