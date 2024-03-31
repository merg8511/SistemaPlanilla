using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaOrbita.ActionFilters;
using SistemaOrbita.DAL.Repository.IRepository;
using SistemaOrbita.Model.Models;
using SistemaOrbita.Model.ViewModels;
using SistemaOrbita.Utilities;
using SistemaOrbita.Web.Areas.Identity.Data;

namespace SistemaOrbita.Areas.Payroll.Controllers
{
    [Area("Payroll")]
    [Authorize]
    public class EventLogController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventLogController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [OrbitaAuthorize(Permissions.EventLog.View)]
        [ServiceFilter(typeof(AuditLogFilter))]
        [Audit($"{DS.Audit_View} EventLogs")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [OrbitaAuthorize(Permissions.EventLog.Upsert)]
        [ServiceFilter(typeof(AuditLogFilter))]
        [Audit($"{DS.Audit_Upsert} EventLogs")]
        public async Task<IActionResult> Upsert(string? id)
        {
            EventLogVM model = new EventLogVM
            {
                EventLog = new EventLog(),
                Employees = _unitOfWork.EventLog.GetAllDropDownList("Employee"),
                AuthorizedBy = _unitOfWork.EventLog.GetAllDropDownList("Authorize"),
                EventTypes = await _unitOfWork.EventType.GetAll(orderBy: o => o.OrderBy(o => o.IsDeduction))
            };

            if (string.IsNullOrEmpty(id))
            {
                model.EventLog.IsActive = 1;
                model.EventLog.CreatedAt = DateTime.Now;
                //model.EventLog.Frequency = 0;
                //model.EventLog.Recurring = 0;
                model.IsRecurring = false;
                return View(model);
            }
            else
            {
                model.EventLog = await _unitOfWork.EventLog.GetFirst(x => x.Id == id, includeProperties: "Employee,Event,AuthorizedByNavigation");
                if (model.EventLog == null)
                {
                    return NotFound();
                }

                model.IsRecurring = model.EventLog.Recurring == 0 ? false : true;

                return View(model);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [OrbitaAuthorize(Permissions.EventLog.Upsert)]
        [ServiceFilter(typeof(AuditLogFilter))]
        [Audit($"{DS.Audit_Upsert} EventLogs")]
        public async Task<IActionResult> Upsert(EventLogVM eventLogVM)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(eventLogVM.EventLog.Id))
                {
                    eventLogVM.EventLog.Id = NUlid.Ulid.NewUlid().ToString();
                    eventLogVM.EventLog.Recurring = eventLogVM.IsRecurring ? (sbyte)1 : (sbyte)0;
                    eventLogVM.EventLog.IsActive = 1;

                    await _unitOfWork.EventLog.Create(eventLogVM.EventLog);
                    TempData[DS.Success] = "EventLog created successfully";
                }
                else
                {
                    eventLogVM.EventLog.Recurring = eventLogVM.IsRecurring ? (sbyte)1 : (sbyte)0;
                    _unitOfWork.EventLog.Update(eventLogVM.EventLog);
                    TempData[DS.Success] = "EventLog updated successfully";
                }

                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            TempData[DS.Error] = "Error while creating EventLog";
            eventLogVM.Employees = _unitOfWork.EventLog.GetAllDropDownList("Employee");
            eventLogVM.AuthorizedBy = _unitOfWork.EventLog.GetAllDropDownList("Employee");
            eventLogVM.EventTypes = await _unitOfWork.EventType.GetAll();
            return View(eventLogVM);

        }

        #region API

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //var all = await _unitOfWork.EventLog.GetAll(includeProperties: "Employee,Event,AuthorizedByNavigation");
            var all = await _unitOfWork.EventLog.GetAll(includeProperties: "Event,Employee");
            return Json(new { data = all });
        }

        [HttpGet]
        [OrbitaAuthorize(Permissions.EventLog.View)]
        [ServiceFilter(typeof(AuditLogFilter))]
        [Audit($"{DS.Audit_View} Event log")]
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var eventLog = await _unitOfWork.EventLog.GetEventLogData(id);
            var eventLog = await _unitOfWork.EventLog.GetFirst(x => x.Id == id, 
                includeProperties: "Employee,Employee.Position,Event,AuthorizedByNavigation");

            if (eventLog == null)
            {
                return NotFound();
            }

            return Json(new { data = eventLog });
        }

        [HttpPost]
        [OrbitaAuthorize(Permissions.Employee.Delete)]
        [ServiceFilter(typeof(AuditLogFilter))]
        [Audit($"{DS.Audit_Delete} Employees")]
        public async Task<IActionResult> Delete(string? id)
        {
            var model = await _unitOfWork.EventLog.Get(id);

            if (model == null)
            {
                return Json(new { success = false, message = "There was an error deleting EventLog" });
            }

            model.IsActive = 0;
            _unitOfWork.EventLog.Update(model);
            await _unitOfWork.Save();

            return Json(new { success = true, message = "EventLog deleted successfully" });
        }

        #endregion
    }
}
