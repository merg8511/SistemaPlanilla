using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaOrbita.Utilities;

namespace SistemaOrbita.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Owner)]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(string id)
        {
            var role = new IdentityRole();
            if (!string.IsNullOrEmpty(id))
            {
                role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return NotFound();
                }
            }
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(IdentityRole role, int type)
        {
            if (role.Name == null)
            {
                TempData[DS.Error] = "Error when creating the role";
                return RedirectToAction(nameof(Index));
            }

            if (type == 0)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(role.Name.Trim()));

                if (result.Succeeded)
                {
                    TempData[DS.Success] = "Successfuly created role";
                }
                else
                {
                    TempData[DS.Error] = "Error when creating the role";
                }
            }
            else
            {
                var existingRole = await _roleManager.FindByIdAsync(role.Id);

                if (existingRole == null)
                {
                    TempData[DS.Error] = "Role not found";
                    return RedirectToAction(nameof(Index));
                }

                existingRole.Name = role.Name.Trim();
                var result = await _roleManager.UpdateAsync(existingRole);

                if (result.Succeeded)
                {
                    TempData[DS.Success] = "Successfully updated role";
                }
                else
                {
                    TempData[DS.Error] = "Error when updating the role";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Json(new { data = roles });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return Json(new { success = true, message = "Role successfully eliminated" });
            }
            else
            {
                return Json(new { success = false, message = "There was an error deleting Role" });
            }

        }

        #endregion
    }
}
