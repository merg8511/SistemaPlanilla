using Microsoft.AspNetCore.Authorization;
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
    public class EPAssignmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public EPAssignmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [OrbitaAuthorize(Permissions.EmployeeProjectAssignment.View)]
        [ServiceFilter(typeof(AuditLogFilter))]
        [Audit($"{DS.Audit_View} EmployeeProjectAssignment")]
        public async Task<IActionResult> Index()
        {
            var project = await _unitOfWork.Project.GetAll(
                e => e.IsActive == 1,
                includeProperties: "Manager,EmployeeProjectAssignments,EmployeeProjectAssignments.Employee");

            return View(project);
        }

        [HttpGet]
        [OrbitaAuthorize(Permissions.EmployeeProjectAssignment.Upsert)]
        [ServiceFilter(typeof(AuditLogFilter))]
        [Audit($"{DS.Audit_Upsert} EmployeeProjectAssignment")]
        public async Task<IActionResult> Upsert(string id)
        {
            var assignment = new EmployeeAssignmentVM();

            assignment.Project = await _unitOfWork.Project.GetFirst(
                p => p.Id == id,
                includeProperties: "EmployeeProjectAssignments,EmployeeProjectAssignments.Employee"
                );

            if (assignment.Project == null)
            {
                return NotFound();
            }

            assignment.Employees = await _unitOfWork.Employee.GetAll(
                e => e.IsActive == 1 && (e.PositionId == "01HSJ8FCZFRQF5CA6QC0BDZBCW" || e.PositionId == "01HSJ8P3EEXBVVNTH6SXPMMNXP"),
                includeProperties: "Position,Gender");

            assignment.SelectedEmployeeIds = assignment.Project
                .EmployeeProjectAssignments
                .Select(epa => epa.EmployeeId).ToList();

            return View(assignment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OrbitaAuthorize(Permissions.EmployeeProjectAssignment.Upsert)]
        [ServiceFilter(typeof(AuditLogFilter))]
        [Audit($"{DS.Audit_Upsert} EmployeeProjectAssignment")]
        public async Task<IActionResult> Upsert(string projectId, string selectedEmployeeIds)
        {
            if (projectId == null)
            {
                return BadRequest("Invalid project Id");
            }

            var selectedIdsList = selectedEmployeeIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var current = await _unitOfWork.EmployeeAssignment.GetAll(ea => ea.ProjectId == projectId);
            var currentAssignment = current.Select(e => e.EmployeeId).ToList();

            var toAdd = selectedIdsList.Except(currentAssignment);

            var toRemove = currentAssignment.Except(selectedIdsList);

            foreach (var id in toAdd)
            {
                var newAssignment = new EmployeeProjectAssignment
                {
                    Id = NUlid.Ulid.NewUlid().ToString(),
                    ProjectId = projectId,
                    EmployeeId = id,
                    AssignedDate = DateTime.Now,
                };

                await _unitOfWork.EmployeeAssignment.Create(newAssignment);
            }

            foreach (var id in toRemove)
            {
                var deleteAssignment = await _unitOfWork.EmployeeAssignment.GetFirst(epa => epa.ProjectId == projectId 
                                                                    && epa.EmployeeId == id);

                if(deleteAssignment != null)
                {
                    _unitOfWork.EmployeeAssignment.Delete(deleteAssignment);
                }
            }

            await _unitOfWork.Save();
            TempData[DS.Success] = "Successfully updated project assignment";
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [OrbitaAuthorize(Permissions.EmployeeProjectAssignment.Delete)]
        [ServiceFilter(typeof(AuditLogFilter))]
        [Audit($"{DS.Audit_Delete} EmployeeProjectAssignment")]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await _unitOfWork.EmployeeAssignment.Get(id);

            if (model == null)
            {
                return Json(new { success = false, message = "There was an error deleting employee assignment" });
            }

            _unitOfWork.EmployeeAssignment.Delete(model);
            await _unitOfWork.Save();
            return Json(new { success = true, message = "Employee assignment successfully eliminated" });
        }
    }
}
