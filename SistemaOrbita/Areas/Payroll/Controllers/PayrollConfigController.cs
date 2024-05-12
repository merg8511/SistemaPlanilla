using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaOrbita.DAL.Repository.IRepository;
using SistemaOrbita.Model.Models;
using System.Security.Claims;

namespace SistemaOrbita.Areas.Payroll.Controllers
{
    [Area("Payroll")]
    [Authorize]
    public class PayrollConfigController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PayrollConfigController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region CONFIGURATION 

        public async Task<IActionResult> SetDate()
        {
            var type = await _unitOfWork.Employer.GetFirst();
            ViewData["Step"] = "SetDate";
            ViewData["Next"] = "SetHours";
            ViewData["Last"] = "Index";
            return View(type.PayrollType);
        }

        public async Task<IActionResult> SetHours()
        {
            ViewData["Next"] = "Verify";
            ViewData["Last"] = "SetDate";

            var employee = await _unitOfWork.Employee.GetAll(
                includeProperties: "Position,Position.OrganizationDepartment",
                orderBy: e => e.OrderBy(o => o.Position.OrganizationDepartment));
            return View(employee);
        }

        public IActionResult Verify()
        {
            ViewData["Step"] = "Verify";
            ViewData["Next"] = "Upsert";
            ViewData["Last"] = "SetHours";

            return View();
        }
        #endregion

        #region API OVERTIMES

        //OVERTIMES
        [HttpGet]
        public async Task<IActionResult> GetAllOverTimes(string startDate, string endDate)
        {
            var all = await _unitOfWork.OverTime.GetAll(o => o.PeriodStart >= DateTime.Parse(startDate) && o.PeriodEnd <= DateTime.Parse(endDate),
                includeProperties: "Employee,Employee.Position.OrganizationDepartment");
            return Json(new { data = all });
        }

        [HttpPost]
        public async Task<IActionResult> AddOverTime(string employeeIds, int dayOvertime, int nightOvertime, string startDate, string endDate)
        {
            try
            {
                var employeeIdList = employeeIds.Split(',');

                foreach (var employee in employeeIdList)
                {
                    var overTime = new OverTime
                    {
                        Id = NUlid.Ulid.NewUlid().ToString(),
                        EmployeeId = employee,
                        DayOvertime = dayOvertime,
                        NightOvertime = nightOvertime,
                        CreatedAt = DateTime.Now,
                        CreatedBy = User.FindFirstValue(ClaimTypes.Name),
                        PeriodStart = DateTime.Parse(startDate),
                        PeriodEnd = DateTime.Parse(endDate)
                    };
                    await _unitOfWork.OverTime.Create(overTime);
                }

                await _unitOfWork.Save();
                return Json(new { success = true, message = "Employee overtime successfully added" });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> OvertimeDetails(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var overTime = await _unitOfWork.OverTime.GetFirst(
                o => o.Id == id,
                includeProperties: "Employee,Employee.Position,Employee.Position.OrganizationDepartment");

            if (overTime == null)
            {
                return NotFound();
            }

            return Json(new { data = overTime });
        }

        [HttpGet]
        public async Task<IActionResult> OvertimeUpdate(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var overTime = await _unitOfWork.OverTime.GetFirst(o => o.Id == id, includeProperties: "Employee");

            if (overTime == null)
            {
                return NotFound();
            }

            return Json(new { data = overTime });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOvertime(string otid, int dayOvertime, int nightOvertime)
        {
            if (string.IsNullOrEmpty(otid))
            {
                return NotFound();
            }

            var overTime = await _unitOfWork.OverTime.GetFirst(o => o.Id == otid);

            if (overTime == null)
            {
                return NotFound();
            }

            overTime.DayOvertime = dayOvertime;
            overTime.NightOvertime = nightOvertime;

            _unitOfWork.OverTime.Update(overTime);
            await _unitOfWork.Save();
            return Json(new { success = true, message = "Successfully updated overtime" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOvertime(string? id)
        {
            var model = await _unitOfWork.OverTime.GetFirst(o => o.Id == id);

            if (model == null)
            {
                return Json(new { success = false, message = "Error while deleting overtime" });
            }

            _unitOfWork.OverTime.Delete(model);
            await _unitOfWork.Save();

            return Json(new { success = true, message = "Overtime successfully deleted" });
        }
        #endregion
    }
}
