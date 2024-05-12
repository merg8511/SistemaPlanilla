using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaOrbita.DAL.Repository.IRepository;
using SistemaOrbita.Model.ViewModels;

namespace SistemaOrbita.Areas.Payroll.Controllers
{
    [Authorize]
    [Area("Payroll")]
    public class PaymentHistoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentHistoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            PaymentHistoryVM paymentHistoryVM = new PaymentHistoryVM()
            {
                Payrolls = _unitOfWork.PaymentHistory.GetAllDropDownList("Payroll"),
                Employees = _unitOfWork.PaymentHistory.GetAllDropDownList("Employee"),
                Organizations = _unitOfWork.PaymentHistory.GetAllDropDownList("Organization")
            };

            return View(paymentHistoryVM);
        }

        #region API 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetEmployeePayment(string employeeId, string startDate, string endDate)
        {

            var model = await _unitOfWork.PaymentHistory.GetAll(e => e.EmployeeId == employeeId &&
                                                          e.PaymentDate >= DateTime.Parse(startDate) &&
                                                          e.PaymentDate <= DateTime.Parse(endDate), 
                                                          includeProperties:"Employee,Payroll,Payroll.PayrollDetails");

            return Json(new { data = model });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetPayrollPayment(string employeeId, string startDate, string endDate)
        {

            var model = _unitOfWork.PaymentHistory.GetAll(e => e.EmployeeId == employeeId &&
                                                          e.PaymentDate >= DateTime.Parse(startDate) &&
                                                          e.PaymentDate <= DateTime.Parse(endDate));

            return Json(new { data = model });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetDepartmentPayment(string employeeId, string startDate, string endDate)
        {

            var model = _unitOfWork.PaymentHistory.GetAll(e => e.EmployeeId == employeeId &&
                                                          e.PaymentDate >= DateTime.Parse(startDate) &&
                                                          e.PaymentDate <= DateTime.Parse(endDate));

            return Json(new { data = model });
        }

        #endregion

    }
}
