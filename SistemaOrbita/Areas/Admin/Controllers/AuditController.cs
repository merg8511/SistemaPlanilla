using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaOrbita.DAL.Repository.IRepository;
using SistemaOrbita.Model.ViewModels;
using SistemaOrbita.Utilities;

namespace SistemaOrbita.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Owner)]
    public class AuditController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public AuditController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var model = new AuditLogVM()
            {
                Users = users.Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                }).ToList()
            };

            return View(model);
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> FilteredLogs(string? userId, string? startDate, string? endDate)
        {
            DateTime? startDateTime = null;
            DateTime? endDateTime = null;

            if (!string.IsNullOrEmpty(startDate))
            {
                startDateTime = DateTime.Parse(startDate);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                endDateTime = DateTime.Parse(endDate).AddDays(1);
            }

            var query = _unitOfWork.AuditLog.GetAll(
               filter: a =>
                   (string.IsNullOrEmpty(userId) || a.UserId == userId) && 
                   (!startDateTime.HasValue || a.ChangeDate >= startDateTime) && 
                   (!endDateTime.HasValue || a.ChangeDate < endDateTime), 
               orderBy: q => q.OrderByDescending(a => a.ChangeDate)
           );


            var audits = await query;

            return Json(new { response = audits });
        }

        #endregion
    }
}
