using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaOrbita.Model.ViewModels;
using SistemaOrbita.Utilities;

namespace SistemaOrbita.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Owner)]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(string? id)
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var userVM = new UserVM
            {
                User = new IdentityUser(),
                Roles = roles.Select(r => new SelectListItem
                {
                    Value = r.Id,
                    Text = r.Name
                })
            };

            if (string.IsNullOrEmpty(id))
            {
                // Creación de nuevo usuario: solo necesitamos enviar la lista de roles
                userVM.Type = 0;
                return View(userVM);
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                // No se encontró el usuario con el ID proporcionado
                return NotFound();
            }

            // Recuperar el rol actual del usuario para preseleccionarlo en la vista
            var userRole = await _userManager.GetRolesAsync(user);
            var roleId = userRole.FirstOrDefault() != null ?
                         (await _roleManager.FindByNameAsync(userRole.FirstOrDefault())).Id :
                         string.Empty;

            userVM.User = user;
            userVM.RoleId = roleId; // Establecer el ID del rol actual para preseleccionarlo
            userVM.Type = 1;

            return View(userVM);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(UserVM model)
        {
            var roles = await _roleManager.Roles.ToListAsync();
            if (!ModelState.IsValid)
            {
                TempData[DS.Error] = "Error when processing the user.";
                
                model.Roles = roles.Select(r => new SelectListItem
                {
                    Value = r.Id,
                    Text = r.Name
                });

                return View(model);
            }

            IdentityResult result = model.Type == 0 ?
                                    await CreateUserWithRole(model) :
                                    await UpdateUserAndRole(model);

            if (!result.Succeeded)
            {
                AddErrorsToModelState(result);
                TempData[DS.Error] = "Error when processing the user.";
                model.Roles = roles.Select(r => new SelectListItem
                {
                    Value = r.Id,
                    Text = r.Name
                });
                return View(model);
            }

            TempData[DS.Success] = model.Type == 0 ?
                                    "Successfully created user." :
                                    "Successfully updated user and role.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<IdentityResult> CreateUserWithRole(UserVM model)
        {
            var result = await _userManager.CreateAsync(model.User, model.Password);
            if (!result.Succeeded) return result;

            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                ModelState.AddModelError("", "Invalid role ID.");
                return IdentityResult.Failed(new IdentityError { Description = "Invalid role ID." });
            }

            return await _userManager.AddToRoleAsync(model.User, role.Name);
        }

        private async Task<IdentityResult> UpdateUserAndRole(UserVM model)
        {
            // Primero, encuentra el usuario por su ID para asegurarte de que trabajas con la instancia rastreada
            var user = await _userManager.FindByIdAsync(model.User.Id);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            // Actualiza las propiedades del usuario según sea necesario
            user.Email = model.User.Email;
            user.UserName = model.User.UserName;
            user.PhoneNumber = model.User.PhoneNumber;
            // Considera actualizar otras propiedades relevantes aquí

            // Actualiza el usuario con las propiedades modificadas
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return result;

            // Manejo de roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded) return removeResult;

            var newRole = await _roleManager.FindByIdAsync(model.RoleId);
            if (newRole == null)
            {
                // Aquí deberías considerar cómo manejar el error. Por ejemplo, puedes lanzar una excepción,
                // devolver un IdentityResult.Failed, o manejarlo de otra manera adecuada para tu aplicación.
                return IdentityResult.Failed(new IdentityError { Description = "Invalid role ID." });
            }

            return await _userManager.AddToRoleAsync(user, newRole.Name);
        }


        private void AddErrorsToModelState(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        #region API

        public async Task<IActionResult> GetAll()
        {
            var all = await _userManager.Users.ToListAsync();

            return Json(new { data = all });
        }

        [HttpPost]
        public async Task<IActionResult> BlockAndUnBlock([FromBody] string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return Json(new { success = false, message = "Error when updating user." });
            }

            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now;
            }
            else
            {
                user.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            await _userManager.UpdateAsync(user);
            return Json(new { success = true, message = "Successfully updated user and role." });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string? id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Json(new { success = false, message = "There was an error deleting the user" });
            }

            await _userManager.DeleteAsync(user);
            return Json(new { success = true, message = "User successfully eliminated" });

        }
        #endregion
    }
}
