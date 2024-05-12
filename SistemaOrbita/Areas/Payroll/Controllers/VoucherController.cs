using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using SistemaOrbita.DAL.Repository;
using SistemaOrbita.DAL.Repository.IRepository;
using SistemaOrbita.Model.Models;

namespace SistemaOrbita.Areas.Payroll.Controllers
{
    [Authorize]
    [Area("Payroll")]
    public class VoucherController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VoucherController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PrintVoucher(string? id)
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

            payroll.Status = 1;
            _unitOfWork.Payroll.Update(payroll);

            await FillPaymentHistory(payroll);

            return new ViewAsPdf("PrintVoucher", payroll)
            {
                FileName = $"Voucher-{payroll.CreatedAt}.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 12"
            };
        }

        private async Task FillPaymentHistory(SistemaOrbita.Model.Models.Payroll payroll)
        {
            foreach (var detail in payroll.PayrollDetails)
            {
                PaymentHistory paymentHistory = new PaymentHistory
                {
                    Id = NUlid.Ulid.NewUlid().ToString(),
                    PayrollId = payroll.Id,
                    EmployeeId = detail.EmployeeId,
                    PaymentDate = DateTime.Now,
                    Amount = detail.ToPay,
                    Status = 1
                };

                await _unitOfWork.PaymentHistory.Create(paymentHistory);
                await _unitOfWork.Save();
            }
        }

        #region API 

        public async Task<IActionResult> GetAll()
        {
            var all = await _unitOfWork.Payroll.GetAll(p => p.IsActive == 1);
            return Json(new { data = all });
        }

        #endregion
    }
}
