﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class PositionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PositionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [OrbitaAuthorize(Permissions.Position.View)]
        [ServiceFilter(typeof(AuditLogFilter))]
        [Audit($"{DS.Audit_View} Position")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [OrbitaAuthorize(Permissions.Position.Upsert)]
        [ServiceFilter(typeof(AuditLogFilter))]
        [Audit($"{DS.Audit_Upsert} Position")]
        public async Task<IActionResult> Upsert(string? id)
        {
            PositionVM positionVM = new PositionVM()
            {
                Position = new Position(),
                Organizations = _unitOfWork.Position.GetAllDropDownList("OrganizationDepartment")
            };

            if (string.IsNullOrEmpty(id))
            {
                positionVM.Position.IsActive = 1;
                return View(positionVM);
            }
            else
            {
                positionVM.Position = await _unitOfWork.Position.Get(id);
                if (positionVM.Position == null)
                {
                    return NotFound();
                }

                return View(positionVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OrbitaAuthorize(Permissions.Position.Upsert)]
        [ServiceFilter(typeof(AuditLogFilter))]
        [Audit($"{DS.Audit_Upsert} Position")]
        public async Task<IActionResult> Upsert(PositionVM positionVM)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(positionVM.Position.Id))
                {
                    positionVM.Position.Id = NUlid.Ulid.NewUlid().ToString();
                    positionVM.Position.IsActive = 1;
                    positionVM.Position.CreatedAt = DateTime.Now;

                    await _unitOfWork.Position.Create(positionVM.Position);
                    TempData[DS.Success] = "Successfully created position";
                }
                else
                {
                    _unitOfWork.Position.Update(positionVM.Position);
                    TempData[DS.Success] = "Successfully updated position";
                }
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            positionVM.Organizations = _unitOfWork.Position.GetAllDropDownList("OrganizationDepartment");
        
            TempData[DS.Error] = "Error when creating the position";
            return View(positionVM);
        }

        #region API 

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var all = await _unitOfWork.Position.GetAll(includeProperties: "OrganizationDepartment");
            return Json(new { data = all });
        }


        [HttpPost]
        [OrbitaAuthorize(Permissions.Position.Delete)]
        [ServiceFilter(typeof(AuditLogFilter))]
        [Audit($"{DS.Audit_Delete} Position")]
        public async Task<IActionResult> Delete(string id)
        {
            var position = await _unitOfWork.Position.Get(id);

            if (position == null)
            {
                return Json(new { success = false, message = "There was an error deleting the position" });
            }
            position.IsActive = 0;
            _unitOfWork.Position.Update(position);
            await _unitOfWork.Save();
            return Json(new { success = true, message = "Position successfully eliminated" });
        }

        #endregion
    }
}
