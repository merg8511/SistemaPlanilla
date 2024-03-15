// This is an auto-generated file.

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaOrbita.Helpers;
using SistemaOrbita.Utilities;
using SistemaOrbita.Model.ViewModels;
using SistemaOrbita.Web.Areas.Identity.Data;


namespace SistemaOrbita.Areas.Admin.Controllers
{
	[Area("Admin")]
	//[Authorize(Roles = "Admin")]
	public class PermissionController : Controller
	{
		private readonly RoleManager<IdentityRole> _roleManager;
		public PermissionController(RoleManager<IdentityRole> roleManager)
		{
			_roleManager = roleManager;
		}
		public async Task<ActionResult> Index(string id)
		{
			var model = new PermissionVM();
			var allPermissions = new List<RoleClaimsVM>();
			allPermissions.GetPermissions(typeof(Permissions.AuditLog), id); 
			allPermissions.GetPermissions(typeof(Permissions.Employee), id); 
			allPermissions.GetPermissions(typeof(Permissions.EmployeeHistory), id); 
			allPermissions.GetPermissions(typeof(Permissions.EmployeeProjectAssignment), id); 
			allPermissions.GetPermissions(typeof(Permissions.EventType), id); 
			allPermissions.GetPermissions(typeof(Permissions.OrganizationDepartment), id); 
			allPermissions.GetPermissions(typeof(Permissions.Position), id); 
			allPermissions.GetPermissions(typeof(Permissions.Project), id); 
			                                  
			var role = await _roleManager.FindByIdAsync(id);
			model.RoleId = id;
			var claims = await _roleManager.GetClaimsAsync(role);
			var allClaimValues = allPermissions.Select(a => a.Value).ToList();
			var roleClaimValues = claims.Select(a => a.Value).ToList();
			var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
			foreach (var permission in allPermissions)
			{
				if (authorizedClaims.Any(a => a == permission.Value))
				{
					permission.Selected = true;
				}
			}
			model.RoleClaimsCount = claims.Count;
			model.RoleClaims = allPermissions;
			model.RoleName = role.Name;
			return View(model);
		}
		public async Task<IActionResult> Update(PermissionVM model)
		{
			var role = await _roleManager.FindByIdAsync(model.RoleId);
			var claims = await _roleManager.GetClaimsAsync(role);
			foreach (var claim in claims)
			{
				await _roleManager.RemoveClaimAsync(role, claim);
			}
			var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
			foreach (var claim in selectedClaims)
			{
				await _roleManager.AddPermissionClaim(role, claim.Value);
			}
			TempData[DS.Success] = $"Successfully updated roles for {role.Name}";
			return RedirectToAction("Index", new { roleId = model.RoleId });
		}
	}
}



