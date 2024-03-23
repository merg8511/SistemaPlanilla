using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaOrbita.ActionFilters;
using SistemaOrbita.DAL.Repository.IRepository;
using SistemaOrbita.Model.Models;
using SistemaOrbita.Model.ViewModels;
using SistemaOrbita.Utilities;
using SistemaOrbita.Web.Areas.Identity.Data;
using System.Security.Claims;

namespace SistemaOrbita.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BusinessController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BusinessController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [OrbitaAuthorize(Permissions.Employer.View)]
        [ServiceFilter(typeof(AuditLogFilter))]
        [Audit($"{DS.Audit_View} Business")]
        public async Task<IActionResult> Index()
        {

            var employer = await _unitOfWork.Employer.Get("1");

            if (employer == null)
            {
                employer = new();
            }

            return View(employer);
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> GetQuotes()
        {
            var all = await _unitOfWork.QuotationType.GetAll(q => q.IsActive == 1);
            return Json(new { data = all });
        }

        [HttpGet]
        public async Task<IActionResult> GetBrackets()
        {
            var all = await _unitOfWork.IncomeTaxBracket.GetAll(q => q.IsActive == 1);
            return Json(new { data = all });
        }

        [OrbitaAuthorize(Permissions.Employer.Upsert)]
        [ServiceFilter(typeof(AuditLogFilter))]
        [Audit($"{DS.Audit_Delete} Business")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBusiness(Employer employer)
        {
            if (string.IsNullOrEmpty(employer.Id))
            {
                employer.Id = "1";

                await _unitOfWork.Employer.Create(employer);
            }
            else
            {
                _unitOfWork.Employer.Update(employer);
            }
            await _unitOfWork.Save();
            return Json(new { success = true, message = "Successfully updated employer" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuotation(QuotationType quotation)
        {
            if (string.IsNullOrEmpty(quotation.Id))
            {
                var quotationExists = await _unitOfWork.QuotationType.Any(q => q.Name == quotation.Name);

                if (quotationExists)
                {
                    return Json(new { success = false, message = $"Quotation {quotation.Name} already exists" });
                }

                quotation.Id = NUlid.Ulid.NewUlid().ToString();
                quotation.IsActive = 1;
                quotation.CreatedAt = DateTime.Now;
                quotation.CreatedBy = User.FindFirstValue(ClaimTypes.Name);

                await _unitOfWork.QuotationType.Create(quotation);
            }
            else
            {
                _unitOfWork.QuotationType.Update(quotation);
            }
            await _unitOfWork.Save();
            return Json(new { success = true, message = "Successfully updated quotation" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateIsr(IncomeTaxBracket incomeTax)
        {
            if(string.IsNullOrEmpty(incomeTax.Id))
            {
                var taxExists = await _unitOfWork.IncomeTaxBracket.Any(q => q.Name == incomeTax.Name);

                if(taxExists)
                {
                    return Json(new { success = false, message = $"Tax bracket {incomeTax.Name} already exists" });
                }

                incomeTax.Id = NUlid.Ulid.NewUlid().ToString();
                incomeTax.IsActive = 1;
                incomeTax.CreatedAt = DateTime.Now;
                incomeTax.CreatedBy = User.FindFirstValue(ClaimTypes.Name);

                await _unitOfWork.IncomeTaxBracket.Create(incomeTax);
            }
            else
            {
                _unitOfWork.IncomeTaxBracket.Update(incomeTax);
            }

            await _unitOfWork.Save();
            return Json(new { success = true, message = "Successfully updated tax bracket" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteQuotation(string id)
        {
            var model = await _unitOfWork.QuotationType.Get(id);

            if (model == null)
            {
                return Json(new { success = false, message = "There was an error deleting quotation" });
            }

            _unitOfWork.QuotationType.Delete(model);

            await _unitOfWork.Save();
            return Json(new { success = true, message = "Quotation deleted successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBrackets(string id)
        {
            var model = await _unitOfWork.IncomeTaxBracket.Get(id);

            if (model == null)
            {
                return Json(new { success = false, message = "There was an error deleting brakect" });
            }

            _unitOfWork.IncomeTaxBracket.Delete(model);

            await _unitOfWork.Save();
            return Json(new { success = true, message = "Bracket deleted successfully" });
        }

        #endregion
    }
}
