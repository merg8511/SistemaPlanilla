using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using SistemaOrbita.DAL.Repository.IRepository;
using SistemaOrbita.Model.Models;
using SistemaOrbita.Model.ViewModels;
using SistemaOrbita.Utilities;
using System.Security.Claims;

namespace SistemaOrbita.Areas.BusinessManagement.Controllers
{
    [Area("BusinessManagement")]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController(IUnitOfWork unitOfWork)
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

            var employee = await _unitOfWork.Employee.GetEmployeeDetails(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(string? id)
        {
            EmployeeVM employeeVM = new EmployeeVM()
            {
                Employee = new Employee(),
                Genders = _unitOfWork.Employee.GetAllDropDownList("Gender"),
                Municipalities = _unitOfWork.Employee.GetAllDropDownList("Municipality"),
                PositionsList = await _unitOfWork.Position.GetAll()
            };

            if (string.IsNullOrEmpty(id))
            {
                employeeVM.Employee.IsActive = 1;
                return View(employeeVM);
            }
            else
            {
                employeeVM.Employee = await _unitOfWork.Employee.GetFirst(e => e.Id == id, includeProperties: "Municipality");
                if (employeeVM.Employee == null)
                {
                    return NotFound();
                }
                return View(employeeVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(EmployeeVM employeeVM)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(employeeVM.Employee.Id))
                {
                    employeeVM.Employee.Id = NUlid.Ulid.NewUlid().ToString();
                    employeeVM.Employee.CreatedAt = DateTime.Now;
                    employeeVM.Employee.CreatedBy = User.FindFirstValue(ClaimTypes.Name);
                    employeeVM.Employee.IsActive = 1;

                    await _unitOfWork.Employee.Create(employeeVM.Employee);
                    await _unitOfWork.Save();

                    string changeReason = "Nuevo empleado";
                    await SaveEmployeeHistory(employeeVM.Employee, changeReason);

                    TempData[DS.Success] = "Successfully created employee";
                }
                else
                {
                    //Work history flow
                    var existingEmployee = await _unitOfWork.Employee.GetFirst(e => e.Id == employeeVM.Employee.Id, includeProperties: "Position");
                    bool hasPositionChanged = existingEmployee.PositionId != employeeVM.Employee.PositionId;
                    bool hasSalaryChanged = existingEmployee.Salary != employeeVM.Employee.Salary;

                    //Update employee
                    _unitOfWork.Employee.Update(employeeVM.Employee);
                    if (hasPositionChanged || hasSalaryChanged)
                    {
                        string changeReason = DetermineChangeReason(employeeVM.Employee, existingEmployee, hasPositionChanged, hasSalaryChanged);
                        await SaveEmployeeHistory(employeeVM.Employee, changeReason, existingEmployee, hasPositionChanged, hasSalaryChanged);
                    }
                    await _unitOfWork.Save();
                    TempData[DS.Success] = "Successfully updated employee";

                }

                return RedirectToAction(nameof(Index));
            }

            TempData[DS.Error] = "Error when creating the employee";
            employeeVM.Genders = _unitOfWork.Employee.GetAllDropDownList("Gender");
            employeeVM.PositionsList = await _unitOfWork.Position.GetAll();
            return View(employeeVM);
        }

        private async Task SaveEmployeeHistory(Employee employee, string changeReason = null, Employee existingEmployee = null,
            bool hasPositionChanged = false, bool hasSalaryChanged = false)
        {
            if (existingEmployee == null)
            {
                var employeeHistory = new EmployeeHistory
                {
                    Id = NUlid.Ulid.NewUlid().ToString(),
                    EmployeeId = employee.Id,
                    PositionId = employee.PositionId,
                    OldSalary = 0, 
                    NewSalary = employee.Salary,
                    ChangeReason = changeReason, 
                    StartDate = employee.StartDate
                };
                await _unitOfWork.EmployeeHistory.Create(employeeHistory);
            }
            else
            {
                var employeeHistory = new EmployeeHistory
                {
                    Id = NUlid.Ulid.NewUlid().ToString(),
                    EmployeeId = employee.Id,
                    PositionId = hasPositionChanged ? employee.PositionId : existingEmployee.PositionId,
                    OldSalary = hasSalaryChanged ? existingEmployee?.Salary ?? 0 : 0,
                    NewSalary = employee.Salary,
                    ChangeReason = changeReason,
                    StartDate = employee.StartDate,
                };
                await _unitOfWork.EmployeeHistory.Create(employeeHistory);

            }

            await _unitOfWork.Save();
        }

        private string DetermineChangeReason(Employee employee, Employee existingEmployee, bool hasPositionChanged, bool hasSalaryChanged)
        {
            if (hasPositionChanged && hasSalaryChanged)
            {
                return "Promoción y ajuste de salario";
            }
            else if (hasPositionChanged)
            {
                return "Cambio de posición";
            }
            else if (hasSalaryChanged)
            {
                if (employee.Salary > existingEmployee.Salary)
                {
                    return "Incremento de salario";
                }
                else
                {
                    return "Decremento de salario";
                }
            }

            return "Actualización general";
        }


        #region API 

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var all = await _unitOfWork.Employee.GetAll(includeProperties: "Position");
            return Json(new { data = all });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string? id)
        {
            var model = await _unitOfWork.Employee.Get(id);

            if (model == null)
            {
                return Json(new { success = false, message = "There was an error deleting the employee" });
            }

            model.IsActive = 0;
            _unitOfWork.Employee.Update(model);
            await _unitOfWork.Save();

            return Json(new { success = true, message = "Employee successfully eliminated" });
        }

        #endregion
    }
}
