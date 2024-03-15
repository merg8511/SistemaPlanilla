using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaOrbita.ActionFilters;
using SistemaOrbita.DAL.Repository.IRepository;
using SistemaOrbita.Model.Models;
using SistemaOrbita.Utilities;
using SistemaOrbita.Web.Areas.Identity.Data;
using System.Security.Claims;

namespace SistemaOrbita.Areas.BusinessManagement.Controllers
{
    [Area("BusinessManagement")]
    [Authorize]
    public class EventTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [OrbitaAuthorize(Permissions.EventType.View)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [OrbitaAuthorize(Permissions.EventType.Upsert)]
        public async Task<IActionResult> Upsert(string? id)
        {
            EventType model = new EventType();

            if (string.IsNullOrEmpty(id))
            {
                model.IsActive = 1;
                return View(model);
            }

            model = await _unitOfWork.EventType.Get(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OrbitaAuthorize(Permissions.EventType.Upsert)]
        public async Task<IActionResult> Upsert(EventType model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    model.Id = NUlid.Ulid.NewUlid().ToString();
                    model.IsActive = 1;
                    model.CreatedAt = DateTime.Now;
                    model.CreatedBy = User.FindFirstValue(ClaimTypes.Name);
                    await _unitOfWork.EventType.Create(model);
                    TempData[DS.Success] = "Successfully created event";
                }
                else
                {
                    _unitOfWork.EventType.Update(model);
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
            var all = await _unitOfWork.EventType.GetAll();
            return Json(new { data = all });
        }

        [HttpPost]
        [OrbitaAuthorize(Permissions.EventType.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await _unitOfWork.EventType.Get(id);

            if (model == null)
            {
                return Json(new { success = false, message = "There was an error deleting the event" });
            }
            model.IsActive = 0;
            _unitOfWork.EventType.Update(model);
            await _unitOfWork.Save();
            return Json(new { success = true, message = "Event successfully eliminated" });
        }

        #endregion

    }
}
