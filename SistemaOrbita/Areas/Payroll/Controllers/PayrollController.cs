using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Rotativa.AspNetCore;
using SistemaOrbita.DAL.Repository.IRepository;
using SistemaOrbita.Model.Models;
using SistemaOrbita.Model.ViewModels;
using SistemaOrbita.Utilities;
using System.Linq;
using System.Security.Claims;
using System.Security.Permissions;

namespace SistemaOrbita.Areas.Payroll.Controllers
{
    [Area("Payroll")]
    [Authorize]
    public class PayrollController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PayrollController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payroll = await _unitOfWork.Payroll.GetFirst(p => p.Id == id,
                includeProperties: "Employer,PayrollDetails,PayrollDetails.Employee,PayrollDetails.Employee.Position");

            if (payroll == null)
            {
                return NotFound();
            }

            return View(payroll);
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(string start, string end, string id)
        {
            var payrollVM = new PayrollVM
            {
                Payroll = new Model.Models.Payroll(),
                Employer = new Employer(),
            };

            if (string.IsNullOrEmpty(id))
            {
                payrollVM.Payroll.StartDate = ConvertDateToDateTime(start);
                payrollVM.Payroll.EndDate = ConvertDateToDateTime(end);
                payrollVM.Employer = await _unitOfWork.Employer.GetFirst(e => e.Id == "1");
            }

            var details = _unitOfWork.PayrollDetail.GetPayrollDetails(ConvertDateToDateTime(start), ConvertDateToDateTime(end));
            payrollVM.PayrollDetails = details.ToList();

            return View(payrollVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(PayrollVM payrollVM)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(payrollVM.Payroll.Id))
                {
                    payrollVM.Payroll.Id = NUlid.Ulid.NewUlid().ToString();
                    payrollVM.Payroll.IsActive = 1;
                    payrollVM.Payroll.CreatedAt = DateTime.Now;
                    payrollVM.Payroll.CreatedBy = User.FindFirstValue(ClaimTypes.Name);
                    await _unitOfWork.Payroll.Create(payrollVM.Payroll);
                    await _unitOfWork.Save();


                    await CreatePayrollDetails(payrollVM.PayrollDetails, payrollVM.Payroll.Id);
                    TempData[DS.Success] = "Payroll created successfully";
                }
                else
                {
                    //eventLogVM.EventLog.Recurring = eventLogVM.IsRecurring ? (sbyte)1 : (sbyte)0;
                    //_unitOfWork.EventLog.Update(eventLogVM.EventLog);
                    TempData[DS.Success] = "Payroll updated successfully";
                }

                //await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(payrollVM);
        }

        private DateTime? ConvertDateToDateTime(string date)
        {
            DateTime? dateTime = null;
            if (!string.IsNullOrEmpty(date))
            {
                dateTime = DateTime.Parse(date);
            }

            return dateTime;
        }

        private async Task<bool> CreatePayrollDetails(List<PayrollDetail> payrollDetails, string id)
        {
            var response = false;
            try
            {
                foreach (var detail in payrollDetails)
                {
                    detail.PayrollId = id;

                    await _unitOfWork.PayrollDetail.Create(detail);
                }
            }
            catch
            {
                response = false;
            }

            await _unitOfWork.Save();
            return response;
        }

        public async Task<IActionResult> PrintPayroll(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payroll = await _unitOfWork.Payroll.GetFirst(p => p.Id == id,
                includeProperties: "Employer,PayrollDetails,PayrollDetails.Employee,PayrollDetails.Employee.Position");
            var lastRecord = payroll.PayrollDetails.Last().Id;

            var model = new PayrollVM
            {
                Payroll = payroll,
                TotalSalaryOvertime = payroll.PayrollDetails.Select(p => p.SalaryOvertime).Sum(),
                TotalSubTotal = payroll.PayrollDetails.Select(p => p.SubTotal).Sum(),
                TotalBonus = payroll.PayrollDetails.Select(p => p.Bonus).Sum(),
                TotalDiscount = payroll.PayrollDetails.Select(p => p.Discount).Sum(),
                TotalEarned = payroll.PayrollDetails.Select(p => p.Earned).Sum(),
                TotalISSS = payroll.PayrollDetails.Select(p => p.Isss).Sum(),
                TotalAFP = payroll.PayrollDetails.Select(p => p.Afp).Sum(),
                TotalISR = payroll.PayrollDetails.Select(p => p.Isr).Sum(),
                TotalFinalDiscounts = payroll.PayrollDetails.Select(p => p.TotalDiscount).Sum(),
                TotalToPay = payroll.PayrollDetails.Select(p => p.ToPay).Sum(),
                LastRecord = lastRecord
            };


            if (model.Payroll == null)
            {
                return NotFound();
            }

            return new ViewAsPdf("PrintPayroll", model)
            {
                FileName = $"Payroll-{model.Payroll.CreatedAt}.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 12"
            };
        }

        #region API 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var all = await _unitOfWork.Payroll.GetAll(includeProperties: "Employer");

            return Json(new { data = all });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string? id)
        {
            var model = await _unitOfWork.Payroll.Get(id);

            if (model == null)
            {
                return Json(new { success = false, message = "There was an error deleting the payroll" });
            }

            model.IsActive = 0;
            _unitOfWork.Payroll.Update(model);
            await _unitOfWork.Save();

            return Json(new { success = true, message = "Payroll successfully eliminated" });
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePayrollStatus(string? id)
        {
            var model = await _unitOfWork.Payroll.Get(id);

            if (model == null)
            {
                return Json(new { success = false, message = "There was an error activated the payroll" });
            }

            model.IsActive = 1;
            _unitOfWork.Payroll.Update(model);
            await _unitOfWork.Save();
            return Json(new { success = true, message = "Payroll successfully activated" });
        }


        #endregion
    }
}
